namespace SharpGraph
{
    public static class Settings
    {
        /// <summary>
        /// Background color of the graph area.
        /// </summary>
        public static Color BgColor { get; set; }

        /// <summary>
        /// General Foreground color.
        /// </summary>
        public static Color FgColor { get; set; }

        /// <summary>
        /// Background color for the UI elements.
        /// </summary>
        public static Color UIColor { get; set; }
        /// <summary>
        /// Backcolor of the Text Boxes in the UI.
        /// </summary>
        public static Color TextBoxColor { get; set; }
        /// <summary>
        /// Backcolor of the Text Boxes of the Expression boxes.
        /// </summary>
        public static Color AddExprColor { get; set; }
        /// <summary>
        /// Backcolor of the Add Expression Color.
        /// </summary>
        public static Color RemoveExprColor { get; set; }

        /// <summary>
        /// Color for error indicators.
        /// </summary>
        public static Color ErrorColor { get; set; }

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
        /// Color of the grid numbering in the cartesian plane.
        /// </summary>
        public static Color GridNumColor { get; set; }

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

            UIColor = Color.LightGray;
            TextBoxColor = Color.White;
            AddExprColor = Color.DimGray;
            RemoveExprColor = Color.DimGray;

            MajorGridColor = Color.DarkGray;
            MinorGridColor = Color.LightGray;
            AxisColor = Color.Black;
            GridNumColor = Color.Black;

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
            ExpressionFont = new("Segoe UI", 12);
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