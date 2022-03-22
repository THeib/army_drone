using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace radar_drone_App_User
{
    internal class DroneAppProxy : DroneApp
    {
        public DroneAppProxy(WebSocket ws_app) : base(ws_app)
        {
        }

        public void FlyToCordinate()
        {

        }

        public void GetPhoto()
        {

        }
    }
}
