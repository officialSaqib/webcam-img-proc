using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace WebcamImgProc.UI
{
    /// <summary>
    /// Generic helpers that the WPF form can use.
    /// </summary>
    internal class Helpers
    {
        /// <summary>
        /// Takes a bitmap and converts it to an image that can be handled by
        /// WPF ImageBrush.
        /// </summary>
        /// <remarks>
        /// Source: https://stackoverflow.com/a/34590774
        /// </remarks>
        /// <param name="src">A bitmap image</param>
        /// <returns>The image as a BitmapImage for WPF.</returns>
        public static BitmapImage Convert(Bitmap src)
        {
            var ms = new MemoryStream();
            ((Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            var image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }
    }
}
