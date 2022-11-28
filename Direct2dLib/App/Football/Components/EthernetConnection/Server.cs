using Direct2dLib.App.CustomUnity.Components.MechanicComponents.Players;
using Direct2dLib.App.CustomUnity.Components.MechanicComponents.UI;
using Direct2dLib.App.Football.Bonuses;
using Direct2dLib.App.Football.Components.EthernetConnection;
using Direct2dLib.App.Football.Components.EthernetConnection.Json;
using Newtonsoft.Json;
using SharpDX;
using SharpDX.Text;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Direct2dLib.App.CustomUnity.Components.MechanicComponents.EthernetConnection
{
    public class Server
    {
        private const string ip = "25.52.21.43";
        private const int port = 8080;
        private const int maxPlayers = 4;

        public event Action OnStartGame;

        private List<TcpClient> _clients;
        private List<Player> _players;
        private Ball _ball;
        private Score _score;
        private BonusSpawner _bonusSpawner;
        private Match _match;

        private Thread _newThread;

        private bool _returnToStartPosition = false;

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

            _newThread = new Thread(() => WriteAndReadMatch());

            OnStartGame?.Invoke();
        }

        public void StartWriteAndReadMatchInNewThread()
        {
            if (_newThread.ThreadState != ThreadState.Running)
            {
                _newThread = new Thread(() => WriteAndReadMatch());
                _newThread.Start();
            }
        }

        private void WriteAndReadMatch()
        {
            foreach (var client in _clients)
            {
                byte[] bytes = new byte[256];
                int length = client.GetStream().Read(bytes, 0, bytes.Length);
                string message = Encoding.UTF8.GetString(bytes, 0, length);

                ClientData clientData = JsonConvert.DeserializeObject<ClientData>(message);

                _players[clientData.playerIndex].transform.position = clientData.position;
            }

            SendMatchDataAsync();
        }

        private void SendMatchDataAsync() 
        {
            List<Vector3> playerPositions = new List<Vector3>();
            foreach (var player in _players)
            {
                playerPositions.Add(player.transform.position);
            }

            bool returnToStartPosition = _returnToStartPosition;

            if (_returnToStartPosition)
            {
                _returnToStartPosition = false;
            }

            ServerData serverData = new ServerData()
            {
                playerPositions = playerPositions,
                ballPosition = _ball.transform.position,
                leftTeamScore = _score.LeftTeamPoint,
                rightTeamScore = _score.RightTeamPoints,
                bonusDatas = _bonusSpawner.GetBonusDataList(),
                ReturnToStartPosition = returnToStartPosition,
            };

            string message = JsonConvert.SerializeObject(serverData);
            byte[] bytes = Encoding.UTF8.GetBytes(message);

            foreach (var client in _clients)
            {
                NetworkStream clientStream = client.GetStream();
                clientStream.Write(bytes, 0, bytes.Length);
                clientStream.Flush();
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

        public void SetScore(Score score)
        {
            _score = score;
        }

        public void SetBonusSpawner(BonusSpawner bonusSpawner)
        {
            _bonusSpawner = bonusSpawner;
        }

        public void SetMatch(Match match)
        {
            _match = match;

            _match.OnGoal += () => _returnToStartPosition = true;
        }
    }
}
