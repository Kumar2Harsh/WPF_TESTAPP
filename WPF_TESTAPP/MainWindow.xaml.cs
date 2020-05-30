using System.Windows;
using WPF_TESTAPP.ViewModel;
using System.Runtime.CompilerServices;

namespace WPF_TESTAPP
{
   
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            TruckViewModel tb = new TruckViewModel();
            ((TruckViewModel)DataContext).AendereHersteller(tb.Hersteller);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ((TruckViewModel)DataContext).AendereHersteller("Hubertus");
        }
    }
}
