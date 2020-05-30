using System.ComponentModel;
using System.Runtime.CompilerServices;
using SCSSdkClient;
using SCSSdkClient.Object;
using WPF_TESTAPP.Annotations;

namespace WPF_TESTAPP.Models
{
    public class TruckModel : INotifyPropertyChanged
    {
        /*

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


            if (!InvokeRequired)
            {
              
            }
        }
        */

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }



        private string _hersteller;

        public string Hersteller
        {
            get { return _hersteller; }
            set
            {
                if (_hersteller == value)
                    return;
                _hersteller = value;
                OnPropertyChanged(nameof(Hersteller));
            }
        }
    }
}
