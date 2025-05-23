using SharpGraph.Cartesian;
using SharpGraph.Expressions;

namespace SharpGraph
{
    public class InitSharpGraph(PictureBox pb)
    {
        private readonly PictureBox pbScreen = pb;
        private GraphRender graph = new(pb);

        /// <summary>
        /// starts the Graphing screen.
        /// </summary>
        /// <param name="pb"></param>
        public void StartScreen()
        {
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
                    await graph.AddExpression(parsed);
                }
            };

            input.ExpressionRemoved += expression =>
            {
                graph.RemoveExpression();
            };
        }
    }
}
