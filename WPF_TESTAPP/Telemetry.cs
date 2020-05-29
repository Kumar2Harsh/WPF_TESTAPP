using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_TESTAPP
{
    public class Telemetry
    {
        private readonly static Logger Log;

        private ETSData v_innerDataETS;

        private TelemetryConnectionState v_etsconnectState;

        private ETSData v_oldDataETS;

        private OMSIData v_innerDataOMSI;

        private TelemetryConnectionState v_omsiconnectState;

        private OMSIData v_oldDataOMSI;

        private string v_path;

        private string v_gamename;

        private bool v_freightAvailable;

        private Thread t_data;

        private WebClient client;

        private int OMSIpessengersEntered;

        private int OMSIpessengersLeft;

        public TelemetryConnectionState ETSConnected
        {
            get
            {
                return this.v_etsconnectState;
            }
        }

        public ETSData ETSData
        {
            get
            {
                return this.v_innerDataETS;
            }
        }

        public TelemetryConnectionState OMSIConnected
        {
            get
            {
                return this.v_omsiconnectState;
            }
        }

        public OMSIData OMSIData
        {
            get
            {
                return this.v_innerDataOMSI;
            }
        }

        public int Time
        {
            get;
            set;
        }

        static Telemetry()
        {
            Telemetry.Log = LogManager.GetCurrentClassLogger();
        }

        public Telemetry(string path, int time)
        {
            this.v_innerDataETS = null;
            this.v_etsconnectState = TelemetryConnectionState.NotSet;
            this.v_oldDataETS = null;
            this.v_innerDataOMSI = null;
            this.v_omsiconnectState = TelemetryConnectionState.NotSet;
            this.v_oldDataOMSI = null;
            this.v_gamename = "";
            this.v_freightAvailable = false;
            this.t_data = new Thread(new ThreadStart(this.BackThread));
            this.client = new WebClient();
            this.OMSIpessengersEntered = 0;
            this.OMSIpessengersLeft = 0;
            this.Time = 1000;
            this.v_path = path;
            this.client.Encoding = new UTF8Encoding(false);
            this.t_data.Start();
        }

        private void BackThread()
        {
            while (true)
            {
                this.refreshData();
                if (this.v_etsconnectState == TelemetryConnectionState.NoConnect && this.v_omsiconnectState == TelemetryConnectionState.NoConnect)
                {
                    Thread.Sleep(15000);
                }
                else if ((this.v_etsconnectState == TelemetryConnectionState.NoGame || this.v_etsconnectState == TelemetryConnectionState.NoConnect) && (this.v_omsiconnectState == TelemetryConnectionState.NoGame || this.v_omsiconnectState == TelemetryConnectionState.NoConnect))
                {
                    Thread.Sleep(10000);
                }
                else if (this.v_omsiconnectState != TelemetryConnectionState.InGame)
                {
                    Thread.Sleep(this.Time);
                }
                else
                {
                    Thread.Sleep(200);
                }
            }
        }

        public string getData(string game)
        {
            string str;
            try
            {
                str = (Operators.CompareString(this.v_path, "127.0.0.1", false) != 0 ? this.GetRemoteData(game) : this.GetMemData(game));
            }
            catch (Exception exception)
            {
                ProjectData.SetProjectError(exception);
                str = "";
                ProjectData.ClearProjectError();
            }
            return str;
        }

        public Tuple<string, string> getData()
        {
            Tuple<string, string> tuple;
            try
            {
                if (Operators.CompareString(this.v_path, "127.0.0.1", false) != 0)
                {
                    JSONKeyValue jSONKeyValue = new JSONKeyValue(this.client.DownloadString(string.Concat("http://", this.v_path.Split(new char[] { ':' })[0], ":25552")));
                    tuple = new Tuple<string, string>(jSONKeyValue.JSON.GetValue("ets2").ToString(), jSONKeyValue.JSON.GetValue("omsi").ToString());
                }
                else
                {
                    tuple = new Tuple<string, string>(this.GetMemData("ets2"), this.GetMemData("omsi"));
                }
            }
            catch (Exception exception)
            {
                ProjectData.SetProjectError(exception);
                tuple = null;
                ProjectData.ClearProjectError();
            }
            return tuple;
        }

        [DllImport("Telemetry\\spedv-telemetry-32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "GetData", ExactSpelling = false)]
        private static extern bool GetETSData32(out string str);

        [DllImport("Telemetry\\spedv-telemetry-64.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "GetData", ExactSpelling = false)]
        private static extern bool GetETSData64(out string str);

        private string GetMemData(string game)
        {
            string str;
            string str1;
            string str2;
            while (true)
            {
                try
                {
                    if (Operators.CompareString(game, "ets2", false) != 0)
                    {
                        string str3 = "";
                        if (Telemetry.GetMemDataOMSI(ref str3))
                        {
                            str3 = str3.Replace("\0", "").Trim();
                            if (string.IsNullOrEmpty(str3))
                            {
                                str1 = null;
                            }
                            else
                            {
                                str1 = str3;
                            }
                            str = str1;
                            break;
                        }
                        else
                        {
                            str = null;
                            break;
                        }
                    }
                    else
                    {
                        string str4 = "";
                        if (Telemetry.GetMemDataETS(ref str4))
                        {
                            str4 = str4.Replace("\0", "").Trim();
                            if (string.IsNullOrEmpty(str4))
                            {
                                str2 = null;
                            }
                            else
                            {
                                str2 = str4;
                            }
                            str = str2;
                            break;
                        }
                        else
                        {
                            str = null;
                            break;
                        }
                    }
                }
                catch (FileNotFoundException fileNotFoundException)
                {
                    ProjectData.SetProjectError(fileNotFoundException);
                    str = null;
                    ProjectData.ClearProjectError();
                    break;
                }
                catch (Exception exception)
                {
                    ProjectData.SetProjectError(exception);
                    Telemetry.Log.Error(exception, string.Format("GetMemData {0}", game));
                    ProjectData.ClearProjectError();
                }
            }
            return str;
        }

        private static bool GetMemDataETS(ref string str)
        {
            bool flag;
            flag = (!Environment.Is64BitProcess ? Telemetry.GetETSData32(out str) : Telemetry.GetETSData64(out str));
            return flag;
        }

        private static bool GetMemDataOMSI(ref string str)
        {
            bool flag;
            flag = (!Environment.Is64BitProcess ? Telemetry.GetOMSIData32(out str) : Telemetry.GetOMSIData64(out str));
            return flag;
        }

        [DllImport("Telemetry\\omsi2-plugin.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "GetData", ExactSpelling = false)]
        private static extern bool GetOMSIData32(out string str);

        [DllImport("Telemetry\\omsi2-plugin-64.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "GetData", ExactSpelling = false)]
        private static extern bool GetOMSIData64(out string str);

        [DllImport("Telemetry\\omsi2-plugin.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "SetTrigger", ExactSpelling = false)]
        private static extern bool GetOMSITrigger32(string str);

        [DllImport("Telemetry\\omsi2-plugin-64.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "SetTrigger", ExactSpelling = false)]
        private static extern bool GetOMSITrigger64(string str);

        private string GetRemoteData(string game)
        {
            return (new JSONKeyValue(this.client.DownloadString(string.Concat("http://", this.v_path.Split(new char[] { ':' })[0], ":25552")))).GetValue<string>(game, null, false, null);
        }

        public ETSData refreshData()
        {

            Current member / type: FPH.SpedV.API.Objects.Telemetry.ETS.ETSData FPH.SpedV.API.Objects.Telemetry.Telemetry::refreshData()
File path: c: \users\edvbl\appdata\local\spedv\FPH SpedV-API.dll

Product version: 2019.1.118.0
Exception in: FPH.SpedV.API.Objects.Telemetry.ETS.ETSData refreshData()

Managed pointer usage not in SSA
bei ..(BinaryExpression ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Steps\ManagedPointersRemovalStep.cs:Zeile 100.
bei ..(BinaryExpression ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Steps\ManagedPointersRemovalStep.cs:Zeile 76.
bei ..Visit(ICodeNode ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Ast\BaseCodeVisitor.cs:Zeile 141.
bei ..() in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Steps\ManagedPointersRemovalStep.cs:Zeile 38.
bei ..(DecompilationContext ,  ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Steps\ManagedPointersRemovalStep.cs:Zeile 20.
bei ..(MethodBody ,  , ILanguage ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Decompiler\DecompilationPipeline.cs:Zeile 88.
bei ..(MethodBody , ILanguage ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Decompiler\DecompilationPipeline.cs:Zeile 70.
bei Telerik.JustDecompiler.Decompiler.Extensions.( , ILanguage , MethodBody , DecompilationContext & ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Decompiler\Extensions.cs:Zeile 95.
bei Telerik.JustDecompiler.Decompiler.Extensions.(MethodBody , ILanguage , DecompilationContext & ,  ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Decompiler\Extensions.cs:Zeile 58.
 bei ..(ILanguage , MethodDefinition ,  ) in C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Decompiler\WriterContextServices\BaseWriterContextService.cs:Zeile 117.

mailto: JustDecompilePublicFeedback@telerik.com

        }

        public bool SendAFK(string text)
        {
            bool flag;
            try
            {
                flag = Conversions.ToBoolean(this.client.DownloadString(string.Concat("http://", this.v_path.Split(new char[] { ':' })[0], ":25552/antiafk/", WebUtility.UrlEncode(text))));
            }
            catch (Exception exception)
            {
                ProjectData.SetProjectError(exception);
                flag = false;
                ProjectData.ClearProjectError();
            }
            return flag;
        }

        public bool SetETSOLData(OverlayData data)
        {
            bool flag;
            string str = JsonConvert.SerializeObject(data ?? new OverlayData());
            try
            {
                if (Operators.CompareString(this.v_path, "127.0.0.1", false) != 0)
                {
                    flag = JObject.Parse(this.client.UploadString(string.Concat("http://", this.v_path.Split(new char[] { ':' })[0], ":25552/ets2/setoldata"), str)).Value<bool>("result");
                    return flag;
                }
                else
                {
                    flag = Telemetry.SetETSOLData(str);
                    return flag;
                }
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                Telemetry.Log.Error(exception, "Set OL Data {data}", new object[] { str });
                ProjectData.ClearProjectError();
            }
            flag = false;
            return flag;
        }

        private static bool SetETSOLData(string str)
        {
            bool flag;
            flag = (!Environment.Is64BitProcess ? Telemetry.SetETSOLData32(str) : Telemetry.SetETSOLData64(str));
            return flag;
        }

        [DllImport("Telemetry\\spedv-telemetry-32.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Unicode, EntryPoint="SetOLData", ExactSpelling=false)]
        private static extern bool SetETSOLData32(string str);

        [DllImport("Telemetry\\spedv-telemetry-64.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Unicode, EntryPoint="SetOLData", ExactSpelling=false)]
        private static extern bool SetETSOLData64(string str);

        public bool SetOMSITrigger(TriggerData data)
        {
            bool flag;
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            string str = JsonConvert.SerializeObject(data);
            try
            {
                if (Operators.CompareString(this.v_path, "127.0.0.1", false) != 0)
                {
                    flag = JObject.Parse(this.client.UploadString(string.Concat("http://", this.v_path.Split(new char[] { ':' })[0], ":25552/omsi/settrigger"), str)).Value<bool>("result");
                    return flag;
                }
                else
                {
                    flag = Telemetry.SetOMSITrigger(str);
                    return flag;
                }
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                Telemetry.Log.Error(exception, "Set OMSI Trigger {data}", new object[] { str });
                ProjectData.ClearProjectError();
            }
            flag = false;
            return flag;
        }

        private static bool SetOMSITrigger(string str)
        {
            bool flag;
            flag = (!Environment.Is64BitProcess ? Telemetry.GetOMSITrigger32(str) : Telemetry.GetOMSITrigger64(str));
            return flag;
        }

        public event Telemetry.ETSConnectionStatusChangedEventHandler ETSConnectionStatusChanged;

        public event Telemetry.ETSDataRefreshedEventHandler ETSDataRefreshed;

        public event Telemetry.ETSFreightChangedEventHandler ETSFreightChanged;

        public event Telemetry.ETSGameNameChangedEventHandler ETSGameNameChanged;

        public event Telemetry.ETSJobCancelledEventHandler ETSJobCancelled;

        public event Telemetry.ETSJobDeliveredEventHandler ETSJobDelivered;

        public event Telemetry.ETSPlayerFinedEventHandler ETSPlayerFined;

        public event Telemetry.ETSPlayerTeleportedEventHandler ETSPlayerTeleported;

        public event Telemetry.ETSPlayerTollgatePaidEventHandler ETSPlayerTollgatePaid;

        public event Telemetry.ETSPlayerUseFerryEventHandler ETSPlayerUseFerry;

        public event Telemetry.ETSTrailerAttachedStateChangedEventHandler ETSTrailerAttachedStateChanged;

        public event Telemetry.OMSIConnectionStatusChangedEventHandler OMSIConnectionStatusChanged;

        public event Telemetry.OMSIDataRefreshedEventHandler OMSIDataRefreshed;

        public event Telemetry.OMSIDelayChangedEventHandler OMSIDelayChanged;

        public event Telemetry.OMSIDestinationChangedEventHandler OMSIDestinationChanged;

        public event Telemetry.OMSIDoorStateChangedEventHandler OMSIDoorStateChanged;

        public event Telemetry.OMSIFuelStateChangedEventHandler OMSIFuelStateChanged;

        public event Telemetry.OMSILineRouteDestChangedEventHandler OMSILineRouteDestChanged;

        public event Telemetry.OMSINextStopChangedEventHandler OMSINextStopChanged;

        public event Telemetry.OMSIPassengerCountChangedEventHandler OMSIPassengerCountChanged;

        public event Telemetry.OMSIPassengerInOutChangedEventHandler OMSIPassengerInOutChanged;

        public event Telemetry.OMSISpeedChangedEventHandler OMSISpeedChanged;

        public event Telemetry.OMSIStopStateChangedEventHandler OMSIStopStateChanged;

        public event Telemetry.OMSITempInsideChangedEventHandler OMSITempInsideChanged;

        public event Telemetry.OMSITempOutsideChangedEventHandler OMSITempOutsideChanged;

        public event Telemetry.OMSITimeChangedEventHandler OMSITimeChanged;

        public event Telemetry.OMSITimetableActiveChangedEventHandler OMSITimetableActiveChanged;

        public delegate void ETSConnectionStatusChangedEventHandler(TelemetryConnectionState newState, ETSData newData);

        public delegate void ETSDataRefreshedEventHandler(ETSData newData, ETSData oldData);

        public delegate void ETSFreightChangedEventHandler(ETSData newData, ETSData olddata);

        public delegate void ETSGameNameChangedEventHandler(string newGame);

        public delegate void ETSJobCancelledEventHandler(JobCancelledEvent e);

        public delegate void ETSJobDeliveredEventHandler(JobDeliveredEvent e);

        public delegate void ETSPlayerFinedEventHandler(PlayerFinedEvent e);

        public delegate void ETSPlayerTeleportedEventHandler(PlayerTeleportedEvent e);

        public delegate void ETSPlayerTollgatePaidEventHandler(PlayerTollgatePaidEvent e);

        public delegate void ETSPlayerUseFerryEventHandler(PlayerUseFerryEvent e);

        public delegate void ETSTrailerAttachedStateChangedEventHandler(ETSData newState, ETSData oldData);

        public delegate void OMSIConnectionStatusChangedEventHandler(TelemetryConnectionState newState, OMSIData newData);

        public delegate void OMSIDataRefreshedEventHandler(OMSIData newData, OMSIData oldData);

        public delegate void OMSIDelayChangedEventHandler(TimeSpan newDelay);

        public delegate void OMSIDestinationChangedEventHandler(string newDest);

        public delegate void OMSIDoorStateChangedEventHandler(int door, bool newState);

        public delegate void OMSIFuelStateChangedEventHandler(double newState);

        public delegate void OMSILineRouteDestChangedEventHandler(string newLineCode, string newRouteCode, string newDestCode, string direction);

        public delegate void OMSINextStopChangedEventHandler(string nextStop);

        public delegate void OMSIPassengerCountChangedEventHandler(int newPassenger);

        public delegate void OMSIPassengerInOutChangedEventHandler(int inPassenger, int outPassenger);

        public delegate void OMSISpeedChangedEventHandler(double newSpeed);

        public delegate void OMSIStopStateChangedEventHandler(bool newState);

        public delegate void OMSITempInsideChangedEventHandler(string newTemp);

        public delegate void OMSITempOutsideChangedEventHandler(int newTemp);

        public delegate void OMSITimeChangedEventHandler(DateTime time);

        public delegate void OMSITimetableActiveChangedEventHandler(bool newState);
    }
    }
}
