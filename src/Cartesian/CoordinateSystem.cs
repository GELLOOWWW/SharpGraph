namespace SharpGraph.Cartesian
{
    //
    public class CoordinateSystem
    {
        public float xMin, xMax, yMin, yMax;
        private int width, height;

        /// <summary>
        /// Initialize a new Coordinate System object.
        /// </summary>
        /// <param name="padding"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void InitCoordinateSystem(int width, int height)
        {
            this.width = width;
            this.height = height;

            CalculateBounds();
        }

        //idk
        public void SetBounds(float xMinNew, float xMaxNew, float yMinNew, float yMaxNew)
        {
            this.xMin = xMinNew;
            this.xMax = xMaxNew;
            this.yMin = yMinNew;
            this.yMax = yMaxNew;
        }

        /// <summary>
        /// Calculate Minimum and Maximum X and Y Cartesian Coordinates.
        /// </summary>
        public void CalculateBounds()
        {
            float unitsPerPixel = 0.03f;

            xMin = -((width - 2) * unitsPerPixel) / 2;
            xMax = -xMin; // Symmetric around 0
            yMin = -((height - 2) * unitsPerPixel) / 2;
            yMax = -yMin; // Symmetric around 0
        }

        /// <summary>
        /// Calculate Minimum and Maximum X and Y Cartesian Coordinates.
        /// </summary>
        /// <param name="unitsPerPixel"></param>
        public void CalculateBounds(float unitsPerPixel)
        {
            xMin = -((width - 2) * unitsPerPixel) / 2;
            xMax = -xMin; // Symmetric around 0
            yMin = -((height - 2) * unitsPerPixel) / 2;
            yMax = -yMin; // Symmetric around 0
        }

        public float GridSpacingX()
        {
            return CalcGridSpacing(xMin, xMax, width - 2);
        }
        public float GridSpacingY()
        {
            return CalcGridSpacing(yMin, yMax, height - 2);
        }

        public void UpdateDimensions(int w, int h)
        {
            this.width = w;
            this.height = h;
        }

        public int MapXToScreen(float x)
            => (int)((x - xMin) / (xMax - xMin) * (width - 2));

        public int MapYToScreen(float y)
            => (int)((yMax - y) / (yMax - yMin) * (height - 2));

        public double MapScreenToX(int px)
            => xMin + px * (xMax - xMin) / (width - 2);

        public double MapScreenToY(int py)
            => yMax - py * (yMax - yMin) / (height - 2);

        private static float CalcGridSpacing(float min, float max, int screenSize, int targetPixelsPerGrid = 40)
        {
            float range = max - min;
            float approxSpacing = range / (screenSize / targetPixelsPerGrid);

            // Calculate a "nice" number (1, 2, 5, 10, 20, etc.) closest to approxSpacing
            float magnitude = (float)Math.Pow(10, Math.Floor(Math.Log10(approxSpacing)));
            float residual = approxSpacing / magnitude;

            float niceResidual;
            if (residual < 1.5)
                niceResidual = 1;
            else if (residual < 3)
                niceResidual = 2;
            else if (residual < 7)
                niceResidual = 5;
            else
                niceResidual = 10;

            return niceResidual * magnitude;
        }
    }
}