using Emgu.CV;
using System.Drawing;
using WebcamImgProc.ImgProc.Capture;
using WebcamImgProc.ImgProc.Frame;

/**TEST SPECIFICATIONS
 * 
 * Project: WebcamImgProc.ImgProc
 * File: Frame/ImageFrame.cs
 * Class: ImageFrame
 * 
 * Description: The constructor, method
 *              <see cref="ImageFrame.ApplyFilter(WebcamImgProc.ImgProc.Frame.FilterType)"/>,
 *              and method <see cref="ImageFrame.GenerateGreyscaleHistogram"/>
 *              are tested here. Other methods not mentioned are inadvertantly
 *              tested by other tests.
 *              
 *              These are NOT integration/end-to-end tests. They simply make
 *              sure that the constructor/methods work/fail based on different
 *              conditions. So outputs are not validated for correctness.
 */

namespace WebcamImgProc.ImgProc.Tests.FrameTests
{
    /// <summary>
    /// Tests <see cref="ImageFrame.ImageFrame(Mat)"/> and
    /// <see cref="ImageFrame.ImageFrame(ImageFrame)"/>
    /// </summary>
    [Collection("Sequential")]
    public class ImageFrameConstructorTests
    {
        /// <summary>
        /// Just a basic construction of a image frame from <see cref="Mat"/>.
        /// Should work.
        /// </summary>
        [Fact]
        public void MatTest()
        {
            ImageFrame frame;
            Mat frameMat = new Mat();
            bool returnedEx = false;

            try
            {
                frame = new ImageFrame(frameMat);
            }
            catch
            {
                returnedEx = true;
            }

            Assert.False(returnedEx);
        }

        /// <summary>
        /// Constructing one frame out of another. Should work.
        /// </summary>
        /// <remarks>
        /// Indirectly Tested:
        /// <list type="bullet">
        /// <item><see cref="ImageFrame.GetBitmap"/></item>
        /// </list>
        /// </remarks>
        [Fact]
        public void FrameFromFrameTest()
        {
            ImageFrame frameOne = Webcam.CaptureFrame();
            ImageFrame frameTwo;
            Bitmap bitmapOne = frameOne.GetBitmap();
            Bitmap bitmapTwo = Constants.BLANK_BITMAP;
            bool returnedEx = false;

            try
            {
                frameTwo = new ImageFrame(frameOne);
                bitmapTwo = frameTwo.GetBitmap();
            }
            catch
            {
                returnedEx = true;
            }

            Assert.False(returnedEx);
            Assert.True(Helpers.BitmapEquality(bitmapOne, bitmapTwo));
        }

        /// <summary>
        /// Constructing one frame out of another, but resetting both frames
        /// after initialization. Should work.
        /// </summary>
        /// <remarks>
        /// Indirectly Tested:
        /// <list type="bullet">
        /// <item><see cref="Webcam.CaptureFrame(int)"/></item>
        /// <item><see cref="ImageFrame.GetBitmap"/></item>
        /// <item><see cref="ImageFrame.Reset"/></item>
        /// </list>
        /// </remarks>
        [Fact]
        public void FrameFromFrameResetTest()
        {
            ImageFrame frameOne = Webcam.CaptureFrame();
            ImageFrame frameTwo;
            Bitmap bitmapOne = frameOne.GetBitmap();
            Bitmap bitmapTwo = Constants.BLANK_BITMAP;
            bool returnedEx = false;

            try
            {
                frameOne.Reset();
                frameTwo = new ImageFrame(frameOne);
                frameTwo.Reset();
                bitmapTwo = frameTwo.GetBitmap();
            }
            catch
            {
                returnedEx = true;
            }

            Assert.False(returnedEx);
            Assert.True(Helpers.BitmapEquality(bitmapOne, bitmapTwo));
        }
    }

