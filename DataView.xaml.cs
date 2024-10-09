using Microsoft.EntityFrameworkCore;
using Nager.TcpClient;
using Newtonsoft.Json;
using Serilog;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace BurnInTestTool
{
    public class FloatToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is float floatValue)
            {
                return floatValue.ToString("0.00"); // Format to 2 decimal places
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    /// 
    public class DataViewViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private DataViewDataClass? _boardData;

        private float _ecuVoltageAvg;
        private float _ecuVoltageMax;
        private float _ecuVoltageMin;

        private float _ecuCurrentAvg;
        private float _ecuCurrentMax;
        private float _ecuCurrentMin;


        private float _pn51xxVoltageAvg;
        private float _pn51xxVoltageMax;
        private float _pn51xxVoltageMin;

        private float _pn51xxCurrentAvg;
        private float _pn51xxCurrentMax;
        private float _pn51xxCurrentMin;

        private float _ecuPowerAvg;
        private float _ecuPowerMax;
        private float _ecuPowerMin;


        private float _ecuCPUUsageAvg;
        private float _ecuCPUUsageMax;
        private float _ecuCPUUsageMin;

        private float _ecuCPUFreqAvg1;
        private float _ecuCPUFreqMax1;
        private float _ecuCPUFreqMin1;

        private float _ecuCPUFreqAvg2;
        private float _ecuCPUFreqMax2;
        private float _ecuCPUFreqMin2;

        private float _ecuCPUFreqAvg3;
        private float _ecuCPUFreqMax3;
        private float _ecuCPUFreqMin3;

        private float _ecuCPUFreqAvg0;
        private float _ecuCPUFreqMax0;
        private float _ecuCPUFreqMin0;

        private float _ctrCPUFreqAvg0;
        private float _ctrCPUFreqMax0;
        private float _ctrCPUFreqMin0;

        private float _ctrCPUFreqAvg1;
        private float _ctrCPUFreqMax1;
        private float _ctrCPUFreqMin1;

        private float _ecuCPUTemperatureAvg;
        private float _ecuCPUTemperatureMax;
        private float _ecuCPUTemperatureMin;


        private float _ctrCPUTemperatureAvg;
        private float _ctrCPUTemperatureMax;
        private float _ctrCPUTemperatureMin;

        private float _pn51xxTemperatureAvg;
        private float _pn51xxTemperatureMax;
        private float _pn51xxTemperatureMin;

        private float _ecuUsedMemAvg;
        private float _ecuUsedMemMax;
        private float _ecuUsedMemMin;

        private float _ctrUsedMemAvg;
        private float _ctrUsedMemMax;
        private float _ctrUsedMemMin;

        private float _ctrCPUUsageAvg;
        private float _ctrCPUUsageMax;
        private float _ctrCPUUsageMin;
        public float EcuVoltageAvg
        {
            get { return _ecuVoltageAvg; }
            set
            {
                if (_ecuVoltageAvg != value)
                {
                    _ecuVoltageAvg = value;
                    OnPropertyChanged(nameof(EcuVoltageAvg)); // Notify when the voltage changes
                }
            }
        }
        public DataViewDataClass? BoardData
        {
            get
            {
                return _boardData;
            }
            set
            {
                if (_boardData != value)
                {
                    _boardData = value;
                    OnPropertyChanged(nameof(BoardData));  // Notify when the property changes
                }
            }
        }

        public float EcuVoltageMax
        {
            get { return _ecuVoltageMax; }
            set
            {
                if (_ecuVoltageMax != value)
                {
                    _ecuVoltageMax = value;
                    OnPropertyChanged(nameof(EcuVoltageMax));
                }
            }
        }

        public float EcuVoltageMin { get => _ecuVoltageMin; set { if (_ecuVoltageMin != value) { _ecuVoltageMin = value; OnPropertyChanged(nameof(EcuVoltageMin)); }; } }

        public float EcuCurrentAvg { get => _ecuCurrentAvg; set { if (_ecuCurrentAvg != value) { _ecuCurrentAvg = value; OnPropertyChanged(nameof(EcuCurrentAvg)); } } }
        public float EcuCurrentMax { get => _ecuCurrentMax; set { if (_ecuCurrentMax != value) { _ecuCurrentMax = value; OnPropertyChanged(nameof(EcuCurrentMax)); } } }
        public float EcuCurrentMin { get => _ecuCurrentMin; set { if (_ecuCurrentMin != value) { _ecuCurrentMin = value; OnPropertyChanged(nameof(EcuCurrentMin)); } } }


        public float EcuPowerMin { get => _ecuPowerMin; set { if (_ecuPowerMin != value) { _ecuPowerMin = value; OnPropertyChanged(nameof(EcuPowerMin)); } } }
        public float EcuPowerMax { get => _ecuPowerMax; set { if (_ecuPowerMax != value) { _ecuPowerMax = value; OnPropertyChanged(nameof(EcuPowerMax)); } } }
        public float EcuPowerAvg { get => _ecuPowerAvg; set { if (_ecuPowerAvg != value) { _ecuPowerAvg = value; OnPropertyChanged(nameof(EcuPowerAvg)); } } }

        public float EcuCPUUsageMin { get => _ecuCPUUsageMin; set { if (_ecuCPUUsageMin != value) { _ecuCPUUsageMin = value; OnPropertyChanged(nameof(EcuCPUUsageMin)); } } }
        public float EcuCPUUsageMax { get => _ecuCPUUsageMax; set { if (_ecuCPUUsageMax != value) { _ecuCPUUsageMax = value; OnPropertyChanged(nameof(EcuCPUUsageMax)); } } }
        public float EcuCPUUsageAvg { get => _ecuCPUUsageAvg; set { if (_ecuCPUUsageAvg != value) { _ecuCPUUsageAvg = value; OnPropertyChanged(nameof(EcuCPUUsageAvg)); } } }

        public float CtrCPUUsageMin { get => _ctrCPUUsageMin; set { if (_ctrCPUUsageMin != value) { _ctrCPUUsageMin = value; OnPropertyChanged(nameof(CtrCPUUsageMin)); } } }
        public float CtrCPUUsageMax { get => _ctrCPUUsageMax; set { if (_ctrCPUUsageMax != value) { _ctrCPUUsageMax = value; OnPropertyChanged(nameof(CtrCPUUsageMax)); } } }
        public float CtrCPUUsageAvg { get => _ctrCPUUsageAvg; set { if (_ctrCPUUsageAvg != value) { _ctrCPUUsageAvg = value; OnPropertyChanged(nameof(CtrCPUUsageAvg)); } } }

        public float EcuCPUFreqAvg0 { get => _ecuCPUFreqAvg0; set { if (_ecuCPUFreqAvg0 != value) { _ecuCPUFreqAvg0 = value; OnPropertyChanged(nameof(EcuCPUFreqAvg0)); } } }
        public float EcuCPUFreqMax0 { get => _ecuCPUFreqMax0; set { if (_ecuCPUFreqMax0 != value) { _ecuCPUFreqMax0 = value; OnPropertyChanged(nameof(EcuCPUFreqMax0)); } } }
        public float EcuCPUFreqMin0 { get => _ecuCPUFreqMin0; set { if (_ecuCPUFreqMin0 != value) { _ecuCPUFreqMin0 = value; OnPropertyChanged(nameof(EcuCPUFreqMin0)); } } }



        public float EcuCPUFreqAvg1 { get => _ecuCPUFreqAvg1; set { if (_ecuCPUFreqAvg1 != value) { _ecuCPUFreqAvg1 = value; OnPropertyChanged(nameof(EcuCPUFreqAvg1)); } } }
        public float EcuCPUFreqMax1 { get => _ecuCPUFreqMax1; set { if (_ecuCPUFreqMax1 != value) { _ecuCPUFreqMax1 = value; OnPropertyChanged(nameof(EcuCPUFreqMax1)); } } }
        public float EcuCPUFreqMin1 { get => _ecuCPUFreqMin1; set { if (_ecuCPUFreqMin1 != value) { _ecuCPUFreqMin1 = value; OnPropertyChanged(nameof(EcuCPUFreqMin1)); } } }


        public float EcuCPUFreqAvg2 { get => _ecuCPUFreqAvg2; set { if (_ecuCPUFreqAvg2 != value) { _ecuCPUFreqAvg2 = value; OnPropertyChanged(nameof(EcuCPUFreqAvg2)); } } }
        public float EcuCPUFreqMax2 { get => _ecuCPUFreqMax2; set { if (_ecuCPUFreqMax2 != value) { _ecuCPUFreqMax2 = value; OnPropertyChanged(nameof(EcuCPUFreqMax2)); } } }
        public float EcuCPUFreqMin2 { get => _ecuCPUFreqMin2; set { if (_ecuCPUFreqMin2 != value) { _ecuCPUFreqMin2 = value; OnPropertyChanged(nameof(EcuCPUFreqMin2)); } } }


        public float EcuCPUFreqAvg3 { get => _ecuCPUFreqAvg3; set { if (_ecuCPUFreqAvg3 != value) { _ecuCPUFreqAvg3 = value; OnPropertyChanged(nameof(EcuCPUFreqAvg3)); } } }
        public float EcuCPUFreqMax3 { get => _ecuCPUFreqMax3; set { if (_ecuCPUFreqMax3 != value) { _ecuCPUFreqMax3 = value; OnPropertyChanged(nameof(EcuCPUFreqMax3)); } } }
        public float EcuCPUFreqMin3 { get => _ecuCPUFreqMin3; set { if (_ecuCPUFreqMin3 != value) { _ecuCPUFreqMin3 = value; OnPropertyChanged(nameof(EcuCPUFreqMin3)); } } }


        public float CtrCPUFreqAvg0 { get => _ctrCPUFreqAvg0; set { if (_ctrCPUFreqAvg0 != value) { _ctrCPUFreqAvg0 = value; OnPropertyChanged(nameof(CtrCPUFreqAvg0)); } } }
        public float CtrCPUFreqMax0 { get => _ctrCPUFreqMax0; set { if (_ctrCPUFreqMax0 != value) { _ctrCPUFreqMax0 = value; OnPropertyChanged(nameof(CtrCPUFreqMax0)); } } }
        public float CtrCPUFreqMin0 { get => _ctrCPUFreqMin0; set { if (_ctrCPUFreqMin0 != value) { _ctrCPUFreqMin0 = value; OnPropertyChanged(nameof(CtrCPUFreqMin0)); } } }

        public float CtrCPUFreqAvg1 { get => _ctrCPUFreqAvg1; set { if (_ctrCPUFreqAvg1 != value) { _ctrCPUFreqAvg1 = value; OnPropertyChanged(nameof(CtrCPUFreqAvg1)); } } }
        public float CtrCPUFreqMax1 { get => _ctrCPUFreqMax1; set { if (_ctrCPUFreqMax1 != value) { _ctrCPUFreqMax1 = value; OnPropertyChanged(nameof(CtrCPUFreqMax1)); } } }
        public float CtrCPUFreqMin1 { get => _ctrCPUFreqMin1; set { if (_ctrCPUFreqMin1 != value) { _ctrCPUFreqMin1 = value; OnPropertyChanged(nameof(CtrCPUFreqMin1)); } } }

        public float EcuCPUTemperatureMin { get => _ecuCPUTemperatureMin; set { if (_ecuCPUTemperatureMin != value) { _ecuCPUTemperatureMin = value; OnPropertyChanged(nameof(EcuCPUTemperatureMin)); } } }
        public float EcuCPUTemperatureMax { get => _ecuCPUTemperatureMax; set { if (_ecuCPUTemperatureMax != value) { _ecuCPUTemperatureMax = value; OnPropertyChanged(nameof(EcuCPUTemperatureMax)); } } }
        public float EcuCPUTemperatureAvg { get => _ecuCPUTemperatureAvg; set { if (_ecuCPUTemperatureAvg != value) { _ecuCPUTemperatureAvg = value; OnPropertyChanged(nameof(EcuCPUTemperatureAvg)); } } }

        public float CtrCPUTemperatureMin { get => _ctrCPUTemperatureMin; set { if (_ctrCPUTemperatureMin != value) { _ctrCPUTemperatureMin = value; OnPropertyChanged(nameof(CtrCPUTemperatureMin)); } } }
        public float CtrCPUTemperatureMax { get => _ctrCPUTemperatureMax; set { if (_ctrCPUTemperatureMax != value) { _ctrCPUTemperatureMax = value; OnPropertyChanged(nameof(CtrCPUTemperatureMax)); } } }
        public float CtrCPUTemperatureAvg { get => _ctrCPUTemperatureAvg; set { if (_ctrCPUTemperatureAvg != value) { _ctrCPUTemperatureAvg = value; OnPropertyChanged(nameof(CtrCPUTemperatureAvg)); } } }


        public float EcuUsedMemMin { get => _ecuUsedMemMin; set { if (_ecuUsedMemMin != value) { _ecuUsedMemMin = value; OnPropertyChanged(nameof(EcuUsedMemMin)); } } }
        public float EcuUsedMemMax { get => _ecuUsedMemMax; set { if (_ecuUsedMemMax != value) { _ecuUsedMemMax = value; OnPropertyChanged(nameof(EcuUsedMemMax)); } } }
        public float EcuUsedMemAvg { get => _ecuUsedMemAvg; set { if (_ecuUsedMemAvg != value) { _ecuUsedMemAvg = value; OnPropertyChanged(nameof(EcuUsedMemAvg)); } } }

        public float CtrUsedMemMin { get => _ctrUsedMemMin; set { if (_ctrUsedMemMin != value) { _ctrUsedMemMin = value; OnPropertyChanged(nameof(CtrUsedMemMin)); } } }
        public float CtrUsedMemMax { get => _ctrUsedMemMax; set { if (_ctrUsedMemMax != value) { _ctrUsedMemMax = value; OnPropertyChanged(nameof(CtrUsedMemMax)); } } }
        public float CtrUsedMemAvg { get => _ctrUsedMemAvg; set { if (_ctrUsedMemAvg != value) { _ctrUsedMemAvg = value; OnPropertyChanged(nameof(CtrUsedMemAvg)); } } }

        public float PN51xxCurrentAvg { get => _pn51xxCurrentAvg; set { if (_pn51xxCurrentAvg != value) { _pn51xxCurrentAvg = value; OnPropertyChanged(nameof(PN51xxCurrentAvg)); } } }
        public float PN51xxCurrentMax { get => _pn51xxCurrentMax; set { if (_pn51xxCurrentMax != value) { _pn51xxCurrentMax = value; OnPropertyChanged(nameof(PN51xxCurrentMax)); } } }
        public float PN51xxCurrentMin { get => _pn51xxCurrentMin; set { if (_pn51xxCurrentMin != value) { _pn51xxCurrentMin = value; OnPropertyChanged(nameof(PN51xxCurrentMin)); } } }


        public float PN51xxVoltageAvg { get => _pn51xxVoltageAvg; set { if (_pn51xxVoltageAvg != value) { _pn51xxVoltageAvg = value; OnPropertyChanged(nameof(PN51xxVoltageAvg)); } } }
        public float PN51xxVoltageMax { get => _pn51xxVoltageMax; set { if (_pn51xxVoltageMax != value) { _pn51xxVoltageMax = value; OnPropertyChanged(nameof(PN51xxVoltageMax)); } } }
        public float PN51xxVoltageMin { get => _pn51xxVoltageMin; set { if (_pn51xxVoltageMin != value) { _pn51xxVoltageMin = value; OnPropertyChanged(nameof(PN51xxVoltageMin)); } } }


        public float PN51xxTemperatureMin { get => _pn51xxTemperatureMin; set { if (_pn51xxTemperatureMin != value) { _pn51xxTemperatureMin = value; OnPropertyChanged(nameof(PN51xxTemperatureMin)); } } }
        public float PN51xxTemperatureMax { get => _pn51xxTemperatureMax; set { if (_pn51xxTemperatureMax != value) { _pn51xxTemperatureMax = value; OnPropertyChanged(nameof(PN51xxTemperatureMax)); } } }
        public float PN51xxTemperatureAvg { get => _pn51xxTemperatureAvg; set { if (_pn51xxTemperatureAvg != value) { _pn51xxTemperatureAvg = value; OnPropertyChanged(nameof(PN51xxTemperatureAvg)); } } }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DataViewDataClass
    {
        public string? IpAddress { get; set; }
        public string? MacAddress { get; set; }




        public override bool Equals(object? obj)
        {
            return obj is DataViewDataClass @class &&
                   IpAddress == @class.IpAddress &&
                   MacAddress == @class.MacAddress;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IpAddress, MacAddress);
        }
    }
    public partial class DataView : UserControl
    {
        private DataViewViewModel viewModel { get; set; }
        public EventHandler? RemoveRequested;
        private const int port = 6868;
        TcpClient? tcpClient;
        CancellationTokenSource? tcpClientConnectCancellationTokenSource;
        private List<Byte> dataFrame = [];
        private List<Byte> receiveBuffer = [];
        private readonly List<Byte> startByte = [0x86, 0x66];
        public DataView()
        {
            InitializeComponent();
            viewModel = new DataViewViewModel();
            DataContext = viewModel;
            this.Loaded += ConnectToTCPServer;
            this.Unloaded += DisconnectToTCPServer;
            ApplyRandomBackgroundColor();
        }

        private void ConnectToTCPServer(Object sender, EventArgs e)
        {
            try
            {
                if (viewModel.BoardData?.IpAddress is not null)
                {
                    tcpClientConnectCancellationTokenSource = new CancellationTokenSource(5000);
                    tcpClient = new TcpClient();
                    tcpClient.DataReceived += OnDataReceived;
                    tcpClient.Disconnected += OnDisconnected;
                    dataFrame.Clear();
                    Task.Run(() =>
                    {
                        bool connected = false;
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();
                        do
                        {
                            connected = tcpClient.ConnectAsync(viewModel.BoardData.IpAddress, port, tcpClientConnectCancellationTokenSource.Token).Result;
                            if (connected == false)
                                Thread.Sleep(1000);
                        }
                        while (connected == false && stopwatch.ElapsedMilliseconds <= 10000);
                        stopwatch.Stop();
                        if (connected == false)
                        {
                            RemoveRequested?.Invoke(this, EventArgs.Empty);
                        }

                    });


                }
            }
            catch (Exception ex)
            {
                {
                    Log.Error(ex.Message);
                }
            }
        }

        private void ApplyRandomBackgroundColor()
        {
            // Create a new Random instance
            Random random = new Random();

            // Generate random RGB values
            byte r = (byte)random.Next(100, 256); // Minimum 100 to avoid very dark red
            byte g = (byte)random.Next(100, 256); // Minimum 100 to avoid very dark green
            byte b = (byte)random.Next(100, 256); // Minimum 100 to avoid very dark blue


            // Create a new SolidColorBrush with the random color
            SolidColorBrush randomBrush = new SolidColorBrush(Color.FromRgb(r, g, b));

            // Set the background of the UserControl (or the Grid in this case)
            DataViewGrid.Background = randomBrush;
        }
        private void DisconnectToTCPServer(Object sender, EventArgs e)
        {
            try
            {
                if (tcpClient is not null)
                {
                    tcpClient.Disconnect();
                    tcpClient.DataReceived -= OnDataReceived;
                }
            }
            catch (Exception ex)
            {
                {
                    Log.Error(ex.Message);
                }
            }
        }

        public void SetBoardID(BoardID boardID)
        {
            viewModel.BoardData = new DataViewDataClass();
            viewModel.BoardData.IpAddress = boardID.IpAddress;
            viewModel.BoardData.MacAddress = boardID.MacAddress;



        }
        public BoardID? GetBoardID()
        {
            BoardID? boardID = new BoardID();
            boardID.IpAddress = viewModel.BoardData?.IpAddress;
            boardID.MacAddress = viewModel.BoardData?.MacAddress;
            return boardID;
        }

        private Int32 FindStartByte(List<Byte> buffer)
        {
            if (buffer.Count >= 2)
            {
                Int32 startByte1 = buffer.IndexOf(0x86);
                if (startByte1 >= 0)
                {
                    if (buffer.ElementAt(startByte1) == 0x86 && buffer.ElementAt(startByte1 + 1) == 0x66)
                    {
                        return startByte1;
                    }
                }
            }
            return -1;
        }

        private Int32 GetFrameLength(ref List<Byte> buffer)
        {
            Int32 StartPos = FindStartByte(buffer);
            Int32 FrameLength = -1;
            if (StartPos >= 0 && buffer.Count >= 4)
            {
                FrameLength = (buffer.ElementAt(StartPos + 2) << 8 | buffer.ElementAt(StartPos + 3)) + 5; // 2 start byte + 1 checksum + 2 byte length
                try
                {
                    buffer.RemoveRange(0, StartPos);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                }
            }

            return FrameLength;
        }

        private void OnDataReceived(byte[] receivedData)
        {
            Int32 FrameLength;
            try
            {
                lock (receiveBuffer)
                {
                    receiveBuffer.AddRange(receivedData);
                    do
                    {
                        FrameLength = GetFrameLength(ref receiveBuffer);
                        if (FrameLength > 0)
                        {
                            if (receiveBuffer.Count >= FrameLength)
                            {
                                List<Byte> oneFrame = receiveBuffer.GetRange(0, FrameLength);
                                receiveBuffer.RemoveRange(0, oneFrame.Count);
                                oneFrame.RemoveRange(0, 4);
                                oneFrame.RemoveAt(oneFrame.Count - 1);
                                if (viewModel.BoardData is not null)
                                {
                                    SysInfoData? sysInfoData = JsonConvert.DeserializeObject<SysInfoData>(Encoding.UTF8.GetString(oneFrame.ToArray()));
                                    using (var context = new DatabaseManager())
                                    {
                                        using (var transaction = context.Database.BeginTransaction())
                                        {
                                            try
                                            {
                                                if (sysInfoData != null)
                                                {
                                                    context.SysInfoData.Add(sysInfoData);
                                                    context.SaveChanges();
                                                    transaction.Commit();
                                                    if (viewModel.BoardData.MacAddress != null)
                                                    {
                                                        try
                                                        {
                                                            viewModel.EcuVoltageAvg = context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Average(item => item.EcuSysInfo!.Voltage);
                                                            viewModel.EcuVoltageMax = context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Max(item => item.EcuSysInfo!.Voltage);
                                                            viewModel.EcuVoltageMin = context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Min(item => item.EcuSysInfo!.Voltage);

                                                            viewModel.EcuCurrentAvg = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Average(item => item.EcuSysInfo!.Current);
                                                            viewModel.EcuCurrentMax = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Max(item => item.EcuSysInfo!.Current);
                                                            viewModel.EcuCurrentMin = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Min(item => item.EcuSysInfo!.Current);

                                                            viewModel.EcuPowerAvg = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Average(item => item.EcuSysInfo!.Power);
                                                            viewModel.EcuPowerMax = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Max(item => item.EcuSysInfo!.Power);
                                                            viewModel.EcuPowerMin = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Min(item => item.EcuSysInfo!.Power);

                                                            viewModel.EcuCPUUsageAvg = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Average(item => item.EcuSysInfo!.CpuUsage);
                                                            viewModel.EcuCPUUsageMax = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Max(item => item.EcuSysInfo!.CpuUsage);
                                                            viewModel.EcuCPUUsageMin = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Min(item => item.EcuSysInfo!.CpuUsage);
                                                            
                                                            viewModel.EcuCPUFreqAvg0 = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Average(item => item.EcuSysInfo!.CpuFrequency!.ElementAt(0));
                                                            viewModel.EcuCPUFreqMax0 = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Max(item => item.EcuSysInfo!.CpuFrequency!.ElementAt(0));
                                                            viewModel.EcuCPUFreqMin0 = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Min(item => item.EcuSysInfo!.CpuFrequency!.ElementAt(0));



                                                            viewModel.EcuCPUFreqAvg1 = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Average(item => item.EcuSysInfo!.CpuFrequency!.ElementAt(1));
                                                            viewModel.EcuCPUFreqMax1 = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Max(item => item.EcuSysInfo!.CpuFrequency!.ElementAt(1));
                                                            viewModel.EcuCPUFreqMin1 = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Min(item => item.EcuSysInfo!.CpuFrequency!.ElementAt(1));



                                                            viewModel.EcuCPUFreqAvg2 = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Average(item => item.EcuSysInfo!.CpuFrequency!.ElementAt(2));
                                                            viewModel.EcuCPUFreqMax2 = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Max(item => item.EcuSysInfo!.CpuFrequency!.ElementAt(2));
                                                            viewModel.EcuCPUFreqMin2 = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Min(item => item.EcuSysInfo!.CpuFrequency!.ElementAt(2));


                                                            viewModel.EcuCPUFreqAvg3 = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Average(item => item.EcuSysInfo!.CpuFrequency!.ElementAt(3));
                                                            viewModel.EcuCPUFreqMax3 = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Max(item => item.EcuSysInfo!.CpuFrequency!.ElementAt(3));
                                                            viewModel.EcuCPUFreqMin3 = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Min(item => item.EcuSysInfo!.CpuFrequency!.ElementAt(3));


                                                            viewModel.EcuCPUTemperatureAvg = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Average(item => item.EcuSysInfo!.CpuTemperature);
                                                            viewModel.EcuCPUTemperatureMax = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Max(item => item.EcuSysInfo!.CpuTemperature);
                                                            viewModel.EcuCPUTemperatureMin = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Min(item => item.EcuSysInfo!.CpuTemperature);


                                                            viewModel.EcuUsedMemAvg = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Average(item => item.EcuSysInfo!.UsedMemory);
                                                            viewModel.EcuUsedMemMax = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Max(item => item.EcuSysInfo!.UsedMemory);
                                                            viewModel.EcuUsedMemMin = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Min(item => item.EcuSysInfo!.UsedMemory);

                                                            viewModel.CtrUsedMemAvg = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Average(item => item.CtrSysInfo!.UsedMemory);
                                                            viewModel.CtrUsedMemMax = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Max(item => item.CtrSysInfo!.UsedMemory);
                                                            viewModel.CtrUsedMemMin = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Min(item => item.CtrSysInfo!.UsedMemory);

                                                            viewModel.CtrCPUTemperatureAvg = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Average(item => item.CtrSysInfo!.CpuTemperature);
                                                            viewModel.CtrCPUTemperatureMax = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Max(item => item.CtrSysInfo!.CpuTemperature);
                                                            viewModel.CtrCPUTemperatureMin = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Min(item => item.CtrSysInfo!.CpuTemperature);

                                                            viewModel.CtrCPUFreqAvg0 = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Average(item => item.CtrSysInfo!.CpuFrequency!.ElementAt(0));
                                                            viewModel.CtrCPUFreqMax0 = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Max(item => item.CtrSysInfo!.CpuFrequency!.ElementAt(0));
                                                            viewModel.CtrCPUFreqMin0 = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Min(item => item.CtrSysInfo!.CpuFrequency!.ElementAt(0));

                                                            viewModel.CtrCPUFreqAvg1 = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Average(item => item.CtrSysInfo!.CpuFrequency!.ElementAt(1));
                                                            viewModel.CtrCPUFreqMax1 = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Max(item => item.CtrSysInfo!.CpuFrequency!.ElementAt(1));
                                                            viewModel.CtrCPUFreqMin1 = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Min(item => item.CtrSysInfo!.CpuFrequency!.ElementAt(1));

                                                            viewModel.CtrCPUUsageAvg = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Average(item => item.CtrSysInfo!.CpuUsage);
                                                            viewModel.CtrCPUUsageMax = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Max(item => item.CtrSysInfo!.CpuUsage);
                                                            viewModel.CtrCPUUsageMin = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Min(item => item.CtrSysInfo!.CpuUsage);

                                                            viewModel.PN51xxTemperatureAvg = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Average(item => item.Pn51xxSysInfo!.Temperature);
                                                            viewModel.PN51xxTemperatureMax = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Max(item => item.Pn51xxSysInfo!.Temperature);
                                                            viewModel.PN51xxTemperatureMin = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Min(item => item.Pn51xxSysInfo!.Temperature);

                                                            viewModel.PN51xxCurrentAvg = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Average(item => item.Pn51xxSysInfo!.Iddpa);
                                                            viewModel.PN51xxCurrentMax = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Max(item => item.Pn51xxSysInfo!.Iddpa);
                                                            viewModel.PN51xxCurrentMin = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Min(item => item.Pn51xxSysInfo!.Iddpa);

                                                            viewModel.PN51xxVoltageAvg = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Average(item => item.Pn51xxSysInfo!.Vddpa);
                                                            viewModel.PN51xxVoltageMax = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Max(item => item.Pn51xxSysInfo!.Vddpa);
                                                            viewModel.PN51xxVoltageMin = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Min(item => item.Pn51xxSysInfo!.Vddpa);

                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Log.Error(ex.Message);
                                                        }
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Log.Error(ex.Message);
                                                transaction.Rollback();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    while (FrameLength > 0 && receiveBuffer.Count >= FrameLength);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }

        private void OnDisconnected()
        {
            RemoveRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}


