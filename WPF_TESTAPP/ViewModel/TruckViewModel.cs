using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPF_TESTAPP.ViewModel
{
    public class TruckViewModel : INotifyPropertyChanged
    {
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

    }
}
