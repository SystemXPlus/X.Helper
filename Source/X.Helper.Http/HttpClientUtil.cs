using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace X.Helper.Http
{
    public class HttpClientUtil
    {
        internal static readonly HttpClient _HttpClient = new HttpClient();

        public static string Post(string url, string content)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("必填参数：url");
            url = url.ToLower();
            if (!url.ToLower().StartsWith("http"))
                throw new ArgumentException("无效参数：url");
            if (url.StartsWith("https://"))
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
            }
            
            using (HttpContent httpContent = new StringContent(content, Encoding.UTF8, "application/json"))
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    httpRequestMessage.Content = httpContent;
                    var httpClient = _HttpClient;
                    using (HttpResponseMessage httpResponseMessage = httpClient.SendAsync(httpRequestMessage).Result)
                    {
                        if (httpResponseMessage.IsSuccessStatusCode)
                        {
                            var responseResult = httpResponseMessage.Content.ReadAsStringAsync().Result;
                            return responseResult;
                        }
                        else
                        {
                            throw new Exception($"请求异常，状态码：{httpResponseMessage.StatusCode}\r\n返回正文：{httpResponseMessage.Content.ReadAsStringAsync().Result}");
                        }
                    }
                }
            }
        }

        public static string Get(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("必填参数：url");
            url = url.ToLower();
            if (!url.ToLower().StartsWith("http"))
                throw new ArgumentException("无效参数：url");
            if (url.StartsWith("https://"))
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
            }
            using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url))
            {
                var httpClient = _HttpClient;
                using (HttpResponseMessage httpResponseMessage = httpClient.SendAsync(httpRequestMessage).Result)
                {
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        var responseResult = httpResponseMessage.Content.ReadAsStringAsync().Result;
                        return responseResult;
                    }
                    else
                    {
                        throw new Exception($"请求异常，状态码：{httpResponseMessage.StatusCode}\r\n返回正文：{httpResponseMessage.Content.ReadAsStringAsync().Result}");
                    }
                }
            }

        }
    }
}
