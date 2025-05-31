namespace SharpGraph.Cartesian
{
    public class GraphCoordinate
    {
        public float xMin, xMax, yMin, yMax;
        public int ScreenWidth { get; private set; }
        public int ScreenHeight { get; private set; }

        /// <summary>
        /// Initialize a new Coordinate System object.
        /// </summary>
        public void UpdateMap(int width, int height)
        {
            this.ScreenWidth = width;
            this.ScreenHeight = height;

            CalculateBounds();
        }

        /// <summary>
        /// Maps x-coordinate to pixel.
        /// </summary>
        public int MapXToScreen(float x)
            => (int)((x - xMin) / (xMax - xMin) * (ScreenWidth - 2));

        /// <summary>
        /// Maps y-coordinate to pixel.
        /// </summary>
        public int MapYToScreen(float y)
            => (int)((yMax - y) / (yMax - yMin) * (ScreenHeight - 2));

        /// <summary>
        /// Maps screen x-coordinate to coordinate system.
        /// </summary>
        public double MapScreenToX(int px)
            => xMin + px * (xMax - xMin) / (ScreenWidth - 2);

        /// <summary>
        /// maps screen y-coordinate to coordinate system.
        /// </summary>
        public double MapScreenToY(int py)
            => yMax - py * (yMax - yMin) / (ScreenHeight - 2);

        /// <summary>
        /// Calculate Minimum and Maximum X and Y Cartesian Coordinates.
        /// </summary>
        public void CalculateBounds(float unitsPerPixel = 0.025f)
        {
            xMin = -((ScreenWidth - 2) * unitsPerPixel) / 2;
            xMax = -xMin; // Symmetric around 0
            yMin = -((ScreenHeight - 2) * unitsPerPixel) / 2;
            yMax = -yMin; // Symmetric around 0
        }

        public float GridSpacingX(int targetPixelsPerGrid = 40)
        {
            return CalcGridSpacing(xMin, xMax, ScreenWidth - 2, targetPixelsPerGrid);
        }
        public float GridSpacingY(int targetPixelsPerGrid = 40)
        {
            return CalcGridSpacing(yMin, yMax, ScreenHeight - 2, targetPixelsPerGrid);
        }
        public static float CalcGridSpacing(float min, float max, int screenSize, int targetPixelsPerGrid = 40)
        {
            float range = max - min;
            float approxSpacing = range / (screenSize / targetPixelsPerGrid);

            // Calculate a "nice" number (1, 2, 5, 10, 20, etc.) closest to approxSpacing
            float magnitude = (float)Math.Pow(10, Math.Floor(Math.Log10(approxSpacing)));
            float residual = approxSpacing / magnitude;

            float niceResidual;
            if (residual < 1.5)     niceResidual = 1;
            else if (residual < 3)  niceResidual = 2;
            else if (residual < 7)  niceResidual = 5;
            else                    niceResidual = 10;

            return niceResidual * magnitude;
        }
    }
}