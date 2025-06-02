using System.Drawing.Drawing2D;
using SharpGraph.Expressions;

namespace SharpGraph.Cartesian
{
    /// <summary>
    /// Object for making a PictureBox control a screen for the Graphing calculator.
    /// </summary>
    public class GraphRender(PictureBox pb)
    {
        private readonly PictureBox pbGraph = pb ?? throw new ArgumentNullException(nameof(pb));
        private readonly GraphCoordinate mapper = new();
        private static readonly int step = 1;
        private float unitsPerPixel = 0.25f;

        private readonly List<Bitmap> graphLayers = [];
        private readonly List<ParsedExpression> expressions = [];

        private CancellationTokenSource? refreshCts;
        private const int ResizeDelay = 20;

        /// <summary>
        /// Starts rendering of the Graph.
        /// </summary>
        public void Start()
        {
            pbGraph.Paint += PbGraph_Paint;
            pbGraph.Resize += PbGraph_Resize;
            pbGraph.MouseEnter += PbGraph_MouseEnter;
            pbGraph.PreviewKeyDown += PbGraph_Scroll;
            mapper.UpdateMap(pbGraph.Width, pbGraph.Height);
        }

        /// <summary>
        /// Adds an expression to graph in the graph screen.
        /// </summary>
        public async Task AddExpression(ParsedExpression exp)
        {
            if (exp?.CompiledFunction == null || !exp.IsValid) return;

            expressions.Add(exp);
            await RefreshGraphsAsync();
        }

        /// <summary>
        /// Removes an expression from the graph.
        /// </summary>
        public async Task RemoveExpression(int index)
        {
            if (index >= expressions.Count) return;

            expressions.RemoveAt(index);
            await RefreshGraphsAsync();
        }

        /// <summary>
        /// Modifies an expression from the graph.
        /// </summary>
        /// <param name="index">the expression's index in the graph list.</param>
        /// <param name="newExpr">the new expression that will replace the previous graph.</param>
        public async Task ModifyExpression(int index, ParsedExpression newExpr)
        {
            if (index < 0 || index >= expressions.Count) throw new ArgumentOutOfRangeException(nameof(index), "Index out of range.");

            expressions[index] = newExpr ?? throw new ArgumentNullException(nameof(newExpr));
            await RefreshGraphsAsync();
        }

        private void PbGraph_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Settings.BgColor);

            GraphDraw.Grids(g, mapper);
            GraphDraw.Axes(g, mapper);

            foreach (var bmp in graphLayers)
            {
                g.DrawImageUnscaled(bmp, 0, 0);
            }
        }

        private void PbGraph_MouseEnter(object? sender, EventArgs e)
        {
            pbGraph.Focus();
        }

        private async void PbGraph_Scroll(object? sender, MouseEventArgs e)
        {
            bool bMouseWheelUp = e.Delta >= 0; // e.Delta is scrolling up when Delta > 0
            bool limit = unitsPerPixel == 1f || unitsPerPixel == 0.0125f;
            if (bMouseWheelUp)
            {
                unitsPerPixel = limit ? 0.75f : unitsPerPixel * 2; // add unit per pixels
            }
            else
            {
                unitsPerPixel = limit ? 0.0025f : unitsPerPixel / 2; // subtract unit per pixels
            }
            mapper.UnitsPerPixel = unitsPerPixel;
            await RefreshGraphsAsync();
        }

        private async void PbGraph_Scroll(object? sender, PreviewKeyDownEventArgs e)
        {
            var key = e.KeyCode;
            float fLowerLimit = 0.00625f, fUpperLimit = 0.75f;
            bool upperLimit = unitsPerPixel >= fUpperLimit;
            bool lowerLimit = unitsPerPixel <= fLowerLimit;
            if (key is Keys.OemCloseBrackets)
            {
                unitsPerPixel = upperLimit ? fUpperLimit : unitsPerPixel * 2; // add unit per pixels
            }
            if (key == Keys.OemOpenBrackets)
            {
                unitsPerPixel = lowerLimit ? fLowerLimit : unitsPerPixel / 2; // subtract unit per pixels
            }
            mapper.UnitsPerPixel = unitsPerPixel;
            await RefreshGraphsAsync();
        }

        private async void PbGraph_Resize(object? sender, EventArgs e)
        {
            refreshCts?.Cancel();
            refreshCts = new CancellationTokenSource();
            var token = refreshCts.Token;

            try
            {
                await Task.Delay(ResizeDelay, token);
                if (!token.IsCancellationRequested)
                {
                    await RefreshGraphsAsync();
                }
            } catch (TaskCanceledException) { }
        }

        private async Task RefreshGraphsAsync()
        {
            refreshCts?.Cancel();
            refreshCts = new CancellationTokenSource();

            // Dispose previous bitmaps
            foreach (var bmp in graphLayers)
            {
                bmp.Dispose();
            }
            graphLayers.Clear();
            mapper.UpdateMap(pbGraph.Width, pbGraph.Height);

            var tmpExpr = expressions;
            for (int i = 0; i < tmpExpr.Count; i++)
            {
                var expr = tmpExpr[i];
                if (tmpExpr[i].CompiledFunction == null) continue;

                Bitmap bmp = await GraphDraw.Graph(mapper, expr, step);
                graphLayers.Add(bmp);
            }

            pbGraph.Invalidate();
        }
    }
}