using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Direct2dLib.App.Football.Components.EthernetConnection;
using Direct2dLib.App.CustomUnity.Components.MechanicComponents.Players;
using Direct2dLib.App.Football.Components.EthernetConnection.Json;
using Newtonsoft.Json;
using System.Net;

namespace Direct2dLib.App.CustomUnity.Components.MechanicComponents.EthernetConnection
{
    public class Client
    {
        private const string ip = "127.0.0.1";
        private const int port = 8080;

        public event Action OnStartGame;

        private Socket _udpClientScoket;
        private IPEndPoint _endPoint;

        private List<Player> _players;
        private Ball _ball;

        public int CountConnections { get; private set; }
        public bool StartGameFlag { get; set; } = false;

        public Client()
        {
            _endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            _udpClientScoket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            NetworkController.IsServer = false;
            NetworkController.Client = this;
            NetworkController.PlayerIndex = 1;
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

            _udpClientScoket.SendTo(bytes, _endPoint);

            GetMatchData();
        }

        private void GetMatchData()
        {
            byte[] bytes = new byte[512];
            int length = _udpClientScoket.Receive(bytes);
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
