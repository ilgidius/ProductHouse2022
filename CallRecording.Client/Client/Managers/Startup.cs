using Client.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Client.Managers
{
    public class Startup
    { 
        public User Login(string url)
        {
            User user = new User();

            Console.Write("Username: ");
            user.Login = Console.ReadLine() ?? string.Empty;
            Console.Write("Password: ");
            user.Token = GetToken(url, user.Login, Console.ReadLine() ?? string.Empty, out string status);
            Console.WriteLine("Response status code: " + status);
            Thread.Sleep(1000);
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(user.Token);
            user.Role = jwt.Claims.First(c => c.Type == ClaimTypes.Role).Value;
            return user;
        }

        public string GetToken(string url, string username, string password, out string status)
        {
            string token = string.Empty;
            using (var httpClient = new HttpClient())
            {
                var bodyContent = new Dictionary<string, string>
                {
                    { "login", username },
                    { "password",  password }
                };

                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(bodyContent),
                    Encoding.Default, "application/json");
                var response = httpClient.PostAsync(url, httpContent);
                if (response.Result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    status = response.Result.StatusCode.ToString();
                    return token;
                }
                token = response.Result.Content.ReadAsStringAsync().Result;
                status = response.Result.StatusCode.ToString();
            }
            return token;
        }

        public async Task WBS(string url, string message)
        {
            using var ws = new ClientWebSocket();
            await ws.ConnectAsync(new Uri("wss://localhost:7192/ws"), CancellationToken.None);

            ArraySegment<byte> arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
            await ws.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);

            ArraySegment<byte> bytesReceived = new ArraySegment<byte>(new byte[1024 * 4]);
            WebSocketReceiveResult result = await ws.ReceiveAsync(bytesReceived, CancellationToken.None);
            var response = Encoding.UTF8.GetString(bytesReceived.Array, 0, result.Count);

            Console.WriteLine(response);
        }
    }
}
