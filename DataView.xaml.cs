using Microsoft.EntityFrameworkCore;
using Nager.TcpClient;
using Newtonsoft.Json;
using Serilog;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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


        private float _ecuPowerAvg;
        private float _ecuPowerMax;
        private float _ecuPowerMin;


        private float _ecuCPUUsageAvg;
        private float _ecuCPUUsageMax;
        private float _ecuCPUUsageMin;

        private float _ecuCPUFreqAvg;
        private float _ecuCPUFreqMax;
        private float _ecuCPUFreqMin;

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

        public float EcuCPUFreqAvg { get => _ecuCPUFreqAvg; set { if (_ecuCPUFreqAvg != value) { _ecuCPUFreqAvg = value; OnPropertyChanged(nameof(EcuCPUFreqAvg)); } } }
        public float EcuCPUFreqMax { get => _ecuCPUFreqMax; set { if (_ecuCPUFreqMax != value) { _ecuCPUFreqMax = value; OnPropertyChanged(nameof(EcuCPUFreqMax)); } } }
        public float EcuCPUFreqMin { get => _ecuCPUFreqMin; set { if (_ecuCPUFreqMin != value) { _ecuCPUFreqMin = value; OnPropertyChanged(nameof(EcuCPUFreqMin)); } } }

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

                                                            viewModel.EcuCPUFreqAvg = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Average(item => item.EcuSysInfo!.CpuFrequency!.First());
                                                            viewModel.EcuCPUFreqMax = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Max(item => item.EcuSysInfo!.CpuFrequency!.First());
                                                            viewModel.EcuCPUFreqMin = (float)context.SysInfoData.Where(item => item.Mac == viewModel.BoardData.MacAddress).Min(item => item.EcuSysInfo!.CpuFrequency!.First());
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


