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
        /// Applies the desired filter onto the frame.
        /// </summary>
        /// <remarks>
        /// NOTE: See <see cref="Reset"/> for how to undo this method.
        /// </remarks>
        /// <param name="type">The type of filter to apply.</param>
        /// <exception cref="Exception">Invalid passing of non-processable filter type.</exception>
        public void ApplyFilter(FilterType type)
        {
            switch (type)
            {
                case FilterType.NONE:
                    break;
                case FilterType.GREYSCALE:
                    ApplyFilterGreyscale();
                    this._colorSpace = ColorSpace.GRAY;
                    break;
                default:
                    throw new Exception($"ERROR: filter type {type} has no handling.");
            }
        }

        /// <summary>
        /// Generates a bar-plot histogram of the current frame if it were
        /// greyscaled (or already is).
        /// </summary>
        /// <remarks>
        /// NOTE: Frame must be in its natural state (i.e. no call to
        /// <see cref="ApplyFilter(FilterType)"/>), or greyscaled already, for
        /// this method to work. See exception details.
        /// </remarks>
        /// <returns>A bitmap representation of the rendered bar-plot histogram.</returns>
        /// <exception cref="Exception">
        /// Invalid state of <see cref="ImageFrame"/>, is not BGR (default) or greyscaled.
        /// </exception>
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
                throw new Exception("ERROR: frame expected to be BGR or already greyscale");
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
    }
}
