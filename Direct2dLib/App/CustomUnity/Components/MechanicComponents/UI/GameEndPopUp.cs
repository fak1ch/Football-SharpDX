using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Direct2dLib.App.CustomUnity.Components.MechanicComponents.UI
{
    public class GameEndPopUp : Component
    {
        private string _gameEndText = "Tie!";
        private RectangleF _rectangleBackground;
        private RectangleF _gameEndTextRectangle;
        private RectangleF _buttonRestartRectangle;
        private RectangleF _buttonExitRectangle;

        private Match _match;
        private Button _restartButton;
        private Button _exitButton;

        public GameEndPopUp(GameObject go) : base(go)
        {
            gameObject.SetActive(false);

            Vector3 screenCenter = DX2D.Instance.ScreenCenter;
            _rectangleBackground = new RectangleF(screenCenter.X - 200, screenCenter.Y - 200, 400, 325);
            _gameEndTextRectangle = new RectangleF(screenCenter.X - 200, screenCenter.Y - 175, 400, 100);
            _buttonRestartRectangle = new RectangleF(screenCenter.X - 175, screenCenter.Y, 150, 100);
            _buttonExitRectangle = new RectangleF(screenCenter.X + 25, screenCenter.Y, 150, 100);

            _restartButton = gameObject.AddComponent(new Button(gameObject, _buttonRestartRectangle));
            _exitButton = gameObject.AddComponent(new Button(gameObject, _buttonExitRectangle));

            _restartButton.OnClick += RestartGame;
            _exitButton.OnClick += ExitGame;
        }

        public void Initialize(Match match)
        {
            _match = match;
        }

        public override void Update()
        {
            UpdateGameEndPopUp();
        }

        private void RestartGame()
        {
            gameObject.SetActive(false);
            _match.RestartMatch();
        }

        private void ExitGame()
        {
            DX2D.Instance.RenderForm.Close();
        }

        private void UpdateGameEndPopUp()
        {
            DX2D.Instance.RenderTarget.FillRectangle(_rectangleBackground, DX2D.Instance.GrayBrush);
            DX2D.Instance.RenderTarget.FillRectangle(_buttonRestartRectangle, DX2D.Instance.BlackBrush);
            DX2D.Instance.RenderTarget.FillRectangle(_buttonExitRectangle, DX2D.Instance.BlackBrush);
            DX2D.Instance.RenderTarget.DrawText($"Restart", DX2D.Instance.TextFormatMessageSmall, _buttonRestartRectangle, DX2D.Instance.WhiteBrush);
            DX2D.Instance.RenderTarget.DrawText($"Exit", DX2D.Instance.TextFormatMessageSmall, _buttonExitRectangle, DX2D.Instance.WhiteBrush);
            DX2D.Instance.RenderTarget.DrawText(_gameEndText, DX2D.Instance.TextFormatMessageSmall, _gameEndTextRectangle, DX2D.Instance.WhiteBrush);
        }

        public void SetTeamName(string teamName)
        {
            _gameEndText = $"Team {teamName}\n Win the Game!";
            gameObject.SetActive(true);
        }

        public void ShowPopUpWithTie()
        {
            _gameEndText = $"Tie!";
            gameObject.SetActive(true);
        }
    }
}