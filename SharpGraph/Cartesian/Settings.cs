namespace SharpGraph.Cartesian
{
    public static class Settings
    {
        /// <summary>
        /// Background color of the graph area.
        /// </summary>
        public static Color BgColor { get; set; }

        /// <summary>
        /// Color used for major grid lines.
        /// </summary>
        public static Color MajorGridColor { get; set; }

        /// <summary>
        /// Color used for minor grid lines.
        /// </summary>
        public static Color MinorGridColor { get; set; }

        /// <summary>
        /// Color used for axis lines.
        /// </summary>
        public static Color AxisColor { get; set; }

        /// <summary>
        /// List of colors used for drawing different expression contours.
        /// </summary>
        public static List<Color> ExpressionColors { get; }

        /// <summary>
        /// Controls whether major grid lines are drawn.
        /// </summary>
        public static bool MajorGridLines { get; set; }

        /// <summary>
        /// Controls whether minor grid lines are drawn.
        /// </summary>
        public static bool MinorGridLines { get; set; }

        // Static constructor to initialize default settings
        static Settings()
        {
            BgColor = Color.White;
            MajorGridColor = Color.DarkGray;
            MinorGridColor = Color.LightGray;
            AxisColor = Color.Black;

            ExpressionColors =
            [
                Color.Red,
                Color.Blue,
                Color.Green,
                Color.Orange,
                Color.Purple,
                Color.Brown,
                Color.Magenta
            ];

            MajorGridLines = true;
            MinorGridLines = true;
        }
    }
}