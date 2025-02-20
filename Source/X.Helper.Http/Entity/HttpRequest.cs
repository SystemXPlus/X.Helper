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
        public string Origin { get; set; }
        public Enums.HttpMethod Method { get; set; }

        public object Body { get; set; }

        private string _ContentType = null;
        public string ContentType
        {
            get
            {
                return _ContentType;
            }
            set
            {
                _ContentType = value;
            }
        }
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Cookies { get; set; } = new Dictionary<string, string>();

    }
}
