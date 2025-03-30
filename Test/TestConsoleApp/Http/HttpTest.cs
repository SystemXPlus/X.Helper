using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApp.Http
{
    public static class HttpTest
    {

        public static void HttpGetTest()
        {
            var url = "https://www.baidu.com";
            var handler = new X.Helper.Http.HttpHandler();
            using (var client = new X.Helper.Http.Client(url, handler.Handler))
            {
                using (var result = client.RequestAsync().Result)
                {
                    Console.WriteLine(X.Helper.Json.Serialize(result));
                }
            }
            
        }
    }
}
