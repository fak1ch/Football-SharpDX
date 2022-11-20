using Direct2dLib.App.CustomUnity.Components.MechanicComponents.Gates;
using Direct2dLib.App.CustomUnity.Components.MechanicComponents.Players;
using Direct2dLib.App.CustomUnity.Components.MechanicComponents.UI;
using Direct2dLib.App.Football.Components.EthernetConnection;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using Timer = Direct2dLib.App.CustomUnity.Components.MechanicComponents.UI.Timer;

namespace Direct2dLib.App.CustomUnity.Components.MechanicComponents
{
    public class MatchData
    {
        public RightGate rightGate;
        public LeftGate leftGate;
        public GameEndPopUp gameEndPopUp;
        public Ball ball;
        public Timer timer;
        public Score score;
    }

    public class Match : Component
    {
        private List<Player> _players;
        private Dictionary<string, List<Player>> _playerTeamDictionary;
        private MatchData _data;

        public List<Player> Players => _players;
        public string Score => _data.score.ScoreString;

        public Match(GameObject go, MatchData data) : base(go)
        {
            _players = new List<Player>();

            _playerTeamDictionary = new Dictionary<string, List<Player>>();
            _playerTeamDictionary.Add("Left", new List<Player>());
            _playerTeamDictionary.Add("Right", new List<Player>());

            _data = data;

            _data.rightGate.OnWasScoredGoal += HandleGoal;
            _data.leftGate.OnWasScoredGoal += HandleGoal;
            _data.timer.OnTimeEnd += TimeEnd;

            _data.gameEndPopUp.Initialize(this);

            _data.timer.RestartTimer();
        }

        public void RestartMatch()
        {
            ReturnPlayersAndBallForHisPlaces();
            _data.timer.RestartTimer();
            _data.score.RestartScore();
            SetGameOnPause(false);
        }

        private void ReturnPlayersAndBallForHisPlaces()
        {
            _data.ball.ReturnToStartPosition();
            foreach (var team in _playerTeamDictionary)
            {
                foreach (var player in team.Value)
                {
                    player.ReturnToStartPosition();
                }
            }
        }

        private void HandleGoal(Type type)
        {
            if (NetworkController.IsServer)
            {
                bool leftTeamScoreGoal = type == typeof(RightGate);

                if (leftTeamScoreGoal)
                {
                    _data.score.LeftTeamPoint++;
                }
                else
                {
                    _data.score.RightTeamPoints++;
                }

                ReturnPlayersAndBallForHisPlaces();

                if (_data.score.WinLeftTeam)
                {
                    _data.gameEndPopUp.SetTeamName("Left");
                    SetGameOnPause(true);
                }
                else if (_data.score.WinRightTeam)
                {
                    _data.gameEndPopUp.SetTeamName("Right");
                    SetGameOnPause(true);
                }
            }
            else
            {
                ReturnPlayersAndBallForHisPlaces();
            }
        }

        private void SetGameOnPause(bool value)
        {
            _data.timer.GameOnPause = value;

            foreach (var team in _playerTeamDictionary)
            {
                foreach (var player in team.Value)
                {
                    player.GameOnPause = value;
                }
            }
        }

        public void AddPlayerToTeamByName(string name, Player player)
        {
            _players.Add(player);
            _playerTeamDictionary[name].Add(player);
        }

        private void TimeEnd()
        {

        }
    }
}
