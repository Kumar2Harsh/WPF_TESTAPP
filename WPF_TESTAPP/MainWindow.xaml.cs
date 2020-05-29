using System.Windows;
using WPF_TESTAPP.ViewModel;
using SCSSdkClient;
using SCSSdkClient.Object;
using System.Runtime.CompilerServices;

namespace WPF_TESTAPP
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SCSSdkTelemetry Telemetry;
        public TruckViewModel TVM = new TruckViewModel();

        public bool InvokeRequired { get; private set; }
        public string PB = "Test3";
        public MainWindow()
        {
            InitializeComponent();

            this.Telemetry = new SCSSdkTelemetry();
            Telemetry.Data += Telemetry_Data;
            Telemetry.JobStarted += TelemetryHandler.JobStarted;
            Telemetry.JobCancelled += TelemetryHandler.JobCancelled;
            Telemetry.JobDelivered += TelemetryHandler.JobDelivered;
            Telemetry.Fined += TelemetryHandler.Fined;
            Telemetry.Tollgate += TelemetryHandler.Tollgate;
            Telemetry.Ferry += TelemetryHandler.FerryUsed;
            Telemetry.Train += TelemetryHandler.TrainUsed;
            Telemetry.RefuelStart += TelemetryHandler.RefuelStart;
            Telemetry.RefuelEnd += TelemetryHandler.RefuelEnd;
            Telemetry.RefuelPayed += TelemetryHandler.RefuelPayed;

        }

        public virtual Telemetry Telemetry
        {
            get
            {
                return this._Telemetry;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                Telemetry.ETSJobCancelledEventHandler eTSJobCancelledEventHandler = new Telemetry.ETSJobCancelledEventHandler(this.Telemetry_JobCancelled);
                Telemetry.ETSPlayerFinedEventHandler eTSPlayerFinedEventHandler = new Telemetry.ETSPlayerFinedEventHandler(this.Telemetry_PlayerFined);
                Telemetry.ETSPlayerTollgatePaidEventHandler eTSPlayerTollgatePaidEventHandler = new Telemetry.ETSPlayerTollgatePaidEventHandler(this.Telemetry_PlayerTollgatePaid);
                Telemetry.ETSPlayerUseFerryEventHandler eTSPlayerUseFerryEventHandler = new Telemetry.ETSPlayerUseFerryEventHandler(this.Telemetry_PlayerUseFerry);
                Telemetry.ETSConnectionStatusChangedEventHandler eTSConnectionStatusChangedEventHandler = new Telemetry.ETSConnectionStatusChangedEventHandler(this.Telemetry_ETSConnectionStatusChanged);
                Telemetry.ETSGameNameChangedEventHandler eTSGameNameChangedEventHandler = new Telemetry.ETSGameNameChangedEventHandler(this.Telemetry_GameNameChanged);
                Telemetry.ETSJobDeliveredEventHandler eTSJobDeliveredEventHandler = new Telemetry.ETSJobDeliveredEventHandler(this.Telemetry_JobDelivered);
                Telemetry.ETSFreightChangedEventHandler eTSFreightChangedEventHandler = new Telemetry.ETSFreightChangedEventHandler(this.Telemetry_FreightChanged);
                Telemetry.ETSDataRefreshedEventHandler eTSDataRefreshedEventHandler = new Telemetry.ETSDataRefreshedEventHandler(this.Telemetry_DataRefreshed);
                Telemetry.OMSIConnectionStatusChangedEventHandler oMSIConnectionStatusChangedEventHandler = new Telemetry.OMSIConnectionStatusChangedEventHandler(this.BBS_connectionStateChanged);
                Telemetry.OMSIDelayChangedEventHandler oMSIDelayChangedEventHandler = new Telemetry.OMSIDelayChangedEventHandler(this.BBS_delayChanged);
                Telemetry.OMSIDestinationChangedEventHandler oMSIDestinationChangedEventHandler = new Telemetry.OMSIDestinationChangedEventHandler(this.BBS_destinationChanged);
                Telemetry.OMSIDataRefreshedEventHandler oMSIDataRefreshedEventHandler = new Telemetry.OMSIDataRefreshedEventHandler(this.Telemetry_OMSIDataRefreshed);
                Telemetry.OMSILineRouteDestChangedEventHandler oMSILineRouteDestChangedEventHandler = new Telemetry.OMSILineRouteDestChangedEventHandler(this.BBS_lineRouteDestChanged);
                Telemetry.OMSINextStopChangedEventHandler oMSINextStopChangedEventHandler = new Telemetry.OMSINextStopChangedEventHandler(this.BBS_nextStopChanged);
                Telemetry.OMSIPassengerCountChangedEventHandler oMSIPassengerCountChangedEventHandler = new Telemetry.OMSIPassengerCountChangedEventHandler(this.BBS_passengerCountChanged);
                Telemetry.OMSIPassengerInOutChangedEventHandler oMSIPassengerInOutChangedEventHandler = new Telemetry.OMSIPassengerInOutChangedEventHandler(this.BBS_passengerInOutChanged);
                Telemetry.OMSIStopStateChangedEventHandler oMSIStopStateChangedEventHandler = new Telemetry.OMSIStopStateChangedEventHandler(this.BBS_stopStateChanged);
                Telemetry telemetry = this._Telemetry;
                if (telemetry != null)
                {
                    telemetry.ETSJobCancelled -= eTSJobCancelledEventHandler;
                    telemetry.ETSPlayerFined -= eTSPlayerFinedEventHandler;
                    telemetry.ETSPlayerTollgatePaid -= eTSPlayerTollgatePaidEventHandler;
                    telemetry.ETSPlayerUseFerry -= eTSPlayerUseFerryEventHandler;
                    telemetry.ETSConnectionStatusChanged -= eTSConnectionStatusChangedEventHandler;
                    telemetry.ETSGameNameChanged -= eTSGameNameChangedEventHandler;
                    telemetry.ETSJobDelivered -= eTSJobDeliveredEventHandler;
                    telemetry.ETSFreightChanged -= eTSFreightChangedEventHandler;
                    telemetry.ETSDataRefreshed -= eTSDataRefreshedEventHandler;
                    telemetry.OMSIConnectionStatusChanged -= oMSIConnectionStatusChangedEventHandler;
                    telemetry.OMSIDelayChanged -= oMSIDelayChangedEventHandler;
                    telemetry.OMSIDestinationChanged -= oMSIDestinationChangedEventHandler;
                    telemetry.OMSIDataRefreshed -= oMSIDataRefreshedEventHandler;
                    telemetry.OMSILineRouteDestChanged -= oMSILineRouteDestChangedEventHandler;
                    telemetry.OMSINextStopChanged -= oMSINextStopChangedEventHandler;
                    telemetry.OMSIPassengerCountChanged -= oMSIPassengerCountChangedEventHandler;
                    telemetry.OMSIPassengerInOutChanged -= oMSIPassengerInOutChangedEventHandler;
                    telemetry.OMSIStopStateChanged -= oMSIStopStateChangedEventHandler;
                }
                this._Telemetry = value;
                telemetry = this._Telemetry;
                if (telemetry != null)
                {
                    telemetry.ETSJobCancelled += eTSJobCancelledEventHandler;
                    telemetry.ETSPlayerFined += eTSPlayerFinedEventHandler;
                    telemetry.ETSPlayerTollgatePaid += eTSPlayerTollgatePaidEventHandler;
                    telemetry.ETSPlayerUseFerry += eTSPlayerUseFerryEventHandler;
                    telemetry.ETSConnectionStatusChanged += eTSConnectionStatusChangedEventHandler;
                    telemetry.ETSGameNameChanged += eTSGameNameChangedEventHandler;
                    telemetry.ETSJobDelivered += eTSJobDeliveredEventHandler;
                    telemetry.ETSFreightChanged += eTSFreightChangedEventHandler;
                    telemetry.ETSDataRefreshed += eTSDataRefreshedEventHandler;
                    telemetry.OMSIConnectionStatusChanged += oMSIConnectionStatusChangedEventHandler;
                    telemetry.OMSIDelayChanged += oMSIDelayChangedEventHandler;
                    telemetry.OMSIDestinationChanged += oMSIDestinationChangedEventHandler;
                    telemetry.OMSIDataRefreshed += oMSIDataRefreshedEventHandler;
                    telemetry.OMSILineRouteDestChanged += oMSILineRouteDestChangedEventHandler;
                    telemetry.OMSINextStopChanged += oMSINextStopChangedEventHandler;
                    telemetry.OMSIPassengerCountChanged += oMSIPassengerCountChangedEventHandler;
                    telemetry.OMSIPassengerInOutChanged += oMSIPassengerInOutChangedEventHandler;
                    telemetry.OMSIStopStateChanged += oMSIStopStateChangedEventHandler;
                }
            }
        }


        private void button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
