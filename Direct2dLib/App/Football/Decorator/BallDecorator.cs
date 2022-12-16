using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Direct2dLib.App.Football.Decorator
{
    public class BallDecorator : IBall
    {
        protected IBall mainBall;

        public BallDecorator(IBall newMainBall)
        {
            mainBall = newMainBall;
        }

        public virtual float GetMaxSpeed()
        {
            return mainBall.GetMaxSpeed();
        }

        public virtual float GetSpeedForPunch()
        {
            return mainBall.GetSpeedForPunch();
        }
    }
}
