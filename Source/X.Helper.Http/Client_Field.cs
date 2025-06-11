using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using X.Helper.Http.Entity;

namespace X.Helper.Http
{
    public partial class Client
    {
        #region BASIC

        private readonly HttpClient _HttpClient;

        private readonly HttpMessageHandler _HttpHandler;


        public Uri _Uri { get; private set; }
        private Enums.HttpMethod _Method { get; set; } = Enums.HttpMethod.GET;
        private string _Referer { get; set; }
        private string _Origin { get; set; }
        /// <summary>
        /// 请求的UserAgent
        /// </summary>
        private string _UserAgent { get; set; } = "X.Helper.Http.Client";
        /// <summary>
        /// 请求的IP地址
        /// </summary>
        private IPEndPoint _IPEndPoint { get; set; } = null;

        /**
 * 在HTTP/1.0中，Keep-Alive功能是默认关闭的，需要在请求头中添加Connection: Keep-Alive来启用。
 * 而在HTTP/1.1中，Keep-Alive功能默认启用，如果需要关闭，则需要在请求头中添加Connection: close‌
 * */
        /// <summary>
        /// KeepAlive
        /// <para>在HTTP/1.0中，Keep-Alive功能是默认关闭的，需要在请求头中添加Connection: Keep-Alive来启用。</para>
        /// <para>而在HTTP/1.1中，Keep-Alive功能默认启用，如果需要关闭，则需要在请求头中添加Connection: close‌</para>
        /// </summary>
        private bool _KeepAlive { get; set; } = true;

        #endregion

        #region REQUEST
        ///// <summary>
        ///// 请求的正文内容文本
        ///// </summary>
        //private string _ContentString { get; set; } = string.Empty;
        ///// <summary>
        ///// 请求的正文内容对象
        ///// </summary>
        //private object _ContentObject { get; set; }
        ///// <summary>
        ///// 请求的正文内容字典
        ///// </summary>
        //private Dictionary<string, string> _ContentDictionary { get; set; } 
        /// <summary>
        /// 请求的正文内容参数列表
        /// </summary>
        private List<HttpContentParam> _RequestContentParams { get; set; } = new List<HttpContentParam>();
        private HttpContent _RequestHttpContent { get; set; } = null;

        private Enums.HttpContentType _RequestBodyContentType { get; set; } = Enums.HttpContentType.RAW_JSON;
        /// <summary>
        /// 自定义请求头列表
        /// </summary>
        private List<Entity.CustomRequestHeader> _CustomRequestHeaders { get; set; } = new List<Entity.CustomRequestHeader>();
        /// <summary>
        /// 请求的ContentType
        /// </summary>
        [Obsolete("弃用，请使用_RequestBodyContentType", true)]
        private MediaTypeHeaderValue _ContentType { get; set; } = new MediaTypeHeaderValue("text/plain") { CharSet = "utf-8" };
        /// <summary>
        /// 请求编码格式
        /// </summary>
        private Encoding _Encoding { get; set; } = Encoding.UTF8;
        /// <summary>
        /// 请求的Accept
        /// </summary>
        private string _Accept { get; set; } = "*/*";
        /// <summary>
        /// 请求的Accept-Charset
        /// </summary>
        private string _AcceptCharset { get; set; } = "utf-8,gbk,gb2312,iso-8859-1; q=0.9,*; q=0.1";
        /// <summary>
        /// 请求的Accept-Encoding
        /// </summary>
        private string _AcceptEncoding { get; set; } = "gzip, deflate, br";
        /// <summary>
        /// 请求的Accept-Language
        /// </summary>
        private string _AcceptLanguage { get; set; } = "zh-CN,zh;q=0.9,ja;q=0.8,zh-TW;q=0.7,en;q=0.6";
        /// <summary>
        /// 请求的Cache-Control
        /// </summary>
        private string _CacheControl { get; set; } = "";

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



        private Version _Version { get; set; } = HttpVersion.Version11;

        private TimeSpan _Timeout = TimeSpan.FromSeconds(100.0);

        private HttpRequestMessage _RequestMessage { get; set; }

        #endregion

        #region RESPONSE


        private HttpResponseMessage _ResponseMessage { get; set; }


        private MemoryStream _StreamContent;

        private bool _DetectEncodingFromByteOrderMarks = true;

        #endregion

        #region FILE
        /// <summary>
        /// 自定义的MultipartFormData边界符
        /// </summary>
        private string _MultipartFormDataBoundary { get; set; } = null;
        /// <summary>
        /// 待上传文件列表
        /// </summary>
        private List<KeyValuePair<string, string>> _Files { get; set; } = new List<KeyValuePair<string, string>>();
        /// <summary>
        /// 下载文件时是否自动创建目录
        /// </summary>
        private bool _AutoCreateDirectory { get; set; } = false;

        #endregion





    }
}
