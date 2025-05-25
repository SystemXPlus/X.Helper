using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;

namespace X.Helper.Http
{
    public partial class Client
    {
        public Client SetMethod(Enums.HttpMethod method)
        {
            this._Method = method;
            return this;
        }

        public Client SetUri(Uri uri)
        {
            this._Uri = uri;
            return this;
        }

        public Client SetUri(string uri)
        {
            this._Uri = new Uri(uri);
            return this;
        }

        public Client SetKeepAlive(bool keepAlive)
        {
            this._KeepAlive = keepAlive;
            return this;
        }

        public Client SetIPEndPoint(IPEndPoint ipEndPoint)
        {
            this._IPEndPoint = ipEndPoint;
            return this;
        }

        #region COOKIE
        /// <summary>
        /// 清除Cookie
        /// </summary>
        /// <returns></returns>
        public Client ClearCookies()
        {
#if NET6_0_OR_GREATER
                this.CookieCollection.Clear();
#else
            this.CookieCollection = new CookieCollection();
#endif
            return this;
        }
        public Client SetCookie(string cookieString)
        {
            if (string.IsNullOrEmpty(cookieString))
                return this;
            var cookieCollection = Helper.CookieHelper.GetCookieCollection(cookieString);
            this.CookieCollection.Add(cookieCollection);
            return this;
        }
        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Client SetCookie(string name, string value)
        {
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                this.CookieCollection.Add(new Cookie(name, value));
            return this;
        }
        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public Client SetCookie(CookieCollection cookies)
        {
            if (cookies != null)
            {
                foreach (Cookie cookie in cookies)
                {
                    this.CookieCollection.Add(cookie);
                }
            }
            return this;
        }
        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public Client SetCookie(Dictionary<string, string> cookies)
        {
            if (cookies != null)
            {
                foreach (var item in cookies)
                {
                    this.CookieCollection.Add(new Cookie(item.Key, item.Value));
                }
            }
            return this;
        }

        #endregion

        #region HEADER
        /// <summary>
        /// 设置全局请求头
        /// <para>在当前实例的所有请求中均会携带该请求头</para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Client SetDefaultRequestHeaders(string name, string value)
        {
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
            {
                if (this._HttpClient.DefaultRequestHeaders.Contains(name))
                    this._HttpClient.DefaultRequestHeaders.Remove(name);
                this._HttpClient.DefaultRequestHeaders.Add(name, value);
            }
            return this;
        }
        /*
         * 
         * //TODO
         * 因为RequestMessage是在发起请求时才创建的，
         * 所以在设置请求头时要创建一个私有对象来存储请求头，
         * 在创建RequestMessage时再将请求头添加到RequestMessage中
         * 
         * */
        /// <summary>
        /// 设置请求头
        /// <para>如果请求头已存在则覆盖</para>
        /// <para>Referer、User-Agent、Origin、Connection请使用对应Set方法配置</para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Client SetHeader(string name, string value)
        {
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
            {
                if (this._RequestMessage.Headers.Contains(name))
                    this._RequestMessage.Headers.Remove(name);
                this._RequestMessage.Headers.Add(name, value);
            }
            return this;
        }
        /// <summary>
        /// 设置请求头
        /// <para>如果请求头已存在则覆盖</para>
        /// <para>Referer、User-Agent、Origin、Connection请使用对应Set方法配置</para>
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public Client SetHeader(Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (var item in headers)
                {
                    if (!string.IsNullOrEmpty(item.Key) && !string.IsNullOrEmpty(item.Value))
                    {
                        if (this._RequestMessage.Headers.Contains(item.Key))
                            this._RequestMessage.Headers.Remove(item.Key);
                        this._RequestMessage.Headers.Add(item.Key, item.Value);
                    }
                }
            }
            return this;
        }
        /// <summary>
        /// Set Referer Header
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public Client SetReferer(Uri uri)
        {
            this._Referer = uri.ToString();
            return this;
        }
        /// <summary>
        /// Set Referer Header
        /// </summary>
        /// <param name="referer"></param>
        /// <returns></returns>
        public Client SetReferer(string referer)
        {
            this._Referer = referer;
            return this;
        }
        /// <summary>
        /// Set Origin Header
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public Client SetOrigin(Uri uri)
        {
            this._Origin = uri.ToString();
            return this;
        }
        /// <summary>
        /// Set Origin Header
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        public Client SetOrigin(string origin)
        {
            this._Origin = origin;
            return this;
        }
        /// <summary>
        /// Set User-Agent
        /// </summary>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        public Client SetUserAgent(string userAgent)
        {
            this._UserAgent = userAgent;
            return this;
        }
        public Client SetContentType(string mediaType)
        {
            if (!string.IsNullOrEmpty(mediaType))
            {
                SetContentType(mediaType, _Encoding.WebName);
            }
            return this;
        }
        /// <summary>
        /// 设置请求ContentType请求头
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public Client SetContentType(string mediaType, string charset)
        {
            if (!string.IsNullOrEmpty(mediaType))
            {
                var contentType = new MediaTypeHeaderValue(mediaType);
                if (!string.IsNullOrEmpty(charset))
                {
                    this._Encoding = Encoding.GetEncoding(charset);
                }
                contentType.CharSet = _Encoding.WebName;
            }
            return this;
        }

