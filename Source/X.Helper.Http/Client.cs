using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace X.Helper.Http
{
    public partial class Client : IDisposable
    {
        #region 私有字段
        private readonly HttpClient _HttpClient;

        //private CookieContainer cookieContainer;

        private Uri _Uri { get; set; }
        private Enums.HttpMethod _Method { get; set; } = Enums.HttpMethod.GET;
        private string _Referer { get; set; }
        private string _Origin { get; set; }
        private string _UserAgent { get; set; } = "X.Helper.Http.Client";
        private IPEndPoint _IPEndPoint { get; set; } = null;
        private string _ContentType { get; set; }
        private Encoding _Encoding { get; set; } = Encoding.UTF8;

        private MemoryStream _StreamContent;

        //使用HttpRequestMessage.Headers处理

        //private WebHeaderCollection _WebHeaderCollection;
        //private WebHeaderCollection WebHeaderCollection
        //{
        //    get
        //    {
        //        if (_WebHeaderCollection == null)
        //        {
        //            _WebHeaderCollection = new WebHeaderCollection();
        //        }
        //        return _WebHeaderCollection;
        //    }
        //    set
        //    {
        //        _WebHeaderCollection = value;
        //    }
        //}

        private CookieCollection _CookieCollection;
        private CookieCollection CookieCollection
        {
            get
            {
                if (_CookieCollection == null)
                {
                    _CookieCollection = new CookieCollection();
                }
                return _CookieCollection;
            }
            set
            {
                _CookieCollection = value;
            }
        }

        /**
         * 在HTTP/1.0中，Keep-Alive功能是默认关闭的，需要在请求头中添加Connection: Keep-Alive来启用。
         * 而在HTTP/1.1中，Keep-Alive功能默认启用，如果需要关闭，则需要在请求头中添加Connection: close‌
         * */
        /// <summary>
        /// 
        /// </summary>
        private bool _KeepAlive { get; set; } = true;

        private Version _Version { get; set; } = HttpVersion.Version11;

        private TimeSpan _Timeout = TimeSpan.FromSeconds(100.0);

        private HttpRequestMessage _RequestMessage { get; set; }
        private HttpResponseMessage _ResponseMessage { get; set; }

        private bool _DetectEncodingFromByteOrderMarks = false;

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
                this._Uri = uri;
            }
        }


        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_RequestMessage != null)
                        _RequestMessage.Dispose();
                    if (_ResponseMessage != null)
                        _ResponseMessage.Dispose();
                    if (_HttpClient != null)
                        _HttpClient.Dispose();
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        ~Client()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion


        #region 请求

        public async Task<Result> RequestAsync()
        {
            if (_Uri == null)
                throw new ArgumentNullException("未指定请求的Uri");
            var url = _Uri.ToString();
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
                using (this._ResponseMessage = await _HttpClient.SendAsync(_RequestMessage))
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

        public async Task<Result> PostFile(string filePath)
        {
            //TODO 上传文件
            return null;
        }

        public async Task<Result> PostFile(string filePath, IProgress<double> progress)
        {
            //TODO 上传文件 带进度
            throw new NotImplementedException();
        }

        public async Task<Result> PostFile(byte[] bytes)
        {
            //TODO 上传文件
            throw new NotImplementedException();
        }

        public async Task<Result> PostFile(byte[] bytes, IProgress<double> progress)
        {
            //TODO 上传文件
            throw new NotImplementedException();
        }

        public async Task<Result> DownloadFile()
        {
            //TODO 下载文件
            throw new NotImplementedException();
        }

        #endregion

        #region 创建请求
        private async Task CreateRequestMessage()
        {
            _RequestMessage = new HttpRequestMessage(new HttpMethod(this._Method.ToString()), _Uri);

            if (!string.IsNullOrEmpty(_Referer))
                _RequestMessage.Headers.Add("Referer", _Referer);
            if (!string.IsNullOrEmpty(_UserAgent))
                _RequestMessage.Headers.Add("User-Agent", _UserAgent);
            if (!string.IsNullOrEmpty(_Origin))
                _RequestMessage.Headers.Add("Origin", _Origin);
            _RequestMessage.Headers.Add("Connection", _KeepAlive ? "Keep-Alive" : "close");

            if (this._Method != Enums.HttpMethod.GET)
            {
                if (!string.IsNullOrEmpty(_ContentType))
                    _ContentType = "text/plain";
                _RequestMessage.Content = new StringContent("", _Encoding, _ContentType);
            }

            //增加HTTPS请求的特殊处理，强制使用Http1.0 BY QQ607432
            if (this._Uri.ToString().StartsWith("https", StringComparison.CurrentCultureIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((message, cert, chain, errors) => true);
                _RequestMessage.Version = HttpVersion.Version10;
            }
            else
            {
                _RequestMessage.Version = _Version;

            }
        }
        #endregion

        #region 处理返回结果
        private async Task HandleResponseMessage(Result result)
        {
            result.StatusCode = _ResponseMessage.StatusCode;
            result.StatusDescription = _ResponseMessage.ReasonPhrase;
            result.ResponseUri = _ResponseMessage.RequestMessage.RequestUri.ToString();
            if (_ResponseMessage.Headers != null)
            {
                result.HeaderCollection = new WebHeaderCollection();
                foreach (var item in _ResponseMessage.Headers)
                {
                    result.HeaderCollection.Add(item.Key, string.Join(";", item.Value));
                }
            }
            result.CookieCollection = Helper.CookieHelper.GetCookieCollection(_ResponseMessage.Headers);

            using (var stream = await _ResponseMessage.Content.ReadAsStreamAsync())
            {
                if (this._StreamContent == null)
                {
                    this._StreamContent = new System.IO.MemoryStream();
                }
                await stream.CopyToAsync(this._StreamContent);
                await this._StreamContent.FlushAsync();
            }
        }

        private async Task GetTextContent(Result result)
        {
            if (this._StreamContent == null)
                return;
            using (var reader = new System.IO.StreamReader(this._StreamContent, _Encoding, this._DetectEncodingFromByteOrderMarks))
            {
                result.TextContent = await reader.ReadToEndAsync();
            }
            this._StreamContent.Position = 0;
        }

        private async Task GetStreamContent(Result result)
        {
            using (var stream = await _ResponseMessage.Content.ReadAsStreamAsync())
            {
                if (this._StreamContent == null)
                {
                    this._StreamContent = new System.IO.MemoryStream();
                }
                await stream.CopyToAsync(this._StreamContent);
                await this._StreamContent.FlushAsync();
            }
        }
        #endregion




    }
}
