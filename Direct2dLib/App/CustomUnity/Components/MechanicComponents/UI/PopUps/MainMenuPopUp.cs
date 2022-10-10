using Direct2dLib.App.CustomUnity.Scenes;
using SharpDX;
using System;

namespace Direct2dLib.App.CustomUnity.Components.MechanicComponents.UI
{
    public class MainMenuPopUp : Component
    {
        private RectangleF _rectangleBackground;
        private RectangleF _createGameRectangle;
        private RectangleF _connectToGameRectangle;

        private Button _createGameButton;
        private Button _connectToGameButton;

        public MainMenuPopUp(GameObject go) : base(go)
        {
            Vector3 screenCenter = DX2D.Instance.ScreenCenter;

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
            SceneManager.Instance.LoadScene<MatchScene>();
        }

        private void ConnectToGame()
        {

        }

        private void UpdateGameEndPopUp()
        {
            DX2D.Instance.RenderTarget.FillRectangle(_rectangleBackground, DX2D.Instance.GrayBrush);
            DX2D.Instance.RenderTarget.FillRectangle(_createGameRectangle, DX2D.Instance.BlackBrush);
            DX2D.Instance.RenderTarget.FillRectangle(_connectToGameRectangle, DX2D.Instance.BlackBrush);
            DX2D.Instance.RenderTarget.DrawText($"Create Game", DX2D.Instance.TextFormatMessage40, _createGameRectangle, DX2D.Instance.WhiteBrush);
            DX2D.Instance.RenderTarget.DrawText($"Connect", DX2D.Instance.TextFormatMessage40, _connectToGameRectangle, DX2D.Instance.WhiteBrush);
        }
    }
}
