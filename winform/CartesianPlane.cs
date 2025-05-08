namespace SharpGraph
{
    /// <summary>
    /// Abstraction for bidirectional mapping between pixel coordinates and
    /// Cartesian Coordinate system.
    /// </summary>
    /// <param name="MinX"></param>
    /// <param name="MinY"></param>
    /// <param name="MaxX"></param>
    /// <param name="MaxY"></param>
    /// <param name="PixelWidth"></param>
    /// <param name="PixelHeight"></param>
    public class CartesianPlane(float MinX, float MinY, float MaxX, float MaxY, int PixelWidth, int PixelHeight)
    {
        /// <summary>
        /// Width of pixel canvas.
        /// </summary>
        public int PixelWidth { get; private set; } = PixelWidth;

        /// <summary>
        /// Height of pixel canvas.
        /// </summary>
        public int PixelHeight { get; private set; } = PixelHeight;

        /// <summary>
        /// Minimum x coord in the cartesian plane.
        /// </summary>
        public float MinX { get; private set; } = MinX;
        /// <summary>
        /// Maximum x coord in the cartesian plane.
        /// </summary>
        public float MaxX { get; private set; } = MaxX;

        /// <summary>
        /// Minimum y coord in the cartesian plane.
        /// </summary>
        public float MinY { get; private set; } = MinY;
        /// <summary>
        /// Maximum y coord in the cartesian plane.
        /// </summary>
        public float MaxY { get; private set; } = MaxY;

        /// <summary>
        /// Converts pixel coordinate and returns a Cartesian coordinate.
        /// </summary>
        /// <param name="px"></param>
        /// <param name="py"></param>
        /// <returns></returns>
        public PointF PixelToCartesian(float px, float py)
        {
            float cx = MinX + px / PixelWidth * (MaxX - MinX);
            float cy = MaxY - py / PixelHeight * (MaxY - MinY);

            return new PointF(cx, cy);
        }
        /// <summary>
        /// Converts Cartesian coordinate and returns a pixel coordinate.
        /// </summary>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <returns></returns>
        public PointF CartesianToPixel(float cx, float cy)
        {
            float px = (float)((cx - MinX) / (MaxX - MinX) * PixelWidth);
            float py = (float)(PixelHeight - (cy - MinY) / (MaxY - MinY) * PixelHeight);

            return new PointF(px, py);
        }
    }
}
