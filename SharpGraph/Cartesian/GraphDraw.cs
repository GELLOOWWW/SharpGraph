using System.Drawing.Drawing2D;
using SharpGraph.Expressions;


namespace SharpGraph.Cartesian
{
    public static class GraphDraw
    {
        /// <summary>
        /// Asynchronously generates a bitmap visualizing the implicit curve f(x, y) = 0 using the Marching Squares algorithm.
        /// </summary>
        /// <param name="mapper">Coordinate mapper defining the Cartesian coordinate system and screen dimensions.</param>
        /// <param name="expr">Parsed expression representing f(x, y).</param>
        /// <param name="color">Color the graph will be in.</param>
        /// <param name="step">Step size in pixels between sampling points for the Marching Squares grid.</param>
        /// <returns>A Task representing the asynchronous operation, with a Bitmap result containing the graph image.</returns>
        public static async Task<Bitmap> Graph(GraphCoordinate mapper, ParsedExpression expr, int step = 1)
        {
            // Validate input arguments: ensure mapper is not null,
            // step is positive, and expression has a compiled function
            ArgumentNullException.ThrowIfNull(mapper);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(step);
            if (expr?.CompiledFunction is null) throw new ArgumentNullException(nameof(expr));

            // Cache screen dimensions for convenience
            int width = mapper.ScreenWidth;
            int height = mapper.ScreenHeight;

            // Calculate the number of points in the grid along x and y
            // Added +2 to cover boundary edges completely
            int xCount = (width / step) + 2;
            int yCount = (height / step) + 2;

            // 2D arrays to hold function evaluations and their validity status
            double[,] values = new double[xCount, yCount];

            // Compute function values asynchronously and in parallel
            await Task.Run(() =>
            {
                Parallel.For(0, xCount, gx =>
                {
                    for (int gy = 0; gy < yCount; gy++)
                    {
                        int ix = gx * step;
                        int iy = gy * step;

                        // Map pixel coordinates to Cartesian coordinates
                        double x = mapper.MapScreenToX(ix);
                        double y = mapper.MapScreenToY(iy);

                        try
                        {
                            // Evaluate the expression at (x, y)
                            double result = expr.CompiledFunction(x, y);
                            values[gx, gy] = result;

                            // On exceptions, mark as invalid and assign 0 for safety
                        } catch { values[gx, gy] = 0; }
                    }
                });
            });

            var bitmap = new Bitmap(width, height);
            using var g = Graphics.FromImage(bitmap);
            Color graphColor = expr.FunctionColor;
            using Pen pen = new(graphColor, 2f);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.Transparent);

            // Local helper to interpolate contour crossing point along an edge
            // between two pixel points p1 and p2 with function values v1 and v2
            static PointF Interpolate(PointF p1, PointF p2, double v1, double v2)
            {
                float t = (float)(v1 / (v1 - v2)); // Linear interpolation factor
                return new PointF(
                    p1.X + t * (p2.X - p1.X),
                    p1.Y + t * (p2.Y - p1.Y)
                );
            }

            // Iterate over every cell defined by four neighboring grid points
            for (int gx = 0; gx < xCount - 1; gx++)
            {
                for (int gy = 0; gy < yCount - 1; gy++)
                {
                    int ix = gx * step;
                    int iy = gy * step;

                    // Define corners of the cell in pixel coordinates
                    PointF p0 = new(ix, iy);
                    PointF p1 = new(ix + step, iy);
                    PointF p2 = new(ix + step, iy + step);
                    PointF p3 = new(ix, iy + step);

                    // Extract corresponding function values at the cell corners
                    double v0 = values[gx, gy];
                    double v1 = values[gx + 1, gy];
                    double v2 = values[gx + 1, gy + 1];
                    double v3 = values[gx, gy + 1];

                    // Heuristic skip: if difference in values on edges is large, likely discontinuity
                    if (Math.Abs(v0 - v1) > 20 || Math.Abs(v1 - v2) > 20 || Math.Abs(v2 - v3) > 20 || Math.Abs(v3 - v0) > 20)
                        continue;

                    // Determine cell state as a 4-bit mask based on sign of corner values
                    int state = 0;
                    if (v0 < 0) state |= 1;
                    if (v1 < 0) state |= 2;
                    if (v2 < 0) state |= 4;
                    if (v3 < 0) state |= 8;

                    // Use marching squares lookup to draw contour lines within this cell
                    switch (state)
                    {
                        case 0:
                        case 15:
                            // No crossings if all corners have same sign
                            break;
                        case 1:
                        case 14:
                            g.DrawLine(pen, Interpolate(p0, p1, v0, v1), Interpolate(p0, p3, v0, v3));
                            break;
                        case 2:
                        case 13:
                            g.DrawLine(pen, Interpolate(p0, p1, v0, v1), Interpolate(p1, p2, v1, v2));
                            break;
                        case 3:
                        case 12:
                            g.DrawLine(pen, Interpolate(p1, p2, v1, v2), Interpolate(p0, p3, v0, v3));
                            break;
                        case 4:
                        case 11:
                            g.DrawLine(pen, Interpolate(p1, p2, v1, v2), Interpolate(p2, p3, v2, v3));
                            break;
                        case 5:
                            g.DrawLine(pen, Interpolate(p0, p1, v0, v1), Interpolate(p0, p3, v0, v3));
                            g.DrawLine(pen, Interpolate(p1, p2, v1, v2), Interpolate(p2, p3, v2, v3));
                            break;
                        case 6:
                        case 9:
                            g.DrawLine(pen, Interpolate(p0, p1, v0, v1), Interpolate(p2, p3, v2, v3));
                            break;
                        case 7:
                        case 8:
                            g.DrawLine(pen, Interpolate(p0, p3, v0, v3), Interpolate(p2, p3, v2, v3));
                            break;
                        case 10:
                            g.DrawLine(pen, Interpolate(p0, p3, v0, v3), Interpolate(p1, p2, v1, v2));
                            break;
                    }
                }
            }
            return bitmap;
        }


