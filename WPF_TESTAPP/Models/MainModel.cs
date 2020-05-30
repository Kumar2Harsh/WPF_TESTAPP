using System.ComponentModel;
using System.Runtime.CompilerServices;
using WPF_TESTAPP.Annotations;

namespace WPF_TESTAPP.Models
{
    public class MainModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }



        private string _content;

        public string Content
        {
            get { return _content; }
            set
            {
                if (_content == value)
                    return;
                _content = value;
                OnPropertyChanged(nameof(Content));
            }
        }

       
    }
}
