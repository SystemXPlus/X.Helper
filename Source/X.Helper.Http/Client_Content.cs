using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;

namespace X.Helper.Http
{
    public partial class Client
    {


        /// <summary>
        /// 创建请求正文内容
        /// </summary>
        private void CreateContent()
        {
            if (null == _ContentType)
                _ContentType = new MediaTypeHeaderValue("text/html") { CharSet = _Encoding.WebName };
            var helper = new Helper.ContentHelper(_ContentType);
            //根据不同CONTENTTYPE类型处理对应CONTENT
            //未来考虑支持直接传入Stream对象
            if (this._Files == null || this._Files.Count == 0 || !_ContentType.MediaType.ToLower().Equals("multipart/form-data"))
            {
                _RequestMessage.Content = GetStringContent();
                //根据不同的CONTENTTYPE类型设置HEADERS的ContentType
                _RequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(_ContentType.MediaType);
                _RequestMessage.Content.Headers.ContentType.CharSet = _ContentType.CharSet;
            }
            else
            {
                //有文件或者Multipart/Form-Data类型


                //Multipart/Form-Data类型由MultipartFormDataContent设定？或者判断有文件要上传时手动设定
                //考虑是否在上传文件时需要手动设置其它ContentType？
            }






        }

        private HttpContent GetStringContent()
        {
            var helper = new Helper.ContentHelper(_ContentType);
            switch (this._RequestContentType)
            {
                case Enums.RequestContentType.STRING:
                    return helper.GetStringContent(this._ContentString);
                case Enums.RequestContentType.DICTIONARY:
                    return helper.GetStringContent(this._ContentDictionary);
                    
                case Enums.RequestContentType.OBJECT:
                    return helper.GetStringContent(this._ContentObject);
                    
                default:
                    //按STRING处理
                    return helper.GetStringContent(this._ContentString);
            }
        }

    }
}
