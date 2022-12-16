using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Direct2dLib.App.Football.Decorator
{
    public interface IBall
    {
        float GetMaxSpeed();
        float GetSpeedForPunch();
    }
}
