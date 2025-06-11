using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Helper.Http
{
    internal class Common
    {
        public static string GetDictionaryValueByKey(Dictionary<string, string> dictionary, string key)
        {
            if (dictionary == null)
                return string.Empty;
            if (dictionary.ContainsKey(key))
                return dictionary[key];
            return string.Empty;
        }

        public static void SetDictionaryValueByKey(Dictionary<string, string> dictionary, string key, string value)
        {
            if (dictionary == null)
                dictionary = new Dictionary<string, string>();
            if (dictionary.ContainsKey(key))
                if (dictionary[key] != value)
                    dictionary[key] = value;
            dictionary.Add(key, value);
        }

        public static readonly IEnumerable<string> UniqueHeaderNames = new string[] {
            "Cache-Control",
            "Connection",
            "Content-Length",
            "Content-Type",
            "Date",
            "Expect",
            "Host",
            "If-Modified-Since",
            "If-None-Match",
            "Max-Forwards",
            "Pragma",
            "Range",
            "Referer",
            "Transfer-Encoding",
            "Upgrade",
            "User-Agent",
            "Via",
            "Warning",
            "Accept",
            "Accept-Charset",
            "Accept-Encoding",
        };
    }
}
