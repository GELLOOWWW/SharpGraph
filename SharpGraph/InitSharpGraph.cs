using SharpGraph.Cartesian;
using SharpGraph.Expressions;

namespace SharpGraph
{
    public class InitSharpGraph
    {
        private PictureBox pbScreen;
        private GraphRender graph;
        /// <summary>
        /// starts the Graphing screen.
        /// </summary>
        /// <param name="pb"></param>
        public void StartScreen(PictureBox pb)
        {
            this.pbScreen = pb;
            this.graph = new GraphRender(pbScreen);
            graph.Start();
        }
        /// <summary>
        /// Starts the panel for expressions input.
        /// </summary>
        /// <param name="pnl"></param>
        public void StartPanel(Panel pnl)
        {
            UI.InputPanel input = new(pnl);
            input.ExpressionSubmitted += async expression =>
            {
                var parsed = await ExpressionParser.TryParseAsync(expression);
                if (parsed.IsValid)
                {
                    graph.AddExpression(parsed);
                }
            };

            input.ExpressionRemoved += expression =>
            {
                graph.RemoveExpression();
            };
        }
    }
}
