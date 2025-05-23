using System.Drawing.Drawing2D;
using SharpGraph.Expressions;

namespace SharpGraph.Cartesian
{
    public class GraphRender(PictureBox pb)
    {
        private readonly PictureBox pbGraph = pb;
        private readonly GraphCoordinate mapper = new();
        private static readonly int step = 1;

        // Store rendered layers
        private readonly List<Bitmap> graphLayers = [];
        public Stack<ParsedExpression> Expressions = [];

        public void Start()
        {
            pbGraph.Paint += PbGraph_Paint;
            pbGraph.Resize += PbGraph_Resize;
            mapper.UpdateMap(pbGraph.Width, pbGraph.Height);
        }

        public async Task AddExpression(ParsedExpression exp)
        {
            if (exp.CompiledFunction is null) return;

            Expressions.Push(exp);
            mapper.UpdateMap(pbGraph.Width, pbGraph.Height);

            var bmp = await GraphDraw.GraphAsync(mapper, exp, step);
            graphLayers.Add(bmp);

            Refresh();
        }
        public void RemoveExpression()
        {
            Expressions.Pop();
            Refresh();
        }

        /**********************************************************

        **********************************************************/

        private void PbGraph_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Settings.BgColor);

            GraphDraw.Grids(g, mapper);
            GraphDraw.Axes(g, mapper);

            foreach (var bmp in graphLayers)
            {
                g.DrawImageUnscaled(bmp, 0, 0);
            }
        }

        private void PbGraph_Resize(object? sender, EventArgs e)
        {
            Refresh();
        }

        private async void Refresh()
        {
            mapper.UpdateMap(pbGraph.Width, pbGraph.Height);

            // Clear old layers and re-render all expressions
            foreach (var bmp in graphLayers)
                bmp.Dispose();
            graphLayers.Clear();

            foreach (var expr in Expressions)
            {
                if (expr.CompiledFunction is null) continue;
                var bmp = await GraphDraw.GraphAsync(mapper, expr, step);
                graphLayers.Add(bmp);
            }

            pbGraph.Invalidate();
        }
    }
}