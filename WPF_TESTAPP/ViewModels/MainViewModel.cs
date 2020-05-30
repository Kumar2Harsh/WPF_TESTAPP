using System.ComponentModel;
using System.Runtime.CompilerServices;
using WPF_TESTAPP.Annotations;
using WPF_TESTAPP.Models;

namespace WPF_TESTAPP.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        private MainModel _mainModel;

        public MainModel MainModel
        {
            get { return _mainModel; }
            set
            {
                if (_mainModel == value)
                    return;
                _mainModel = value;
                OnPropertyChanged(nameof(MainModel));

            }
        }



    }
}
