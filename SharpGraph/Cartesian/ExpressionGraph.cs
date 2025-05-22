namespace SharpGraph.Cartesian
{
    /// <summary>
    /// Encapsulates a scalar field expression and its contour color.
    /// </summary>
    /// <remarks>
    /// Creates an expression graph with the function and the color for contours.
    /// </remarks>
    /// <param name="func">Scalar field function</param>
    /// <param name="color">Color for contours</param>
    /// <param name="penWidth">Pen width, optional</param>
    public class ExpressionGraph(Func<double, double, double> func, Color color, float penWidth = 2f)
    {
        public Func<double, double, double> ExpressionFunc { get; } = func ?? throw new ArgumentNullException(nameof(func));
        public Pen ExpressionPen { get; } = new(color, penWidth);

        /// <summary>
        /// Draw this expression's contours on the given graphics surface.
        /// </summary>
        /// <param name="graphics">Graphics object</param>
        /// <param name="mapper">Coordinate mapper</param>
        /// <param name="step">Grid step size</param>
        public void DrawContours(Graphics graphics, Coordinate mapper, int step = 5)
        {
            Draw.Graph(graphics, mapper, ExpressionFunc, ExpressionPen, step);
        }
    }
}