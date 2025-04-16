using Emgu.CV;
using WebcamImgProc.ImgProc.Frame;

namespace WebcamImgProc.ImgProc.Capture
{
    /// <summary>
    /// Webcam-reading class used to constantly read webcam frames, and grab the current frame.
    /// </summary>
    public class Webcam
    {
        /// <summary>
        /// Captures a single frame from the webcam without occupying webcam
        /// feed.
        /// </summary>
        /// <param name="camIndex">Specific camera to use, 0 is the first camera, etc.</param>
        public static ImageFrame CaptureFrame(int camIndex = 0) => new Webcam(camIndex).GrabFrame();

        /// <summary>
        /// Main internal capture interface used to interact with the webcam.
        /// </summary>
        private VideoCapture _capture = default!;

        /// <summary>
        /// Create an instance of webcam input reading.
        /// </summary>
        /// <param name="camIndex">Specific camera to use, 0 is the first camera, etc.</param>
        public Webcam(int camIndex = 0)
        {
            _capture = new VideoCapture(camIndex);
        }

        /// <summary>
        /// Grabs current frame of webcam video capture.
        /// </summary>
        /// <returns>A material representation of the current frame.</returns>
        public ImageFrame GrabFrame()
        {
            var frame = new Mat();
            _capture.Read(frame);
            return new ImageFrame(frame);
        }
    }
}
