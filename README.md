# Webcam Image Processor

![.NET](https://img.shields.io/badge/.NET-8.0-8A2BEF)
![Language](https://img.shields.io/badge/Language-C%23-brightgreen)
![OS](https://img.shields.io/badge/OS-Windows-blue)

A C# webcam image processing tool/library using EmguCV that can be used to capture frames from the webcam, apply various image filters, and generate greyscale histograms for analytical purposes.

## Features

* Capturing frames from main webcam.
* Apply various filters to captured frames:
  * Greyscale
  * Gaussian Blur
  * Canny Edge Detection
* Generate and render greyscale histogram for analysis.
* Live webcam feed for filtering/settings.

## Installation

### Environment

WebcamImgProc toolset was developed on a Windows 10 machine, with a single main webcam, using Visual Studio 2022 Community Edition (v17.13.35931.197).

### Dependencies

* [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* [Emgu.CV](https://www.nuget.org/packages/emgu.cv) `v4.10.0.5680`
* [Emgu.CV.Bitmap](https://www.nuget.org/packages/Emgu.CV.Bitmap) `v4.10.0.5680`
* [Emgu.CV.runtime.windows](https://www.nuget.org/packages/Emgu.CV.runtime.windows) `v4.10.0.5680`
* [ScottPlot](https://www.nuget.org/packages/ScottPlot) `v5.0.55`
* [Mono.Options](https://www.nuget.org/packages/mono.options) `6.12.0.148`

## Building

1. Ensure you have installed [dependencies](###Dependencies), and (optionally) are using the [development environment](###Environment).
2. Open `Src/WebcamImgProc.sln` in Visual Studio.
3. Build the solution (`F6` or `Build > Build Solution`).

_If you aren't using Visual Studio, see [.NET CLI](https://learn.microsoft.com/en-us/dotnet/core/tools) for building from the terminal._

## Running

### Command-Line

1. Ensure you have [built](##Building) the project.
2. Go to `WebcamImgProc.CLI\bin\Debug\net8.0-windows\`, find `WebcamImgProc.CLI.exe`.
3. Run `WebcamImgProc.CLI.exe --help` or see [usage section](##Usage) on how to use the tool.

### Desktop (WPF)

1. Ensure you have [built](##Building) the project.
2. Go to `WebcamImgProc.UI\bin\Debug\net8.0-windows\`, find `WebcamImgProc.UI.exe`.
3. Launch `WebcamImgProc.UI.exe`

## Usage

### Command-Line

| Argument                 | Behavior                                                                                                                                |
|--------------------------|-----------------------------------------------------------------------------------------------------------------------------------------|
| Default (or No Argument) | Reads a singular frame from main webcam (0), outputs PNG in same directory of executable as `webcam.png`.                               |
| `-h` / `--help`          | Prints help information and exits.                                                                                                      |
| `-i` / `-in`             | Overrides the default behavior and instead reads from an input image file, outputs PNG in same directory of executable as `custom.png`. |

_Run tool with `--help` argument to see image filter options/settings. All image filter outputs will be in the same directory as the executable but suffixed by the filter type._

### Desktop (WPF)

The Desktop-UI is fairly straightforward, on the left is the webcam image and on the right a greyscale histogram of that image. Below the webcam image you can change the filter being applied to the webcam image, pause the image, and modify appropriate settings for the filter.

![WebcamImgProc_UI_Example](https://github.com/user-attachments/assets/f46acb8a-aefb-498f-8975-e98b2f71d40b)

## Testing

There are classes of unit tests available in `WebcamImgProc.ImgProc.Tests` unit-testing project. This requires [xUnit](https://www.nuget.org/packages/xunit) to run the tests. Test classes/functions have comment summaries explaining testing logic. There are no integration/E2E tests.

## FAQ

### Are there any assumptions made by the library?

Yes, there were assumptions made:
* EmguCV, and more precisely OpenCV, function properly with working internals.
* Results of EmguCV are accurate (i.e. Histogram Calculation, Gaussian Blur Filtering, etc.)
* EmguCV throws exceptions for unhandlable parameters.
* UI application works as-expected (minimal testing here, as this was done at the end).

### What was the thought process for project structure?

Starting off I understood that I needed to use EmguCV to interact with the webcam and grabbed frames. So I abstracted the EmguCV part of it into `Webcam` and `ImageFrame` classes. This permitted me to start a webcam instance that is capable of continuously reading frames from the webcam, then I can grab them and modify appropriately as I see fit. Rather than invoking the default OpenCV calls to modify a frame, it would be simplified into something _like_ `ImageFrame.ApplyFilter()`.

It only made sense to put this abstraction into its own project library which can be used by other project types (i.e. CLI, WPF, etc.)

### Are there features you wish you could've implemented, or didn't have time to implement?

Yes, there are features like this:
* Useful integration/e2e testing for the images processed.
* UI that used MVVM pattern.
* Better-designed UI

## License

No license (not including dependency licenses), use as you see fit.
