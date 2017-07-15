using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        HttpClient client;

        public MainPage()
        {
            this.InitializeComponent();
            client = new HttpClient();

            string response = Request("http://172.16.80.2/api/task");
            List<Task> tasks;

            try
            {
                tasks = JsonConvert.DeserializeObject<List<Task>>(response);
            }
            catch
            {
                tasks = new List<Task>();
                return;
            }
            
            foreach (var task in tasks)
            {
                string value = Request(string.Format("http://172.16.80.2/api/assignment/{0}", task.TaskID));
                try
                {
                    task.assignments = JsonConvert.DeserializeObject<List<User>>(value);
                }
                catch
                {
                    task.assignments = new List<User>();
                }
            }
        }

        private string Request(string uri)
        {
            return Request(uri, HttpMethod.Get);
        }

        private string Request(string uri, HttpMethod method)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri(uri)
            };

            HttpResponseMessage response = client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().Result;
            }

            return null;
        }

        private async void Button_Holding(object sender, HoldingRoutedEventArgs e)
        {
            await new MessageDialog("Hej", "Hej2").ShowAsync();
        }

        public class Task
        {
            public int TaskID;
            public DateTime BeginDateTime;
            public DateTime DeadlineDateTime;
            public string Title;
            public string Requirements;
            public List<User> assignments;
        }

        public class User
        {
            public int UserID;
        }
    }
}
