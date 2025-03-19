using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace X.Helper.Http.Helper
{
    internal class CookieHelper
    {
        /// <summary>
        /// 从COOKIE字典中获取拼接后的COOKIE字符串
        /// </summary>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public string GetCookieStr(Dictionary<string, string> cookies)
        {
            if (cookies == null || cookies.Count == 0) return string.Empty;
            var result = string.Empty;
            foreach (var item in cookies)
            {
                result += $"{item.Key}={item.Value};";
            }
            return result;
        }

        /// <summary>
        /// 从COOKIE集合中获取拼接后的COOKIE字符串
        /// </summary>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public string GetCookieStr(CookieCollection cookies)
        {
            if (cookies == null || cookies.Count == 0) return string.Empty;
            var result = string.Empty;
            foreach (Cookie item in cookies)
            {
                result += $"{item.Name}={item.Value};";
            }
            return result;
        }
        /// <summary>
        /// 从COOKIE集合中获取整理后的COOKIE字典
        /// </summary>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetCookieDicrionary(CookieCollection cookies)
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
        public Dictionary<string, string> GetCookieDictionary(string cookies)
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
        public CookieCollection GetCookieDictionary(string cookies)
        {
            //TODO
            return null;
        }
        /// <summary>
        /// 从COOKIE字典转换为COOKIE集合
        /// </summary>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public CookieCollection GetCookieCollection(Dictionary<string, string> cookies)
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
        public CookieCollection GetCookieCollection(HttpHeaders headers)
        {
            var result = new CookieCollection();

            if (headers.TryGetValues("Set-Cookie", out var setCookies))
            {
                foreach (var cookieItem in setCookies)
                {
                    if (string.IsNullOrWhiteSpace(cookieItem)) continue;

                    if (!cookieItem.Contains('=')) continue;
                    //var temp = item.ToLower().Replace("\r", string.Empty).Replace("\n", string.Empty).Trim();
                    //if (string.IsNullOrWhiteSpace(temp)) continue;
                    var tempItem = cookieItem.Replace("\r", string.Empty).Replace("\n", string.Empty).Trim();
                    if (string.IsNullOrWhiteSpace(tempItem)) continue;

                    var cookie = new Cookie();

                    var arr = cookieItem.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    if (arr.Length == 0) continue;
                    foreach (var item in arr)
                    {

                        if (item.Contains('='))
                        {
                            var tempArr = tempItem.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                            if (tempArr.Length == 2)
                            {
                                var tempArrValue = tempArr[0]?.ToLower();
                                if ("name".Equals(tempArrValue))
                                    cookie.Name = tempArr[1];
                                if ("path".Equals(tempArrValue))
                                    cookie.Path = tempArr[1];
                                if ("domain".Equals(tempArrValue))
                                    cookie.Domain = tempArr[1];
                                if ("version".Equals(tempArrValue))
                                {
                                    if (int.TryParse(tempArr[1], out int tempVersion))
                                        cookie.Version = tempVersion;
                                }
                                if ("max-age".Equals(tempArrValue))
                                {
                                    //TIMESTAMP SECOND
                                    if (long.TryParse(tempArr[1], out long tempMaxAge))
                                    {
                                        cookie.Expires = DateTime.UtcNow.AddSeconds(tempMaxAge);
                                    }
                                }
                                if ("expires".Equals(tempArrValue))
                                {
                                    //DATE
                                    if (DateTime.TryParse(tempArr[1], out DateTime tempExpires))
                                    {
                                        cookie.Expires = tempExpires;
                                    }
                                }
                            }
                        }
                        else
                        {
                            var itemLower = item.ToLower();
                            if ("secure".Equals(itemLower))
                            {
                                cookie.Secure = true;
                            }
                            if ("httponly".Equals(itemLower))
                            {
                                cookie.HttpOnly = true;
                            }
                        }
                    }
                    if (cookie.Name != null)
                        result.Add(cookie);
                }
            }

            return result;
        }


    }
}
