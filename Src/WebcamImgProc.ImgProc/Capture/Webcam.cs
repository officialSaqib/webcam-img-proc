using Emgu.CV;
using System.Collections.Concurrent;
using WebcamImgProc.ImgProc.Frame;

namespace WebcamImgProc.ImgProc.Capture
{
    /// <summary>
    /// Webcam-reading class used to constantly read webcam frames, and grab the current frame.
    /// </summary>
    /// <remarks>
    /// Recommended to call <see cref="Release"/> when done, or wrap in a
    /// <c>using</c> statement.
    /// </remarks>
    public class Webcam : IDisposable
    {
        /// <summary>
        /// Keeps track of all in-use cameras based on their index.
        /// </summary>
        /// <remarks>
        /// Keeps track only for project-local <see cref="Webcam"/> instances,
        /// external access of a particular camera will result in less-
        /// explicit constructor exception.
        /// </remarks>
        private static ConcurrentDictionary<int, bool> _cameraIndices = new ConcurrentDictionary<int, bool>();

        /// <summary>
        /// Captures a single frame from the webcam without occupying webcam
        /// feed.
        /// </summary>
        /// <param name="camIndex">Specific camera to use, 0 is the first camera, etc.</param>
        public static ImageFrame CaptureFrame(int camIndex = 0)
        {
            using var webcam = new Webcam(camIndex);
            return webcam.GrabFrame();
        }

        /// <summary>
        /// Main internal capture interface used to interact with the webcam.
        /// </summary>
        private VideoCapture _capture = default!;
        /// <summary>
        /// Camera index defined at creation.
        /// </summary>
        private int _index;
        /// <summary>
        /// Tracker for if <see cref="Dispose"/> was ever called.
        /// </summary>
        private bool _isDisposed = false;

        /// <summary>
        /// Create an instance of webcam input reading.
        /// </summary>
        /// <param name="camIndex">Specific camera to use, 0 is the first camera, etc.</param>
        /// <exception cref="InvalidOperationException">Was not able to start capture.</exception>
        public Webcam(int camIndex = 0)
        {
            // Checks if camera already has instance of Webcam object
            if (_cameraIndices.TryGetValue(camIndex, out _))
            {
                throw new ArgumentException("ERROR: camera already in use by another instance.");
            }

            // Starting our capture and storing camera index
            this._capture = new VideoCapture(camIndex);
            this._index = camIndex;

            // After starting capture, we make sure it is opened and that
            // we can read a frame from it
            if (!this._capture.IsOpened || this._capture.QueryFrame() == null)
            {
                throw new InvalidOperationException("ERROR: could not start capture.");
            }

            // Marking camera index as being in-use
            _cameraIndices.TryAdd(camIndex, true);
        }

        /// <summary>
        /// Grabs current frame of webcam video capture.
        /// </summary>
        /// <returns>A material representation of the current frame.</returns>
        public ImageFrame GrabFrame()
        {
            if (this._isDisposed)
            {
                throw new InvalidOperationException("ERROR: grab frame on released webcam.");
            }

            var frame = new Mat();
            this._capture.Read(frame);
            return new ImageFrame(frame);
        }

        /// <summary>
        /// Calls <see cref="Dispose"/>
        /// </summary>
        public void Release() => this.Dispose();
        /// <summary>
        /// Disposes (releases) internal video capture, then removes camera
        /// from being considered in-use internally.
        /// </summary>
        public void Dispose()
        {
            // Although in practice this is undefined, double calls of this
            // method will just do nothing
            if (this._isDisposed)
            {
                return;
            }

            this._capture.Release();
            Webcam._cameraIndices.TryRemove(this._index, out _);
            this._isDisposed = true;
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Attempts disposal on finalizer (not ideal).
        /// </summary>
        ~Webcam()
        {
            this.Dispose();
        }
    }
}
