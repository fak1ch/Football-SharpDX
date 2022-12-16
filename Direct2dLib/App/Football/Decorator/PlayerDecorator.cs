using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Direct2dLib.App.Football.Decorator
{
    public class PlayerDecorator : IPlayer
    {
        protected IPlayer player;

        public PlayerDecorator(IPlayer newPlayer)
        {
            player = newPlayer;
        }

        public virtual float GetPlayerSpeed()
        {
            return player.GetPlayerSpeed();
        }
    }
}
