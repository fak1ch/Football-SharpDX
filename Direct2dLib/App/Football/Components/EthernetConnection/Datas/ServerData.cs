using Direct2dLib.App.Football.Bonuses;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Direct2dLib.App.Football.Components.EthernetConnection.Json
{
    public class ServerData
    {
        public List<Vector3> playerPositions;
        public Vector3 ballPosition;
        public int leftTeamScore;
        public int rightTeamScore;
        public List<BonusData> bonusDatas;

        public bool ReturnToStartPosition;
    }
}
