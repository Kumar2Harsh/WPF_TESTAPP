using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_TESTAPP.ViewModel
{
    public class Person
    {
        public string Vorname { get; set; }
        public string Nachname { get; set; }

        private string _gesamtname;
        public string Gesamtname
        {
            get
            {
                return Vorname + " " + Nachname;
            }
            set
            {
                this._gesamtname = value;
            }
        }
    }
}
