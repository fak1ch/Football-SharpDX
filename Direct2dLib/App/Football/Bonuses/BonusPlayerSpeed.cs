using Direct2dLib.App.CustomUnity.Components;
using Direct2dLib.App.CustomUnity.Components.MechanicComponents;
using Direct2dLib.App.CustomUnity.Components.MechanicComponents.Players;
using Direct2dLib.App.Football.Components.EthernetConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Match = Direct2dLib.App.CustomUnity.Components.MechanicComponents.Match;

namespace Direct2dLib.App.Football.Bonuses
{
    public class BonusPlayerSpeed : Bonus
    {
        private float _addSpeedValue = 3;
        private float _addSpeedDuration = 3;

        public BonusPlayerSpeed(GameObject go) : base(go)
        {
        }

        protected override void ActivateBonus(Player player)
        {
            if (player.PlayerMovement != null)
            {
                player.PlayerMovement.SetSpeedTemp(_addSpeedValue, _addSpeedDuration);
            }
            SetActiveBonus(false);
        }
    }
}
