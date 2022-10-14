using Direct2dLib.App.Football.Components.EthernetConnection;
using SharpDX.Direct2D1.Effects;
using SharpDX.Text;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Direct2dLib.App.CustomUnity.Components.MechanicComponents.EthernetConnection
{
    public class Server
    {
        private const string ip = "127.0.0.1";
        private const int port = 8080;
        private const int maxPlayers = 4;

        public event Action OnStartGame;

        private List<TcpClient> _clients;

        public int CountConnections => _clients.Count + 1;
        public bool StartGameFlag { get; set; } = false;

        public Server()
        {
            _clients = new List<TcpClient>();
            Task task = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            TcpListener serverSocket = new TcpListener(endPoint);
            serverSocket.Start();

            while (true)
            {
                if (StartGameFlag) break;

                TcpClient clientSocket = await serverSocket.AcceptTcpClientAsync();

                _clients.Add(clientSocket);
                StartGameFlag = CountConnections == maxPlayers;

                byte[] bytes = Encoding.UTF8.GetBytes($"{CountConnections}:{StartGameFlag}");

                foreach (var client in _clients)
                {
                    NetworkStream clientStream = client.GetStream();
                    await clientStream.WriteAsync(bytes, 0, bytes.Length);
                    await clientStream.FlushAsync();
                }
            }

            NetworkController.IsServer = true;
            NetworkController.PlayerIndex = 0;
            NetworkController.Server = this;

            OnStartGame?.Invoke();
        }
    }
}
