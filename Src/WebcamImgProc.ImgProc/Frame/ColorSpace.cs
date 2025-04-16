namespace WebcamImgProc.ImgProc.Frame
{
    /// <summary>
    /// Channel-representations associated with <see cref="ImageFrame"/> object.
    /// </summary>
    enum ColorSpace
    {
        /// <summary>
        /// Has Blue, Green, Red channels (in this order).
        /// </summary>
        BGR,
        /// <summary>
        /// Has singular "brightness" channel.
        /// </summary>
        GRAY,
        /// <summary>
        /// Has other/unknown assortment of channels.
        /// </summary>
        OTHER,
    }
}
