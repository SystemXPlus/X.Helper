using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Helper.Http.Enums
{
    /// <summary>
    /// 请求正文内容类型
    /// </summary>
    [Flags]
    public enum HttpContentType
    {
        /// <summary>
        /// 纯文本
        /// </summary>
        [Description("text/plain")]
        RAW_TEXT,
        /// <summary>
        /// JSON文本
        /// </summary>
        [Description("application/json")]
        RAW_JSON,
        /// <summary>
        /// XML文本
        /// </summary>
        [Description("application/xml")]
        RAW_XML,
        /// <summary>
        /// HTML文本
        /// </summary>
        [Description("text/html")]
        RAW_HTML,

        /// <summary>
        /// JavaScript文本
        /// </summary>
        [Description("application/javascript")]
        RAW_JAVASCRIPT,

        /// <summary>
        /// 表单数据/键值对
        /// </summary>
        [Description("application/x-www-form-urlencoded")]
        X_WWW_FORM_URLENCODED,
        /// <summary>
        /// 多部分表单数据/键值对
        /// </summary>
        [Description("multipart/form-data")]
        MULTIPART_FORM_DATA,
        /// <summary>
        /// 二进制数据
        /// </summary>
        [Description("application/octet-stream")]
        BINARY,



        ///// <summary>
        ///// 文本内容
        ///// </summary>
        //STRING,
        ///// <summary>
        ///// 键值对字典
        ///// </summary>
        //DICTIONARY,
        ///// <summary>
        ///// 数据流
        ///// </summary>
        //STREAM,
        ///// <summary>
        ///// 字节数组
        ///// </summary>
        //BYTE_ARRAY,
        ///// <summary>
        ///// 对象
        ///// </summary>
        //OBJECT,

    }
}
