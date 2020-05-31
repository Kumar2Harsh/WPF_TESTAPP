using SCSSdkClient;
using SCSSdkClient.Object;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WPF_TESTAPP.Annotations;
using WPF_TESTAPP.Classes;
using WPF_TESTAPP.Models;

namespace WPF_TESTAPP.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public SCSSdkTelemetry Telemetry;
        public bool InvokeRequired { get; private set; }

        public string truckBrand;

        public string TruckBrand
        {
            get
            {
                return truckBrand;
            }
            set
            {
                if (truckBrand == value)
                    return;
                truckBrand = value;
                OnPropertyChanged(nameof(TruckBrand));
                
            }
        }

      

        public MainViewModel()
        {
           
        }



        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Telemetry_Data(SCSTelemetry data, bool updated)
        {

            try
            {
                if (!InvokeRequired)
                {
                    TruckBrand = data.TruckValues.ConstantsValues.BrandId;
                }
            }
            catch
            { }
        }

    }
}
