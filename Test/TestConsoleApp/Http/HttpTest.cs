using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
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
                    ShowResult(result);
                }
            }
        }

        public static void HttpGet301Test()
        {
            var url = "http://zgsmile.com";
            var handler = new X.Helper.Http.HttpHandler();
            handler.AllowAutoRedirect(true);
            using (var client = new X.Helper.Http.Client(url, handler.Handler))
            {
                
                client.SetMethod(X.Helper.Http.Enums.HttpMethod.GET);
                using (var result = client.RequestTextContent().Result)
                {
                    ShowResult(result);
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
                    ShowResult(result);
                }
            }
        }

        public static void HttpDownloadFileTest()
        {
            var url = "http://www.zgsmile.com/Image/logo.png";
            var handler = new X.Helper.Http.HttpHandler();
            using (var client = new X.Helper.Http.Client(url, handler.Handler))
            {
                client.SetMethod(X.Helper.Http.Enums.HttpMethod.GET);
                using (var result = client.RequestDownloadFile(@"D:\testdownload.png").Result)
                {
                    ShowResult(result);
                }
            }
        }

        public static void HttpDownloadFileWithCookieTest()
        {
            var url = "http://oa.zgsmile.com/Page/Attachment/Down.aspx?fileid=07fd0df1388a4106ba7dcaef5d2c0d82";
            var handler = new X.Helper.Http.HttpHandler();
            using (var client = new X.Helper.Http.Client(url, handler.Handler))
            {
                client.SetMethod(X.Helper.Http.Enums.HttpMethod.GET);
                client.SetCookie("ASP.NET_SessionId", "h5f45ggm4w20xkg1uzbajpzz");
                using (var result = client.RequestDownloadFile(@"D:\test.html").Result)
                {
                    ShowResult(result);
                }
            }
        }


        private static void ShowResult(X.Helper.Http.Result result)
        {
            Console.WriteLine(X.Helper.Json.Serialize(result));
            Console.WriteLine();
            Console.WriteLine("HEADERS\t".PadRight(80, '_'));
            Console.WriteLine();
            var headers = result.HeaderCollection;
            foreach (string item in headers.Keys)
            {
                Console.WriteLine($"{(item+":").PadRight(30,' ')}{headers.Get(item)}");
            }
            Console.WriteLine();
            Console.WriteLine("COOKIES\t".PadRight(80, '_'));
            Console.WriteLine();
            var cookies = result.CookieCollection;
            foreach (Cookie item in cookies)
            {
                Console.WriteLine(X.Helper.Json.Serialize(item));
                Console.WriteLine("".PadRight(40, '_') + "\r\n");
                //Console.WriteLine($"{(item.Name + ":").PadRight(30, ' ')}{item.Value}");
            }
        }

        private static void ShowRequestHeader()
        {

        }
        
    }
}
