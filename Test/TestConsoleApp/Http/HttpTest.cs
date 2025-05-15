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
                //client.SetMethod(X.Helper.Http.Enums.HttpMethod.GET);
                using (var result = client.RequestAsync().Result)
                {
                    Console.WriteLine(X.Helper.Json.Serialize(result));
                }
            }
            
        }
        public static void HttpPostTest()
        {
            var url = "http://api.zgsmile.com/Api/Resume/GetAllSiteRegularList";
            var handler = new X.Helper.Http.HttpHandler();
            using (var client = new X.Helper.Http.Client(url, handler.Handler))
            {
                client.SetMethod(X.Helper.Http.Enums.HttpMethod.POST);
                using (var result = client.RequestTextContent().Result)
                {
                    Console.WriteLine(X.Helper.Json.Serialize(result));
                }
            }
        }

        public static void HttpDownloadFileTestt()
        {
            var url = "http://www.zgsmile.com/Image/logo.png";
            var handler = new X.Helper.Http.HttpHandler();
            using (var client = new X.Helper.Http.Client(url, handler.Handler))
            {
                client.SetMethod(X.Helper.Http.Enums.HttpMethod.GET);
                using (var result = client.RequestDownloadFile(@"D:\testdownload.png").Result)
                {
                    Console.WriteLine(X.Helper.Json.Serialize(result));
                }
            }
        }
        
    }
}
