using SharpGraph.Cartesian;
using SharpGraph.Expressions;
using SharpGraph.UI;

namespace SharpGraph
{
    public class InitSharpGraph
    {
        /// <summary>
        /// Starts SharpGraph.
        /// </summary>
        /// <param name="pnl">The panel to be used for the InputPanel.</param>
        /// <param name="pb">The PictureBox control to be used as a screen by GraphRender.</param>
        public static void Start(Panel pnl, PictureBox pb)
        {
            // Initialize GraphRender
            var graphRender = new GraphRender(pb);
            graphRender.Start();

            // Initialize InputPanel
            var input = new InputPanel(pnl, limit: 8);
            InputEvents(input, graphRender);
        }

        /// <summary>
        /// handle expression input events (submit, remove, modify) from InputEvent object.
        /// </summary>
        /// <param name="input">the InputPanel to handle events on.</param>
        /// <param name="graphRender">the GraphRender object to pass arguments from event handling.</param>
        private static void InputEvents(InputPanel input, GraphRender graphRender)
        {
            input.ExpressionSubmitted += async (expression, color) =>
            {
                var parseResult = await ExpressionParser.TryParseAsync(expression, color);
                await graphRender.AddExpression(parseResult);
            };

            input.ExpressionRemoved += async (index) =>
            {
                await graphRender.RemoveExpression(index);
            };

            input.ExpressionModified += async (i, newExpr) =>
            {
                await graphRender.ModifyExpression(i, newExpr);
            };
        }
    }
}
