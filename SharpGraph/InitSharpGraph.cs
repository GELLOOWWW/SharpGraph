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
            input.ExpressionSubmitted += OnExpressionSubmit;
            input.ExpressionRemoved += OnExpressionRemove;
            input.ExpressionModified += OnExpressionModify;

            async Task OnExpressionSubmit(string expression, Color color)
            {
                var parseResult = await ExpressionParser.TryParseAsync(expression, color);
                await graphRender.AddExpression(parseResult);
            }
           async void OnExpressionRemove(int index)
            {
                await graphRender.RemoveExpression(index);
            }
            async void OnExpressionModify(int i, ParsedExpression newExpr)
            {
                await graphRender.ModifyExpression(i, newExpr);
            }
        }
    }
}
