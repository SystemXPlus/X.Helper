using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using X.Helper.Http.Entity;
using X.Helper.Http.Enums;
using X.Helper.Http.Interface;

namespace X.Helper.Http.Helper
{
    public class ContentHelper
    {
        private HttpContentType HttpContentType { get; set; }
        private List<HttpContentParam> HttpContentParams { get; set; }
        private Encoding Encoding { get; set; }
        public ContentHelper(HttpContentType contentType, List<HttpContentParam> contentParam, Encoding encoding)
        {
            this.HttpContentType = contentType;
            this.HttpContentParams = contentParam;
            this.Encoding = encoding ?? Encoding.UTF8;
        }
        public HttpContent GetContent()
        {
            IHttpContentCreater contentCreater = null;
            switch (this.HttpContentType)
            {
                case HttpContentType.RAW_TEXT:
                    contentCreater = new HttpContentCreater.TextPlainHttpContentCreater();
                    break;
                case HttpContentType.RAW_JSON:
                    contentCreater = new HttpContentCreater.ApplicationJsonHttpContentCreater();
                    break;
                case HttpContentType.RAW_XML:
                    contentCreater = new HttpContentCreater.ApplicationXmlHttpContentCreater();
                    break;
                case HttpContentType.RAW_HTML:
                    contentCreater = new HttpContentCreater.TextHtmlHttpContentCreater();
                    break;
                case HttpContentType.RAW_JAVASCRIPT:
                    contentCreater = new HttpContentCreater.ApplicationJavaScriptHttpContentCreater();
                    break;
                case HttpContentType.X_WWW_FORM_URLENCODED:
                    contentCreater = new HttpContentCreater.ApplicationFormUrlEncodedHttpContentCreater();
                    break;
                case HttpContentType.MULTIPART_FORM_DATA:
                    contentCreater = new HttpContentCreater.MultipartFormDataHttpContentCreater();
                    break;
                case HttpContentType.BINARY:
                    contentCreater = new HttpContentCreater.ApplicationOctetStreamHttpContentCreater();
                    break;
                default:
                    throw new NotSupportedException($"不支持的HTTP内容类型: {this.HttpContentType}");
            }
            return contentCreater.Create(this.HttpContentParams,this.Encoding);
        }
    }
}
