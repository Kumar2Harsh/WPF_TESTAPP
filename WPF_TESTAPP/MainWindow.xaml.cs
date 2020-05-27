using System.Windows;
using WPF_TESTAPP.ViewModel;
using SCSSdkClient;
using SCSSdkClient.Object;

namespace WPF_TESTAPP
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SCSSdkTelemetry Telemetry;
        public bool InvokeRequired { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            this.Telemetry = new SCSSdkTelemetry();
            Telemetry.Data += Telemetry_Data;
            Telemetry.JobStarted += TelemetryHandler.JobStarted;
            Telemetry.JobCancelled += TelemetryHandler.JobCancelled;
            Telemetry.JobDelivered += TelemetryHandler.JobDelivered;
            Telemetry.Fined += TelemetryHandler.Fined;
            Telemetry.Tollgate += TelemetryHandler.Tollgate;
            Telemetry.Ferry += TelemetryHandler.FerryUsed;
            Telemetry.Train += TelemetryHandler.TrainUsed;
            Telemetry.RefuelStart += TelemetryHandler.RefuelStart;
            Telemetry.RefuelEnd += TelemetryHandler.RefuelEnd;
            Telemetry.RefuelPayed += TelemetryHandler.RefuelPayed;

     

        }

        public void Telemetry_Data(SCSTelemetry data, bool updated)
        {
            try
            {

                if (!InvokeRequired)
                {
                    ((TruckViewModel)DataContext).Hersteller = data.TruckValues.ConstantsValues.Brand.ToString();

                }
            }
            catch
            { }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ((TruckViewModel)DataContext).AendereHersteller("Thomas he Great");
        }
    }
}
