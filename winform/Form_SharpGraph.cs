using System.Drawing.Drawing2D;

namespace SharpGraph
{
    public partial class FormSharpGraph : Form
    {
        private CartesianPlane mapper;
        public FormSharpGraph()
        {
            InitializeComponent();
            InitializeMapper();
            Text = "SharpGraph";

            GraphScreen.BorderStyle = BorderStyle.FixedSingle;
        }

        // TODO: Code Refactoring
        // develop subroutines for all of these
        // to keep all Form*.cs clean
        private void InitializeMapper()
        {
            float MinX = -20, MinY = -20, MaxX = 20, MaxY = 20;
            int pixelWidth = GraphScreen.Width;
            int pixelHeight = GraphScreen.Height;

            mapper = new(MinX, MinY, MaxX, MaxY, pixelWidth, pixelHeight);
        }

        private void GraphScreen_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Pen axisPen = new(Color.LightGray, 1);
            Pen graphPen = new(Color.Red, 3);

            float minY = mapper.MinY;
            float minX = mapper.MinY;
            float maxX = mapper.MaxX;
            float maxY = mapper.MaxY;
            float space = 2.0f;

            for (float x = minX; x < maxX; x+=space)
            {
                PointF p1 = mapper.CartesianToPixel(x, minY);
                PointF p2 = mapper.CartesianToPixel(x, maxY);
                g.DrawLine(axisPen, p1, p2);
            } 
            for (float y = minY;  y < maxY; y+=space)
            {
                PointF p1 = mapper.CartesianToPixel(minX, y);
                PointF p2 = mapper.CartesianToPixel(maxX, y);
                g.DrawLine(axisPen, p1, p2);
            }
            axisPen = new(Color.Black, 3);

            // Draw X Axis
            PointF axisX1 = mapper.CartesianToPixel(minX, 0);
            PointF axisX2 = mapper.CartesianToPixel(maxX, 0);
            g.DrawLine(axisPen, axisX1, axisX2);

            // Draw Y Axis
            PointF axisY1 = mapper.CartesianToPixel(0 , minY);
            PointF axisY2 = mapper.CartesianToPixel(0 , maxY);
            g.DrawLine(axisPen, axisY1, axisY2);
            axisPen.Dispose();

            // Render equation
            Brush graphBrush = Brushes.Blue;
            GraphicsPath path = new();
            for (int px = 0; px < GraphScreen.Width; px++)
            {
                for (int py = 0; py < GraphScreen.Height; py++)
                {
                    PointF cartesianPoint = mapper.PixelToCartesian(px, py);
                    float x = cartesianPoint.X;
                    float y = cartesianPoint.Y;
                    // Evaluate the equation (example: x^2 - y = 0)
                    if (Math.Abs(x*x + y*y - 100) < 1f) // Replace with your equation
                    {
                        // Add point to path
                        path.AddRectangle(new Rectangle(px, py, 1, 1));
                    }
                    if (Math.Abs(Math.Sin(x)-y) < 0.1f)
                    {
                        // Add point to path
                        path.AddRectangle(new Rectangle(px, py, 1, 1));
                    }
                }
            }
            // Draw the path
            g.FillPath(graphBrush, path);
            path.Dispose();
        }

        private void GraphScreen_MouseMove(object sender, MouseEventArgs e)
        {
            int X = e.Location.X;
            int Y = e.Location.Y;

            PointF c_point = mapper.PixelToCartesian((float)X, (float)Y);
            float nX = c_point.X;
            float nY = c_point.Y;

            lblCoords.Text = $"{X}, {Y}; {nX:F3}, {nY:F3}";
        }
    }
}