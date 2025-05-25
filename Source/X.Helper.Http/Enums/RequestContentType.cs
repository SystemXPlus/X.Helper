using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Helper.Http.Enums
{
    /// <summary>
    /// 请求正文内容类型
    /// </summary>
    internal enum RequestContentType
    {
        /// <summary>
        /// 文本内容
        /// </summary>
        STRING,
        /// <summary>
        /// 键值对字典
        /// </summary>
        DICTIONARY,
        /// <summary>
        /// 对象
        /// </summary>
        OBJECT,

    }
}
