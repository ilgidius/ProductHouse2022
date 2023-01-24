using Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Client.Managers.UserManager
{
    public class UserManager
    {
        public void AddNewUser(string url, string token)
        {
            User user = new User();

            Console.Write("Enter login for new user: ");
            user.Login = Console.ReadLine();
            Console.Write("Enter role for new user: ");
            user.Role = Console.ReadLine();
            Console.Write("Enter password for new user: ");

            using (var httpClient = new HttpClient())
            {
                var bodyContent = new Dictionary<string, string>
                {
                    { "login", user.Login },
                    { "password", Console.ReadLine() },
                    { "role", user.Role }
                };

                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(bodyContent),
                    Encoding.Default, "application/json");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.PostAsync(url, httpContent);
                Console.WriteLine("Response status code: " + response.Result.StatusCode);
            }
        }

        public void UpdateUser(string url, string token)
        {
            Console.Write("Enter user's login: ");
            string username = Console.ReadLine();
            Console.Write("Enter new password: ");

            using (var httpClient = new HttpClient())
            {
                var bodyContent = new Dictionary<string, string>
                {
                    { "login", username },
                    { "password", Console.ReadLine() }
                };

                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(bodyContent),
                    Encoding.Default, "application/json");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = httpClient.PutAsync(url, httpContent);
                Console.WriteLine("Response status code: " + response.Result.StatusCode);
            }
        }

        public void DeleteUser(string url, string token)
        {
            Console.Write("Enter user's identifier: ");
            string userInfo = Console.ReadLine();
            int.TryParse(userInfo, out int id);

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                Task<HttpResponseMessage> response;
                if (id != 0)
                {
                    response = httpClient.DeleteAsync(url + $"/id/{id}");
                }
                else
                {
                    response = httpClient.DeleteAsync(url + $"/username/{userInfo}");
                }
                Console.WriteLine("Response status code: " + response.Result.StatusCode);
            }
        }
    }
}
