using System.Windows;
using SCSSdkClient;
using SCSSdkClient.Object;
using WPF_TESTAPP.ViewModel;

namespace WPF_TESTAPP
{ 
    public partial class MainWindow : Window
    {
        public SCSSdkTelemetry Telemetry;
        public bool InvokeRequired { get; private set; }
        public MainWindow()
        {
            Telemetry = new SCSSdkTelemetry();
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
            InitializeComponent();

        }
        public void Telemetry_Data(SCSTelemetry data, bool updated)
        {
            try
            {
                if (!InvokeRequired)
                {
                   // Here comes Data from DLL
                }
            }
            catch
            { }
        }



    }
}
