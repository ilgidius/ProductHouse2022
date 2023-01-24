using Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Client.Managers.EventManager
{
    public class EventManager
    {
        public void AddEvent(string url, string token)
        {
            Console.Write("Enter event type: ");
            var bodyContent = new Dictionary<string, string>
                {
                    { "eventType", Console.ReadLine() }
                };
            Console.Write("Enter key: ");
            bodyContent.Add("key", Console.ReadLine());
            Console.Write("Enter value: ");
            bodyContent.Add("value", Console.ReadLine());
            Console.Write("Enter user identifier: ");
            string identifier = Console.ReadLine();
            int.TryParse(identifier, out int id);
            AddEventsWithId newEvent = new AddEventsWithId();
            HttpContent httpContent;
            if (id != 0)
            {
                url += "id";
                newEvent.AddedTime = DateTime.Now.ToString();
                newEvent.SentTime = DateTime.Now.ToString();
                newEvent.UserId = id;
                newEvent.Value = bodyContent["value"];
                newEvent.Key = bodyContent["key"];
                newEvent.EventType = bodyContent["eventType"];
                httpContent = new StringContent(JsonConvert.SerializeObject(newEvent),
                    Encoding.Default, "application/json");
            }
            else
            {
                url += "username";
                bodyContent.Add("login", identifier);
                bodyContent.Add("addedTime", DateTime.Now.ToString());
                bodyContent.Add("sentTime", DateTime.Now.ToString());
                httpContent = new StringContent(JsonConvert.SerializeObject(bodyContent),
                    Encoding.Default, "application/json");
            }

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.PostAsync(url, httpContent);
                Console.WriteLine("Response status code: " + response.Result.StatusCode);
            }
        }
    }
}
