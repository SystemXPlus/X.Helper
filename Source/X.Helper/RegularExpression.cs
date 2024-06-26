﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace X.Helper
{
    public static class RegularExpression
    {
        /// <summary>
        /// 单次匹配查找
        /// </summary>
        /// <param name="input">被查找的字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <param name="options">正则表达式选项</param>
        /// <returns>匹配到的第一个结果</returns>
        public static string MatchingSingle(string input, string pattern, RegexOptions options = RegexOptions.None)
        {
            try
            {
                string temp = string.Empty;
                Regex reg = new Regex(pattern, options);
                temp = reg.Match(input).Value;
                return temp;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string MatchingSingle(string input, string pattern, string group, RegexOptions options = RegexOptions.None)
        {
            try
            {
                string temp = string.Empty;
                Regex reg = new Regex(pattern, options);
                temp = reg.Match(input).Groups[group].Value;
                return temp;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string MatchingSingle(string input, string pattern, int group, RegexOptions options = RegexOptions.None)
        {
            try
            {
                string temp = string.Empty;
                Regex reg = new Regex(pattern, options);
                temp = reg.Match(input).Groups[group].Value;
                return temp;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 多次匹配查找
        /// </summary>
        /// <param name="input">被查找的字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <param name="options">正则表达式选项</param>
        /// <returns>匹配到的结果列表</returns>
        public static List<string> MatchingMulti(string input, string pattern, RegexOptions options = RegexOptions.None)
        {
            try
            {
                List<string> list = new List<string>();
                Regex reg = new Regex(pattern, options);
                MatchCollection mc = reg.Matches(input);
                foreach (Match m in mc)
                {
                    //list.Add(m.Groups[0].Value);
                    list.Add(m.Value);
                }
                return list;
            }
            catch
            {
                return new List<string>();
            }
        }

        public static List<string> MatchingMulti(string input, string pattern, int group, RegexOptions options = RegexOptions.None)
        {
            try
            {
                List<string> list = new List<string>();
                Regex reg = new Regex(pattern, options);
                MatchCollection mc = reg.Matches(input);
                foreach (Match m in mc)
                {
                    list.Add(m.Groups[group].Value);
                }
                return list;
            }
            catch
            {
                return new List<string>();
            }
        }

        public static List<string> MatchingMulti(string input, string pattern, string group, RegexOptions options = RegexOptions.None)
        {
            try
            {
                List<string> list = new List<string>();
                Regex reg = new Regex(pattern, options);
                MatchCollection mc = reg.Matches(input);
                foreach (Match m in mc)
                {
                    list.Add(m.Groups[group].Value);
                }
                return list;
            }
            catch
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// 正则表达式过滤
        /// </summary>
        /// <param name="input">原始字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <returns>根据正则表达式过滤后的字符串</returns>
        public static string RegexReplace(string input, string pattern, RegexOptions options = RegexOptions.IgnoreCase)
        {
            try
            {
                if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(pattern))
                    return string.Empty;
                return Regex.Replace(input, pattern, string.Empty, options);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 正则表达式替换
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string RegexReplace(string input, string pattern, string replacement, RegexOptions options = RegexOptions.IgnoreCase)
        {
            try
            {
                if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(pattern))
                    return string.Empty;
                return Regex.Replace(input, pattern, replacement, options);
            }
            catch
            {
                return string.Empty;
            }
        }




        #region 常用验证方法

        public static bool IsMatch(string input, string pattern, RegexOptions options = RegexOptions.None)
        {
            try
            {
                Regex regex = new Regex(pattern, options);
                return regex.Match(input).Success;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Email
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static bool IsEmail(string _value)
        {
            //@"^\w+([-+.]\w+)*@(\w+([-.]\w+)*\.)+([a-zA-Z]+)+_
            //\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*
            Regex regex = new Regex(@"^[A-Za-z0-9]+([A-Za-z0-9\-_.]+)*@([A-Za-z0-9]+[-.])+[A-Za-z0-9]{2,5}$", RegexOptions.IgnoreCase);
            return regex.Match(_value).Success;
        }

        /// <summary>
        /// 验证是否是电话--手机
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static bool IsTel(string _value)
        {
            Regex regex = new Regex(@"^1\d{10}$", RegexOptions.IgnoreCase);
            return regex.Match(_value).Success;
        }
        /// <summary>
        /// 验证 中国身份证号码 是否为15位或18位
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static bool IsValidCNID(string ID)
        {   //验证身份证是否为15位或18位
            Regex regex = new Regex(@"d{18}|d{15}");
            return regex.IsMatch(ID);
        }
        /// <summary>
        /// 中国大陆身份证号码（中国大陆身份证）
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static bool IsIDCard(string _value)
        {
            Regex regex;
            string[] strArray;
            DateTime time;
            if ((_value.Length != 15) && (_value.Length != 0x12))   //0x12=18　十六进制
            {
                return false;
            }
            if (_value.Length == 15)
            {
                regex = new Regex(@"^(\d{6})(\d{2})(\d{2})(\d{2})(\d{3})_");
                if (!regex.Match(_value).Success)
                {
                    return false;
                }
                strArray = regex.Split(_value);
                try
                {
                    time = new DateTime(int.Parse("19" + strArray[2]), int.Parse(strArray[3]), int.Parse(strArray[4]));
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            regex = new Regex(@"^(\d{6})(\d{4})(\d{2})(\d{2})(\d{3})([0-9Xx])_");
            if (!regex.Match(_value).Success)
            {
                return false;
            }
            strArray = regex.Split(_value);
            try
            {
                time = new DateTime(int.Parse(strArray[2]), int.Parse(strArray[3]), int.Parse(strArray[4]));
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 是否整型数值（long范围）
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static bool IsInt(string _value)
        {
            Regex regex = new Regex(@"^(-){0,1}\d+_");
            if (regex.Match(_value).Success)
            {
                if ((long.Parse(_value) > 0x7fffffffL) || (long.Parse(_value) < -2147483648L))
                {
                    return false;
                }
                return true;
            }
            return false;
        }



        /// <summary>
        /// 是否只有英文字母组成的字符串
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public static bool IsOnlyEnglishLetters(string character)
        {
            Regex regex = new Regex(@"^.[A-Za-z]+$");
            return regex.IsMatch(character);
        }
        /// <summary>
        /// 判断是否中国手机号
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static bool IsChinaMobilePhone(string _value)
        {
            Regex regex = new Regex(@"^(13[0-9]|14[0-9]|15[0-9]|17[0-9]|18[0-9])\d{8}$");
            return regex.Match(_value).Success;
        }
        /// <summary>
        /// 英文字母与数字组成的字符串
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static bool IsLetterOrNumber(string _value)
        {
            return QuickValidate("^[a-zA-Z0-9_]*_", _value);
        }
        /// <summary>
        /// 验证中国 手机号
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static bool IsCNMobileNum(string _value)
        {
            //13\d{9}_
            Regex regex = new Regex(@"^((\(\d{3}\))|(\d{3}\-))?13[0-9]\d{8}|15[0-9]\d{8}|18[0-9]\d{8}", RegexOptions.IgnoreCase);
            return regex.Match(_value).Success;
        }
        /// <summary>
        /// 验证中国 电话
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsCNPhoneNum(string _value)
        {
            //^(86)?(-)?(0\d{2,3})?(-)?(\d{7,8})(-)?(\d{3,5})?_
            Regex regex = new Regex(@"(^[0-9]{3,4}\-[0-9]{3,8}$)|(^[0-9]{3,8}$)|(^\([0-9]{3,4}\)[0-9]{3,8}$)|(^0{0,1}13[0-9]{9}", RegexOptions.IgnoreCase);
            return regex.Match(_value).Success;
        }
        /// <summary>
        /// 验证中国 邮政编码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsValidCNZipCode(string zipcode)
        {
            Regex regex = new Regex(@"d{6}");
            return regex.IsMatch(zipcode);
        }
        /// <summary>
        /// 验证是否美国/加拿大地区手机号码
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static bool IsUS_CA_PhoneNumber(string _value)
        {
            Regex regex = new Regex(@"^001\d{10}$", RegexOptions.None);
            return regex.Match(_value).Success;
        }
        /// <summary>
        /// 验证美国US电话
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsValidUSPhone(string phone)
        {
            Regex regex = new Regex(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}");
            return regex.IsMatch(phone);
        }
        /// <summary>
        /// 验证美国zip 邮政编码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsValidUSZipCode(string zipcode)
        {
            Regex regex = new Regex(@"\d{5}(-\d{4})?");
            return regex.IsMatch(zipcode);
        }
        /// <summary>
        /// 只是 数字
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static bool IsNumber(string _value)
        {
            //^(0|([1-9]+[0-9]*))(.[0-9]+)?_
            return QuickValidate("^.[0-9]*$", _value);
        }

        public static bool IsNumeric(string _value)
        {
            return QuickValidate("^[1-9]*[0-9]*_", _value);
        }

        /// <summary>
        /// 验证日期类型为yyyy-MM-dd
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsValidDate(string date)
        {   //验证日期类型为yyyy-MM-dd
            Regex regex = new Regex(@"^((((19|20)(([02468][048])|([13579][26]))-02-29))|((20[0-9][0-9])|(19[0-9][0-9]))-((((0[1-9])|(1[0-2]))-((0[1-9])|(1\d)|(2[0-8])))|((((0[13578])|(1[02]))-31)|(((0[1,3-9])|(1[0-2]))-(29|30)))))$");
            return regex.IsMatch(date);
        }
        /// <summary>
        /// 验证日期类型为 yyyy-MM-dd hh:mm:ss
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsValidDateTime(string dateTime)
        {   //验证日期类型为yyyy-MM-dd hh:mm:ss
            Regex regex = new Regex(@"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$");
            return regex.IsMatch(dateTime);
        }
        /// <summary>
        /// 是否为日期字符串
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static bool IsStringDate(string _value)
        {
            try
            {
                DateTime dTime = DateTime.Parse(_value);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }
        /// <summary>
        /// URL/网址（包含HTTP）
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static bool IsUrl(string _value)
        {
            //(http://)?([\w-]+\.)*[\w-]+(/[\w- ./?%&=]*)?
            Regex regex = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.IgnoreCase);
            return regex.Match(_value).Success;
        }
        /// <summary>
        /// 是否 字母 和 数字
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_defaultValue"></param>
        /// <returns></returns>
        public static bool IsWordAndNum(string _value)
        {
            Regex regex = new Regex("[a-zA-Z0-9]?");
            return regex.Match(_value).Success;
        }
        /// <summary>
        /// 快速验证
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_defaultValue"></param>
        /// <returns></returns>
        public static bool QuickValidate(string _express, string _value)
        {
            Regex myRegex = new Regex(_express);
            if (_value.Length == 0)
            {
                return false;
            }
            return myRegex.IsMatch(_value);
        }
        /// <summary>
        /// 字符串 转 日期
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_defaultValue"></param>
        /// <returns></returns>
        public static DateTime StrToDate(string _value, DateTime _defaultValue)
        {
            if (IsStringDate(_value))
            {
                return Convert.ToDateTime(_value);
            }
            return _defaultValue;
        }
        /// <summary>
        /// 字符串 转 int
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_defaultValue"></param>
        /// <returns></returns>
        public static int StrToInt(string _value, int _defaultValue)
        {
            if (IsNumber(_value))
            {
                return int.Parse(_value);
            }
            return _defaultValue;
        }
        /// <summary>
        /// 验证是否韩文
        /// </summary>
        /// <param name="korean"></param>
        /// <returns></returns>
        public static bool IsValidKorean(string korean)
        {
            Regex regex = new Regex(@"^.[\uac00-\ud7af\u1100-\u11FF\u3130-\u318f]+$");
            return regex.IsMatch(korean);
        }


        #endregion

        #region 常用过滤方法
        /// <summary>
        /// 过滤HTML标签（<>中不管任何内容均视为HTML标签）
        /// </summary>
        /// <param name="html">包含HTML标签的字符串</param>
        /// <param name="length">过滤后截取字符串长度（默认不截取）</param>
        /// <returns></returns>
        public static string ReplaceHtmlTag(string html, int length = 0)
        {
            //过滤以<开头以>结束的字符串
            string strText = Regex.Replace(html, "<[^>]+>", "");
            //过滤&开头;结尾的字符串　暂时不用
            //strText = Regex.Replace(strText, "&[^;]+;", "");

            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);

            return strText;
        }
        #endregion
    }
}
