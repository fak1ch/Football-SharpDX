using Direct2dLib.App.CustomUnity.Components.MechanicComponents.Gates;
using Direct2dLib.App.CustomUnity.Components.MechanicComponents.UI;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Direct2dLib.App.CustomUnity.Components.MechanicComponents
{
    public class Match : Component
    {
        private float _startSeconds = 180;
        private int _pointsForWin = 3;

        private int _leftTeamPoint = 0;
        private int _rightTeamPoints = 0;

        private RightGate _rightGate;
        private LeftGate _leftGate;
        private GameEndPopUp _gameEndPopUp;
        private Ball _ball;

        private Dictionary<string, List<PlayerMovement>> _playerTeamDictionary;

        private Vector3 _playerPosition;
        private float _currentSeconds;
        private bool _gameOnPause = false;

        public Match(GameObject go, RightGate rightGate, LeftGate leftGate, GameEndPopUp gameEndPopUp, Ball ball) : base(go)
        {
            _playerTeamDictionary = new Dictionary<string, List<PlayerMovement>>();
            _playerTeamDictionary.Add("Left", new List<PlayerMovement>());
            _playerTeamDictionary.Add("Right", new List<PlayerMovement>());

            _rightGate = rightGate;
            _leftGate = leftGate;
            _gameEndPopUp = gameEndPopUp;
            _ball = ball;

            _rightGate.OnWasScoredGoal += HandleGoal;
            _leftGate.OnWasScoredGoal += HandleGoal;

            _playerPosition = new Vector3(250, 350, 0);
            StartTimer();

            gameEndPopUp.Initialize(this);
        }

        public override void Update()
        {
            UpdatePointsText();
            UpdateTimeText();
        }

        public void RestartMatch()
        {
            _currentSeconds = _startSeconds;
            _leftTeamPoint = 0;
            _rightTeamPoints = 0;

            ReturnPlayersAndBallForHisPlaces();
            StartTimer();
            SetGameOnPause(false);
        }

        private void ReturnPlayersAndBallForHisPlaces()
        {
            _ball.ReturnToStartPosition();
            foreach (var team in _playerTeamDictionary)
            {
                foreach (var player in team.Value)
                {
                    player.transform.position = _playerPosition;
                }
            }
        }

        private void HandleGoal(Type type)
        {
            bool leftTeamScoreGoal = type == typeof(RightGate);

            if (leftTeamScoreGoal)
            {
                _leftTeamPoint++;
            }
            else
            {
                _rightTeamPoints++;
            }

            ReturnPlayersAndBallForHisPlaces();

            if (_leftTeamPoint == _pointsForWin)
            {
                _gameEndPopUp.SetTeamName("Left");
                SetGameOnPause(true);
            }
            else if (_rightTeamPoints == _pointsForWin)
            {
                _gameEndPopUp.SetTeamName("Right");
                SetGameOnPause(true);
            }
        }

        private void SetGameOnPause(bool value)
        {
            _gameOnPause = value;

            foreach (var team in _playerTeamDictionary)
            {
                foreach (var player in team.Value)
                {
                    player.SetGameOnPause(value);
                }
            }
        }

        public void AddPlayerToTeamByName(string name, PlayerMovement playerMovement)
        {
            _playerTeamDictionary[name].Add(playerMovement);
        }

        private void StartTimer()
        {
            _currentSeconds = _startSeconds;
        }

        private void UpdatePointsText()
        {
            Vector3 screenCenter = DX2D.Instance.ScreenCenter;
            RectangleF textRectangle = new RectangleF(screenCenter.X - 200, 0, 400, 100);
            DX2D.Instance.RenderTarget.DrawText($"{_leftTeamPoint} : {_rightTeamPoints}", DX2D.Instance.TextFormatMessage, textRectangle, DX2D.Instance.WhiteBrush);
        }

        private void UpdateTimeText()
        {
            if (!_gameOnPause)
            {
                _currentSeconds -= 0.0166666667f;
                if (_currentSeconds <= 0)
                {
                    _currentSeconds = 0;
                    _gameEndPopUp.ShowPopUpWithTie();
                }
            }

            int minutes = (int)_currentSeconds / 60;
            int seconds = (int)_currentSeconds - minutes * 60;

            string time = $"{minutes}:{seconds}";

            if (seconds < 10)
            {
                time = $"{minutes}:0{seconds}";
            }

            Vector3 screenCenter = DX2D.Instance.ScreenCenter;
            RectangleF textRectangle = new RectangleF(screenCenter.X - 200, 75, 400, 100);
            DX2D.Instance.RenderTarget.DrawText(time, DX2D.Instance.TextFormatMessageSmall, textRectangle, DX2D.Instance.WhiteBrush);
        }
    }
}
