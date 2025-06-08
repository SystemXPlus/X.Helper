using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace X.Helper.Http.Helper
{
    [Obsolete("This class is obsolete. ", true)]
    public class ContentHelper_OBSOLETE
    {
        private readonly MediaTypeHeaderValue _mediaType;
        public ContentHelper_OBSOLETE(MediaTypeHeaderValue mediaType)
        {
            _mediaType = mediaType;
        }

        public HttpContent GetStringContent<T>(T content) where T : class, new()
        {
            var jsonString = X.Helper.Json.Serialize(content);
            return GetStringContent(jsonString);
        }
        public HttpContent GetStringContent(string content)
        {
            var httpContent = new StringContent(content, Encoding.GetEncoding(_mediaType.CharSet), _mediaType.MediaType);
            return httpContent;
        }

        public HttpContent GetFormUrlEncodedContent<T>(T content) where T: class, new()
        {
            var dic = Common.GetPropertiesAsDictionary(content);
            return GetFormUrlEncodedContent(dic);
        }

        public HttpContent GetFormUrlEncodedContent(IEnumerable<KeyValuePair<string, string>> keyValuePairs)
        {
            var httpContent = new FormUrlEncodedContent(keyValuePairs);
            return httpContent;
        }
        public HttpContent GetFormUrlEncodedContent(Dictionary<string, string> content)
        {
            var httpContent = new FormUrlEncodedContent(content);
            return httpContent;
        }

        public HttpContent GetMultipartFormDataContent(IEnumerable<string> filePaths, string boundary = null)
        {
            using (var content = boundary == null ? new MultipartFormDataContent() : new MultipartFormDataContent(boundary))
            {
                foreach(var streamContent in GetStreamContent(filePaths))
                {
                    content.Add(streamContent);
                }

                return content;
            }
        }
        public HttpContent GetMultipartFormDataContent(IEnumerable<KeyValuePair<string, string>> fileKeyValuePairs)
        {
            var content = new MultipartFormDataContent();
            foreach (var streamContent in GetStreamContent(fileKeyValuePairs))
            {
                content.Add(streamContent);
            }
            return content;
        }

        public HttpContent GetMultipartFormDataContent(IEnumerable<KeyValuePair<string, string>> formData, IEnumerable<string> filePaths, string boundary = null)
        {
            using (var content = boundary == null ? new MultipartFormDataContent() : new MultipartFormDataContent(boundary))
            {
                foreach (var kv in formData)
                {
                    var stringContent = new StringContent(kv.Value);
                    stringContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = kv.Key
                    };
                    content.Add(stringContent);
                }
                foreach (var streamContent in GetStreamContent(filePaths))
                {
                    content.Add(streamContent);
                }
                return content;
            }
        }

        public HttpContent GetMultipartFormDataContent(IEnumerable<KeyValuePair<string, string>> formData, IEnumerable<KeyValuePair<string, string>> fileKeyValuePairs, string boundary = null)
        {
            using (var content = boundary == null ? new MultipartFormDataContent() : new MultipartFormDataContent(boundary))
            {
                foreach (var kv in formData)
                {
                    var stringContent = new StringContent(kv.Value);
                    stringContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = kv.Key
                    };
                    content.Add(stringContent);
                }
                foreach (var streamContent in GetStreamContent(fileKeyValuePairs))
                {
                    content.Add(streamContent);
                }
                return content;
            }
        }

        public HttpContent GetMultipartFormDataContent(Dictionary<string, string> formData, IEnumerable<string> filePaths, string boundary = null)
        {
            using (var content = boundary == null ? new MultipartFormDataContent() : new MultipartFormDataContent(boundary))
            {
                foreach (var kv in formData)
                {
                    var stringContent = new StringContent(kv.Value);
                    stringContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = kv.Key
                    };
                    content.Add(stringContent);
                }
                foreach (var streamContent in GetStreamContent(filePaths))
                {
                    content.Add(streamContent);
                }
                return content;
            }
        }

        public HttpContent GetMultipartFormDataContent(Dictionary<string, string> formData, IEnumerable<KeyValuePair<string, string>> fileKeyValuePairs, string boundary = null)
        {
            using (var content = boundary == null ? new MultipartFormDataContent() : new MultipartFormDataContent(boundary))
            {
                foreach (var kv in formData)
                {
                    var stringContent = new StringContent(kv.Value);
                    stringContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = kv.Key
                    };
                    content.Add(stringContent);
                }
                foreach (var streamContent in GetStreamContent(fileKeyValuePairs))
                {
                    content.Add(streamContent);
                }
                return content;
            }
        }

        public HttpContent GetMultipartFormDataContent<T>(T formData, IEnumerable<string> filePaths, string boundary = null) where T : class, new()
        {
            var dic = Common.GetPropertiesAsDictionary(formData);
            return GetMultipartFormDataContent(dic, filePaths, boundary);
        }

        public HttpContent GetMultipartFormDataContent<T>(T formData, IEnumerable<KeyValuePair<string, string>> fileKeyValuePairs, string boundary = null) where T : class, new()
        {
            var dic = Common.GetPropertiesAsDictionary(formData);
            return GetMultipartFormDataContent(dic, fileKeyValuePairs, boundary);
        }


        private static IEnumerable<StreamContent> GetStreamContent(IEnumerable<string> filePaths)
        {

            var lstFileNames = filePaths.Select(filePath =>
            {
                var filename = filePath.Substring(filePath.LastIndexOf('\\')+1) ;
                return new KeyValuePair<string, string>(filename, filePath);
            });

            foreach(var content in GetStreamContent(lstFileNames))
            {
                yield return content;
            }
        }

        private static IEnumerable<StreamContent> GetStreamContent(IEnumerable<KeyValuePair<string, string>> fileKeyValuePairs)
        {
            var lstFileInfos = new List<KeyValuePair<string, FileInfo>>();
            foreach (var kv in fileKeyValuePairs)
            {
                var fileinfo = new FileInfo(kv.Value);
                if (!fileinfo.Exists)
                {
                    throw new FileNotFoundException($"File not found: {kv.Value}");
                }
                lstFileInfos.Add(new KeyValuePair<string, FileInfo>(kv.Key, fileinfo));
            }
            foreach(var kv in lstFileInfos)
            {
                using (var stream = kv.Value.OpenRead())
                {
                    var streamContent = new StreamContent(stream);
                    //.Headers.ContentType = new MediaTypeHeaderValue(_mediaType.MediaType);
                    streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = kv.Key,
                        FileName = kv.Value.Name
                    };
                    yield return streamContent;
                }
            }
        }

        
    }
}
