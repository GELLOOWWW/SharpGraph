using SharpGraph.Expressions;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SharpGraph.Cartesian
{
    public static class GraphDraw
    {
        public static async Task<Bitmap> GraphAsync(Coordinate mapper, ParsedExpression expr, int step = 5, CancellationToken token = default)
        {
            if (mapper is null) throw new ArgumentNullException(nameof(mapper));
            if (expr?.CompiledFunction is null) throw new ArgumentNullException(nameof(expr));
            if (step <= 0) throw new ArgumentOutOfRangeException(nameof(step));

            int width = mapper.ScreenWidth;
            int height = mapper.ScreenHeight;

            return await Task.Run(() =>
            {
                var bitmap = new Bitmap(width, height);
                using var g = Graphics.FromImage(bitmap);
                using var pen = new Pen(Color.Blue, 2);

                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                double[,] values = new double[(width / step) + 2, (height / step) + 2];

                // Precompute values for performance
                for (int ix = 0, gx = 0; ix < width; ix += step, gx++)
                {
                    for (int iy = 0, gy = 0; iy < height; iy += step, gy++)
                    {
                        if (token.IsCancellationRequested) return bitmap;
                        double x = mapper.MapScreenToX(ix);
                        double y = mapper.MapScreenToY(iy);
                        values[gx, gy] = expr.CompiledFunction(x, y);
                    }
                }

                // Helper for interpolation
                static PointF Interpolate(PointF p1, PointF p2, double v1, double v2)
                {
                    float t = (float)(v1 / (v1 - v2));
                    return new PointF(
                        p1.X + t * (p2.X - p1.X),
                        p1.Y + t * (p2.Y - p1.Y)
                    );
                }

                // Marching Squares
                for (int ix = 0, gx = 0; ix < width - step; ix += step, gx++)
                {
                    for (int iy = 0, gy = 0; iy < height - step; iy += step, gy++)
                    {
                        if (token.IsCancellationRequested) return bitmap;

                        PointF p0 = new(ix, iy);
                        PointF p1 = new(ix + step, iy);
                        PointF p2 = new(ix + step, iy + step);
                        PointF p3 = new(ix, iy + step);

                        double v0 = values[gx, gy];
                        double v1_ = values[gx + 1, gy];
                        double v2 = values[gx + 1, gy + 1];
                        double v3 = values[gx, gy + 1];

                        int state = 0;
                        if (v0 < 0) state |= 1;
                        if (v1_ < 0) state |= 2;
                        if (v2 < 0) state |= 4;
                        if (v3 < 0) state |= 8;

                        switch (state)
                        {
                            case 0:
                            case 15:
                                break;
                            case 1:
                            case 14:
                                g.DrawLine(pen, Interpolate(p0, p1, v0, v1_), Interpolate(p0, p3, v0, v3));
                                break;
                            case 2:
                            case 13:
                                g.DrawLine(pen, Interpolate(p0, p1, v0, v1_), Interpolate(p1, p2, v1_, v2));
                                break;
                            case 3:
                            case 12:
                                g.DrawLine(pen, Interpolate(p1, p2, v1_, v2), Interpolate(p0, p3, v0, v3));
                                break;
                            case 4:
                            case 11:
                                g.DrawLine(pen, Interpolate(p1, p2, v1_, v2), Interpolate(p2, p3, v2, v3));
                                break;
                            case 5:
                                g.DrawLine(pen, Interpolate(p0, p1, v0, v1_), Interpolate(p0, p3, v0, v3));
                                g.DrawLine(pen, Interpolate(p1, p2, v1_, v2), Interpolate(p2, p3, v2, v3));
                                break;
                            case 6:
                            case 9:
                                g.DrawLine(pen, Interpolate(p0, p1, v0, v1_), Interpolate(p2, p3, v2, v3));
                                break;
                            case 7:
                            case 8:
                                g.DrawLine(pen, Interpolate(p0, p3, v0, v3), Interpolate(p2, p3, v2, v3));
                                break;
                            case 10:
                                g.DrawLine(pen, Interpolate(p0, p3, v0, v3), Interpolate(p1, p2, v1_, v2));
                                break;
                        }
                    }
                }

                return bitmap;

            }, token);
        }

        /// <summary>
        /// Draws minor and major grids in a Graphics object.
        /// </summary>
        /// <remarks>grid spacing is handled by the Coordinate object.</remarks>
        /// <param name="g"></param>
        /// <param name="mapper"></param>
        public static void Grids(Graphics g, Coordinate mapper)
        {
            using Pen minorPen = new(Settings.MinorGridColor, 1) { DashStyle = DashStyle.Dash };
            using Pen majorPen = new(Settings.MajorGridColor, 1) { DashStyle = DashStyle.Dash };

            float minorGridSpacingX = mapper.GridSpacingX(20);
            float minorGridSpacingY = mapper.GridSpacingY(20);
            DrawGridLines(g, mapper, false, minorGridSpacingX, minorPen, false);
            DrawGridLines(g, mapper, false, minorGridSpacingY, minorPen, true);

            float majorGridSpacingX = mapper.GridSpacingX(80);
            float majorGridSpacingY = mapper.GridSpacingY(80);
            DrawGridLines(g, mapper, true, majorGridSpacingX, majorPen, false);
            DrawGridLines(g, mapper, true, majorGridSpacingY, majorPen, true);

            static void DrawGridLines(Graphics g, Coordinate m, bool major, float gridSpacing, Pen pen, bool horizontal)
            {
                int w = m.ScreenWidth;
                int h = m.ScreenHeight;

                Brush labelBrush = new SolidBrush(Settings.FgColor);
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
                            ? new PointF(m.MapXToScreen(0) + 2, pos - size.Height / 2)
                            : new PointF(pos - size.Width / 2, m.MapYToScreen(0) + 2);
                        g.DrawString(label, labelFont, labelBrush, labelPos);
                    }
                }
            }
        }

        /// <summary>
        /// Draws the x and y axes in a Graphics object
        /// </summary>
        /// <param name="g"></param>
        /// <param name="mapper"></param>
        public static void Axes(Graphics g, Coordinate mapper)
        {
            using Pen axisPen = new(Settings.AxisColor, 2);

            int w = mapper.ScreenWidth;
            int h = mapper.ScreenHeight;
            int half_w = w / 2; // x = 0 origin point
            int half_h = h / 2; // y = 0 origin point

            g.DrawLine(axisPen, 0, half_h, w, half_h); // draw x-axis
            g.DrawLine(axisPen, half_w, 0, half_w, h); // draw y-axis

            // draw origin point label (0)
            string s = "0";
            Brush labelBrush = new SolidBrush(Settings.FgColor);
            SizeF size = g.MeasureString(s, Settings.FontDefault);
            PointF labelPos = new(mapper.MapXToScreen(0) - 10, mapper.MapYToScreen(-0.225f) - size.Height / 2);
            g.DrawString(s, Settings.FontDefault, labelBrush, labelPos);
        }
    }
}
