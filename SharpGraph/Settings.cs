namespace SharpGraph
{
    public static class Settings
    {
        public static Color FgColor { get; set; }
        public static Color ControlColor { get; set; }
        public static Color ErrorColor { get; set; }

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

        public static Font FontDefault { get; set; }
        public static Font ExpressionFont {  get; set; }

        // Static constructor to initialize default settings
        static Settings()
        {
            BgColor = Color.White;
            FgColor = Color.Black;

            ErrorColor = Color.MistyRose;

            MajorGridColor = Color.DarkGray;
            MinorGridColor = Color.LightGray;
            AxisColor = Color.Black;

            ExpressionColors =
            [
                Color.Red,
                Color.Green,
                Color.Blue,
                Color.Purple,
                Color.Orange,
                Color.Yellow,
                Color.Black,
            ];

            MajorGridLines = true;
            MinorGridLines = true;

            FontDefault = new("Arial", 8);
            ExpressionFont = new("Segui UI", 12);
            ControlColor = Color.DarkGray;
        }
        /// <summary>
        /// Scales a base value according to current system DPI.
        /// </summary>
        public static int Scale(float baseValue)
        {
            using var g = Graphics.FromHwnd(IntPtr.Zero);
            return (int)(baseValue * g.DpiX / 96f);
        }
    }
}