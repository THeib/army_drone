using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace radar_drone_App_User
{
    /// <summary>
    /// Interaction logic for ReportApp.xaml
    /// </summary>
    public partial class ReportApp : Window
    {
        public int  Chat_type;
        public int Location_Type;
        private DateTime Time;
        public int  TypeFile;
     


     



        public void ShowText()
        {

        }

        private void WriteText()
        {

        }
        public ReportApp(WebSocketSharp.WebSocket ws_app)
        {
            InitializeComponent();
            ws_app.OnMessage += (sender1, e1) => {
                DateTime time = new DateTime(DateTime.Now.Ticks);
                string data = e1.Data;

                this.Dispatcher.Invoke(() =>
                {
                    listBox_events.Items.Add(data);

                });

            };
        }
    }
}
