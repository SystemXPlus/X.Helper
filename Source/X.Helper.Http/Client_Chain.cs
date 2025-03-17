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
        public Client SetCookie(string name, string value)
        {
            //TODO
            return this;
        }
        public Client SetCookie(CookieCollection cookies)
        {
            //TODO 将COOKIE添加到COOKIECONTAINER
            return this;
        }

        public Client SetCookie(Dictionary<string, string> cookies)
        {
            //TODO
            return this;
        }
        public Client AddCookie(string name, string value)
        {
            //TODO 
            return this;
        }

        public Client AddCookie(CookieCollection cookies)
        {
            //TODO 将COOKIE添加到COOKIECONTAINER
            return this;
        }

        public Client AddCookie(Dictionary<string, string> cookies)
        {
            //TODO
            return this;
        }
        #endregion

        #region HEADER
        public Client AddHeader(string name, string value)
        {
            //TODO
            return this;
        }
        public Client AddHeader(Dictionary<string, string> headers)
        {
            //TODO
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

        public Client SetContentType(string contentType)
        {
            //TODO
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
            TimeOut = TimeSpan.FromSeconds(second);
            return this;
        }

    }
}
