using Emgu.CV;

namespace WebcamImgProc.ImgProc.Frame
{
    /// <summary>
    /// Settings expected to be applied when using
    /// <see cref="ImageFrame.ApplyFilter(FilterType, FilterSettings)"/>
    /// method.
    /// </summary>
    /// <remarks>
    /// Only relevant settings are expected to be read, setting non-relevant
    /// settings for a filter type is expected to have no impact. Default
    /// settings values are arbitrary and don't mean anything.
    /// </remarks>
    public class FilterSettings
    {
        /// <summary>
        /// Used by <see cref="ImageFrame.ApplyFilter(FilterType)"/> for
        /// <see cref="FilterType.GAUSSIAN_BLUR"/> filter as the size of kernel
        /// for effect on each pixel.
        /// </summary>
        /// <remarks>
        /// See <see cref="CvInvoke.GaussianBlur(IInputArray, IOutputArray, System.Drawing.Size, double, double, Emgu.CV.CvEnum.BorderType)"/>
        /// for more information, and
        /// <a href="https://docs.opencv.org/4.x/d4/d13/tutorial_py_filtering.html#autotoc_md1356">OpenCV documentation</a>.
        /// </remarks>
        public (int, int) KernelSize { get; set; } = (5, 5);
        /// <summary>
        /// Used by <see cref="ImageFrame.ApplyFilter(FilterType)"/> for
        /// <see cref="FilterType.GAUSSIAN_BLUR"/> filter as the standard
        /// deviation in the X direction.
        /// </summary>
        /// <remarks>
        /// See <see cref="CvInvoke.GaussianBlur(IInputArray, IOutputArray, System.Drawing.Size, double, double, Emgu.CV.CvEnum.BorderType)"/>
        /// for more information, and
        /// <a href="https://docs.opencv.org/4.x/d4/d13/tutorial_py_filtering.html#autotoc_md1356">OpenCV documentation</a>.
        /// </remarks>
        public double SigmaX { get; set; } = 0;

        /// <summary>
        /// Used by <see cref="ImageFrame.ApplyFilter(FilterType)"/> for
        /// <see cref="FilterType.CANNY_EDGE_DETECTION"/> filter as the
        /// threshold.
        /// </summary>
        /// <remarks>
        /// See <see cref="CvInvoke.Canny(IInputArray, IOutputArray, double, double, int, bool)"/>
        /// for more information, and
        /// <a href="https://docs.opencv.org/4.x/da/d22/tutorial_py_canny.html">OpenCV documentation</a>.
        /// </remarks>
        public (double, double) Threshold { get; set; } = (100, 200);

        /// <summary>
        /// Initialize new, blank, <see cref="FilterSettings"/> object.
        /// </summary>
        internal FilterSettings() { }
        /// <summary>
        /// Initializes new <see cref="FilterSettings"/> object with kernel
        /// size and sigma X.
        /// </summary>
        /// <param name="kernelSize">See <see cref="KernelSize"/> for reference.</param>
        /// <param name="sigmaX">See <see cref="SigmaX"/> for reference.</param>
        public FilterSettings((int, int) kernelSize, double sigmaX)
        {
            this.KernelSize = kernelSize;
            this.SigmaX = sigmaX;
        }
        /// <summary>
        /// Initializes new <see cref="FilterSettings"/> object with threshold.
        /// </summary>
        /// <param name="threshold">See <see cref="Threshold"/> for reference.</param>
        public FilterSettings((double, double) threshold)
        {
            this.Threshold = threshold;
        }
    }
}
