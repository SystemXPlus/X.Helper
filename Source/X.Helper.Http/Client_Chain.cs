using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;

namespace X.Helper.Http
{
    public partial class Client
    {
        public Client SetMethod(Enums.HttpMethod method)
        {
            this.Method = method;
            return this;
        }

        public Client SetUri(Uri uri)
        {
            this.Uri = uri;
            return this;
        }

        public Client SetUri(string uri)
        {
            this.Uri = new Uri(uri);
            return this;
        }

        public Client SetKeepAlive(bool keepAlive)
        {
            this.KeepAlive = keepAlive;
            return this;
        }

        public Client SetIPEndPoint(IPEndPoint ipEndPoint)
        {
            this.IPEndPoint = ipEndPoint;
            return this;
        }

        #region COOKIE
        public Client ClearCookies()
        {
#if NET6_0_OR_GREATER
                this.CookieCollection.Clear();
#else
            this.CookieCollection = new CookieCollection();
#endif
            return this;
        }
        public Client SetCookie(string name, string value)
        {
            if(!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                this.CookieCollection.Add(new Cookie(name, value));
            return this;
        }
        public Client SetCookie(CookieCollection cookies)
        {
            if(cookies != null)
            {
                foreach (Cookie cookie in cookies)
                {
                    this.CookieCollection.Add(cookie);
                }
            }
            return this;
        }

        public Client SetCookie(Dictionary<string, string> cookies)
        {
            if(cookies != null)
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
            if(!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
            {
                if(this.RequestMessage.Headers.Contains(name))
                    this.RequestMessage.Headers.Remove(name);
                this.RequestMessage.Headers.Add(name, value);
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
            if(headers != null)
            {
                foreach (var item in headers)
                {
                    if (!string.IsNullOrEmpty(item.Key) && !string.IsNullOrEmpty(item.Value))
                    {
                        if (this.RequestMessage.Headers.Contains(item.Key))
                            this.RequestMessage.Headers.Remove(item.Key);
                        this.RequestMessage.Headers.Add(item.Key, item.Value);
                    }
                }
            }
            return this;
        }

        public Client SetReferer(Uri uri)
        {
            this.Referer = uri.ToString();
            return this;
        }
        public Client SetReferer(string referer)
        {
            this.Referer = referer;
            return this;
        }
        public Client SetOrigin(Uri uri)
        {
            this.Origin = uri.ToString();
            return this;
        }
        public Client SetOrigin(string origin)
        {
            this.Origin = origin;
            return this;
        }
        public Client SetUserAgent(string userAgent)
        {
            this.UserAgent = userAgent;
            return this;
        }
        /// <summary>
        /// 设置ContentType请求头
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public Client SetContentType(string contentType)
        {
            this.RequestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
            return this;
        }
        /// <summary>
        /// 设置Accept请求头
        /// </summary>
        /// <param name="accept"></param>
        /// <returns></returns>
        public Client SetAccept(string accept)
        {
            if(string.IsNullOrEmpty(accept))
                return this;
            var acceptArr = accept.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if(null == acceptArr || acceptArr.Length == 0)
                return this;
            if (this.RequestMessage.Headers.Accept.Count > 0)
                this.RequestMessage.Headers.Accept.Clear();
            foreach (var item in acceptArr)
            {
                this.RequestMessage.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(accept));
            }
            return this;
        }

        public Client SetEncoding(Encoding encoding)
        {
            this.Encoding = encoding;
            return this;
        }

        public Client SetEncoding(string encoding)
        {
            this.Encoding = Encoding.GetEncoding(encoding);
            return this;
        }
        public Client SetEncoding(int codepage)
        {
            this.Encoding = Encoding.GetEncoding(codepage);
            return this;
        }
        #endregion

        public Client SetTimeout(int second)
        {
            Timeout = TimeSpan.FromSeconds(second);
            return this;
        }

        public Client SetTimeout(TimeSpan timeSpan)
        {
            Timeout = timeSpan;
            return this;
        }

    }
}