    /// <summary>
    /// Tests <see cref="ImageFrame.ApplyFilter(FilterType)"/> and its
    /// overloads.
    /// </summary>
    /// <remarks>
    /// Only tests that the methods are valid in terms of being operational,
    /// not that the results are good (integration tests for this).
    /// </remarks>
    [Collection("Sequential")]
    public class ImageFrameApplyFilterTests
    {
        /// <summary>
        /// Applying one filter after another, resetting, then doing it again.
        /// Should work.
        /// </summary>
        /// <remarks>
        /// Indirectly Tested:
        /// <list type="bullet">
        /// <item><see cref="Webcam.CaptureFrame(int)"/></item>
        /// <item><see cref="ImageFrame.Reset"/></item>
        /// </list>
        /// </remarks>
        [Fact]
        public void AllFiltersTest()
        {
            ImageFrame frameOne = Webcam.CaptureFrame();
            bool returnedEx = false;

            try
            {
                for (int i = 0; i < 2; i++)
                {
                    frameOne.ApplyFilter(FilterType.NONE);
                    frameOne.ApplyFilter(FilterType.GREYSCALE);
                    frameOne.ApplyFilter(FilterType.GAUSSIAN_BLUR);
                    frameOne.ApplyFilter(FilterType.CANNY_EDGE_DETECTION);

                    frameOne.Reset();
                }
            }
            catch
            {
                returnedEx = true;
            }

            Assert.False(returnedEx);
        }

        /// <summary>
        /// Applying one filter after another (with settings), resetting, then
        /// doing it again. Should work.
        /// </summary>
        /// <remarks>
        /// Indirectly Tested:
        /// <list type="bullet">
        /// <item><see cref="Webcam.CaptureFrame(int)"/></item>
        /// <item><see cref="ImageFrame.Reset"/></item>
        /// </list>
        /// </remarks>
        [Fact]
        public void AllFiltersSettingsTest()
        {
            ImageFrame frameOne = Webcam.CaptureFrame();
            bool returnedEx = false;

            try
            {
                for (int i = 0; i < 2; i++)
                {
                    frameOne.ApplyFilter(FilterType.NONE);
                    frameOne.ApplyFilter(FilterType.GREYSCALE);
                    frameOne.ApplyFilter(FilterType.GAUSSIAN_BLUR, (1, 1), 0);
                    frameOne.ApplyFilter(FilterType.CANNY_EDGE_DETECTION, (1, 2));

                    frameOne.Reset();
                }
            }
            catch
            {
                returnedEx = true;
            }

            Assert.False(returnedEx);
        }

        /// <summary>
        /// Applying Gaussian Blur with settings and resetting. Should work.
        /// </summary>
        /// <remarks>
        /// Indirectly Tested:
        /// <list type="bullet">
        /// <item><see cref="Webcam.CaptureFrame(int)"/></item>
        /// <item><see cref="ImageFrame.Reset"/></item>
        /// </list>
        /// </remarks>
        [Fact]
        public void GaussianBlurSettingsTest()
        {
            ImageFrame frameOne = Webcam.CaptureFrame();
            bool returnedEx = false;

            try
            {
                frameOne.ApplyFilter(FilterType.GAUSSIAN_BLUR, (1, 1), 1.3);
                frameOne.Reset();
                frameOne.ApplyFilter(FilterType.GAUSSIAN_BLUR, (1, 4), 2);
                frameOne.Reset();
                // sigmaX = 0 is valid, OpenCV handles this by setting it to
                // the width
                frameOne.ApplyFilter(FilterType.GAUSSIAN_BLUR, (4, 1), 0);
            }
            catch
            {
                returnedEx = true;
            }

            Assert.True(returnedEx);
        }

        /// <summary>
        /// Applying Gaussian Blur with bad settings. Should not work.
        /// </summary>
        /// <remarks>
        /// Indirectly Tested:
        /// <list type="bullet">
        /// <item><see cref="Webcam.CaptureFrame(int)"/></item>
        /// <item><see cref="ImageFrame.Reset"/></item>
        /// </list>
        /// </remarks>
        [Fact]
        public void GaussianBlurBadSettingsTest()
        {
            ImageFrame frameOne = Webcam.CaptureFrame();
            bool returnedEx = false;

            try
            {
                frameOne.ApplyFilter(FilterType.GAUSSIAN_BLUR, (0, 0), -1);
            }
            catch
            {
                returnedEx = true;
            }

            Assert.True(returnedEx);
        }

        /// <summary>
        /// Applying Gaussian Blur with more bad settings. Should not work.
        /// </summary>
        /// <remarks>
        /// Indirectly Tested:
        /// <list type="bullet">
        /// <item><see cref="Webcam.CaptureFrame(int)"/></item>
        /// <item><see cref="ImageFrame.Reset"/></item>
        /// </list>
        /// </remarks>
        [Fact]
        public void GaussianBlurBadSettings2Test()
        {
            ImageFrame frameOne = Webcam.CaptureFrame();
            bool returnedEx = false;

            try
            {
                frameOne.ApplyFilter(FilterType.GAUSSIAN_BLUR, (int.MaxValue, int.MaxValue));
            }
            catch
            {
                returnedEx = true;
            }

            Assert.True(returnedEx);
        }

