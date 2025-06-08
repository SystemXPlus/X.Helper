using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Helper.Http.Enums
{
    [Obsolete("This enum is deprecated. Use HttpContentType instead.", true)]
    public enum HttpContentRawType
    {
        None,
        [Description("text/plain")]
        TEXT, //text/plain
        [Description("application/json")]
        JSON, //application/json
        [Description("application/xml")]
        XML, //application/xml
        [Description("text/html")]
        HTML, //text/html
        [Description("application/javascript")]
        JAVASCRIPT, //application/javascript

    }
}
