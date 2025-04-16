using Emgu.CV;
using Emgu.CV.Util;
using ScottPlot;
using System.Drawing;
using System.IO;

namespace WebcamImgProc.ImgProc.Frame
{
    /// <summary>
    /// An abstraction representing a frame in OpenCV, internally uses a
    /// <see cref="Mat"/> object to represent the frame.
    /// </summary>
    public class ImageFrame
    {
        /// <summary>
        /// Number of bins used to represent a greyscale frame (i.e. in a
        /// histogram).
        /// </summary>
        /// <remarks>
        /// See <see cref="GenerateGreyscaleHistogram"/>.
        /// </remarks>
        private const int GREYSCALE_BIN_COUNT = 256;

        /// <summary>
        /// Backup of the internal frame originally used to represent frame
        /// upon initialization by constructor.
        /// </summary>
        private readonly Mat _originalFrame;
        /// <summary>
        /// Internal representation of our frame (abstracted away from public).
        /// </summary>
        private Mat _frame;
        /// <summary>
        /// The color space of <see cref="_frame"/>, assumed to be BGR by
        /// default following EmguCV/OpenCV assumption.
        /// </summary>
        private ColorSpace _colorSpace = ColorSpace.BGR;

        /// <summary>
        /// Creates new instance of image frame as a copy of another
        /// <see cref="ImageFrame"/> object.
        /// </summary>
        /// <param name="frame">Frame to <b>copy</b> contents of.</param>
        public ImageFrame(ImageFrame frame)
        {
            this._originalFrame = frame._frame.Clone();
            this._frame = frame._frame.Clone();
        }
        /// <summary>
        /// Creates new instance of image frame from <see cref="Mat"/> object.
        /// </summary>
        /// <param name="frame">Image material to use for internal processing.</param>
        public ImageFrame(Mat frame)
        {
            this._originalFrame = frame.Clone();
            this._frame = frame;
        }

        /// <summary>
        /// Gets current frame as bitmap.
        /// </summary>
        /// <returns>A bitmap representation of the current frame.</returns>
        public Bitmap GetBitmap()
        {
            return this._frame.ToBitmap();
        }

        /// <summary>
        /// Sets the frame back to what it was upon creation.
        /// </summary>
        public void Reset()
        {
            this._frame = this._originalFrame.Clone();
            this._colorSpace = ColorSpace.BGR;
        }

        /// <summary>
        /// Applies the desired filter onto the frame with settings.
        /// </summary>
        /// <remarks>
        /// NOTE: See <see cref="Reset"/> for how to undo this method.
        /// </remarks>
        /// <param name="type">The type of filter to apply.</param>
        /// <param name="settings">Settings to apply with the filter.</param>
        /// <exception cref="ArgumentOutOfRangeException">Invalid passing of non-processable filter type.</exception>
        public void ApplyFilter(FilterType type, FilterSettings settings)
        {
            switch (type)
            {
                case FilterType.NONE:
                    break;
                case FilterType.GREYSCALE:
                    ApplyFilterGreyscale();
                    this._colorSpace = ColorSpace.GRAY;
                    break;
                case FilterType.GAUSSIAN_BLUR:
                    ApplyFilterGuassianBlur(settings.KernelSize, settings.SigmaX);
                    this._colorSpace = ColorSpace.BGR;
                    break;
                case FilterType.CANNY_EDGE_DETECTION:
                    ApplyCannyEdgeDetection(settings.Threshold);
                    this._colorSpace = ColorSpace.OTHER;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"ERROR: filter type {type} has no handling.");
            }
        }
        /// <summary>
        /// Applies the desired filter onto the frame with default (or no)
        /// settings.
        /// </summary>
        /// <remarks>
        /// NOTE: See <see cref="ApplyFilter(FilterType, FilterSettings)"/> for
        /// reference.
        /// </remarks>
        /// <param name="type">The type of filter to apply.</param>
        public void ApplyFilter(FilterType type)
        {
            this.ApplyFilter(type, new FilterSettings());
        }
        /// <summary>
        /// Applies expected <see cref="FilterType.GAUSSIAN_BLUR"/> filter onto
        /// the frame with related settings.
        /// </summary>
        /// <remarks>
        /// NOTE: See <see cref="ApplyFilter(FilterType, FilterSettings)"/> for
        /// reference.
        /// </remarks>
        /// <param name="type">The type of filter to apply, expected <see cref="FilterType.GAUSSIAN_BLUR"/>.</param>
        /// <param name="kernelSize">Size of the kernel (area-of-effect) when blurring.</param>
        /// <param name="sigmaX">Standard deviation in X-axis when blurring.</param>
        /// <exception cref="ArgumentException">Invalid type given, not <see cref="FilterType.GAUSSIAN_BLUR"/> type.</exception>
        public void ApplyFilter(FilterType type, (int, int) kernelSize, double sigmaX)
        {
            if (type != FilterType.GAUSSIAN_BLUR)
            {
                throw new ArgumentException("ERROR: Expected filter of Guassian Blur" +
                    $" not {type}.");
            }

            this.ApplyFilter(type, new FilterSettings(kernelSize, sigmaX));
        }
        /// <summary>
        /// Applies expected <see cref="FilterType.CANNY_EDGE_DETECTION"/>
        /// filter onto the frame with related settings.
        /// </summary>
        /// <remarks>
        /// NOTE: See <see cref="ApplyFilter(FilterType, FilterSettings)"/> for
        /// reference.
        /// </remarks>
        /// <param name="type">The type of filter to apply, expected <see cref="FilterType.CANNY_EDGE_DETECTION"/>.</param>
        /// <param name="threshold">Starting/ending threshold for edge-strength detection.</param>
        /// <exception cref="ArgumentException">Invalid type given, not <see cref="FilterType.CANNY_EDGE_DETECTION"/> type.</exception>
        public void ApplyFilter(FilterType type, (double, double) threshold)
        {
            if (type != FilterType.CANNY_EDGE_DETECTION)
            {
                throw new ArgumentException("ERROR: Expected filter of Canny Edge Detection" +
                    $" not {type}.");
            }

            this.ApplyFilter(type, new FilterSettings(threshold));
        }