        /// <summary>
        /// Applying Canny Edge Detection with settings and resetting.
        /// Should work.
        /// </summary>
        /// <remarks>
        /// Indirectly Tested:
        /// <list type="bullet">
        /// <item><see cref="Webcam.CaptureFrame(int)"/></item>
        /// <item><see cref="ImageFrame.Reset"/></item>
        /// </list>
        /// </remarks>
        [Fact]
        public void CannyEdgeDetectionSettingsTest()
        {
            ImageFrame frameOne = Webcam.CaptureFrame();
            bool returnedEx = false;

            try
            {
                frameOne.ApplyFilter(FilterType.CANNY_EDGE_DETECTION, (0, 0));
                frameOne.Reset();
                frameOne.ApplyFilter(FilterType.CANNY_EDGE_DETECTION, (0, 199.2));
                frameOne.Reset();
                // Valid but undefined by OpenCV
                frameOne.ApplyFilter(FilterType.CANNY_EDGE_DETECTION, (0, int.MaxValue));
            }
            catch
            {
                returnedEx = true;
            }

            Assert.False(returnedEx);
        }
    }

    /// <summary>
    /// Tests <see cref="ImageFrame.GenerateGreyscaleHistogram"/>
    /// </summary>
    /// <remarks>
    /// Only tests that the method is valid in terms of being operational,
    /// not that the results are good (integration tests for this).
    /// </remarks>
    [Collection("Sequential")]
    public class ImageFrameGenerateGreyscaleHistogramTests
    {
        /// <summary>
        /// Generating a histogram from captured frame. Should work.
        /// </summary>
        /// <remarks>
        /// Indirectly Tested:
        /// <list type="bullet">
        /// <item><see cref="Webcam.CaptureFrame(int)"/></item>
        /// </list>
        /// </remarks>
        [Fact]
        public void GenerateFromDefaultTest()
        {
            ImageFrame frameOne = Webcam.CaptureFrame();
            bool returnedEx = false;

            try
            {
                frameOne.GenerateGreyscaleHistogram();
            }
            catch
            {
                returnedEx = true;
            }

            Assert.False(returnedEx);
        }

        /// <summary>
        /// Generating a histogram from captured frame after applying greyscale
        /// filter to it. Should work.
        /// </summary>
        /// <remarks>
        /// Indirectly Tested:
        /// <list type="bullet">
        /// <item><see cref="Webcam.CaptureFrame(int)"/></item>
        /// <item><see cref="ImageFrame.ApplyFilter(FilterType)"/></item>
        /// </list>
        /// </remarks>
        [Fact]
        public void GenerateFromGreyscaleTest()
        {
            ImageFrame frameOne = Webcam.CaptureFrame();
            bool returnedEx = false;

            try
            {
                frameOne.ApplyFilter(FilterType.GREYSCALE);
                frameOne.GenerateGreyscaleHistogram();
            }
            catch
            {
                returnedEx = true;
            }

            Assert.False(returnedEx);
        }

        /// <summary>
        /// Generating a histogram from captured frame, then doing it again
        /// after applying greyscale filter to it. Ensuring both are equal to
        /// each other. Should work.
        /// </summary>
        /// <remarks>
        /// Indirectly Tested:
        /// <list type="bullet">
        /// <item><see cref="Webcam.CaptureFrame(int)"/></item>
        /// <item><see cref="ImageFrame.ApplyFilter(FilterType)"/></item>
        /// </list>
        /// </remarks>
        [Fact]
        public void GenerateFromDefaultGreyTest()
        {
            ImageFrame frameOne = Webcam.CaptureFrame();
            Bitmap histogramOne = frameOne.GetBitmap();
            Bitmap histogramTwo = Constants.BLANK_BITMAP;
            bool returnedEx = false;

            try
            {
                histogramOne = frameOne.GenerateGreyscaleHistogram();
                frameOne.ApplyFilter(FilterType.GREYSCALE);
                histogramTwo = frameOne.GenerateGreyscaleHistogram();
            }
            catch
            {
                returnedEx = true;
            }

            Assert.False(returnedEx);
            Assert.True(Helpers.BitmapEquality(histogramOne, histogramTwo));
        }
    }
}