        /// <summary>
        /// 设置请求ContentType请求头
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public Client SetContentType(MediaTypeHeaderValue contentType)
        {
            if (contentType != null)
            {
                this._ContentType = contentType;
                if (!string.IsNullOrEmpty(contentType.CharSet))
                {
                    this._Encoding = Encoding.GetEncoding(contentType.CharSet);
                }
            }
            return this;
        }
        /// <summary>
        /// 设置Accept请求头
        /// </summary>
        /// <param name="accept"></param>
        /// <returns></returns>
        public Client SetAccept(string accept)
        {
            if (string.IsNullOrEmpty(accept))
                return this;
            this._Accept = accept;
            return this;
        }
        /// <summary>
        /// 设置Accept-Charset请求头
        /// </summary>
        /// <param name="acceptCharset"></param>
        /// <returns></returns>
        public Client SetAcceptCharset(string acceptCharset)
        {
            if (string.IsNullOrEmpty(acceptCharset))
                return this;
            this._AcceptCharset = acceptCharset;
            return this;
        }
        /// <summary>
        /// 设置Accept-Encoding请求头
        /// </summary>
        /// <param name="acceptEncoding"></param>
        /// <returns></returns>
        public Client SetAcceptEncoding(string acceptEncoding)
        {
            if (!string.IsNullOrEmpty(acceptEncoding))
                this._AcceptEncoding = acceptEncoding;
            return this;
        }
        /// <summary>
        /// 设置Accept-Language请求头
        /// </summary>
        /// <param name="acceptLanguage"></param>
        /// <returns></returns>
        public Client SetAcceptLanguage(string acceptLanguage)
        {
            if (!string.IsNullOrEmpty(acceptLanguage))
                this._AcceptLanguage = acceptLanguage;
            return this;
        }

        /// <summary>
        /// 设置Cache-Control请求头
        /// </summary>
        /// <param name="cacheControl"></param>
        /// <returns></returns>
        public Client SetCacheControl(string cacheControl)
        {
            if (!string.IsNullOrEmpty(cacheControl))
                _CacheControl = cacheControl;
            return this;
        }

