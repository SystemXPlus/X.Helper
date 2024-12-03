using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Helper.Http.Entity
{
    public class HttpRequest
    {
        public string URL { get; set; }
        public string Referer { get; set; }
        public Enums.HttpMethod Method { get; set; }

        public string ContentType
        {
            get
            {
                return Common.GetDictionaryValueByKey(Headers, "Content-Type");
            }
            set
            {
                Common.SetDictionaryValueByKey(Headers, "Content-Type", value);
            }
        }
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Cookies { get; set; } = new Dictionary<string, string>();

    }
}
