using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace X.Helper.Http.Entity
{
    /// <summary>
    /// HTTP RESPONSE MESSAGE
    /// </summary>
    public class HttpResponse
    {
        /// <summary>
        /// IS THE HTTP REQUEST SUCCESSFULL
        /// </summary>
        public bool Success
        {
            get
            {
                return StatusCode >= 200 && StatusCode < 400;
            }
        }
        /// <summary>
        /// HTTP STATUS CODE
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// HTTP STATUS DESCRIPTION
        /// </summary>
        public string StatusDescription { get; set; }
        /// <summary>
        /// HTTP RESPONSE CONTENT
        /// </summary>
        public string Content { get; set; }
        public string Cookie {  get; set; }
        /// <summary>
        /// HTTP RESPONSE COOKIES DICTIONARY
        /// </summary>
        public Dictionary<string, string> CookiesDictionary { get; set; } = new Dictionary<string, string>();
        /// <summary>
        /// COOKIE COLLECTION
        /// </summary>
        public CookieCollection CookieCollection { get; set; }
        /// <summary>
        /// HTTP RESPONSE HEADERS DICTIONARY
        /// </summary>
        public Dictionary<string, string> HeadersDictionary { get; set; } = new Dictionary<string, string>();
        /// <summary>
        /// HEADERS CONNECTION
        /// </summary>
        public WebHeaderCollection WebHeadersCollection { get; set; }
    }
}
