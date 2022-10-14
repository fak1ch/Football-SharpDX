using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Direct2dLib.App.Football.Components.EthernetConnection;

namespace Direct2dLib.App.CustomUnity.Components.MechanicComponents.EthernetConnection
{
    public class Client
    {
        private const string ip = "127.0.0.1";
        private const int port = 8080;

        public event Action OnStartGame;

        private TcpClient _tcpClient;
        private NetworkStream _serverStream;
        private int? _playerIndex = null;

        public int CountConnections { get; private set; }
        public bool StartGameFlag { get; set; } = false;

        public Client()
        {
            Task task = Initialize();
        }

        private async Task Initialize()
        {
            _tcpClient = new TcpClient(ip, port);
            _serverStream = _tcpClient.GetStream();

            while (true)
            {
                if (StartGameFlag) break;

                byte[] bytes = new byte[256];
                int length = await _serverStream.ReadAsync(bytes, 0, bytes.Length);
                string[] message = Encoding.UTF8.GetString(bytes, 0, length).Split(':');

                CountConnections = int.Parse(message[0]);
                StartGameFlag = bool.Parse(message[1]);

                if (_playerIndex == null)
                {
                    _playerIndex = CountConnections - 1;
                }
            }

            NetworkController.IsServer = false;
            NetworkController.PlayerIndex = _playerIndex.Value;
            NetworkController.Client = this;

            OnStartGame?.Invoke();
        }
    }
}
