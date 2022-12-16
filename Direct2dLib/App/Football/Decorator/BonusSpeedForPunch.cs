using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Direct2dLib.App.Football.Decorator
{
    public class BonusSpeedForPunch : BallDecorator
    {
        private float _multiplier = 2;

        public BonusSpeedForPunch(IBall newMainBall) : base(newMainBall)
        {

        }

        public override float GetSpeedForPunch()
        {
            return base.GetSpeedForPunch() * _multiplier;
        }
    }
}
