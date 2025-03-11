using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace X.Helper.Http
{
    public class HttpHandler
    {
        private readonly HttpClientHandler _HttpHandler;

        private bool _UserCookieContainer = false;

        public HttpHandler() : this(null)
        {

        }

        public HttpHandler(HttpClientHandler handler)
        {
            if (handler == null)
            {
                _HttpHandler = new HttpClientHandler();
            }
            else
            {
                _HttpHandler = handler;
            }
        }
        public HttpHandler UseProxy(WebProxy proxy)
        {
            if (proxy != null)
            {

                this._HttpHandler.UseProxy = true;
                this._HttpHandler.Proxy = proxy;
            }
            return this;
        }
        /// <summary>
        /// 是否自动处理Set-Cookie头
        /// </summary>
        /// <param name="useCookies"></param>
        /// <returns></returns>
        public HttpHandler UseCookies(bool useCookies)
        {
            this._HttpHandler.UseCookies = useCookies;
            return this;
        }
        public HttpHandler SetCookieContainer(bool useCookieContainer)
        {
            _UserCookieContainer = useCookieContainer;
            if (_UserCookieContainer)
            {
                this._HttpHandler.CookieContainer = new CookieContainer();
            }
            return this;
        }


        /// <summary>
        /// 是否自动跟随3xx重定向（默认true）
        /// </summary>
        /// <param name="allowAutoRedirect"></param>
        /// <returns></returns>
        public HttpHandler AllowAutoRedirect(bool allowAutoRedirect)
        {
            this._HttpHandler.AllowAutoRedirect = allowAutoRedirect;
            return this;
        }
        /// <summary>
        /// 设置最大重定向次数限制
        /// </summary>
        /// <param name="maxAutomaticRedirections"></param>
        /// <returns></returns>
        public HttpHandler SetMaxAutomaticRedirections(int maxAutomaticRedirections)
        {
            this._HttpHandler.MaxAutomaticRedirections = maxAutomaticRedirections;
            return this;
        }
        /// <summary>
        /// 设置连接存活时间
        /// <para>HttpClientHandler不适用，SocketsHttpHandler适用</para>
        /// </summary>
        /// <param name="maxConnections"></param>
        /// <returns></returns>
        //public Handler PooledConnectionLifetime(TimeSpan lifetime)
        //{
        //    this._HttpHandler.PooledConnectionLifetime = lifetime;
        //    return this;
        //}

        public HttpHandler SetClientCertificateOptions(ClientCertificateOption clientCertificateOptions)
        {
            this._HttpHandler.ClientCertificateOptions = clientCertificateOptions;
            return this;
        }
#if NETSTANDARD2_0_OR_GREATER || NET6_0_OR_GREATER
        /// <summary>
        /// 设置每台主机的最大并发连接数
        /// </summary>
        public HttpHandler SetMaxConnectionsPerServer(int maxConnections)
        {
            this._HttpHandler.MaxConnectionsPerServer = maxConnections;
            return this;
        }
        /// <summary>
        /// 自定义服务器证书验证，设置服务器证书验证回调 
        /// <para>如需忽略证书验证可将回调函数设置为永远返回true</para>
        /// <para> (message, cert, chain, errors) => true</para>
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public HttpHandler SetServerCertificateCustomValidationCallback(Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> callback)
        {
            //如需忽略证书验证可将回调函数设置为永远返回true (message, cert, chain, errors) => true
            this._HttpHandler.ServerCertificateCustomValidationCallback = callback;
            return this;
        }

        /// <summary>
        /// 设置客户端证书
        /// </summary>
        public HttpHandler SetClientCertificates(X509Certificate2 certificates)
        {
            this._HttpHandler.ClientCertificates.Add(certificates);
            return this;
        }
#endif

        #region SSL/TLS验证
        #endregion
        #region 认证
        public HttpHandler SetPreAuthenticate(bool preAuthenticate)
        {
            this._HttpHandler.PreAuthenticate = preAuthenticate;
            return this;
        }

        public HttpHandler SetCredentials(ICredentials credentials)
        {
            this._HttpHandler.Credentials = credentials;
            return this;
        }
        #endregion
    }
}
