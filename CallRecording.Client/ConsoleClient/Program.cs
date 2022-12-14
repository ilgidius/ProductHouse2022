using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net.Http;

namespace ConsoleClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string res = GetString("https://localhost:7018/api/post/newuser/Artur/srth3/admin");
            Console.WriteLine(res);
        }

        static string GetString(string requestUrl)
        {
            string res = string.Empty;

            try
            {
                using (var client = new HttpClient())
                {
                    res = client.GetAsync(requestUrl).Result.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }

            return res;
        }
    }
}
