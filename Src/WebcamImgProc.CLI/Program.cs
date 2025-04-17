using Emgu.CV;
using Mono.Options;
using WebcamImgProc.CLI;
using WebcamImgProc.CLI.Testing;
using WebcamImgProc.ImgProc.Capture;
using WebcamImgProc.ImgProc.Frame;

///////////////////////////////////////////////////////////////////////////////
//
// Argument Flags
//
bool showHelp = false;
bool doTesting = false;
string input = string.Empty;
bool outputGreyscale = false;
bool outputGreyscaleHistogram = false;
bool outputGaussianBlur = false;
bool outputCannyEdgeDetection = false;
bool outputAll = false;

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
    { "t|integration-e2e-test", "performs integration/end-to-end tests on processing library", arg => doTesting = true },
    { "i|in=", "as opposed to webcam, reads from custom image input (BMP, JPG, PNG, etc.)", arg => input = arg },
    { "g|grey", "output greyscale image", arg => outputGreyscale = true },
    { "histogram", "output greyscale histogram", arg => outputGreyscaleHistogram = true },
    { "b|blur", "output image with Gaussian Blur", arg => outputGaussianBlur = true },
    { "ksize-x=", "kernel x-size for Gaussian Blur (default=5)", arg => kSizeX = int.Parse(arg) },
    { "ksize-y=", "kernel y-size for Gaussian Blur (default=5)", arg => kSizeY = int.Parse(arg) },
    { "sigma-x=", "STD in x-axis for Gaussian Blur (default=0.0)", arg => sigmaX = double.Parse(arg) },
    { "e|edgedetection", "output image with Canny Edge Detection", arg => outputCannyEdgeDetection = true },
    { "threshold-l=", "lower threshold for Canny Edge Detection (default=100.0)", arg => thresholdLower = double.Parse(arg) },
    { "threshold-u=", "upper threshold for Canny Edge Detection (default=200.0)", arg => thresholdUpper = double.Parse(arg) },
    { "a|all", "output all image filter types", arg => outputAll = true },
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
// Argument Processing : -(-i)ntegration-e2e-test
//
if (doTesting)
{
    Testing.Run();
    return;
}

///////////////////////////////////////////////////////////////////////////////
//
// Default (No/Many Arguments)
//
ImageFrame frame;
string saveName;
if (string.IsNullOrEmpty(input))
{
    frame = Webcam.CaptureFrame();
    saveName = "webcam";
}
else
{
    frame = new ImageFrame(CvInvoke.Imread(input));
    saveName = Path.GetFileNameWithoutExtension(input);
}
frame.GetBitmap().Save(saveName + ".png");

///////////////////////////////////////////////////////////////////////////////
//
// Argument Processing: -(-a)ll
//
if (outputAll)
{
    outputGreyscale = true;
    outputGreyscaleHistogram = true;
    outputGaussianBlur = true;
    outputCannyEdgeDetection = true;
}

///////////////////////////////////////////////////////////////////////////////
//
// Argument Processing: -(-g)rey
//
if (outputGreyscale)
{
    frame.ApplyFilter(FilterType.GREYSCALE);
    frame.GetBitmap().Save(saveName + "_greyscale.png");
    frame.Reset();
}

///////////////////////////////////////////////////////////////////////////////
//
// Argument Processing: --histogram
//
if (outputGreyscaleHistogram)
{
    frame.GenerateGreyscaleHistogram().Save(saveName + "_greyscale_histogram.png");
}

///////////////////////////////////////////////////////////////////////////////
//
// Argument Processing: -(-b)lur
//
if (outputGaussianBlur)
{
    frame.ApplyFilter(FilterType.GAUSSIAN_BLUR, (kSizeX, kSizeY), sigmaX);
    frame.GetBitmap().Save(saveName + "_gaussian_blur.png");
    frame.Reset();
}

///////////////////////////////////////////////////////////////////////////////
//
// Argument Processing: -(-e)dgedetection
//
if (outputCannyEdgeDetection)
{
    frame.ApplyFilter(FilterType.CANNY_EDGE_DETECTION, (thresholdLower, thresholdUpper));
    frame.GetBitmap().Save(saveName + "_canny_edge_detection.png");
    frame.Reset();
}
