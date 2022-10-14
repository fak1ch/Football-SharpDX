using Direct2dLib.App.CustomUnity.Components.MechanicComponents.Players;
using Direct2dLib.App.CustomUnity.Utils;
using Direct2dLib.App.Football.Components.EthernetConnection;
using SharpDX;
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
        private List<Player> _players;
        private Ball _ball;

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

        public void WriteAndReadMatch()
        {
            Task task = GetPlayerDataAsync();
        }

        private async Task SendMatchDataAsync() 
        {
            string message = string.Empty;

            foreach (var player in _players)
            {
                message += player.transform.position + ':';
            }
            message += _ball.transform.position;

            byte[] bytes = Encoding.UTF8.GetBytes(message);

            foreach (var client in _clients)
            {
                if (!client.Connected) continue;

                NetworkStream clientStream = client.GetStream();
                await clientStream.WriteAsync(bytes, 0, bytes.Length);
                await clientStream.FlushAsync();
            }
        }

        private async Task GetPlayerDataAsync()
        {
            while (true)
            {
                foreach (var client in _clients)
                {
                    if (!client.Connected) continue;

                    byte[] bytes = new byte[256];
                    int length = await client.GetStream().ReadAsync(bytes, 0, bytes.Length);
                    string[] message = Encoding.UTF8.GetString(bytes, 0, length).Split(':');

                    Vector3 position = Converter.StringToVector3(message[0]);
                    int playerId = int.Parse(message[1]);

                    _players[playerId].transform.position = position;
                }

                await SendMatchDataAsync();
            }
        }

        public void SetPlayersList(List<Player> players)
        {
            _players = players;
        }

        public void SetBall(Ball ball)
        {
            _ball = ball;
        }
    }
}
