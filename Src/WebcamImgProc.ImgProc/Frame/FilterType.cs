namespace WebcamImgProc.ImgProc.Frame
{
    /// <summary>
    /// Filters which can be applied to a <see cref="ImageFrame"/> object.
    /// </summary>
    public enum FilterType
    {
        /// <summary>
        /// No change to the frame.
        /// </summary>
        NONE = 0,
        /// <summary>
        /// Frame is changed to greyscale (aka one "brightness" channel).
        /// </summary>
        GREYSCALE = 1,
        /// <summary>
        /// A Gaussian Blur is applied to the frame.
        /// </summary>
        GAUSSIAN_BLUR = 2,
        /// <summary>
        /// Canny Edge Detection is applied to the frame.
        /// </summary>
        CANNY_EDGE_DETECTION = 3,
    }
}
