using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
//using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace X.Helper.Http.Helper
{
    public class CookieHelper
    {
        /// <summary>
        /// 从COOKIE字典中获取拼接后的COOKIE字符串
        /// </summary>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public static string GetCookieStr(Dictionary<string, string> cookies, Encoding encoding = null)
        {
            if (cookies == null || cookies.Count == 0) return string.Empty;
            if (encoding == null) encoding = Encoding.UTF8;
            var result = string.Empty;
            foreach (var item in cookies)
            {
                var bytes = encoding.GetBytes(item.Value);
                //var value = HttpUtility.UrlEncode(bytes);
                var value = Encoding.ASCII.GetString(bytes);
                result += $"{item.Key}={value};";
                //result += $"{item.Key}={Uri.EscapeDataString(item.Value)};";
                //result += $"{item.Key}={item.Value};";
            }
            return result;
        }

        /// <summary>
        /// 从COOKIE集合中获取拼接后的COOKIE字符串
        /// </summary>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public static string GetCookieStr(CookieCollection cookies, Encoding encoding = null)
        {
            if (cookies == null || cookies.Count == 0) return string.Empty;
            if(encoding == null) encoding = Encoding.UTF8;
            var result = string.Empty;
            foreach (Cookie item in cookies)
            {
                var bytes = encoding.GetBytes(item.Value);
                //var value = HttpUtility.UrlEncode(bytes);
                var value = Encoding.ASCII.GetString(bytes);
                result += $"{item.Name}={value};";
                //result += $"{item.Name}={Uri.EscapeDataString(item.Value)};";
                //result += $"{item.Name}={item.Value};";
            }
            return result;
        }
        /// <summary>
        /// 从COOKIE集合中获取整理后的COOKIE字典
        /// </summary>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetCookieDicrionary(CookieCollection cookies)
        {
            var result = new Dictionary<string, string>();
            if (cookies != null && cookies.Count > 0)
            {
                foreach (Cookie item in cookies)
                {
                    result[item.Name] = item.Value;
                }
            }
            return result;
        }
        /// <summary>
        /// 从COOKIE字符串里获取整理后的COOKIE字典
        /// </summary>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetCookieDictionary(string cookies)
        {
            var result = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(cookies)) return result;
            var arrCookie = cookies.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < arrCookie.Length; i++)
            {
                var cookieItem = arrCookie[i];
                if (string.IsNullOrWhiteSpace(cookieItem)) continue;
                var temp = cookieItem.ToLower().Replace("\r", string.Empty).Replace("\n", string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(temp)) continue;
                //Secure
                //HttpOnly
                if (!temp.Contains('=')) continue;
                if (temp.StartsWith("path=")) continue;
                if (temp.StartsWith("expires=")) continue;
                if (temp.StartsWith("max-age=")) continue;
                if (temp.StartsWith("domain=")) continue;

                var tempArray = cookieItem.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (tempArray.Length == 0) continue;
                string cookieName, cookieValue = string.Empty;
                cookieName = tempArray[0].Trim();
                if (tempArray.Length > 1)
                    cookieValue = tempArray[1].Trim();
                result.Add(cookieName, cookieValue);
            }
            return result;
        }
        /// <summary>
        /// 从COOKIE字符串里获取整理后的COOKIE集合
        /// </summary>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public static CookieCollection GetCookieCollection(string cookies)
        {
            var result = new CookieCollection();

            if (string.IsNullOrWhiteSpace(cookies)) return result;
            var arrCookie = cookies.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < arrCookie.Length; i++)
            {
                var cookieItem = arrCookie[i];
                var cookie = GetCookie(cookieItem);
                if (cookie != null)
                    result.Add(cookie);
            }
            return result;
        }
        /// <summary>
        /// 从COOKIE字典转换为COOKIE集合
        /// </summary>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public static CookieCollection GetCookieCollection(Dictionary<string, string> cookies)
        {
            var result = new CookieCollection();
            if (cookies != null && cookies.Count > 0)
            {
                foreach (var item in cookies)
                {
                    result.Add(new Cookie(item.Key, item.Value));
                }
            }
            return result;
        }
        /// <summary>
        /// 从HTTPHEADERS中提取COOKIE集合
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static CookieCollection GetCookieCollection(HttpHeaders headers)
        {
            var result = new CookieCollection();

            if (headers.TryGetValues("Set-Cookie", out var setCookies))
            {
                foreach (var cookieItem in setCookies)
                {
                    var cookie = GetCookie(cookieItem);
                    if (cookie != null)
                        result.Add(cookie);
                }
            }
            return result;
        }

        /// <summary>
        /// 从单条COOKIE字符串里获取整理后的COOKIE对象
        /// </summary>
        /// <param name="cookieString"></param>
        /// <returns></returns>
        private static Cookie GetCookie(string cookieString)
        {

            if (string.IsNullOrWhiteSpace(cookieString)) return null;

            if (!cookieString.Contains('=')) return null;
            //var temp = item.ToLower().Replace("\r", string.Empty).Replace("\n", string.Empty).Trim();
            //if (string.IsNullOrWhiteSpace(temp)) continue;
            var tempItem = cookieString.Replace("\r", string.Empty).Replace("\n", string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(tempItem)) return null;

            var cookie = new Cookie();

            var arr = cookieString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length == 0) return null; ;
            var tempArr0 = arr[0].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
            if (tempArr0.Length == 0) return null;
            cookie.Name = tempArr0[0].Trim();
            if (tempArr0.Length > 1)
            {
                if (tempArr0.Length == 2)
                {
                    cookie.Value = tempArr0[1];
                }
                else
                {
                    //部分COOKIE值中包含=号 如BASE64或者其它编码
                    cookie.Value = arr[0].Substring(arr[0].IndexOf('=') + 1);
                }

            }
            for (int i = 1; i < arr.Length; i++)
            {
                var item = arr[i];
                if (item.Contains('='))
                {
                    var tempArr = item.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tempArr.Length == 2)
                    {
                        var tempArrValue = tempArr[0]?.ToLower();

                        switch (tempArrValue.Trim())
                        {
                            case "path":
                                cookie.Path = tempArr[1];
                                break;
                            case "domain":
                                cookie.Domain = tempArr[1];
                                break;
                            case "version":
                                if (int.TryParse(tempArr[1], out int tempVersion))
                                    cookie.Version = tempVersion;
                                break;
                            case "max-age":
                                //TIMESTAMP SECOND
                                if (long.TryParse(tempArr[1], out long tempMaxAge))
                                {
                                    cookie.Expires = DateTime.UtcNow.AddSeconds(tempMaxAge);
                                }
                                break;
                            case "expires":
                                //DATE
                                //TODO 此处转换后的是本地时间。实际应该是UTC时间
                                if (DateTime.TryParse(tempArr[1], out DateTime tempExpires))
                                {
                                    cookie.Expires = tempExpires;
                                }
                                break;
                            case "port":
                                cookie.Port = tempArr[1];
                                break;
                            case "comment":
                                cookie.Comment = tempArr[1];
                                break;
                            case "commenturi":
                                cookie.CommentUri = new Uri(tempArr[1]);
                                break;
                        }

                        //if ("path".Equals(tempArrValue))
                        //    cookie.Path = tempArr[1];
                        //if ("domain".Equals(tempArrValue))
                        //    cookie.Domain = tempArr[1];
                        //if ("version".Equals(tempArrValue))
                        //{
                        //    if (int.TryParse(tempArr[1], out int tempVersion))
                        //        cookie.Version = tempVersion;
                        //}
                        //if ("max-age".Equals(tempArrValue))
                        //{
                        //    //TIMESTAMP SECOND
                        //    if (long.TryParse(tempArr[1], out long tempMaxAge))
                        //    {
                        //        cookie.Expires = DateTime.UtcNow.AddSeconds(tempMaxAge);
                        //    }
                        //}
                        //if ("expires".Equals(tempArrValue))
                        //{
                        //    //DATE
                        //    if (DateTime.TryParse(tempArr[1], out DateTime tempExpires))
                        //    {
                        //        cookie.Expires = tempExpires;
                        //    }
                        //}
                    }
                }
                else
                {
                    var itemLower = item.ToLower();
                    switch (itemLower)
                    {
                        case "secure":
                            cookie.Secure = true;
                            break;
                        case "httponly":
                            cookie.HttpOnly = true;
                            break;
                    }

                    //if ("secure".Equals(itemLower))
                    //{
                    //    cookie.Secure = true;
                    //}
                    //if ("httponly".Equals(itemLower))
                    //{
                    //    cookie.HttpOnly = true;
                    //}
                }
            }
            if (cookie.Name != null)
                return cookie;
            return null;
        }

    }
}
