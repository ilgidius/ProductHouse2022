using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Net.Http;
using System;

namespace Client.Managers
{
    public class HttpRequestsManager
    {
        private string _token { get; set; }
        private Dictionary<string, string> _bodyContent { get; set; } = new Dictionary<string, string>();

        public void RefreshToken(string token)
        {
            _token = token;
        }

        public Dictionary<string, string> GetBodyContent()
        {
            return _bodyContent;
        }

        public void ResetBodyContent(params string[] keyValuePair)
        {
            if (keyValuePair.Length % 2 != 0)
            {
                throw new ArgumentException("Absent value argument");
            }
            _bodyContent.Clear();
            for (int i = 0; i < keyValuePair.Length; i += 2)
            {
                _bodyContent.Add(keyValuePair[i], keyValuePair[i + 1]);
            }
        }
        public async Task GetAsync(string url)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                HttpResponseMessage response;
                try
                {
                    response = await httpClient.GetAsync(url);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Console.WriteLine("Request is failed!\nStatus code: " + response.StatusCode.ToString());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to reach the server");
                    Console.WriteLine(ex.Message);
                    return;
                }
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
        }
        public async Task PostAsync(string url, params string[] keyValuePair)
        {
            try
            {
                ResetBodyContent(keyValuePair);
            }
            catch
            {
                throw;
            }
            using (var httpClient = new HttpClient())
            {
                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(_bodyContent),
                    Encoding.Default, "application/json");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                HttpResponseMessage response;
                try
                {
                    response = await httpClient.PostAsync(url, httpContent);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Console.WriteLine("Request is failed!\nStatus code: " + response.StatusCode.ToString());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to reach the server");
                    Console.WriteLine(ex.Message);
                    return;
                }
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
        }
        public async Task PutAsync(string url, params string[] keyValuePair)
        {
            try
            {
                ResetBodyContent(keyValuePair);
            }
            catch
            {
                throw;
            }
            using (var httpClient = new HttpClient())
            {
                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(_bodyContent),
                    Encoding.Default, "application/json");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                HttpResponseMessage response;
                try
                {
                    response = await httpClient.PutAsync(url, httpContent);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Console.WriteLine("Request is failed!\nStatus code: " + response.StatusCode.ToString());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to reach the server");
                    Console.WriteLine(ex.Message);
                    return;
                }
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
        }
        public async Task PatchAsync(string url)
        {
            using (var httpClient = new HttpClient())
            {
                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(string.Empty),
                    Encoding.Default, "application/json");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                HttpResponseMessage response;
                try
                {
                    response = await httpClient.PatchAsync(url, httpContent);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Console.WriteLine("Request is failed!\nStatus code: " + response.StatusCode.ToString());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to reach the server");
                    Console.WriteLine(ex.Message);
                    return;
                }
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
        }

        public async Task DeleteAsync(string url)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                HttpResponseMessage response;
                try
                {
                    response = await httpClient.DeleteAsync(url);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Console.WriteLine("Request is failed!\nStatus code: " + response.StatusCode.ToString());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to reach the server");
                    Console.WriteLine(ex.Message);
                    return;
                }
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
        }

        public async Task ChunkedRequest()
        {
            string page = "https://localhost:7192/api/chunked";

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                using (var responce = await httpClient.GetStreamAsync(page))
                {
                    StreamReader reader = new StreamReader(responce);
                    while (!reader.EndOfStream)
                    {
                        var chunkSizeStr = reader.ReadLine().Trim();
                        Console.WriteLine(chunkSizeStr);
                    }
                }
            }
        }
    }
}
