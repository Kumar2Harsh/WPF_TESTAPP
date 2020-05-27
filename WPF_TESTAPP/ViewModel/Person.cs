using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_TESTAPP.ViewModel
{
    public class Person : INotifyPropertyChanged
    {
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private string _gesamtname;
        public string Gesamtname
        {
            get
            {
                return _gesamtname;
            }
            set
            {
                if (value != this._gesamtname)
                {
                    this._gesamtname = value;
                    NotifyPropertyChanged();
                    
                }
            }
        }


        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            MessageBox.Show(propertyName);
        }
    }
}
