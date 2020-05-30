using SCSSdkClient;
using SCSSdkClient.Object;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WPF_TESTAPP.Models;

namespace WPF_TESTAPP.ViewModels
{
    public class TruckViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        private TruckModel _truckModel;

        public TruckModel TruckModel
        {
            get { return _truckModel; }
            set
            {
                if (_truckModel == value)
                    return;
                _truckModel = value;
                OnPropertyChanged(nameof(TruckModel));

            }
        }




    }
}
