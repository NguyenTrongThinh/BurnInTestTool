using Nager.TcpClient;
using Newtonsoft.Json;
using Serilog;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace BurnInTestTool
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    /// 
    public class DataViewViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private DataViewDataClass? _boardData;

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

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DataViewDataClass
    {
        private BoardID? id;
        private SystemInformationDataClass? sysInfo;

        public SystemInformationDataClass? SysInfo { get => sysInfo; set => sysInfo = value; }
        public BoardID? ID { get => id; set => id = value; }
    }
    public partial class DataView : UserControl
    {
        private DataViewViewModel viewModel { get; set; }
        private DataViewDataClass dataViewDataClass { get; set; }
        public EventHandler? RemoveRequested;
        private const int port = 6868;
        TcpClient? tcpClient;
        CancellationTokenSource? tcpClientConnectCancellationTokenSource;
        CancellationTokenSource? processDataCancellationTokenSource;
        private List<Byte> dataFrame = [];
        private List<Byte> receiveBuffer = [];
        private readonly List<Byte> startByte = [0x86, 0x66];
        public DataView()
        {
            InitializeComponent();
            dataViewDataClass = new DataViewDataClass();
            viewModel = new DataViewViewModel();
            viewModel.BoardData = dataViewDataClass;
            DataContext = viewModel;
            this.Loaded += ConnectToTCPServer;
            this.Unloaded += DisconnectToTCPServer;
        }

        private void ConnectToTCPServer(Object sender, EventArgs e)
        {
            try
            {
                if (viewModel.BoardData?.ID?.IpAddress is not null)
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
                            connected = tcpClient.ConnectAsync(viewModel.BoardData.ID.IpAddress, port, tcpClientConnectCancellationTokenSource.Token).Result;
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
            if (viewModel.BoardData is not null)
            {
                viewModel.BoardData.ID = boardID;
            }
        }
        public BoardID? GetBoardID()
        {
            return viewModel.BoardData?.ID ?? null;
        }

        private Int32 FindStartByte(List<Byte> buffer)
        {
            if (buffer.Count >= 2) {
                Int32 startByte1 = buffer.IndexOf(0x86);
                if (startByte1 >= 0)
                {
                    if (buffer.ElementAt(startByte1) == 0x86 && buffer.ElementAt(startByte1+1) == 0x66)
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
            if (StartPos >=0 && buffer.Count >= 4) {
                FrameLength = (buffer.ElementAt(StartPos+2) << 8 | buffer.ElementAt(StartPos + 3)) + 5; // 2 start byte + 1 checksum + 2 byte length
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
                lock (receiveBuffer) {
                    receiveBuffer.AddRange(receivedData);
                    do {
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
                                    viewModel.BoardData.SysInfo = JsonConvert.DeserializeObject<SystemInformationDataClass>(Encoding.UTF8.GetString(oneFrame.ToArray()));
                            }
                        }
                    }
                    while (FrameLength > 0);
                }
            }
            catch (Exception ex) {
                Log.Error(ex.Message);
            }
        }

        private void OnDisconnected()
        {
            RemoveRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}

    
