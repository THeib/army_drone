using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for Radar.xaml
    /// </summary>
    public partial class Radar : Window
    {

        private int lat;
        private int lng;
        Boolean isConnected = false;

        WebSocketSharp.WebSocket ws_Radar = new WebSocketSharp.WebSocket(url: "ws://localhost:8000/radar");
        public Radar()
        {

            InitializeComponent();
            lat = 33;
            lng = 35;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            long dateTime = DateTime.Now.Ticks;
            DateTime now = new DateTime(dateTime);
            if (isConnected)
            {
                ws_Radar.Close();
                button_rdr.Content = "start radar";
                isConnected = false;
                textBlock_Radar.Text += now + ";" + " the radar disconnected\n";
            }
            else
            {
                ws_Radar.Connect();
                isConnected = true;
                button_rdr.Content = "stop";
                Thread thread = new Thread(SendCordinate);
                thread.Start();
                textBlock_Radar.Text += now + ";" + " the radar connected to the server\n";
            }
        }

        
        public string RadarDetail(){

            return Lat + "," + Lng;   
        }


        public int Lng
        {
            get { return lng; }
            set { lng = value; }
        }

        public int Lat
        {
            get { return lat; }
            set { lat = value; }
        }

        public void SendCordinate()
        {
            try
            {
                Random rnd = new Random();
                for (int i = 0; i < 3; i++)
                {
                    if (isConnected)
                    {
                        Thread.Sleep(rnd.Next(3000, 5000));
                        long dateTime = DateTime.Now.Ticks;
                        DateTime now = new DateTime(dateTime);
                     
                        int lat = rnd.Next(268402834, 289498871);
                        int lng = rnd.Next(562935498, 583540187);
                        string location = "33." + lat + ",35." + lng;
                        string json = new JavaScriptSerializer().Serialize(new
                        {
                            name = "radar1",
                            data_type = "msg",
                            data = new
                            {
                                location = location
                            },

                        });
                        ws_Radar.Send(json);

                        this.Dispatcher.Invoke(() =>
                        {
                            textBlock_Radar.Text += now + "; threat detected  => location: " + location + "\n";
                            textBlock_Radar.Text += now + ";" + "sending event to the server\n";
                        });
                    }

                }
            }
            catch (Exception ex) { }
        }

    
    }
}
