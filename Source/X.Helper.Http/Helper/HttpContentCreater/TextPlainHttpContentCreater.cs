using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using X.Helper.Http.Entity;

namespace X.Helper.Http.Helper.HttpContentCreater
{
    public class TextPlainHttpContentCreater : Abstract.HttpContentCreaterBase
    {
        public TextPlainHttpContentCreater()
        {
            ContentMediaType = "text/plain";
        }
        public override HttpContent Create(List<HttpContentParam> contentParams, Encoding encoding)
        {
            var sb = new StringBuilder();
            foreach (var item in contentParams)
            {
                sb.AppendLine(item.Value.ToString());
            }
            var content = new StringContent(sb.ToString(), encoding, ContentMediaType);
            return content;
        }
    }
}
