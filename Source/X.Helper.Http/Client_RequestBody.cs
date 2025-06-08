using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace X.Helper.Http
{
    public partial class Client
    {
        public Client SetContentType(Enums.HttpContentType contentType)
        {
            SetContentType(contentType.GetDescription());
            _RequestBodyContentType = contentType;
            //CLEAR PARAMS LIST
            _RequestContentParams.Clear();
            return this;
        }

        #region 设置请求内容
        /// <summary>
        /// 检查请求正文内容类型是否包含指定的HTTP内容类型标志
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        private bool HasHttpContentTypeFlag(Enums.HttpContentType contentType)
        {
#if NET6_0_OR_GREATER
            return this._RequestBodyContentType.HasFlag(contentType);
#else
            return (this._RequestBodyContentType & contentType) == contentType;
#endif
        }

        private bool HasHttpContentTypeFlag(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return false;

#if NET6_0_OR_GREATER
            return this._RequestBodyContentType.ToString().Contains(keyword, StringComparison.OrdinalIgnoreCase);
#else
            return this._RequestBodyContentType.ToString().IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
#endif
        }
        public Client AddContent(string content)
        {
            if (null == content)
                throw new ArgumentException("content is null");
            if(!HasHttpContentTypeFlag("RAW"))
            {
                throw new Exception("当前请求正文内容类型不支持添加文本，请先设置请求正文内容类型为RAW。");
            }
            if(_RequestContentParams.Count > 0)
            {
                this._RequestContentParams.Clear();
                //throw new Exception("当前请求正文内容类型已包含内容，请先清除内容后再添加新的内容。");
            }
            //检测内容二次设置CONTENTTYPE

            this._RequestContentParams.Add(new Entity.HttpContentParam { Value = content });
            return this;
        }
        public Client AddContent(string name, object value)
        {
            if (null == name)
                throw new ArgumentException("name is null");
            //if (null == value)
            //    throw new ArgumentException("value is null");
            if (!HasHttpContentTypeFlag(Enums.HttpContentType.X_WWW_FORM_URLENCODED | Enums.HttpContentType.MULTIPART_FORM_DATA))
            {
                throw new Exception("当前请求正文内容类型不支持添加键值对，请先设置请求正文内容类型为X_WWW_FORM_URLENCODED或MULTIPART_FORM_DATA。");
            }
            //if (_ContentParams.Any(p => p.Name == name))
            //{
            //    throw new Exception($"当前请求正文内容类型已包含名称为{name}的内容，请先清除内容后再添加新的内容。");
            //}
            this._RequestContentParams.Add(new Entity.HttpContentParam { Name = name, Value = value });
            return this;
        }
        public Client AddContent(Dictionary<string, object> content)
        {
            if (content == null) throw new ArgumentException("content is null");
            if(!HasHttpContentTypeFlag(
                Enums.HttpContentType.RAW_TEXT
                | Enums.HttpContentType.X_WWW_FORM_URLENCODED
                | Enums.HttpContentType.MULTIPART_FORM_DATA))
            {
                throw new Exception("当前请求ContentType不支持添加字典键值对，请先设置请求正文内容类型为TEXT_PLAIN或X_WWW_FORM_URLENCODED或MULTIPART_FORM_DATA。");
            }
            foreach(var kv in content)
            {
                //if (kv.Value == null)
                //    throw new ArgumentException($"content[{kv.Key}] is null");
                this._RequestContentParams.Add(new Entity.HttpContentParam { Name = kv.Key, Value = kv.Value });
            }
            return this;
        }

        public Client AddContent<T>(T content) where T : class, new()
        {
            if (content == null)
                throw new ArgumentException("content is null");
            if (!HasHttpContentTypeFlag(Enums.HttpContentType.BINARY)
                && !HasHttpContentTypeFlag("RAW"))
            {
                throw new Exception("当前请求ContentType不支持添加实体对象，请先设置请求正文内容类型为RAW或BINARY。");
            }
            this._RequestContentParams.Clear();
            this._RequestContentParams.Add(new Entity.HttpContentParam { Value = content });
            return this;
        }

#endregion

        #region 文件
        /// <summary>
        /// 下载文件时目录不存在是否自动创建目录
        /// </summary>
        /// <param name="autoCreateDirectory"></param>
        /// <returns></returns>
        public Client SetAutoCreateDirectory(bool autoCreateDirectory)
        {
            this._AutoCreateDirectory = autoCreateDirectory;
            return this;
        }
        /// <summary>
        /// 设置多部分表单数据（MultipartFormData）的边界符
        /// </summary>
        /// <param name="boundary"></param>
        /// <returns></returns>
        public Client SetMultipartFormDataBoundary(string boundary)
        {
            if (!string.IsNullOrEmpty(boundary))
            {
                this._MultipartFormDataBoundary = boundary;
            }
            return this;
        }

        /// <summary>
        /// 添加文件到上传文件列表
        /// </summary>
        /// <param name="filePath">文件完整路径</param>
        /// <returns></returns>
        public Client AddFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException(@"filePath is null or empty");

            var name = filePath.Substring(filePath.IndexOf("\\") + 1);

            return AddFile(name, filePath);
        }
        /// <summary>
        /// 添加文件到上传文件列表
        /// </summary>
        /// <param name="name">提交上传文件使用的名称</param>
        /// <param name="fileInfo">文件完整路径</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Client AddFile(string name, string filePath)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(name + " is null or empty");
            }
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException(@"filePath is null or empty");
            }
            if (!filePath.Contains("\\"))
            {
                throw new ArgumentException(filePath + " is not a valid file path");
            }

            this._Files.Add(new KeyValuePair<string, string>(name, filePath));

            return this;
        }

        #endregion


        private Client SetContentType(string mediaType)
        {
            if (!string.IsNullOrEmpty(mediaType))
            {
                SetContentType(mediaType, _Encoding.WebName);
            }
            return this;
        }
        /// <summary>
        /// 设置请求ContentType请求头
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        private Client SetContentType(string mediaType, string charset)
        {
            if (!string.IsNullOrEmpty(mediaType))
            {
                var contentType = new MediaTypeHeaderValue(mediaType);
                if (!string.IsNullOrEmpty(charset))
                {
                    this._Encoding = Encoding.GetEncoding(charset);
                }
                this._ContentType = contentType;
                //此处注释，this._ContentType.CharSet在发起请求时使用this._Encoding.WebName设置
                //contentType.CharSet = _Encoding.WebName;
            }
            return this;
        }

        /// <summary>
        /// 设置请求ContentType请求头
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        private Client SetContentType(MediaTypeHeaderValue contentType)
        {
            if (contentType != null)
            {
                this._ContentType = contentType;
                if (!string.IsNullOrEmpty(contentType.CharSet))
                {
                    this._Encoding = Encoding.GetEncoding(contentType.CharSet);
                }
            }
            return this;
        }
    }
}
