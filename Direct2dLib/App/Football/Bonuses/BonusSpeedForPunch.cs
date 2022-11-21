using Direct2dLib.App.CustomUnity.Components;
using Direct2dLib.App.CustomUnity.Components.MechanicComponents;
using Direct2dLib.App.CustomUnity.Components.MechanicComponents.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Direct2dLib.App.Football.Bonuses
{
    public class BonusSpeedForPunch : Bonus
    {
        private Ball _ball;

        private float _addSpeedForPunch = 7;
        private float _timeBonusDuration = 5;

        public BonusSpeedForPunch(GameObject go, Ball ball) : base(go)
        {
            _ball = ball;
        }

        protected override void ActivateBonus(Player player)
        {
            _ball.IncreaseSpeedForPunchBonus(_addSpeedForPunch, _timeBonusDuration);
            SetActiveBonus(false);
        }
    }
}
