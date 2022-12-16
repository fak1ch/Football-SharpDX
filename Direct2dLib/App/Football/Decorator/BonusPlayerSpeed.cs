using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Direct2dLib.App.Football.Decorator
{
    public class BonusPlayerSpeed : PlayerDecorator
    {
        private float _multiplier = 2;

        public BonusPlayerSpeed(IPlayer newPlayer) : base(newPlayer)
        {
        }

        public override float GetPlayerSpeed()
        {
            return base.GetPlayerSpeed() * _multiplier;
        }
    }
}
