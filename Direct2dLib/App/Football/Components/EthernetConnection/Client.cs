using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Direct2dLib.App.Football.Components.EthernetConnection;
using Direct2dLib.App.CustomUnity.Components.MechanicComponents.Players;
using Direct2dLib.App.Football.Components.EthernetConnection.Json;
using Newtonsoft.Json;

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
            ClientData clientData = new ClientData()
            {
                position = _players[NetworkController.PlayerIndex].transform.position,
                playerIndex = NetworkController.PlayerIndex,
            };

            string message = JsonConvert.SerializeObject(clientData);
            byte[] bytes = Encoding.UTF8.GetBytes(message);

            _serverStream.Write(bytes, 0, bytes.Length);
            _serverStream.Flush();

            GetMatchData();
        }

        private void GetMatchData()
        {
            byte[] bytes = new byte[512];
            int length = _serverStream.Read(bytes, 0, bytes.Length);
            string message = Encoding.UTF8.GetString(bytes, 0, length);

            ServerData serverData = JsonConvert.DeserializeObject<ServerData>(message);

            for (int i = 0; i < _players.Count; i++)
            {
                _players[i].transform.position = serverData.playerPositions[i];
            }

            _ball.transform.position = serverData.ballPosition;
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
