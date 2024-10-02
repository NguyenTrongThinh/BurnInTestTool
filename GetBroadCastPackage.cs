using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BurnInTestTool
{
  
    class GetBroadCastPackage
    {
   
        private const Int32 port = 45454;
        private UdpClient? udpClient;
        private IPEndPoint? remoteEndPoint;
        public GetBroadCastPackage()
        {
            try
            {
                udpClient = new UdpClient(port);
                udpClient.EnableBroadcast = true;
                remoteEndPoint = new IPEndPoint(IPAddress.Any, port);
            }
            catch (Exception ex)
            {
                Log.Debug($"Create UDP Client error: {ex.Message}");
            }
        }

        public void CleanUp()
        {
            udpClient?.Close();
            udpClient?.Dispose();
        }
        public BoardID? DiscoverBoardID()
        {
            BoardID? boardID = null;
            if (udpClient is not null)
            {
                try
                {
                    byte[] data = udpClient.Receive(ref remoteEndPoint);
                    string receivedMessage = Encoding.UTF8.GetString(data);
                    boardID = JsonConvert.DeserializeObject<BoardID>(receivedMessage);
                    Log.Debug($"{receivedMessage}");
                }
                catch (Exception ex)
                {
                    Log.Debug($"Recevied UDP broadcast Error: {ex.Message}");
                }
            }


            return boardID;
        }
    }
}
