using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

    public static partial class String
    {
    #region 正则验证部分  （未完全测试）

    private static RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Compiled;

    private static Regex LetterOrNumber = new Regex(@"^[a-zA-Z0-9_]*$", options);
    /// <summary>
    /// 验证是否字母数字组合（包含下划线）
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsLetterOrNumber(this string str)
    {
        return LetterOrNumber.IsMatch(str);
    }

    private static Regex CNMobileNumber = new Regex(@"^1\d{10}$", options);
    /// <summary>
    /// 验证是否中国大陆地区手机号
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsCNMobileNumber(this string str)
    {
        return CNMobileNumber.IsMatch(str);
    }

    private static Regex CNTelephoneNumber = new Regex(@"^([0-9]{3,4}-)?[0-9]{7,8}$", options);
    /// <summary>
    /// 验证是否中国大陆地区固定电话号码
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsCNTelephoneNumber(this string str)
    {
        return CNTelephoneNumber.IsMatch(str);
    }

    private static Regex Email = new Regex(@"^[A-Za-z0-9]+([A-Za-z0-9_.-]+)*@([A-Za-z0-9]+[-.])+[A-Za-z0-9]{2,5}$", options);
    /// <summary>
    /// 验证是否有效的电子邮件地址
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsEMail(this string str)
    {
        return Email.IsMatch(str);
    }

    private static Regex CNZipCode = new Regex(@"^\d{6}$", options);
    /// <summary>
    /// 验证是否中国大陆地区邮政编码
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsCNZipCode(this string str)
    {
        return CNZipCode.IsMatch(str);
    }

    private static Regex Number = new Regex(@"^(0|[1-9][0-9]*)$", options);

    /// <summary>
    /// 验证字符串中是否为纯数字组合（不允许0开头）
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsNumber(this string str)
    {
        return Number.IsMatch(str);
    }

    private static Regex NumberStr = new Regex(@"^[0-9]*$", options);
    /// <summary>
    /// 验证字符串中是否纯数字组合（允许0开头）
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsNumberStr(this string str)
    {
        return NumberStr.IsMatch(str);
    }

    private static Regex Numeric = new Regex(@"^(0|[1-9]\d)(\.\d+)?$", options);

    /// <summary>
    /// 判断字符串是否小数格式
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsNumeric(this string str)
    {
        return Numeric.IsMatch(str);
    }

    #endregion
}
