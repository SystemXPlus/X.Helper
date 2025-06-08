using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Helper.Http.Enums
{
    public enum HttpContentParamType { 
        Null,
        String, //text/plain
        String_Json, //application/json
        String_Xml, //application/xml
        String_Html, //text/html
        String_JavaScript, //application/javascript

        Number,
        Boolean,
        File,
        Binary, //byte[]
        Stream, 
        Object,
    }
}
