using Direct2dLib.App.CustomUnity.Components.MechanicComponents.EthernetConnection;
using Direct2dLib.App.CustomUnity.Scenes;
using SharpDX;
using System;

namespace Direct2dLib.App.CustomUnity.Components.MechanicComponents.UI
{
    public class MainMenuPopUp : Component
    {
        private RectangleF _countConnectionsRectangle;
        private RectangleF _rectangleBackground;
        private RectangleF _createGameRectangle;
        private RectangleF _connectToGameRectangle;

        private Button _createGameButton;
        private Button _connectToGameButton;

        private Server _server;
        private Client _client;
        private int _countConnections;
        private bool _isServer = false;

        private bool _buttonsIsClose = false;

        public MainMenuPopUp(GameObject go) : base(go)
        {
            Vector3 screenCenter = DX2D.Instance.ScreenCenter;

            _countConnectionsRectangle = new RectangleF(screenCenter.X - 200, screenCenter.Y - 175, 400, 100);
            _rectangleBackground = new RectangleF(screenCenter.X - 200, screenCenter.Y - 200, 400, 325);
            _createGameRectangle = new RectangleF(screenCenter.X - 175, screenCenter.Y, 150, 100);
            _connectToGameRectangle = new RectangleF(screenCenter.X + 25, screenCenter.Y, 150, 100);

            _createGameButton = gameObject.AddComponent(new Button(gameObject, _createGameRectangle));
            _connectToGameButton = gameObject.AddComponent(new Button(gameObject, _connectToGameRectangle));

            _createGameButton.OnClick += CreateGame;
            _connectToGameButton.OnClick += ConnectToGame;
        }

        public override void Update()
        {
            UpdateGameEndPopUp();
        }

        private void CreateGame()
        {
            _createGameButton.OnClick -= CreateGame;

            _isServer = true;
            _buttonsIsClose = true;
            _server = new Server();
            StartGame();
        }

        private void ConnectToGame()
        {
            _connectToGameButton.OnClick -= ConnectToGame;

            _isServer = false;
            _buttonsIsClose = true;
            _client = new Client();
            StartGame();
        }

        private void StartGame()
        {
            SceneManager.Instance.LoadScene<MatchScene>();
        }

        private void UpdateGameEndPopUp()
        {
            DX2D.Instance.RenderTarget.FillRectangle(_rectangleBackground, DX2D.Instance.GrayBrush);
            DX2D.Instance.RenderTarget.DrawText($"{_countConnections}/4", DX2D.Instance.TextFormatMessage, _countConnectionsRectangle, DX2D.Instance.WhiteBrush);

            if (!_buttonsIsClose)
            {
                DX2D.Instance.RenderTarget.FillRectangle(_createGameRectangle, DX2D.Instance.BlackBrush);
                DX2D.Instance.RenderTarget.FillRectangle(_connectToGameRectangle, DX2D.Instance.BlackBrush);
                
                DX2D.Instance.RenderTarget.DrawText($"Create Game", DX2D.Instance.TextFormatMessage40, _createGameRectangle, DX2D.Instance.WhiteBrush);
                DX2D.Instance.RenderTarget.DrawText($"Connect", DX2D.Instance.TextFormatMessage40, _connectToGameRectangle, DX2D.Instance.WhiteBrush);
            }
        }
    }
}
