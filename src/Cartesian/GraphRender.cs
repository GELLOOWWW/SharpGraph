using System.Drawing.Drawing2D;

#nullable enable

namespace SharpGraph.Cartesian
{
    public class GraphRender
    {
        private readonly PictureBox pbGraph;
        private CoordinateSystem mapper = new();

        // user definables
        bool bMinorGrids = true;
        bool bAxisNums = true;
        public Color bgColor = Color.White;
        public Color axisColor = Color.Black;
        public Color gridColor = Color.LightGray;
        public List<Color> graphColors = [];

        private const float axisThickness = 2f;

        // private List<exp> expressions;

        /// <summary>
        /// GraphRender constructor
        /// </summary>
        /// <param name="pb"></param>
        public GraphRender(PictureBox pb)
        {
            pbGraph = pb ?? throw new ArgumentNullException(nameof(pb));

            pbGraph.Paint += PbGraph_Paint;
            pbGraph.Resize += PbGraph_Resize;

            mapper.InitCoordinateSystem(pbGraph.Width, pbGraph.Height);
        }

        private void PbGraph_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(bgColor);


            DrawGrid(g);
            DrawAxis(g);
            DrawExpression(g);
        }

        private void PbGraph_Resize(object? sender, EventArgs e)
        {
            mapper.UpdateDimensions(pbGraph.Width, pbGraph.Height);
            mapper.CalculateBounds();
            pbGraph.Invalidate();
        }

        double func(double x, double y) => Math.Sin(x) * Math.Cos(y);

        /// <summary>
        /// Linearly interpolates between two points (p1 and p2) where the scalar values (val1 and val2)
        /// are known, to estimate the zero crossing point.
        /// </summary>
        /// <param name="p1">First point</param>
        /// <param name="p2">Second point</param>
        /// <param name="val1">Function value at p1</param>
        /// <param name="val2">Function value at p2</param>
        /// <returns>Interpolated point along the edge where the function crosses zero</returns>
        private static PointF Interpolate(PointF p1, PointF p2, double val1, double val2)
        {
            float t = (float)(val1 / (val1 - val2));
            return new PointF(p1.X + t * (p2.X - p1.X), p1.Y + t * (p2.Y - p1.Y));
        }

        /// <summary>
        /// Draws contours of the scalar field function using a grid-based marching squares algorithm.
        /// </summary>
        /// <param name="g">Graphics object to draw on</param>
        private void DrawExpression(Graphics g)
        {
            const int step = 4; // Grid cell size in pixels

            int drawWidth = pbGraph.Width - 2;
            int drawHeight = pbGraph.Height - 2;

            using Pen pen = new(Color.Blue, 2f);

            // Iterate over each cell in the grid
            for (int i = 0; i < drawWidth; i += step)
            {
                for (int j = 0; j < drawHeight; j += step)
                {
                    // Define the four corners of the current grid cell
                    PointF p0 = new(0 + i, j);             // Top-left
                    PointF p1 = new(0 + i + step, j);     // Top-right
                    PointF p2 = new(0 + i + step, j + step); // Bottom-right
                    PointF p3 = new(0 + i, j + step);     // Bottom-left

                    // Evaluate the scalar field at each corner
                    double v0 = func(mapper.MapScreenToX((int)p0.X), mapper.MapScreenToY((int)p0.Y));
                    double v1 = func(mapper.MapScreenToX((int)p1.X), mapper.MapScreenToY((int)p1.Y));
                    double v2 = func(mapper.MapScreenToX((int)p2.X), mapper.MapScreenToY((int)p2.Y));
                    double v3 = func(mapper.MapScreenToX((int)p3.X), mapper.MapScreenToY((int)p3.Y));

                    // Determine the state of the cell (bitmask based on sign of each corner)
                    int state = 0;
                    if (v0 < 0) state |= 1;
                    if (v1 < 0) state |= 2;
                    if (v2 < 0) state |= 4;
                    if (v3 < 0) state |= 8;

                    // Handle each case of the marching squares configuration
                    switch (state)
                    {
                        case 0:
                        case 15:
                            // All corners same sign → no contour
                            break;

                        case 1:
                        case 14:
                            g.DrawLine(pen,
                                Interpolate(p0, p1, v0, v1),
                                Interpolate(p0, p3, v0, v3));
                            break;

                        case 2:
                        case 13:
                            g.DrawLine(pen,
                                Interpolate(p0, p1, v0, v1),
                                Interpolate(p1, p2, v1, v2));
                            break;

                        case 3:
                        case 12:
                            g.DrawLine(pen,
                                Interpolate(p1, p2, v1, v2),
                                Interpolate(p0, p3, v0, v3));
                            break;

                        case 4:
                        case 11:
                            g.DrawLine(pen,
                                Interpolate(p1, p2, v1, v2),
                                Interpolate(p2, p3, v2, v3));
                            break;

                        case 5:
                            // Ambiguous case: two contours across the square
                            g.DrawLine(pen,
                                Interpolate(p0, p1, v0, v1),
                                Interpolate(p0, p3, v0, v3));
                            g.DrawLine(pen,
                                Interpolate(p1, p2, v1, v2),
                                Interpolate(p2, p3, v2, v3));
                            break;

                        case 6:
                        case 9:
                            g.DrawLine(pen,
                                Interpolate(p0, p1, v0, v1),
                                Interpolate(p2, p3, v2, v3));
                            break;

                        case 7:
                        case 8:
                            g.DrawLine(pen,
                                Interpolate(p0, p3, v0, v3),
                                Interpolate(p2, p3, v2, v3));
                            break;

                        case 10:
                            g.DrawLine(pen,
                                Interpolate(p0, p3, v0, v3),
                                Interpolate(p1, p2, v1, v2));
                            break;
                    }
                }
            }
        }

