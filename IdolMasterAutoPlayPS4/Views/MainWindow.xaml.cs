using LibCronusMAX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using System.Globalization;
using System.Threading;
using IdolMasterAutoPlayPS4.Models;

namespace IdolMasterAutoPlayPS4.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() {
            InitializeComponent();
            Device.DeviceInformationChanged += DeviceInformationChanged;
            Device.IOStatusChanged += IoStatusChanged;
            Device.StartWorkerThreads();
        }

        private void IoStatusChanged(object sender, IOStatus e) {
            Device.RequestIoStatus();
            Dispatcher.Invoke(new Action<IOStatus>(status => {
                IoDisplay.UpdateStatus(status);
            }), e);
        }

        private void DeviceInformationChanged(object sender, DeviceInformation e) {

            Dispatcher.Invoke(new Action<DeviceInformation>(info =>
            { // The dispatcher call is required as we're jumping from a background worker thread to the UI thread
                StateBlock.Text = info.State.ToString();
                FwBlock.Text = info.Fw != null ? string.Format("{0}.{1:D02}", info.Fw.Major, info.Fw.Minor) : "Unknown";
                OpModeBlock.Text = info.OperationalMode.ToString();
                HubCompatibleBlock.Text = info.IsHubCompatible ? "Yes" : "No";
            }), e);
            Device.RequestIoStatus();
        }

        private void btnRecord_Click(object sender, RoutedEventArgs e) {
            //Thread thread = new Thread(() =>
            //{
            //    RecordWindow w = new RecordWindow();
            //    w.Show();

            //    w.Closed += (sender2, e2) =>
            //        w.Dispatcher.InvokeShutdown();

            //    Dispatcher.Run();
            //});

            //thread.SetApartmentState(ApartmentState.STA);
            //thread.Start();
        }
    }

    public class InformationBackgroundValueConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value.ToString() == "Connected") {
                return "LightGreen";
            } else if (value.ToString() == "ApiMode") {
                return "Yellow";
            } else {
                return "Red";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
