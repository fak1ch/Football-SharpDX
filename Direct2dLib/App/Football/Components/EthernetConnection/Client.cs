using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Direct2dLib.App.Football.Components.EthernetConnection;
using Direct2dLib.App.CustomUnity.Components.MechanicComponents.Players;
using SharpDX;
using Direct2dLib.App.CustomUnity.Utils;
using SharpDX.Direct2D1;

namespace Direct2dLib.App.CustomUnity.Components.MechanicComponents.EthernetConnection
{
    public class Client
    {
        private const string ip = "127.0.0.1";
        private const int port = 8080;

        public event Action OnStartGame;

        private int? _playerIndex = null;

        private TcpClient _tcpClient;
        private NetworkStream _serverStream;
        private List<Player> _players;
        private Ball _ball;

        public int CountConnections { get; private set; }
        public bool StartGameFlag { get; set; } = false;

        public Client()
        {
            Task task = InitializeAsync();
        }

        private async Task InitializeAsync()
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

        public void WriteAndReadMatch()
        {
            Task task = SendPositionToServer();
        }

        private async Task SendPositionToServer()
        {
            while (true)
            {
                if (!_tcpClient.Connected) continue;

                string message = string.Empty;
                message += Converter.Vector3ToString(_players[NetworkController.PlayerIndex].transform.position);
                message += ':';
                message += NetworkController.PlayerIndex;

                byte[] bytes = Encoding.UTF8.GetBytes(message);

                await _serverStream.WriteAsync(bytes, 0, bytes.Length);
                await _serverStream.FlushAsync();

                await GetMatchData();
            }
        }

        private async Task GetMatchData()
        {
            while (true)
            {
                if (!_tcpClient.Connected) continue;

                byte[] bytes = new byte[256];
                int length = await _serverStream.ReadAsync(bytes, 0, bytes.Length);
                string[] message = Encoding.UTF8.GetString(bytes, 0, length).Split(':');

                for (int i = 0; i < _players.Count; i++)
                {
                    Vector3 playerPosition = Converter.StringToVector3(message[i]);
                    _players[i].transform.position = playerPosition;
                }

                Vector3 ballPosition = Converter.StringToVector3(message.Last());
                _ball.transform.position = ballPosition;
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
