using SharpGraph.Cartesian;
using SharpGraph.Expressions;
using SharpGraph.UI;

namespace SharpGraph
{
    public class InitSharpGraph
    {
        public static void Start(Panel pnl, PictureBox pb)
        {
            // Initialize GraphRender
            using var graphRender = new GraphRender(pb);
            graphRender.Start();

            // Initialize InputPanel
            var input = new InputPanel(pnl, limit: 10);

            input.ExpressionSubmitted += async (expression, color) =>
            {
                var parseResult = await ExpressionParser.TryParseAsync(expression, color);
                await graphRender.AddExpression(parseResult);
            };

            input.ExpressionRemoved += async (index) => 
                await graphRender.RemoveExpression(index);

            input.ExpressionModified += async (i, newExpr) =>
                await graphRender.ModifyExpression(i, newExpr);
        }
    }
}
