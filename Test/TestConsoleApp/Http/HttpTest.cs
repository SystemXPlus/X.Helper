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

        public static void HttpGetWithCookieTest()
        {
            //var url = "http://oa.zgsmile.com/Page/Attachment/Detail.aspx?fileid=07fd0df1388a4106ba7dcaef5d2c0d82";
            var url = "http://localhost:5088/Api/Test/TestCookie";
            var handler = new X.Helper.Http.HttpHandler().SetUseCookies(false).UseProxy(new WebProxy("127.0.0.1:8888"));
            using (var client = new X.Helper.Http.Client(url, handler.Handler))
            {
                client.SetMethod(X.Helper.Http.Enums.HttpMethod.GET);
                var cookiestr = "ActivityCode=374693EAC7E5889467EBF6BCD7D3D74B; UseOldResumeDetail=false; LiveWSALA64567996=90adf78df0f145b6be3d71a17a1ffdfd; NALA64567996fistvisitetime=1747322319148; NALA64567996visitecounts=1; NALA64567996IP=%7C112.10.248.233%7C; Hm_lvt_4fecd4bd0b0840b8187dca3933577306=1747322320; NALA64567996lastvisitetime=1747322321303; NALA64567996visitepages=2; ASP.NET_SessionId=5yiemi013byfimeyj33mjjlr; Token=27793CA6F9F0BABF; Account=13888888888; Avatar=http://oa.zgsmile.com/Images/account.jpg; Name=Admin（admin）; Appkey=f5ad2a74417c4076a25ef4cae92964a3; UserId=19; NickName=管理员";
                var cookies = X.Helper.Http.Helper.CookieHelper.GetCookieCollection(cookiestr);
                //client.SetCookie(cookies);

                //var cookieComtainer = new CookieContainer();
                //cookieComtainer.Add(cookies);
                //handler.SetCookieContainer(cookieComtainer);
                //client.SetCookie("ASP.NET_SessionId", "l3f0yowyqujz0c1cvmel0rkn");
                //client.SetCookie("Account", "13888888888");
                //client.SetCookie("UserId", "19");
                //client.SetCookie("Token", "A92C607205D8445F");
                //client.SetCookie("Appkey", "f5ad2a74417c4076a25ef4cae92964a3");
                //client.SetCookie("ActivityCode", "374693EAC7E5889467EBF6BCD7D3D74B");
                using (var result = client.RequestTextContent().Result)
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
            if (headers != null)
            {
                foreach (string item in headers.Keys)
                {
                    Console.WriteLine($"{(item + ":").PadRight(30, ' ')}{headers.Get(item)}");
                }
            }
            Console.WriteLine();
            Console.WriteLine("COOKIES\t".PadRight(80, '_'));
            Console.WriteLine();
            var cookies = result.CookieCollection;
            if (cookies != null)
            {
                foreach (Cookie item in cookies)
                {
                    Console.WriteLine(X.Helper.Json.Serialize(item));
                    Console.WriteLine("".PadRight(40, '_') + "\r\n");
                    //Console.WriteLine($"{(item.Name + ":").PadRight(30, ' ')}{item.Value}");
                }
            }
        }

        private static void ShowRequestHeader()
        {

        }
        
    }
}
