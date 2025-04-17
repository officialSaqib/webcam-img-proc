using System.Drawing;

namespace WebcamImgProc.ImgProc.Tests.FrameTests
{
    /// <summary>
    /// Generic helpers to be used by any frame unit-tests.
    /// </summary>
    internal static class Helpers
    {
        /// <summary>
        /// Checks if two bitmaps are equal.
        /// </summary>
        /// <param name="one">First bitmap to check.</param>
        /// <param name="two">Second bitmap to check based on first.</param>
        /// <returns>True if equal, false otherwise.</returns>
        public static bool BitmapEquality(Bitmap one, Bitmap two)
        {
            // Some base-cases that can tell us immediately the equality
            if (one == two)
                return true;
            if (one == null || two == null)
                return false;
            if (one.Size != two.Size || one.PixelFormat != two.PixelFormat)
                return false;

            // Checking pixel-by-pixel for equality
            for (int y = 0; y < one.Height; y++)
            {
                for (int x = 0; x < one.Width; x++)
                {
                    if (one.GetPixel(x, y) != two.GetPixel(x, y))
                        return false;
                }
            }

            return true;
        }
    }
}
