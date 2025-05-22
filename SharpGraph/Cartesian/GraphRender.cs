using System.Drawing.Drawing2D;

namespace SharpGraph.Cartesian
{
    /// <summary>
    /// GraphRender constructor
    /// </summary>
    /// <param name="pb"></param>
    public class GraphRender(PictureBox pb)
    {
        private readonly PictureBox pbGraph = pb ?? throw new ArgumentNullException(nameof(pb));
        private Coordinate mapper = new();

        public void Start()
        {
            pbGraph.Paint += PbGraph_Paint;
            pbGraph.Resize += PbGraph_Resize;

            mapper.UpdateMap(pbGraph.Width, pbGraph.Height);
        }

        private void PbGraph_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Settings.BgColor);

            var graphs = new List<ExpressionGraph>
            {
                new((x,y) => Math.Pow(x - 2, 2) + Math.Pow(y - 2, 2) - 25, Color.Red),
                new((x,y) => Math.Sin(x*2)*5 - y, Color.Blue),
            };

            Cartesian.Draw.Grid(g, mapper);
            Cartesian.Draw.Axis(g, mapper);
            Cartesian.Draw.Graphs(g, mapper, graphs, 2);
        }

        private void PbGraph_Resize(object? sender, EventArgs e)
        {
            mapper.UpdateMap(pbGraph.Width, pbGraph.Height);
            pbGraph.Invalidate();
        }
    }
}