using Nancy.Json;
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
    /// Interaction logic for EventApp.xaml
    /// </summary>
    public partial class EventApp : Window
    {

        private EventDetails EventsD;
        public List<EventDetails> m_EventDetails;
        private static readonly HttpClient client = new HttpClient();
        int index = -1;

        public EventApp(WebSocketSharp.WebSocket ws_app)
        {
            InitializeComponent();

            m_EventDetails = new List<EventDetails>();

            ws_app.OnMessage += (sender1, e1) => {
                DateTime time = new DateTime(DateTime.Now.Ticks);
                string data = e1.Data;

                this.Dispatcher.Invoke(() =>
                {

                    string[] dataSplit = Convert.ToString(data).Split(':');


                    if (!dataSplit[0].Contains("DATA") && !dataSplit[0].Contains("PHOTO"))
                    {
                        EventDetails eventD = (new JavaScriptSerializer()).Deserialize<EventDetails>(data);
                        m_EventDetails.Add(eventD);
                        listBox.Items.Add("new event detected > id:" + eventD.id + ", loc:" + eventD.location);

                    }




                });

            };
        }

        public async Task CloseEventAsync()
        {
            int chosenIndex = index;
            string json = new JavaScriptSerializer().Serialize(new
            {

                status = "closed",
                end_time = DateTime.Now,

        }); ;
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("http://localhost:8000/api/events/" + m_EventDetails[chosenIndex].id, content);
            if (response.StatusCode == HttpStatusCode.OK)
            {

                MessageBox.Show("event closed");
                m_EventDetails.RemoveAt(chosenIndex);
                DroneApp.m_EventDetails.RemoveAt(chosenIndex);
                listBox.Items.RemoveAt(chosenIndex);

            }


        }

        public void GetEventDetail()
        {

        }

        public async Task updateEventAsync()
        {
            int chosenIndex = index;
            string json = new JavaScriptSerializer().Serialize(new
            {
                type = textBoxtype.Text,
                status = textBoxstatus.Text,

            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("http://localhost:8000/api/events/" + m_EventDetails[chosenIndex].id, content);
            if (response.StatusCode == HttpStatusCode.OK)
            {

                MessageBox.Show("event updated");
                m_EventDetails[chosenIndex].type = textBoxtype.Text;
                m_EventDetails[chosenIndex].status = textBoxstatus.Text;
               
            }


        }


        private async void button1_Click(object sender, RoutedEventArgs e)
        {
          await  updateEventAsync();

        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            await CloseEventAsync();
        }



        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            index = listBox.SelectedIndex;
            if (index == -1) return;

            textBoxid.Text = m_EventDetails[index].id + "";
            textBoxlocation.Text = m_EventDetails[index].location + "";
            textBoxstatus.Text = m_EventDetails[index].status + "";
            textBoxtype.Text = m_EventDetails[index].type + "";
        }
    }
}
