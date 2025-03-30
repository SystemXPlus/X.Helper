using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace X.Helper.Http
{
    /// <summary>
    /// 保存请求结果
    /// </summary>
    public class Result : IDisposable
    {
        private bool disposedValue;

        public CookieCollection CookieCollection { get; set; }

        public WebHeaderCollection HeaderCollection { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public string StatusDescription { get; set; }


        /// <summary>
        /// 返回文本内容
        /// </summary>
        internal string TextContent { get; set; }

        /// <summary>
        /// 返回正文
        /// </summary>
        public string Content
        {
            get
            {
                return TextContent;
            }        
         }
        /// <summary>
        /// 返回二进制字节数组
        /// </summary>
        public byte[] Bytes { get; set; }

        public string ResponseUri { get;  set; }

        public string RedirectUrl { get; set; }


        /// <summary>
        /// 请求是否成功
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return this.StatusCode >= HttpStatusCode.OK && this.StatusCode < HttpStatusCode.Ambiguous;
            }
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        ~Result()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

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
