using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace X.Helper.Http
{
    public partial class Client
    {


        /// <summary>
        /// 创建请求正文内容
        /// </summary>
        private void CreateContent()
        {
            var helper = new Helper.ContentHelper(_ContentType);
            //根据不同CONTENTTYPE类型处理对应CONTENT
            //未来考虑支持直接传入Stream对象
            if (this._Files == null || this._Files.Count == 0)
            {
                //未添加文件
                switch(this._RequestContentType)
                {
                    case Enums.RequestContentType.STRING:
                        this._ResponseMessage.Content = helper.GetStringContent(this._ContentString);
                        break;
                    case Enums.RequestContentType.DICTIONARY:
                        this._ResponseMessage.Content = helper.GetStringContent(this._ContentDictionary);
                        break;
                    case Enums.RequestContentType.OBJECT:
                        this._ResponseMessage.Content = helper.GetStringContent(this._ContentObject);
                        break;
                        default:
                        //按STRING处理
                        this._ResponseMessage.Content = helper.GetStringContent(this._ContentString);
                        break;
                }
            }

            //根据不同的CONTENTTYPE类型设置HEADERS的ContentType
            //Multipart/Form-Data类型由MultipartFormDataContent设定？或者判断有文件要上传时手动设定
            //考虑是否在上传文件时需要手动设置其它ContentType？
            if (null == _ContentType)
                _ContentType = new MediaTypeHeaderValue("text/html") { CharSet = _Encoding.WebName };
            _RequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(_ContentType.MediaType);
            _RequestMessage.Content.Headers.ContentType.CharSet = _ContentType.CharSet;

           
           
        }

        private void CreateStringContent()
        {
            if (_RequestMessage.Content == null)
                return;
            if (_RequestMessage.Content.Headers.ContentType == null)
            {
                _RequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(_ContentType.MediaType);
                _RequestMessage.Content.Headers.ContentType.CharSet = _ContentType.CharSet;
            }
            _RequestMessage.Content = new StringContent("", _Encoding, _ContentType.MediaType);
        }
    }
}
