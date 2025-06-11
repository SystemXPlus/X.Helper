using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using X.Helper.Http.Entity;

namespace X.Helper.Http.Helper.HttpContentCreator
{
    public class TextPlainHttpContentCreator : Abstract.HttpContentCreatorBase
    {
        public TextPlainHttpContentCreator()
        {
            ContentMediaType = "text/plain";
        }
        public override HttpContent Create(IEnumerable<HttpContentParam> contentParams, Encoding encoding)
        {
#if NET6_0_OR_GREATER
            encoding ??= ContentEncoding;
#else
            if (encoding == null)
            {
                encoding = ContentEncoding;
            }
#endif
            var sb = new StringBuilder();
            foreach (var item in contentParams)
            {
                if (item != null && item.Value != null)
                {
                    if(!sb.Length.Equals(0))
                    {
                        sb.Append(Environment.NewLine);
                    }
                    sb.Append(item.Value.ToString());
                }
            }
            var content = new StringContent(sb.ToString(), ContentEncoding, ContentMediaType);
            return content;
        }
    }
}
