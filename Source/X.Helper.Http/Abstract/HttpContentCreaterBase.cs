using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using X.Helper.Http.Entity;

namespace X.Helper.Http.Abstract
{
    public abstract class HttpContentCreaterBase : Interface.IHttpContentCreater
    {
        /// <summary>
        /// HttpContentTypeName
        /// </summary>
        protected string ContentMediaType{ get; set; }
        /// <summary>
        /// HttpContentEncoding
        /// </summary>
        protected Encoding ContentEncoding { get; set; } = Encoding.UTF8;

        public abstract HttpContent Create(List<HttpContentParam> contentParams, Encoding encoding);
    }
}
