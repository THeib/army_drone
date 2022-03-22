using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace radar_drone_App_User
{
    internal class ReportAppProxy : ReportApp
    {
        public ReportAppProxy(WebSocket ws_app) : base(ws_app)
        {
        }

        public void ShowTexts()
        {

        }

        public void WriteTexts()
        {

        }
    }
}
