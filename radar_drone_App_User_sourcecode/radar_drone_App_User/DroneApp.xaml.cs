using Nancy.Json;
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
    /// Interaction logic for DroneApp.xaml
    /// </summary>
    public partial class DroneApp : Window
    {


        private Battery battery;
        private int DroneID;  
        private string photo;
        private ClassMap map;
        private List<string> route;



        public static List<EventDetails> m_EventDetails;
        WebSocketSharp.WebSocket ws_app;
        int selected_index = -1;
        public DroneApp(WebSocketSharp.WebSocket ws_app)
        {
            InitializeComponent();
            m_EventDetails = new List<EventDetails>();
            this.ws_app = ws_app;
            ws_app.OnMessage += (sender1, e1) => {
                DateTime time = new DateTime(DateTime.Now.Ticks);
                string data = e1.Data;

                this.Dispatcher.Invoke(() =>
                {
                    string[] dataSplit = Convert.ToString(data).Split(':');

          
                    if (dataSplit[0].Contains("DATA"))
                    {
                        string[] droneData = dataSplit[1].Split(',');
                        textBoxId.Text = droneData[0];
                        textBoxBattery.Text = droneData[1];
                        textBoxLocation.Text = droneData[2];

                    }
                    else if (dataSplit[0].Contains("PHOTO"))
                    {
                        image.Source = new ImageSourceConverter().ConvertFromString("https://"+ dataSplit[1]) as ImageSource;
                   

                    }
                    else
                    {
                        EventDetails eventD = (new JavaScriptSerializer()).Deserialize<EventDetails>(data);
                        m_EventDetails.Add(eventD);
                        listBox_events.Items.Add("new threat detected > id:" + eventD.id + ", loc:" + eventD.location);
                    }

                });

            };
        }

        public void DisplayDataDrone()
        {
            ws_app.Send("DATA:");
        }

        public void FlyToCordinates(string cord)
        {
            ws_app.Send("SCAN:" + cord);

        }



        public void GetPhoto()
        {
            ws_app.Send("PHOTO:");

        }

        public void ReturnToCharge()
        {

        }


        private void button_Click(object sender, RoutedEventArgs e)
        {
            DisplayDataDrone();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (selected_index == -1) return;
            FlyToCordinates( m_EventDetails[selected_index].location);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            GetPhoto();
        }

        private void listBox_events_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           selected_index = listBox_events.SelectedIndex;
        }
    }
}
