using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WebcamImgProc.ImgProc.Capture;
using WebcamImgProc.ImgProc.Frame;

namespace WebcamImgProc.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Main webcam which is running to capture frames.
        /// </summary>
        private Webcam webcam = new Webcam();

        public MainWindow()
        {
            InitializeComponent();

            // A timer that ticks once per nanosecond, updates the view
            var dispatchTimer = new DispatcherTimer();
            dispatchTimer.Tick += new EventHandler(TimerTickEvent);
            dispatchTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dispatchTimer.Start();

            // WPF sliders have a bug (I think?) where it calls the event if I
            // set the minimum in the properties, before the window is even
            // initialized. Could probably be avoided if I had a good MVVM
            // system instead
            this.KernelXSlider.Minimum = 1;
            this.KernelYSlider.Minimum = 1;
            this.SigmaXSlider.Minimum = 1;
            this.ThresholdLowerSlider.Minimum = 1;
            this.ThresholdUpperSlider.Minimum = 1;
        }

        private void TimerTickEvent(object sender, EventArgs e)
        {
            if (this.PauseFrameCheckBox.IsChecked == true)
                return;

            ImageFrame fr = webcam.GrabFrame();
            switch (this.FilterTypeComboBox.SelectedIndex)
            {
                case 1:
                    fr.ApplyFilter(FilterType.GREYSCALE);
                    break;

                case 2:
                    fr.ApplyFilter(FilterType.GAUSSIAN_BLUR,
                        ((int)this.KernelXSlider.Value, (int)this.KernelYSlider.Value),
                        this.SigmaXSlider.Value);
                    break;

                case 3:
                    fr.ApplyFilter(FilterType.CANNY_EDGE_DETECTION,
                        (this.ThresholdLowerSlider.Value, this.ThresholdUpperSlider.Value));
                    break;

                default:
                    break;
            }

            // Setting source for each WPF image
            this.WebcamFrameImage.Source = Helpers.Convert(fr.GetBitmap());
            this.WebcamFrameImageGreyscale.Source = Helpers.Convert(fr.GenerateGreyscaleHistogram());
        }

        private void KernelXSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.KernelXValueLabel.Content = $"({e.NewValue})";
        }
        private void KernelYSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.KernelYValueLabel.Content = $"({e.NewValue})";
        }
        private void SigmaXSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.SigmaXValueLabel.Content = $"({e.NewValue})";
        }
        private void ThresholdLowerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.ThresholdUpperSlider.Value < e.NewValue)
                this.ThresholdUpperSlider.Value = e.NewValue;

            this.ThresholdLowerValueLabel.Content = $"({e.NewValue*10})";
        }
        private void ThresholdUpperSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.ThresholdLowerSlider.Value > e.NewValue)
                this.ThresholdLowerSlider.Value = e.NewValue;

            this.ThresholdUpperValueLabel.Content = $"({e.NewValue*10})";
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // This is important!
            this.webcam.Release();
        }
    }
}