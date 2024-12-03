using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <summary>
        /// HTTP RESPONSE HEADERS DICTIONARY
        /// </summary>
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        /// <summary>
        /// HTTP RESPONSE COOKIES DICTIONARY
        /// </summary>
        public Dictionary<string, string> Cookies { get; set; } = new Dictionary<string, string>();
    }
}