        /// <summary>
        /// Generates a bar-plot histogram of the current frame if it were
        /// greyscaled (or already is).
        /// </summary>
        /// <remarks>
        /// NOTE: Frame must be in color space BGR or GREY for this method to
        /// work. See exception details.
        /// </remarks>
        /// <returns>A bitmap representation of the rendered bar-plot histogram.</returns>
        /// <exception cref="Exception">Invalid state of <see cref="ImageFrame"/>, is not BGR (default) or greyscaled.</exception>
        public Bitmap GenerateGreyscaleHistogram()
        {
            // Reference for what frame to use, useful in the case where we
            // need to do a greyscale conversion first (that way we can clone
            // data into this variable IF NEEDED)
            Mat frame = this._frame;

            // If we aren't working with a greyscale image, clone its contents
            // and make it greyscale. If the image is already in greyscale do
            // nothing, and otherwise throw an exception.
            //
            // This does not affect the _frame field AT ALL of this class
            if (this._colorSpace == ColorSpace.BGR)
            {
                frame = this._frame.Clone();
                CvInvoke.CvtColor(frame, frame, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
            }
            else if (this._colorSpace != ColorSpace.GRAY)
            {
                throw new Exception("ERROR: frame expected to be BGR or already greyscale.");
            }

            // Calculating histogram matrix, then grabbing the bin values
            var histMat = new Mat();
            CvInvoke.CalcHist(new VectorOfMat(frame), [0], null, histMat,
                [ImageFrame.GREYSCALE_BIN_COUNT], [0, ImageFrame.GREYSCALE_BIN_COUNT], false);
            var binValues = (float[,])histMat.GetData();

            // Creating a bar plot based on the bin values we grabbed before
            var plot = new Plot();
            for (int i = 0; i < ImageFrame.GREYSCALE_BIN_COUNT; i++)
                plot.Add.Bar(i, binValues[i, 0]);

            // Plot as a rendered Bitmap image (bytes)
            byte[] plotBytes = plot.GetImage(512, 512).GetImageBytes(ImageFormat.Bmp);

            // Reading Bitmap image from the bytes in a memory stream, then
            // cloning it to avoid losing Bitmap data when stream closes
            Bitmap bmp;
            using (var ms = new MemoryStream(plotBytes))
            {
                bmp = new Bitmap(ms);
            }
            var clone = new Bitmap(bmp);

            return clone;
        }

        /// <summary>
        /// Applies a greyscale filter onto the internal <see cref="_frame"/>.
        /// </summary>
        private void ApplyFilterGreyscale()
        {
            CvInvoke.CvtColor(this._frame, this._frame, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
        }
        /// <summary>
        /// Applies a Guassian Blur filter onto the internal
        /// <see cref="_frame"/> with given kernel size and X-axis standard-
        /// deviation.
        /// </summary>
        /// <param name="kernelSize">Size of the kernel (area-of-effect) when blurring.</param>
        /// <param name="sigmaX">Standard deviation in X-axis when blurring.</param>
        private void ApplyFilterGuassianBlur((int, int) kernelSize, double sigmaX)
        {
            CvInvoke.GaussianBlur(this._frame, this._frame,
                new Size(kernelSize.Item1, kernelSize.Item2), sigmaX);
        }
        /// <summary>
        /// Applies Canny Edge Detection onto the internal <see cref="_frame"/>
        /// with given threshold.
        /// </summary>
        /// <param name="threshold">Starting/ending threshold for edge-strength detection.</param>
        private void ApplyCannyEdgeDetection((double, double) threshold)
        {
            // Canny() function does not like input equalling output, so we
            // make a temporary frame to hold the output
            var newFrame = new Mat();
            CvInvoke.Canny(this._frame, newFrame, threshold.Item1, threshold.Item2);
            this._frame = newFrame;
        }
    }
}
