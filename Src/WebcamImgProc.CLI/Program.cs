using Mono.Options;
using WebcamImgProc.CLI;
using WebcamImgProc.ImgProc.Capture;
using WebcamImgProc.ImgProc.Frame;

///////////////////////////////////////////////////////////////////////////////
//
// Argument Flags
//
bool showHelp = false;
bool outputGreyscale = false;
bool outputGreyscaleHistogram = false;
bool outputGaussianBlur = false;
bool outputCannyEdgeDetection = false;

///////////////////////////////////////////////////////////////////////////////
//
// Filter Settings
//
int kSizeX = 5;              // Gaussian Blur
int kSizeY = 5;              // Gaussian Blur
double sigmaX = 0;           // Gaussian Blur
double thresholdLower = 100; // Canny Edge Detection
double thresholdUpper = 200; // Canny Edge Detection

///////////////////////////////////////////////////////////////////////////////
//
// Arguments
//
var options = new OptionSet
{
    { "h|help", "show this message then exit", arg => showHelp = true },
    { "g|grey", "output greyscale image", arg => outputGreyscale = true },
    { "histogram", "output greyscale histogram", arg => outputGreyscaleHistogram = true },
    { "b|blur", "output image with Gaussian Blur", arg => outputGaussianBlur = true },
    { "ksize-x=", "kernel x-size for Gaussian Blur (default=5)", arg => kSizeX = int.Parse(arg) },
    { "ksize-y=", "kernel y-size for Gaussian Blur (default=5)", arg => kSizeY = int.Parse(arg) },
    { "sigma-x=", "STD in x-axis for Gaussian Blur (default=0.0)", arg => sigmaX = double.Parse(arg) },
    { "e|edgedetection", "output image with Canny Edge Detection", arg => outputCannyEdgeDetection = true },
    { "threshold-l=", "lower threshold for Canny Edge Detection (default=100.0)", arg => thresholdLower = double.Parse(arg) },
    { "threshold-u=", "upper threshold for Canny Edge Detection (default=200.0)", arg => thresholdUpper = double.Parse(arg) },
};

///////////////////////////////////////////////////////////////////////////////
//
// Argument Parsing
//
try
{
    options.Parse(args);
} catch(Exception e)
{
    Console.WriteLine(e.Message);
    Helpers.PrintHelp(options);
    return;
}

///////////////////////////////////////////////////////////////////////////////
//
// Argument Processing: -(-h)elp
//
if (showHelp)
{
    Helpers.PrintHelp(options);
    return;
}

///////////////////////////////////////////////////////////////////////////////
//
// Default (No/Many Arguments)
//
ImageFrame frame = Webcam.CaptureFrame();
frame.GetBitmap().Save("webcam.png");

///////////////////////////////////////////////////////////////////////////////
//
// Argument Processing: -(-g)rey
//
if (outputGreyscale)
{
    frame.ApplyFilter(FilterType.GREYSCALE);
    frame.GetBitmap().Save("webcam_greyscale.png");
    frame.Reset();
}

///////////////////////////////////////////////////////////////////////////////
//
// Argument Processing: --histogram
//
if (outputGreyscaleHistogram)
{
    frame.GenerateGreyscaleHistogram().Save("webcam_greyscale_histogram.png");
}

///////////////////////////////////////////////////////////////////////////////
//
// Argument Processing: -(-b)lur
//
if (outputGaussianBlur)
{
    frame.ApplyFilter(FilterType.GAUSSIAN_BLUR, (kSizeX, kSizeY), sigmaX);
    frame.GetBitmap().Save("webcam_gaussian_blur.png");
    frame.Reset();
}

///////////////////////////////////////////////////////////////////////////////
//
// Argument Processing: -(-e)dgedetection
//
if (outputCannyEdgeDetection)
{
    frame.ApplyFilter(FilterType.CANNY_EDGE_DETECTION, (thresholdLower, thresholdUpper));
    frame.GetBitmap().Save("webcam_canny_edge_detection.png");
    frame.Reset();
}
