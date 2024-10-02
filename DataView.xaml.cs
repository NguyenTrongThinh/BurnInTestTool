using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        public BoardID? ID { get => id; set => id = value; }
    }
    public partial class DataView : UserControl
    {
        private DataViewViewModel viewModel { get; set; }
        private DataViewDataClass dataViewDataClass { get; set; }
        public EventHandler? RemoveRequested;
        private CancellationTokenSource? cancellationTokenSource;
        private Task? getSystemInfo;
        private TcpClient? tcpClient;
        private delegate void SystemInformationReadyCallback(SystemInformationDataClass systemInformationData);
        public DataView()
        {
            InitializeComponent();
            dataViewDataClass = new DataViewDataClass();
            viewModel = new DataViewViewModel();
            viewModel.BoardData = dataViewDataClass;
            DataContext = viewModel;
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
           
        }


        private void StartGetSystemInformationWorkerTask(SystemInformationReadyCallback? systemInfoCallback)
        {

            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            try
            {
                getSystemInfo = Task.Run(async () =>
                {
                    tcpClient = new TcpClient();
                    if (viewModel.BoardData?.ID?.IpAddress is not null)
                    {
                        //Task connectTask = tcpClient.ConnectAsync(viewModel.BoardData.ID.IpAddress, 6868);
                        //Task completedTask = await Task.WhenAny(connectTask, Task.Delay(15000));
                        //if (tcpClient.Connected)
                        //{
                        //    NetworkStream stream = tcpClient.GetStream();
                        //    stream.ReadTimeout = 1;
                        //    byte[] buffer = new byte[1024];
                        //    while (token.IsCancellationRequested == false && tcpClient.Connected)
                        //    {
                        //            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        //            if (bytesRead > 0)
                        //            {
                        //                string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        //            }
                        //            //Dispatcher.Invoke(() => systemInfoCallback?.Invoke(discoveredBoard));

                        //            Thread.Sleep(100);
                        //        }
                        //    }
                        while (token.IsCancellationRequested == false )
                        {
                            Thread.Sleep(100);
                        }
                    }
                    Dispatcher.Invoke(() => RemoveRequest());
                }, token);
            }
            catch (Exception ex)
            {
                Log.Error("Create get Discover Thread Error: " + ex.Message);
            }
        }
        private void StopGetSystemInformationWorkerTask()
        {
            try
            {
                if (cancellationTokenSource != null)
                {
                    cancellationTokenSource.Cancel();
                }
                getSystemInfo?.Wait();
            }
            catch (Exception ex)
            {
                Log.Error($"Stop get discover Thread Error: {ex.Message}");
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
            return viewModel.BoardData?.ID??null;
        }

        private void RemoveRequest()
        {
            RemoveRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Log.Debug("User Control Loaded");
            StartGetSystemInformationWorkerTask(UpdateSystemInformation);
        }
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Log.Debug("User Control Unloaded");
            StopGetSystemInformationWorkerTask();
            this.Loaded -= OnLoaded;
            this.Unloaded -= OnUnloaded;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveRequest();
        }

        private void UpdateSystemInformation(SystemInformationDataClass systemInformationData) {
            Log.Debug("Update System information is called");
        }

    }


}
