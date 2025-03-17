using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace X.Helper.Http
{
    /// <summary>
    /// 保存请求结果
    /// </summary>
    public class Result
    {
        public CookieCollection CookieCollection { get; set; }

        public WebHeaderCollection HeaderCollection { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public string StatusDescription { get; set; }


        /// <summary>
        /// 返回正文
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 返回正文
        /// </summary>
        public string Html { get { return Content; } }
        /// <summary>
        /// 返回二进制字节数组
        /// </summary>
        public byte[] Bytes { get; set; }

        public string ResponseUri { get;  set; }

        public string RedirectUrl { get; set; }

        //public string RedirectUrl
        //{
        //    get
        //    {
        //        string result;
        //        try
        //        {
        //            if (this.HeaderCollection != null && this.HeaderCollection.Count > 0)
        //            {
        //                if (this.HeaderCollection.AllKeys.Any((string k) => k.ToLower().Contains("location")))
        //                {
        //                    string text = this.HeaderCollection["location"].ToString().Trim();
        //                    string text2 = text.ToLower();
        //                    if (!string.IsNullOrWhiteSpace(text2))
        //                    {
        //                        if (!text2.StartsWith("http://") && !text2.StartsWith("https://"))
        //                        {
        //                            text = new Uri(new Uri(this.ResponseUri), text).AbsoluteUri;
        //                        }
        //                    }
        //                    result = text;
        //                    return result;
        //                }
        //            }
        //        }
        //        catch
        //        {
        //        }
        //        result = string.Empty;
        //        return result;
        //    }
        //}


    }
}
