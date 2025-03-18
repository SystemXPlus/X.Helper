using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace X.Helper.Http
{
    public partial class Client
    {
        #region 私有字段
        private readonly HttpClient _HttpClient;

        private CookieContainer cookieContainer;

        private Uri Uri { get; set; }
        private Enums.HttpMethod Method { get; set; } = Enums.HttpMethod.GET;
        private string Referer { get; set; }
        private string Origin { get; set; }
        private string UserAgent { get; set; } = "X.Helper.Http.Client";
        private IPEndPoint IPEndPoint { get; set; } = null;
        private string ContentType { get; set; }
        private Encoding Encoding { get; set; } = Encoding.UTF8;

        private CookieCollection CookieCollection { get; set; }

        /**
         * 在HTTP/1.0中，Keep-Alive功能是默认关闭的，需要在请求头中添加Connection: Keep-Alive来启用。
         * 而在HTTP/1.1中，Keep-Alive功能默认启用，如果需要关闭，则需要在请求头中添加Connection: close‌
         * */
        /// <summary>
        /// 
        /// </summary>
        private bool KeepAlive { get; set; } = true;

        private Version Version { get; set; } = HttpVersion.Version11;

        private TimeSpan Timeout = TimeSpan.FromSeconds(100.0);

        private HttpRequestMessage RequestMessage { get; set; }
        private HttpResponseMessage ResponseMessage { get; set; }
        #endregion

        #region 构造
        public Client() : this(uri: null, handler: null)
        {

        }

        public Client(Uri uri) : this(uri: uri, handler: null)
        {

        }

        public Client(string url) : this(uri: new Uri(url), handler: null)
        {

        }

        public Client(string url, HttpMessageHandler handler) : this(uri: new Uri(url), handler: handler)
        {
        }

        public Client(Uri uri, HttpMessageHandler handler)
        {
            if (handler != null)
            {
                _HttpClient = new HttpClient(handler);
            }
            else
            {
                _HttpClient = new HttpClient();
            }

            if (uri != null)
            {
                this.Uri = uri;
            }
        }

        ~Client()
        {
            try
            {
                if (RequestMessage != null)
                    RequestMessage.Dispose();
                if (_HttpClient != null)
                    _HttpClient.Dispose();
            }
            catch
            {
            }
        }
        #endregion


        #region 请求

        public async Task<Result> RequestAsync()
        {
            if (Uri == null)
                throw new ArgumentNullException("未指定请求的Uri");
            var url = Uri.ToString();
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("必填参数：url");
            var tmpUrl = url.ToLower();
            if (!tmpUrl.ToLower().StartsWith("http"))
                throw new ArgumentException("无效参数：url");
            if (tmpUrl.StartsWith("https://"))
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
            }
            Result tmpResult;
            try
            {
                await CreateRequestMessage();
            }
            catch (Exception ex)
            {
                tmpResult = new Result
                {
                    StatusDescription = $"构建请求消息失败：{ex.Message}"
                };
                return tmpResult;
            }
            var result = new Result();
            try
            {
                using (this.ResponseMessage = await _HttpClient.SendAsync(RequestMessage))
                {

                    //处理ResponseMessage返回Result
                    await HandleResponseMessage(result);
                }
            }
            catch (Exception ex)
            {
                tmpResult = new Result
                {
                    StatusDescription = $"请求失败：{ex.Message}"
                };
                return tmpResult;
            }
            return result;
        }

        #endregion

        #region 创建请求
        private async Task CreateRequestMessage()
        {
            RequestMessage = new HttpRequestMessage(new HttpMethod(Method.ToString()), Uri);

            if (!string.IsNullOrEmpty(Referer))
                RequestMessage.Headers.Add("Referer", Referer);
            if (!string.IsNullOrEmpty(UserAgent))
                RequestMessage.Headers.Add("User-Agent", UserAgent);
            if (!string.IsNullOrEmpty(Origin))
                RequestMessage.Headers.Add("Origin", Origin);
            RequestMessage.Headers.Add("Connection", KeepAlive ? "Keep-Alive" : "close");

            if (Method != Enums.HttpMethod.GET)
            {
                if (!string.IsNullOrEmpty(ContentType))
                    ContentType = "text/plain";
                RequestMessage.Content = new StringContent("", Encoding, ContentType);
            }

            //增加HTTPS请求的特殊处理，强制使用Http1.0 BY QQ607432
            if (this.Uri.ToString().StartsWith("https", StringComparison.CurrentCultureIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((message, cert, chain, errors) => true);
                RequestMessage.Version = HttpVersion.Version10;
            }
            else
            {
                RequestMessage.Version = Version;

            }
        }
        #endregion

        #region 处理返回结果
        private async Task HandleResponseMessage(Result result)
        {
            result.StatusCode = ResponseMessage.StatusCode;
            result.StatusDescription = ResponseMessage.ReasonPhrase;
            //result.HeaderCollection = ResponseMessage.Headers;
            result.Content = await ResponseMessage.Content.ReadAsStringAsync();
            result.ResponseUri = ResponseMessage.RequestMessage.RequestUri.ToString();
            //result.CookieCollection = cookieContainer.GetCookies(Uri);

        }
        #endregion




    }
}
