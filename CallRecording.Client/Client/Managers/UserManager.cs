using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Client.Managers
{
    public class UserManager
    {
        public string? Username { get; private set; }
        public string? Role { get; private set; }
        private string? _password { get; set; }
        private string? _token { get; set; }

        private WebSocketManager _wbs;
        public readonly HttpRequestsManager HttpManager = new HttpRequestsManager();

        private CancellationTokenSource _CTS;

        private TimeSpan _period = TimeSpan.FromMinutes(14) + TimeSpan.FromSeconds(30);

        private readonly Thread _threadRefreshToken;

        public UserManager()
        {
            _threadRefreshToken = new Thread(RefreshToken);
        }

        public async Task Authorization()
        {
            Username = string.Empty;
            Role = string.Empty;
            _password = string.Empty;
            _token = string.Empty;
            HttpStatusCode code;
            do
            {
                Console.WriteLine("Please enter your correct login and password.");
                SetCredentials();
                code = await GetToken();
                if (code == HttpStatusCode.OK)
                {
                    Console.WriteLine("Authorized successfully!\nStatus code: " + code);
                }
                Console.WriteLine();
            } while (code != HttpStatusCode.OK);
            Task.Delay(1000).Wait();
        }

        public void SetCredentials()
        {
            Console.Write("Username: ");
            Username = Console.ReadLine() ?? string.Empty;
            Console.Write("Password: ");
            _password = Console.ReadLine() ?? string.Empty;
        }

        public async Task<HttpStatusCode> GetToken()
        {
            HttpManager.ResetBodyContent("Login", Username, "Password", _password);
            using (var httpClient = new HttpClient())
            {
                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(HttpManager.GetBodyContent()),
                    Encoding.Default, "application/json");
                HttpResponseMessage response;
                try
                {
                    response = await httpClient.PostAsync("https://localhost:7192/api/users/login", httpContent);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Console.WriteLine("Authorization is failed!\nStatus code: " + response.StatusCode.ToString());
                        return response.StatusCode;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to reach the server");
                    Console.WriteLine(ex.Message);
                    return HttpStatusCode.NotFound;
                }
                _token = response.Content.ReadAsStringAsync().Result;
                var jwtReader = new JwtSecurityTokenHandler().ReadJwtToken(_token);
                Role = jwtReader.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
                HttpManager.RefreshToken(_token);
            }
            if (_threadRefreshToken.ThreadState != ThreadState.Running)
            {
                _threadRefreshToken.Start();
            }
            return HttpStatusCode.OK;
        }
        private async void RefreshToken()
        {
            _CTS = new CancellationTokenSource();
            using PeriodicTimer timer = new PeriodicTimer(_period);
            while (!_CTS.IsCancellationRequested && await timer.WaitForNextTickAsync(_CTS.Token))
            {
                if (await GetToken() != HttpStatusCode.OK)
                {
                    Console.WriteLine("For further requests you need to reauthorize!");
                    _CTS.Cancel();
                }
            }
            _CTS.Dispose();
        }

        public void WBS(string eventType)
        {
            try
            {
                _wbs = new WebSocketManager();
                try
                {
                    _wbs.ConnectAsync(_token, eventType).Wait();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    _wbs.Dispose();
                    return;
                }

                Console.WriteLine("Press 'Esc' key to close connection");
                while (_wbs.isConnected)
                {
                    if (Console.ReadKey().Key == ConsoleKey.Escape)
                    {
                        break;
                    }
                }
                _wbs.DisconnectAsync().Wait();
                _wbs = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _wbs.Dispose();
            }
        }
    }
}
