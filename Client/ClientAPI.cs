using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ClientAPI
    {
        private HttpClient Client;
        private string Host;
        private string HttpHost;

        public ClientAPI(string host)
        {
            Client = new HttpClient();
            Host = host;
            HttpHost = string.Format("http://{0}", host);
        }

        public List<Task> GetTasks()
        {
            string response = Request(string.Format("{0}/api/task", HttpHost));
            List<Task> tasks;

            try
            {
                tasks = JsonConvert.DeserializeObject<List<Task>>(response);
            }
            catch
            {
                return new List<Task>();
            }

            foreach (var task in tasks)
            {
                string value = Request(string.Format("{0}/api/assignment/{1}", HttpHost, task.TaskID));
                try
                {
                    task.assignments = JsonConvert.DeserializeObject<List<User>>(value);
                }
                catch
                {
                    task.assignments = new List<User>();
                }
            }

            return tasks;
        }

        public Task GetTask(int taskID)
        {
            Task task;

            string response = Request(string.Format("{0}/api/task/{1}", HttpHost, taskID));
            try
            {
                task = JsonConvert.DeserializeObject<Task>(response);
            }
            catch
            {
                return null;
            }

            response = Request(string.Format("{0}/api/assignment/{1}", HttpHost, task.TaskID));
            try
            {
                task.assignments = JsonConvert.DeserializeObject<List<User>>(response);
            }
            catch
            {
                task.assignments = new List<User>();
            }

            return task;
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

            HttpResponseMessage response = Client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().Result;
            }

            return null;
        }
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
        public string Firstname;
        public string Lastname;
    }
}
