using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace BurnInTestTool
{
    public partial class MainWindow : Window
    {
        private delegate void BoardsAddedCallback(BoardID discoveredBoards);
        List<BoardID> discoveredBoards = new List<BoardID>();
        private void StartGetBroadCastWorkerTask(BoardsAddedCallback? boardAddedCallback)
        {
            
            cancellationTokenSource = new CancellationTokenSource();
            var token  = cancellationTokenSource.Token;
            GetBroadCastPackage getBroadCastPackage = new GetBroadCastPackage();
            try
            {
                getBroadCastTask = Task.Run(() =>
                {
                    while (token.IsCancellationRequested == false)
                    {
                        BoardID? discoveredBoard = getBroadCastPackage.DiscoverBoardID();
                        if (discoveredBoard != null)
                        {
                            if (discoveredBoards.Contains(discoveredBoard) == false)
                            {
                                discoveredBoards.Add(discoveredBoard);
                                Dispatcher.Invoke(() => boardAddedCallback?.Invoke(discoveredBoard));
                            }
                        } 
                        Thread.Sleep(100);
                    }
                    getBroadCastPackage.CleanUp();
                }, token);
            }
            catch (Exception ex)
            {
                Log.Error("Create get Discover Thread Error: " + ex.Message);
            }
        }
        private void StopGetBroadCastWorkerTask()
        {
            try
            {
                if (cancellationTokenSource != null)
                {
                    cancellationTokenSource.Cancel();
                }
                getBroadCastTask?.Wait();
            }
            catch (Exception ex)
            {
                Log.Error($"Stop get discover Thread Error: {ex.Message}");
            }
        }

        private void DataViewRemoveRequested(object? sender, System.EventArgs e)
        {
            if (sender is DataView dataView)
            {
               discoveredBoards.RemoveAll(item => (item.MacAddress == dataView.GetBoardID()?.MacAddress));
               Dispatcher.Invoke(()=> ChildDataViewStackPanel.Children.Remove(dataView));
            }
        }

        private void AddDataView(BoardID discoveredBoard)
        {
            Log.Debug("Added Data View After board Discovered");
            Log.Debug($"{discoveredBoard.IpAddress}" + " " + $"{discoveredBoard.MacAddress}");
            DataView dataView = new DataView();
            dataView.SetBoardID(discoveredBoard);
            dataView.RemoveRequested = DataViewRemoveRequested;
            ChildDataViewStackPanel.Children.Add(dataView);
            
        }
    }
}