        /// <summary>
        /// Draws minor and major grids in a Graphics object.
        /// </summary>
        /// <remarks>grid spacing is handled by the Coordinate object.</remarks>
        public static void Grids(Graphics g, GraphCoordinate mapper)
        {
            using Pen minorPen = new(Settings.MinorGridColor, 1) { DashStyle = DashStyle.Dash };
            using Pen majorPen = new(Settings.MajorGridColor, 1) { DashStyle = DashStyle.Dash };

            float minorGridSpacingX = mapper.GridSpacingX(40);
            float minorGridSpacingY = mapper.GridSpacingY(40);
            DrawGridLines(g, mapper, false, minorGridSpacingX, minorPen, false);
            DrawGridLines(g, mapper, false, minorGridSpacingY, minorPen, true);

            float majorGridSpacingX = mapper.GridSpacingX(120);
            float majorGridSpacingY = mapper.GridSpacingY(120);
            DrawGridLines(g, mapper, true, majorGridSpacingX, majorPen, false);
            DrawGridLines(g, mapper, true, majorGridSpacingY, majorPen, true);

            static void DrawGridLines(Graphics g, GraphCoordinate m, bool major, float gridSpacing, Pen pen, bool horizontal)
            {
                int w = m.ScreenWidth;
                int h = m.ScreenHeight;

                Brush labelBrush = new SolidBrush(Settings.GridNumColor);
                Font labelFont = Settings.FontDefault;

                if (gridSpacing <= 0) return;

                // Draw from min to max, including negative side
                float minVal = horizontal ? m.yMin : m.xMin;
                float maxVal = horizontal ? m.yMax : m.xMax;

                // Start slightly before min, to ensure coverage
                float start = (float)Math.Floor(minVal / gridSpacing) * gridSpacing;
                float end = (float)Math.Ceiling(maxVal / gridSpacing) * gridSpacing;

                for (float val = start; val <= end + 0.001f; val += gridSpacing)
                {
                    int pos = horizontal ? m.MapYToScreen(val) : m.MapXToScreen(val);

                    if (horizontal)
                    {
                        // horizontal line across full width
                        g.DrawLine(pen, 0, pos, w, pos);
                    }
                    else
                    {
                        // vertical line down full height
                        g.DrawLine(pen, pos, 0, pos, h);
                    }
                    // Draw label if major grid and not near zero to avoid duplicate origin label
                    if (major && Math.Abs(val) > 1e-5)
                    {
                        string label = val.ToString("0.##");
                        SizeF size = g.MeasureString(label, labelFont);
                        PointF labelPos = horizontal
                            ? new PointF(m.MapXToScreen(0) + 2, pos - size.Height / 2) // horizontal gridlines
                            : new PointF(pos - size.Width / 2, m.MapYToScreen(0) + 2); // vertical
                        g.DrawString(label, labelFont, labelBrush, labelPos);
                    }
                }
            }
        }

        /// <summary>
        /// Draws the x and y axes in a Graphics object
        /// </summary>
        public static void Axes(Graphics g, GraphCoordinate mapper)
        {
            using Pen axisPen = new(Settings.AxisColor, 2);

            int w = mapper.ScreenWidth;
            int h = mapper.ScreenHeight;
            int half_w = w / 2; // x = 0 origin point
            int half_h = h / 2; // y = 0 origin point

            g.DrawLine(axisPen, 0, half_h, w, half_h); // draw x-axis
            g.DrawLine(axisPen, half_w, 0, half_w, h); // draw y-axis
        }
    }
}