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
using WebSocketSharp;
using System.IO;
namespace radar_drone_App_User
{
    /// <summary>
    /// Interaction logic for Drone.xaml
    /// </summary>
    /// 


    public partial class Drone : Window
    {

        private Battery battery;
        private int DroneID;
        private List<string> photos;
        private string location;
        
        Boolean isConnected_drone = false;
        WebSocketSharp.WebSocket ws_drone = new WebSocketSharp.WebSocket(url: "ws://localhost:8000/drone");
      
        public Drone()
        {
            battery = new Battery(100, 100);
            DroneID = 1;
            photos = new List<string>();
            photos.Add("firebasestorage.googleapis.com/v0/b/armydrone-8ee13.appspot.com/o/1.jpeg?alt=media&token=359e14e3-7b9c-42b3-88c2-bf86bcc07a6b");
            photos.Add("firebasestorage.googleapis.com/v0/b/armydrone-8ee13.appspot.com/o/2.jpeg?alt=media&token=e992ddaa-5995-4964-a3b1-124b3939a9ce");
            photos.Add("firebasestorage.googleapis.com/v0/b/armydrone-8ee13.appspot.com/o/3.jpeg?alt=media&token=a250aa75-0ca9-4675-bd29-5cc9d9a38d20");
            photos.Add("firebasestorage.googleapis.com/v0/b/armydrone-8ee13.appspot.com/o/4.jpeg?alt=media&token=0bcf0295-cbf2-421c-9a0f-e6dcb464601d");
            photos.Add("firebasestorage.googleapis.com/v0/b/armydrone-8ee13.appspot.com/o/5.jpeg?alt=media&token=4f0f4438-80cb-4c8d-9393-10c4294dda06");
            photos.Add("firebasestorage.googleapis.com/v0/b/armydrone-8ee13.appspot.com/o/8.jpeg?alt=media&token=6803eea0-dada-4bdd-bcc9-a8aae55506cb");
            photos.Add("firebasestorage.googleapis.com/v0/b/armydrone-8ee13.appspot.com/o/9.jpeg?alt=media&token=3b28248a-6629-4526-b6bb-8338b46f870f");
            photos.Add("firebasestorage.googleapis.com/v0/b/armydrone-8ee13.appspot.com/o/7.jpeg?alt=media&token=efa66632-b602-4af2-b523-e54ada62d313");
            InitializeComponent();

            Radar radar = new Radar();
            radar.Show();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            long dateTime = DateTime.Now.Ticks;
            DateTime now = new DateTime(dateTime);

            if (isConnected_drone)
            {
                ws_drone.Close();
                button.Content = "start drones";
                isConnected_drone = false;
                textBlock_drone.Text += now + ";" + " the drones disconnected\n";
            }
            else
            {
                ws_drone.Connect();
                ws_drone.OnMessage += (sender1, e1) => {
                    DateTime time = new DateTime(DateTime.Now.Ticks);
                    string data = e1.Data;

                    this.Dispatcher.Invoke(() =>
                    {
                        
                        string[] dataSplit = data.Split(':');
                        switch (dataSplit[0])
                        {
                            case "SCAN":
                                textBlock_drone.Text += time + ";" + "new message from MainApp =>  scan :" + dataSplit[1]+ "\n";
                                textBlock_drone.Text += time + ";" + "scanner drone start to scan : " + dataSplit[1] + "\n";
                                this.location = dataSplit[1];
                                if (battery.consumption > 0) battery.consumption--;
                                break;
                            case "PHOTO":
                                Random rnd = new Random();
                                int number = rnd.Next(0, 8);
                                textBlock_drone.Text += time + ";" + "new message from MainApp =>  get Photo \n";
                                ws_drone.Send("PHOTO:" +photos[number]);
                                break;
                            case "DATA":
                                textBlock_drone.Text += time + ";" + "new message from MainApp =>  get Data \n";
                                ws_drone.Send("DATA:" + DroneID + "," + battery.consumption + "," + location);
                                break;
                            default:
                                // code block
                                break;
                        }
                        
                    });

                };
                isConnected_drone = true;
                button.Content = "stop";
                textBlock_drone.Text += now + ";" + " the drones connected to the server\n";
            }
        }
    }
}
