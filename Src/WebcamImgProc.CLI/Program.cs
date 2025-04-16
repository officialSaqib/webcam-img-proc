using System.Drawing;
using System.Windows.Controls;
using WebcamImgProc.ImgProc.Capture;
using WebcamImgProc.ImgProc.Frame;

var capture = new Webcam();
capture.GrabFrame();

void TakeWebcamImage()
{
    capture.GrabFrame().GetBitmap().Save("webcam.png");
}
void TakeWebcamImageGreyscale()
{
    ImageFrame frame = capture.GrabFrame();
    frame.GetBitmap().Save("webcam.png");
    frame.ApplyFilter(FilterType.GREYSCALE);
    frame.GetBitmap().Save("webcam_greyscale.png");
    frame.GenerateGreyscaleHistogram().Save("webcam_greyscale_histogram.png");
}
void TakeWebcamImageAll()
{
    ImageFrame frame = capture.GrabFrame();
    frame.GetBitmap().Save("webcam.png");
    frame.ApplyFilter(FilterType.GREYSCALE);
    frame.GetBitmap().Save("webcam_greyscale.png");
    // Do the others
}

bool continueLoop = true;
while (continueLoop) {
    // Input prompt
    Console.WriteLine("+===============================+\r\n" +
        "| IMAGE PROCESSING CLI (WEBCAM) |\r\n" +
        "+===============================+\r\n\r\n" +
        "1. Take normal image\r\n" +
        "2. Take normal image, apply greyscale filters (plus histogram)\r\n" +
        "3. Take normal image, apply all filters\r\n" +
        "4. Exit\r\n");

    char key = Console.ReadKey().KeyChar;
    switch (key)
    {
        case '1':
            TakeWebcamImage();
            break;

        case '2':
            TakeWebcamImageGreyscale();
            break;

        case '3':
            TakeWebcamImageAll();
            break;

        case '4':
            continueLoop = false;
            break;
    }

    Console.Clear();
}