        /// <summary>
        /// 设置编码
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public Client SetEncoding(Encoding encoding)
        {
            this._Encoding = encoding;
            this._ContentType.CharSet = this._Encoding.WebName;
            return this;
        }
        /// <summary>
        /// 设置编码
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public Client SetEncoding(string encoding)
        {
            this._Encoding = Encoding.GetEncoding(encoding);
            this._ContentType.CharSet = this._Encoding.WebName;
            return this;
        }
        /// <summary>
        /// 设置编码
        /// </summary>
        /// <param name="codepage"></param>
        /// <returns></returns>
        public Client SetEncoding(int codepage)
        {
            this._Encoding = Encoding.GetEncoding(codepage);
            this._ContentType.CharSet = this._Encoding.WebName;
            return this;
        }
        #endregion

        /// <summary>
        /// 设置超时时间
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        public Client SetTimeout(int second)
        {
            this._Timeout = TimeSpan.FromSeconds(second);
            return this;
        }
        /// <summary>
        /// 设置超时时间
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public Client SetTimeout(TimeSpan timeSpan)
        {
            this._Timeout = timeSpan;
            return this;
        }
        /// <summary>
        /// 是否自动检测文件/文本的字节顺序标记（Byte Order Mark，简称BOM），从而确定文件/文本的编码方式。
        /// </summary>
        /// <param name="detectEncodingFromByteOrderMarks"></param>
        /// <returns></returns>
        public Client SetDetectEncodingFromByteOrderMarks(bool detectEncodingFromByteOrderMarks)
        {
            this._DetectEncodingFromByteOrderMarks = detectEncodingFromByteOrderMarks;
            return this;
        }


        #region 请求内容

        public Client SetContent(string content)
        {
            if(null == content)
                throw new ArgumentException("content is null");
            this._ContentString = content;
            this._RequestContentType = Enums.RequestContentType.STRING;
            return this;
        }

        public Client SetContent(Dictionary<string,string> content)
        {
            if(content == null) throw new ArgumentException("content is null");
            this._ContentDictionary = content;
            this._RequestContentType = Enums.RequestContentType.DICTIONARY;
            return this;
        }

        public Client SetContent<T>(T content) where T : class, new()
        {
            if(content == null)
                throw new ArgumentException("content is null");
            this._ContentObject = content;
            this._RequestContentType = Enums.RequestContentType.OBJECT;
            return this;
        }

        #endregion

        #region 文件
        /// <summary>
        /// 下载文件时目录不存在是否自动创建目录
        /// </summary>
        /// <param name="autoCreateDirectory"></param>
        /// <returns></returns>
        public Client SetAutoCreateDirectory(bool autoCreateDirectory)
        {
            this._AutoCreateDirectory = autoCreateDirectory;
            return this;
        }
        /// <summary>
        /// 设置多部分表单数据（MultipartFormData）的边界符
        /// </summary>
        /// <param name="boundary"></param>
        /// <returns></returns>
        public Client SetMultipartFormDataBoundary(string boundary)
        {
            if (!string.IsNullOrEmpty(boundary))
            {
                this._MultipartFormDataBoundary = boundary;
            }
            return this;
        }

        /// <summary>
        /// 添加文件到上传文件列表
        /// </summary>
        /// <param name="filePath">文件完整路径</param>
        /// <returns></returns>
        public Client AddFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException(@"filePath is null or empty");
            if (!filePath.Contains("\\"))
                throw new ArgumentException(filePath + " is not a valid file path");

            var name = filePath.Substring(filePath.IndexOf("\\") + 1);
            this._Files.Add(new KeyValuePair<string, string>(name, filePath));

            return this;
        }
        /// <summary>
        /// 添加文件到上传文件列表
        /// </summary>
        /// <param name="name">提交上传文件使用的名称</param>
        /// <param name="fileInfo">文件完整路径</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Client AddFile(string name, string filePath)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(name + " is null or empty");
            }
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException(filePath + " is null or empty");
            }
            if (!filePath.Contains("\\"))
            {
                throw new ArgumentException(filePath + " is not a valid file path");
            }

            this._Files.Add(new KeyValuePair<string, string>(name, filePath));

            return this;
        }

        #endregion
    }
}
