using Direct2dLib.App.CustomUnity.Components.MechanicComponents.EthernetConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Direct2dLib.App.Football.Components.EthernetConnection
{
    public static class NetworkController
    {
        public static bool IsServer { get; set; }
        public static int PlayerIndex { get; set; }

        public static Server Server { get; set; }
        public static Client Client { get; set; }
    }
}
