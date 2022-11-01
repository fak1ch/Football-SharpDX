using Direct2dLib.App.CustomUnity.Components.MechanicComponents.Players;
using Direct2dLib.App.CustomUnity.Utils;
using Direct2dLib.App.Football.Components.EthernetConnection;
using Direct2dLib.App.Football.Components.EthernetConnection.Json;
using Newtonsoft.Json;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct2D1.Effects;
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
        private const string ip = "127.0.0.1";
        private const int port = 8080;
        private const int maxPlayers = 2;

        public event Action OnStartGame;

        private UdpClient _udpServer;
        private IPEndPoint _endPoint;

        private List<Player> _players;
        private Ball _ball;

        private Thread _newThread;

        public bool StartGameFlag { get; set; } = false;

        public Server()
        {
            _endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            _udpServer = new UdpClient(_endPoint);

            NetworkController.IsServer = true;
            NetworkController.Server = this;
            NetworkController.PlayerIndex = 0;

            _newThread = new Thread(() => WriteAndReadMatch());
        }

        public void StartWriteAndReadMatchInNewThread()
        {
            if (_newThread.ThreadState != ThreadState.Running)
            {
                _newThread = new Thread(() => WriteAndReadMatch());
                _newThread.Start();
            }
        }

        public void WriteAndReadMatch()
        {
            while (true)
            {
                byte[] bytes = _udpServer.Receive(ref _endPoint);

                string message = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                ClientData clientData = JsonConvert.DeserializeObject<ClientData>(message);

                _players[clientData.playerIndex].transform.position = clientData.position;

                SendMatchDataAsync();
            }
        }

        private void SendMatchDataAsync() 
        {
            List<Vector3> playerPositions = new List<Vector3>();
            foreach (var player in _players)
            {
                playerPositions.Add(player.transform.position);
            }

            ServerData serverData = new ServerData()
            {
                playerPositions = playerPositions,
                ballPosition = _ball.transform.position,
            };

            string message = JsonConvert.SerializeObject(serverData);
            byte[] bytes = Encoding.UTF8.GetBytes(message);

            _udpServer.Send(bytes, bytes.Length, _endPoint);
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
