using System.Drawing.Drawing2D;

namespace SharpGraph.Cartesian
{
    public static class Draw
    {
        /// <summary>
        /// Draw the contours for one scalar field expression function.
        /// </summary>
        /// <param name="graphics">Graphics to draw on</param>
        /// <param name="mapper">Coordinate mapper</param>
        /// <param name="expressionFunc">Function defining scalar field</param>
        /// <param name="pen">Pen to use for drawing contours</param>
        /// <param name="step">Grid step size for marching squares</param>
        public static void Graph(Graphics graphics, Coordinate mapper,
            Func<double, double, double> expressionFunc, Pen pen, int step = 5)
        {
            ArgumentNullException.ThrowIfNull(graphics);
            ArgumentNullException.ThrowIfNull(mapper);
            ArgumentNullException.ThrowIfNull(expressionFunc);
            ArgumentNullException.ThrowIfNull(pen);
            if (step <= 0) throw new ArgumentOutOfRangeException(nameof(step), "Step must be positive");

            int width = mapper.ScreenWidth;
            int height = mapper.ScreenHeight;

            double EvaluateExpression(int screenX, int screenY)
            {
                double x = mapper.MapScreenToX(screenX);
                double y = mapper.MapScreenToY(screenY);
                return expressionFunc(x, y);
            }

            // Linear interpolation between points for contour crossing
            PointF Interpolate(PointF p1, PointF p2, double val1, double val2)
            {
                float t = (float)(val1 / (val1 - val2));
                return new PointF(
                    p1.X + t * (p2.X - p1.X),
                    p1.Y + t * (p2.Y - p1.Y)
                );
            }

            for (int ix = 0; ix < width; ix += step)
            {
                for (int iy = 0; iy < height; iy += step)
                {
                    PointF p0 = new(ix, iy);
                    PointF p1 = new(ix + step, iy);
                    PointF p2 = new(ix + step, iy + step);
                    PointF p3 = new(ix, iy + step);

                    double v0 = EvaluateExpression((int)p0.X, (int)p0.Y);
                    double v1 = EvaluateExpression((int)p1.X, (int)p1.Y);
                    double v2 = EvaluateExpression((int)p2.X, (int)p2.Y);
                    double v3 = EvaluateExpression((int)p3.X, (int)p3.Y);

                    int state = 0;
                    if (v0 < 0) state |= 1;
                    if (v1 < 0) state |= 2;
                    if (v2 < 0) state |= 4;
                    if (v3 < 0) state |= 8;

                    switch (state)
                    {
                        case 0:
                        case 15:
                            break;

                        case 1:
                        case 14:
                            graphics.DrawLine(pen, Interpolate(p0, p1, v0, v1), Interpolate(p0, p3, v0, v3));
                            break;

                        case 2:
                        case 13:
                            graphics.DrawLine(pen, Interpolate(p0, p1, v0, v1), Interpolate(p1, p2, v1, v2));
                            break;

                        case 3:
                        case 12:
                            graphics.DrawLine(pen, Interpolate(p1, p2, v1, v2), Interpolate(p0, p3, v0, v3));
                            break;

                        case 4:
                        case 11:
                            graphics.DrawLine(pen, Interpolate(p1, p2, v1, v2), Interpolate(p2, p3, v2, v3));
                            break;

                        case 5:
                            graphics.DrawLine(pen, Interpolate(p0, p1, v0, v1), Interpolate(p0, p3, v0, v3));
                            graphics.DrawLine(pen, Interpolate(p1, p2, v1, v2), Interpolate(p2, p3, v2, v3));
                            break;

                        case 6:
                        case 9:
                            graphics.DrawLine(pen, Interpolate(p0, p1, v0, v1), Interpolate(p2, p3, v2, v3));
                            break;

                        case 7:
                        case 8:
                            graphics.DrawLine(pen, Interpolate(p0, p3, v0, v3), Interpolate(p2, p3, v2, v3));
                            break;

                        case 10:
                            graphics.DrawLine(pen, Interpolate(p0, p3, v0, v3), Interpolate(p1, p2, v1, v2));
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Draw multiple expression graphs.
        /// </summary>
        /// <param name="graphics">Graphics object</param>
        /// <param name="mapper">Coordinate mapper</param>
        /// <param name="expressions">Enumerable of expression graphs</param>
        /// <param name="step">Grid step size</param>
        public static void Graphs(Graphics graphics, Coordinate mapper,
            IEnumerable<ExpressionGraph> expressions, int step = 5)
        {
            ArgumentNullException.ThrowIfNull(graphics);
            ArgumentNullException.ThrowIfNull(mapper);
            ArgumentNullException.ThrowIfNull(expressions);

            foreach (var exprGraph in expressions)
            {
                exprGraph.DrawContours(graphics, mapper, step);
            }
        }

        public static void Grid(Graphics graphics, Coordinate mapper)
        {
            ArgumentNullException.ThrowIfNull(graphics);
            ArgumentNullException.ThrowIfNull(mapper);

            int width = mapper.ScreenWidth;
            int height = mapper.ScreenHeight;

            Color gridColor = Color.LightGray;

            using Pen gridPen = new(gridColor, 1) { DashStyle = DashStyle.Dash };
            using Brush labelBrush = new SolidBrush(Color.Black);
            using Font labelFont = new("Arial", 8);

            float gridSpacingX = mapper.GridSpacingX();
            float gridSpacingY = mapper.GridSpacingY();

            // Draw vertical grid lines with labels
            for (float x = 0; x <= mapper.xMax; x += gridSpacingX)
            {
                foreach (float val in new[] { x, -x })
                {
                    int px = mapper.MapXToScreen(val);
                    graphics.DrawLine(gridPen, px, 0, px, height);

                    if (Math.Abs(val) > 1e-5)  // Avoid double labeling the origin
                    {
                        string label = val.ToString("0.##");
                        SizeF size = graphics.MeasureString(label, labelFont);
                        graphics.DrawString(label, labelFont, labelBrush, px - size.Width / 2, mapper.MapYToScreen(0) + 2);
                    }
                }
            }

            // Draw horizontal grid lines with labels
            for (float y = 0; y <= mapper.yMax; y += gridSpacingY)
            {
                foreach (float val in new[] { y, -y })
                {
                    int py = mapper.MapYToScreen(val);
                    graphics.DrawLine(gridPen, 0, py, width, py);

                    if (Math.Abs(val) > 1e-5)
                    {
                        string label = val.ToString("0.##");
                        SizeF size = graphics.MeasureString(label, labelFont);
                        graphics.DrawString(label, labelFont, labelBrush, mapper.MapXToScreen(0) + 2, py - size.Height / 2);
                    }
                }
            }
        }

        public static void Axis(Graphics graphics, Coordinate mapper)
        {
            ArgumentNullException.ThrowIfNull(graphics);
            ArgumentNullException.ThrowIfNull(mapper);

            Color axisColor = Color.Black;
            float axisThickness = 2f;

            using Pen axisPen = new(axisColor, axisThickness);

            // Draw X-axis
            int yZero = mapper.MapYToScreen(0);
            graphics.DrawLine(axisPen,
                new PointF(mapper.MapXToScreen(mapper.xMin), yZero),
                new PointF(mapper.MapXToScreen(mapper.xMax), yZero));

            // Draw Y-axis
            int xZero = mapper.MapXToScreen(0);
            graphics.DrawLine(axisPen,
                new PointF(xZero, mapper.MapYToScreen(mapper.yMin)),
                new PointF(xZero, mapper.MapYToScreen(mapper.yMax)));
        }
    }
}