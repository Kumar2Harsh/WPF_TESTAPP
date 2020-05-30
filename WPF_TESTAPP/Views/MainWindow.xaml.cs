using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using WPF_TESTAPP.Annotations;
using WPF_TESTAPP.Models;
using WPF_TESTAPP.ViewModels;

namespace WPF_TESTAPP.Views
{

    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public MainWindow()
        {
            InitializeComponent();

            MainViewModel = new MainViewModel();
            MyGrid.DataContext = MainViewModel;
            Content = "Testing the Application";

        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        private MainViewModel _mainViewModel;

        public MainViewModel MainViewModel
        {
            get { return _mainViewModel; }
            set
            {
                if (_mainViewModel == value)
                    return;
                _mainViewModel = value;
                OnPropertyChanged(nameof(MainViewModel));

            }
        }

    }
}
