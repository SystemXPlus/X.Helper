using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace X.Helper.Http
{
    public partial class Client : IDisposable
    {

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
        public Client(string url, HttpHandler handler) : this(uri: new Uri(url), handler: handler.Handler)
        {

        }
        public Client(string url, HttpMessageHandler handler) : this(uri: new Uri(url), handler: handler)
        {

        }

        public Client(Uri uri, HttpMessageHandler handler)
        {
            if (handler != null)
            {
                _HttpHandler = handler;
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
        #endregion

        #region DISPOSE
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
                    if (_StreamContent != null)
                        _StreamContent.Dispose();
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
        /// <summary>
        /// 发起请求前预处理
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        private void PreRequest()
        {
            if (_Uri == null)
                throw new ArgumentNullException("未指定请求的Uri");
            var url = _Uri.ToString();
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("必填参数：url");
            var tmpUrl = url.ToLower();
            if (!tmpUrl.ToLower().StartsWith("http"))
                throw new ArgumentException("无效参数：url，仅支持Http/Https协议");
            if (tmpUrl.StartsWith("https://"))
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
            }

            CreateRequestMessage();
            CreateRequestHeader();
            CreateRequestContent();
        }

        public async Task<Result> RequestAsync()
        {
            PreRequest();



            var result = new Result();
            try
            {

                _HttpClient.Timeout = this._Timeout;
                if (this._Method != Enums.HttpMethod.GET)
                {
                    if (this._RequestHttpContent != null)
                    {
                        _RequestMessage.Content = this._RequestHttpContent;
                    }
                }
                using (this._ResponseMessage = await _HttpClient.SendAsync(_RequestMessage))
                {
                    //处理ResponseMessage返回Result
                    await HandleResponseMessage(result);
                    //AllowAutoRedirect = false 时判断返回3XX重定向状态处理
                    if (_HttpHandler is HttpClientHandler httpClientHandler)
                    {
                        if (!httpClientHandler.AllowAutoRedirect && ((int)result.StatusCode >= 300 && ((int)result.StatusCode <= 399)))
                        {
                            //记录3XX重定向URL到result.RedirectUrl
                            var redirectUrl = _ResponseMessage.Headers.Location;
                            if (redirectUrl != null)
                            {
                                result.RedirectUrl = redirectUrl.ToString();
                            }
                        }
                    }
                }
            }
            catch (TaskCanceledException ex)
            {
                var errMsg = $"请求取消：{ex.Message}";
                if (ex.InnerException != null)
                    errMsg += $" -> {ex.InnerException.Message}";
                return new Result
                {
                    StatusDescription = errMsg
                };
            }
            catch (Exception ex)
            {
                var errMsg = $"请求失败：{ex.Message}";
                if (ex.InnerException != null)
                    errMsg += $" -> {ex.InnerException.Message}";
                return new Result
                {
                    StatusDescription = errMsg
                };
            }
            return result;
        }

        public async Task<Result> RequestStringContent()
        {
            var result = await RequestAsync();
            if (result.IsSuccess)
            {
                await GetStringContent(result);
            }
            return result;
        }
        /// <summary>
        /// 发送请求下载文件到指定路径
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="Exception">文件已存在</exception>
        /// <exception cref="Exception">目录不存在</exception>
        public async Task<Result> RequestDownloadFile(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
                throw new Exception($"文件已存在：{fileInfo.FullName}");


            var result = await RequestAsync();

            if (result.IsSuccess)
            {
                //请求成功后再尝试创建目录
                var directory = fileInfo.Directory;
                if (!directory.Exists)
                {
                    if (_AutoCreateDirectory)
                        Directory.CreateDirectory(directory.FullName);
                    else
                        throw new Exception($"目录不存在：{directory.FullName}。如需自动创建目录请使用SetAutoCreateDirectory(true)配置");
                }
                //TODO 下载文件
                using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await _StreamContent.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                }

            }
            return result;
        }

        [Obsolete("未实现方法")]
        public async Task<Result> RequestUploadFile(string filePath)
        {
            //TODO 上传文件
            return null;
        }
        [Obsolete("未实现方法")]
        public async Task<Result> RequestUploadFile(string filePath, IProgress<double> progress)
        {
            //TODO 上传文件 带进度
            throw new NotImplementedException();
        }
        [Obsolete("未实现方法")]
        public async Task<Result> RequestUploadFile(byte[] bytes)
        {
            //TODO 上传文件
            throw new NotImplementedException();
        }
        [Obsolete("未实现方法")]
        public async Task<Result> RequestUploadFile(byte[] bytes, IProgress<double> progress)
        {
            //TODO 上传文件
            throw new NotImplementedException();
        }

        #endregion

        #region 创建请求消息体
        /// <summary>
        /// 初始化请求消息体
        /// </summary>
        private void CreateRequestMessage()
        {
            _RequestMessage = new HttpRequestMessage(new HttpMethod(this._Method.ToString()), _Uri);




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
        /// <summary>
        /// 创建请求内容
        /// </summary>
        private void CreateRequestContent()
        {
            this._RequestHttpContent = new Helper.ContentHelper(_RequestBodyContentType, _RequestContentParams, _Encoding).GetContent();
        }


        #endregion

        #region 创建请求HEADER

        /// <summary>
        /// 创建请求Header配置
        /// </summary>
        private void CreateRequestHeader()
        {
            _RequestMessage.Headers.Clear();
            if (!string.IsNullOrEmpty(_Referer))
                _RequestMessage.Headers.Add("Referer", _Referer);
            _RequestMessage.Headers.TryAddWithoutValidation("Accept", _Accept ?? "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8");
            if (!string.IsNullOrEmpty(_UserAgent))
                _RequestMessage.Headers.Add("User-Agent", _UserAgent);
            if (!string.IsNullOrEmpty(_Origin))
                _RequestMessage.Headers.Add("Origin", _Origin);
            _RequestMessage.Headers.Add("Connection", _KeepAlive ? "Keep-Alive" : "close");



            CreateHeaderCookie();
            CreateHeaderAccept();
            CreateHeaderAcceptCharset();
            CreateHeaderAcceptEncoding();
            CreateHeaderAcceptLanguage();
            CreateHeaderCacheControl();


            //将用户自定义的HEADERS集合添加到RequestMessage.Headers中
            //在设置完COOKIE/ACCEPT等头部后再添加用户自定义的Headers，用户自定义HEADER将覆盖之前值 
            CreateCustomHeader();


            //CONTENT TYPE
            //已在CreateRequestMessage中设置
            //if (!string.IsNullOrEmpty(_ContentType))
            //{
            //this._RequestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(_ContentType);
            //}
        }

        private void CreateHeaderCookie()
        {
            //COOKIE
            if (_CookieCollection != null && _CookieCollection.Count > 0)
            {
                //手动携带Cookie时 自动禁用HttpHandler的UseCookie
                if (_HttpHandler is HttpClientHandler httpClientHandler)
                {
                    httpClientHandler.UseCookies = false;
                }
                var cookieStr = Helper.CookieHelper.GetCookieStr(_CookieCollection, _Encoding);
                //var cookieEncodeStr = HttpUtility.UrlEncode(_Encoding.GetBytes(cookieStr));
                this._RequestMessage.Headers.Add("Cookie", cookieStr);



                //foreach (Cookie cookie in _CookieCollection)
                //{
                //    if (!string.IsNullOrEmpty(cookie.Name) && !string.IsNullOrEmpty(cookie.Value))
                //    {
                //        this._RequestMessage.Headers.Add("Cookie", $"{cookie.Name}={cookie.Value}");
                //    }
                //}
            }
        }

        private void CreateHeaderAccept()
        {
            //ACCEPT
            if (!string.IsNullOrEmpty(_Accept))
            {
                var acceptArr = _Accept.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (null != acceptArr && acceptArr.Length > 0)
                {
                    if (this._RequestMessage.Headers.Accept.Count > 0)
                        this._RequestMessage.Headers.Accept.Clear();
                    foreach (var item in acceptArr)
                    {
                        this._RequestMessage.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(item.Trim()));
                    }
                }
            }
        }

        private void CreateHeaderAcceptCharset()
        {
            //ACCEPT CHARSET
            if (!string.IsNullOrEmpty(_AcceptCharset))
            {
                var acceptCharsetArr = _AcceptCharset.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (null != acceptCharsetArr && acceptCharsetArr.Length > 0)
                {
                    if (this._RequestMessage.Headers.AcceptCharset.Count > 0)
                        this._RequestMessage.Headers.AcceptCharset.Clear();
                    foreach (var item in acceptCharsetArr)
                    {
                        if (item.Contains(';'))
                        {
                            //有quality参数
                            var tempArr = item.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            double quality = 1;
                            if (tempArr.Length > 0)
                            {
                                var value = tempArr[0].Trim();

                                if (tempArr[1].Contains('='))
                                {
                                    var tempArr2 = tempArr[1].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (tempArr2.Length > 1)
                                    {
                                        if (!double.TryParse(tempArr2[1].Trim(), out quality))
                                            quality = 1;
                                    }
                                }
                            }
                            this._RequestMessage.Headers.AcceptCharset.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue(tempArr[0].Trim(), quality));
                        }
                        else
                        {
                            this._RequestMessage.Headers.AcceptCharset.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue(item.Trim()));
                        }
                    }
                }
            }
        }

        private void CreateHeaderAcceptEncoding()
        {
            //ACCEPT ENCODING
            if (!string.IsNullOrEmpty(_AcceptEncoding))
            {
                var acceptEncodingArr = _AcceptEncoding.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (null != acceptEncodingArr && acceptEncodingArr.Length > 0)
                {
                    if (this._RequestMessage.Headers.AcceptEncoding.Count > 0)
                        this._RequestMessage.Headers.AcceptEncoding.Clear();
                    foreach (var item in acceptEncodingArr)
                    {
                        if (item.Contains(';'))
                        {
                            //有quality参数
                            double quality = 1;
                            var tempArr = item.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            if (tempArr.Length > 0)
                            {
                                var value = tempArr[0].Trim();

                                if (tempArr[1].Contains('='))
                                {
                                    var tempArr2 = tempArr[1].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (tempArr2.Length > 1)
                                    {
                                        if (!double.TryParse(tempArr2[1].Trim(), out quality))
                                            quality = 1;
                                    }
                                }
                            }
                            this._RequestMessage.Headers.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue(tempArr[0].Trim(), quality));
                        }
                        else
                        {
                            this._RequestMessage.Headers.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue(item.Trim()));
                        }
                    }
                }
            }
        }

        private void CreateHeaderAcceptLanguage()
        {
            //ACCEPT LANGUAGE
            if (!string.IsNullOrEmpty(_AcceptLanguage))
            {
                var acceptLanguageArr = _AcceptLanguage.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (null != acceptLanguageArr && acceptLanguageArr.Length > 0)
                {
                    if (this._RequestMessage.Headers.AcceptLanguage.Count > 0)
                        this._RequestMessage.Headers.AcceptLanguage.Clear();
                    foreach (var item in acceptLanguageArr)
                    {
                        if (item.Contains(';'))
                        {
                            //有quality参数
                            var tempArr = item.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            double quality = 1;
                            if (tempArr.Length > 0)
                            {
                                var value = tempArr[0].Trim();

                                if (tempArr[1].Contains('='))
                                {
                                    var tempArr2 = tempArr[1].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (tempArr2.Length > 1)
                                    {
                                        if (!double.TryParse(tempArr2[1].Trim(), out quality))
                                            quality = 1;
                                    }
                                }
                            }
                            this._RequestMessage.Headers.AcceptLanguage.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue(tempArr[0].Trim(), quality));
                        }
                        else
                        {
                            this._RequestMessage.Headers.AcceptLanguage.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue(item.Trim()));
                        }
                    }
                }
            }
        }

        private void CreateHeaderCacheControl()
        {
            if (string.IsNullOrEmpty(_CacheControl))
                return;
            var arr = _CacheControl.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (arr == null || arr.Length == 0)
                return;
            this._RequestMessage.Headers.Add("Cache-Control", arr.Select(a => a.Trim()));
        }

        private void CreateCustomHeader()
        {
            if (_CustomRequestHeaders == null || _CustomRequestHeaders.Count == 0)
                return;
            foreach (var item in _CustomRequestHeaders)
            {
                if (item.HasAdded)
                    continue;

                var headerName = item.Name;
                var headerValue = item.Value ?? string.Empty;
                var isUniqueHeader = Common.UniqueHeaderNames.Any(n => n.Equals(headerName, StringComparison.OrdinalIgnoreCase));
                if (isUniqueHeader)
                {
                    if (_RequestMessage.Headers.Contains(item.Name))
                        _RequestMessage.Headers.Remove(item.Name);
                }
                if (item.Value is IEnumerable<string> values)
                {
                    if (isUniqueHeader)
                    {
                        _RequestMessage.Headers.Add(item.Name, string.Join(",", values));
                    }
                    else
                    {
                        foreach (var value in values)
                        {
                            _RequestMessage.Headers.Add(item.Name, value);
                        }
                    }
                }
                else
                {
                    _RequestMessage.Headers.Add(item.Name, item.Value.ToString());
                }

                item.HasAdded = true;
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

            await GetStreamContent(result);
            //await GetTextContent(result);
            //using (var stream = await _ResponseMessage.Content.ReadAsStreamAsync())
            //{
            //    if (this._StreamContent == null)
            //    {
            //        this._StreamContent = new System.IO.MemoryStream();
            //    }
            //    await stream.CopyToAsync(this._StreamContent);
            //    await this._StreamContent.FlushAsync();
            //}
        }

        private async Task GetStringContent(Result result)
        {
            if (this._StreamContent == null && !this._StreamContent.CanRead)
                return;
            var encoding = _Encoding;
            if (this._ResponseMessage.Content.Headers.ContentType != null)
            {
                if (this._ResponseMessage.Content.Headers.ContentType.CharSet != null)
                    encoding = Encoding.GetEncoding(this._ResponseMessage.Content.Headers.ContentType.CharSet);
            }
            using (var reader = new System.IO.StreamReader(this._StreamContent, _Encoding, this._DetectEncodingFromByteOrderMarks))
            {
                result.TextContent = await reader.ReadToEndAsync();
                this._StreamContent.Position = 0;
            }
        }

        private async Task GetStreamContent(Result result)
        {
            using (var stream = await _ResponseMessage.Content.ReadAsStreamAsync())
            {
                // var stream = await _ResponseMessage.Content.ReadAsStreamAsync();
                if (this._StreamContent == null)
                {
                    this._StreamContent = new System.IO.MemoryStream();
                }
                else
                {
                    this._StreamContent.SetLength(0);
                }
                //var tempStream = new MemoryStream();
                //await stream.CopyToAsync(tempStream);
                //tempStream.Position = 0;
                //this._StreamContent = tempStream;
                await stream.CopyToAsync(this._StreamContent);
                //await this._StreamContent.FlushAsync();
                this._StreamContent.Position = 0;
            }
        }
        #endregion




    }
}
