using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Direct2dLib.App.Football.Decorator
{
    public class BonusMaxSpeed : BallDecorator
    {
        private float _multiplier = 2;

        public BonusMaxSpeed(IBall newMainBall) : base(newMainBall)
        {

        }

        public override float GetMaxSpeed()
        {
            return base.GetMaxSpeed() * _multiplier;
        }
    }
}
