using WebcamImgProc.ImgProc.Capture;
using WebcamImgProc.ImgProc.Frame;

/**TEST SPECIFICATIONS
 * 
 * Project: WebcamImgProc.ImgProc
 * File: Capture/Webcam.cs
 * Class: Webcam
 * 
 * Description: The constructor, and method <see cref="Webcam.GrabFrame"/> are
 *              tested here. <see cref="Webcam.Release"/> is inadvertantly
 *              tested as all tests utilize it to dispose of the instance.
 */

namespace WebcamImgProc.ImgProc.Tests.CaptureTests
{
    /// <summary>
    /// Tests <see cref="Webcam.Webcam(int)"/>
    /// </summary>
    [Collection("Sequential")]
    public class WebcamConstructorTests
    {
        /// <summary>
        /// Expected normal usage of creating webcam with camera index. Should
        /// work (unless you have no cameras).
        /// </summary>
        /// <remarks>
        /// Indirectly Tested:
        /// <list type="bullet">
        /// <item><see cref="Webcam.Release"/></item>
        /// </list>
        /// </remarks>
        [Fact]
        public void NormalCameraIndexTest()
        {
            Webcam webcam;
            bool returnedEx = false;

            try
            {
                webcam = new Webcam(0);
                webcam.Release();
            }
            catch
            {
                returnedEx = true;
            }

            Assert.False(returnedEx);
        }

        /// <summary>
        /// Constructing with camera index that does not exist.
        /// Should not work.
        /// </summary>
        /// <remarks>
        /// Indirectly Tested:
        /// <list type="bullet">
        /// <item><see cref="Webcam.Release"/></item>
        /// </list>
        /// </remarks>
        [Fact]
        public void InvalidCameraIndexTest()
        {
            Webcam webcam;
            bool returnedEx = false;

            try
            {
                webcam = new Webcam(int.MaxValue);
                webcam.Release();
            }
            catch
            {
                returnedEx = true;
            }

            Assert.True(returnedEx);
        }

        /// <summary>
        /// Constructing two webcam objects with the same camera index is not
        /// allowed. Should not work.
        /// </summary>
        /// <remarks>
        /// Indirectly Tested:
        /// <list type="bullet">
        /// <item><see cref="Webcam.Release"/></item>
        /// </list>
        /// </remarks>
        [Fact]
        public void DoubleCameraIndexTest()
        {
            Webcam otherWebcam;
            bool returnedEx = false;

            try
            {
                using var webcam = new Webcam();
                otherWebcam = new Webcam(0);

                // Shouldn't be reached, but bad implementation might reach it,
                // in which case we properly release resources
                otherWebcam.Release();
            }
            catch
            {
                returnedEx = true;
            }

            Assert.True(returnedEx);
        }

        /// <summary>
        /// Constructing two webcam objects but the first one releases before
        /// the second is constructed, allowing the second to be constructed.
        /// Should work.
        /// </summary>
        /// <remarks>
        /// Indirectly Tested:
        /// <list type="bullet">
        /// <item><see cref="Webcam.Release"/></item>
        /// </list>
        /// </remarks>
        [Fact]
        public void DoubleCameraReleaseIndexTest()
        {
            Webcam webcam;
            Webcam otherWebcam;
            bool returnedEx = false;

            try
            {
                webcam = new Webcam();
                webcam.Release();
                otherWebcam = new Webcam(0);
                otherWebcam.Release();
            }
            catch
            {
                returnedEx = true;
            }

            Assert.False(returnedEx);
        }
    }

    /// <summary>
    /// Tests <see cref="Webcam.GrabFrame"/>
    /// </summary>
    [Collection("Sequential")]
    public class WebcamGrabFrameTests
    {
        /// <summary>
        /// Tests grabbing frame once. Should work.
        /// </summary>
        /// <remarks>
        /// Indirectly Tested:
        /// <list type="bullet">
        /// <item><see cref="Webcam.Webcam(int)"/></item>
        /// <item><see cref="Webcam.Dispose"/></item>
        /// </list>
        /// </remarks>
        [Fact]
        public void OneGrabTest()
        {
            ImageFrame frame;
            bool returnedEx = false;

            try
            {
                using var webcam = new Webcam();
                frame = webcam.GrabFrame();
            }
            catch
            {
                returnedEx = true;
            }

            Assert.False(returnedEx);
        }

        /// <summary>
        /// Tests grabbing frame twice. Should work.
        /// </summary>
        /// <remarks>
        /// Indirectly Tested:
        /// <list type="bullet">
        /// <item><see cref="Webcam.Webcam(int)"/></item>
        /// <item><see cref="Webcam.Dispose"/></item>
        /// </list>
        /// </remarks>
        [Fact]
        public void TwoGrabsTest()
        {
            ImageFrame frame;
            bool returnedEx = false;

            try
            {
                using var webcam = new Webcam();
                frame = webcam.GrabFrame();
                frame = webcam.GrabFrame();
            }
            catch
            {
                returnedEx = true;
            }

            Assert.False(returnedEx);
        }

        /// <summary>
        /// Trying to grab a frame on a released webcam. Should not work.
        /// </summary>
        /// <remarks>
        /// Indirectly Tested:
        /// <list type="bullet">
        /// <item><see cref="Webcam.Webcam(int)"/></item>
        /// <item><see cref="Webcam.Release"/></item>
        /// </list>
        /// </remarks>
        [Fact]
        public void ReleasedWebcamTest()
        {
            Webcam webcam = new Webcam();
            ImageFrame frame;
            bool returnedEx = false;

            webcam.Release();
            try
            {
                frame = webcam.GrabFrame();
            }
            catch
            {
                returnedEx = true;
            }

            Assert.True(returnedEx);
        }
    }
}
