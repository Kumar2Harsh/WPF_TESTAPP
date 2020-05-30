using SCSSdkClient;
using SCSSdkClient.Object;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;

namespace WPF_TESTAPP.ViewModel
{
    public class TruckViewModel : INotifyPropertyChanged
    {
        public SCSSdkTelemetry Telemetry;
        public bool InvokeRequired { get; set; }
        
        public void Telemetry_Data(SCSTelemetry data, bool updated)
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

            try
            {
                if (!InvokeRequired)
                {
                    this.Hersteller = data.TruckValues.ConstantsValues.BrandId;
                }
            }
            catch { }
        }

        private string _hersteller;
        public string Hersteller 
        { 
            get { return _hersteller; }
            set
            {
                _hersteller = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AendereHersteller(string value)
        {
            Hersteller = value;
        }
    }
}