        private void DrawGrid(Graphics g)
        {
            int h = pbGraph.Height;
            int w = pbGraph.Width;

            using Pen gridPen = new(gridColor, 1) { DashStyle = DashStyle.Dash };
            using Brush labelBrush = new SolidBrush(Color.Gray);
            using Font labelFont = new("Arial", 8);

            float gridSpacingX = mapper.GridSpacingX();
            float gridSpacingY = mapper.GridSpacingY();

            // Draw vertical grid lines and X-axis labels
            for (float x = 0; x <= mapper.xMax; x += gridSpacingX)
            {
                foreach (float xVal in new[] { x, -x })
                {
                    int px = mapper.MapXToScreen(xVal);
                    g.DrawLine(gridPen, px, 0, px, h);

                    if (Math.Abs(xVal) > 1e-5) // Avoid drawing "0" twice at the origin
                    {
                        string label = xVal.ToString("0.##");
                        SizeF labelSize = g.MeasureString(label, labelFont);
                        g.DrawString(label, labelFont, labelBrush, px - labelSize.Width / 2, mapper.MapYToScreen(0) + 2);
                    }
                }
            }

            // Draw horizontal grid lines and Y-axis labels
            for (float y = 0; y <= mapper.yMax; y += gridSpacingY)
            {
                foreach (float yVal in new[] { y, -y })
                {
                    int py = mapper.MapYToScreen(yVal);
                    g.DrawLine(gridPen, 0, py, w, py);

                    if (Math.Abs(yVal) > 1e-5)
                    {
                        string label = yVal.ToString("0.##");
                        SizeF labelSize = g.MeasureString(label, labelFont);
                        g.DrawString(label, labelFont, labelBrush, mapper.MapXToScreen(0) + 2, py - labelSize.Height / 2);
                    }
                }
            }
        }

        private void DrawAxis(Graphics g)
        {
            using Pen axisPen = new(axisColor, axisThickness);
            // X-axis
            int yZero = mapper.MapYToScreen(0);
            g.DrawLine(axisPen,
                new PointF(mapper.MapXToScreen(mapper.xMin), yZero),
                new PointF(mapper.MapXToScreen(mapper.xMax), yZero));

            // Y-axis
            int xZero = mapper.MapXToScreen(0);
            g.DrawLine(axisPen,
                new PointF(xZero, mapper.MapYToScreen(mapper.yMin)),
                new PointF(xZero, mapper.MapYToScreen(mapper.yMax)));
        }
    }
}