using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace X.Helper.Http.Interface
{
    public interface IHttpContentCreater
    {
        /// <summary>
        /// 创建HTTP内容对象
        /// </summary>
        /// <param name="contentParams"></param>
        /// <param name="contentType"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        HttpContent Create(List<Entity.HttpContentParam> contentParams,
                           //ContentType contentType,
                           Encoding encoding);
    }
}
