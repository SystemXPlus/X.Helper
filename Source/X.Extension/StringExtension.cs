using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;



public static class StringExtension
{
    #region 常用

    public static bool IsNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }

    public static bool IsNotNullOrEmpty(this string str)
    {
        return !string.IsNullOrEmpty(str);
    }

    public static bool IsNullOrWhiteSpace(this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    public static bool IsNotNullOrWhiteSpace(this string str)
    {
        return !string.IsNullOrWhiteSpace(str);
    }

    #endregion

    #region 加密解密/摘要
    /// <summary>
    /// 返回字符串的MD5摘要编码
    /// </summary>
    /// <param name="get32Bit">是否返回32位结果，否则返回16位</param>
    /// <param name="upperCase">是否返回大写结果</param>
    /// <param name="pams"></param>
    /// <returns></returns>
    public static string ToMD5(this string input, bool get32Bit = true, bool upperCase = true)
    {
        var result = string.Empty;
        if (string.IsNullOrEmpty(input))
            return result;
        var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        var bytes = Encoding.Default.GetBytes(input);
        var hash = md5.ComputeHash(bytes);
        if (get32Bit)
        {
            result = BitConverter.ToString(hash);
        }
        else
        {
            result = BitConverter.ToString(hash, 4, 8);
        }
        result = result?.Replace("-",string.Empty);
        result = upperCase ? result.ToUpper() : result.ToLower();
        return result;
    }


    #endregion

    #region BASE64

    /// <summary>
    /// 字符串转为BASE64字符串
    /// </summary>
    /// <param name="source">源文本</param>
    /// <param name="encoding">默认NULL为当前系统默认字符集</param>
    /// <returns></returns>
    public static string ToBase64(this string source,System.Text.Encoding encoding = null)
    {
        if (encoding.IsNull())
            encoding = System.Text.Encoding.Default;
        var bytes = encoding.GetBytes(source);
        return Convert.ToBase64String(bytes);
    }

    #endregion

    #region 数组



    #endregion

    #region 文件操作（文件路径）



    #endregion

    #region SQL过滤

    /// <summary>
    /// 简单SQL防注入过滤
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    [Obsolete("仅限在测试项目中使用")]
    public static string SqlFilter(this string source)
    {
        if (string.IsNullOrEmpty(source))
            return string.Empty;
        string temp = source.Trim();
        string[] arr = { "select", "delete", "update", "insert", "where", "and", "or", "%", "*", "'", "\"" };
        foreach (string s in arr)
        {
            temp = temp.Replace(s, "");
        }
        return temp;
    }

    #endregion


    #region 文件/文件夹


    public static bool IsFileExists(this string path)
    {
        return File.Exists(path);
    }

    public static bool IsDirectoryExists(this string path)
    {
        return Directory.Exists(path);
    }

    /// <summary>
    /// 根据文件路径返回文件名（包含扩展名）
    /// </summary>
    /// <param name="filepath"></param>
    /// <returns></returns>
    public static string GetFileName(this string filepath)
    {
        if (filepath.IndexOf('\\') == -1)
            return string.Empty;
        return filepath.Substring(filepath.LastIndexOf('\\') + 1);
        //return Path.GetFileName(filepath);
    }
    /// <summary>
    /// 根据文件路径返回文件名（不包含扩展名）
    /// </summary>
    /// <param name="filepath"></param>
    /// <returns></returns>
    public static string GetFileNameWithoutExtension(this string filepath)
    {
        return Path.GetFileNameWithoutExtension(filepath);
    }

    /// <summary>
    /// 根据文件路径返回文件扩展名（不包含点）
    /// </summary>
    /// <param name="filepath"></param>
    /// <returns></returns>
    public static string GetFileExtensionName(this string filepath)
    {
        if (filepath.IndexOf('.') == -1)
            return string.Empty;
        return filepath.Substring(filepath.LastIndexOf('.') + 1);
        //return System.IO.Path.GetExtension(filepath);   //忘了这样用带不带点
    }

    /// <summary>
    /// BASE64编码的文件字符串保存为指定位置文件
    /// </summary>
    /// <param name="fileBase64Str"></param>
    /// <param name="fileExtName">文件扩展名 不包含.</param>
    /// <param name="savePath">保存位置（系统绝对路径）</param>
    /// <returns></returns>
    public static string Base64ToFile(this string fileBase64Str, string fileExtName, string savePath)
    {
        try
        {
            var fileName = $"{Guid.NewGuid().ToString() }.{ fileExtName}";
            var path = savePath;
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
            var fullPath = System.IO.Path.Combine(path, fileName);

            var bytes = Convert.FromBase64String(fileBase64Str);
            bytes.ToFile(fullPath);
            return fullPath;
        }
        catch
        {
            return null;
        }
    }

    #endregion


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

    private static Regex CNMobileNumber= new Regex(@"^1\d{10}$", options);
    /// <summary>
    /// 验证是否中国大陆地区手机号
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsCNMobileNumber(this string str)
    {
        return CNMobileNumber.IsMatch(str);
    }

    private static Regex CNTelephoneNumber= new Regex(@"^([0-9]{3,4}-)?[0-9]{7,8}$", options);
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

    private static Regex CNZipCode = new Regex(@"^\d{0}$", options);
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

    private static Regex Numeric = new Regex(@"^\d+(\.\d+)?$", options);

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


