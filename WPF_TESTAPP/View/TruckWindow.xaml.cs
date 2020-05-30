using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using WPF_TESTAPP.Annotations;
using WPF_TESTAPP.ViewModels;

namespace WPF_TESTAPP.Views
{

    public partial class TruckWindow2 : Window, INotifyPropertyChanged
    {
        public TruckWindow2()
        {
            InitializeComponent();

            TruckViewModel = new TruckViewModel();
            TruckGrid.DataContext = TruckViewModel;


        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }


        private TruckViewModel _truckViewModel;
        public TruckViewModel TruckViewModel
        {
            get { return _truckViewModel; }
            set
            {
                if (_truckViewModel == value)
                    return;
                _truckViewModel = value;
                OnPropertyChanged(nameof(TruckViewModel));

            }
        }
    }
}
