using Nancy.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    /// Interaction logic for AppUser.xaml
    /// </summary>
    public partial class AppUser : Window
    {
        private DroneApp droneAp;
        private EventApp eventAp;
        private ReportApp reportAp;
        private User user;
        private static readonly HttpClient client = new HttpClient();
        WebSocketSharp.WebSocket ws_app;
        public AppUser()
        {
            InitializeComponent();
        }



        private async void btn_click(object sender, RoutedEventArgs e)
        {
            await LoginAsync();
        }


          private async Task LoginAsync()
        {

 
            string json = new JavaScriptSerializer().Serialize(new
            {
                username = textBoxUser.Text,
                password = textBoxPass.Text,

            });
            var content=  new StringContent(json, Encoding.UTF8, "application/json");    
            var response = await client.PostAsync("http://localhost:8000/api/auth/login", content);          
            if (response.StatusCode == HttpStatusCode.OK)
            {
               string responseString = await response.Content.ReadAsStringAsync();
               user = (new JavaScriptSerializer()).Deserialize<User>(responseString);

                ws_app = new WebSocketSharp.WebSocket(url: "ws://localhost:8000/app");
                ws_app.Connect();
                this.Hide();
                switch (user.role)
                {
                    case "HELP_DESK":
                     droneAp = new DroneApp(ws_app);
                     eventAp = new EventApp(ws_app);       
                     droneAp.Show();
                     eventAp.Show();
                        break;
                    case "DRONE_OPERATOR":
                        droneAp = new DroneApp(ws_app);           
                        reportAp = new ReportApp(ws_app);
                        droneAp.Show();
                        reportAp.Show();
                        break;
                    case "FORCES":
                        droneAp = new DroneApp(ws_app);
                        reportAp = new ReportApp(ws_app);
                        droneAp.Show();
                        reportAp.Show();
                        break;
                    default:
                        // code block
                        break;
                }

            }
            else
            {
                MessageBox.Show("wrong user, password");

            }
                


        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Radar radar = new Radar();
            Drone dron = new Drone();

            dron.Show();
            radar.Show();
        }
    }
}
