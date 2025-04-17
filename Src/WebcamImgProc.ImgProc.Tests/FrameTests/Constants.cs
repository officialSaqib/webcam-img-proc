using System.Drawing;

namespace WebcamImgProc.ImgProc.Tests.FrameTests
{
    /// <summary>
    /// Generic constants/readonlys to be used by any frame unit-tests.
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// A blank 10x10 bitmap, use it as a placeholder.
        /// </summary>
        public static readonly Bitmap BLANK_BITMAP = new(10, 10);
    }
}
