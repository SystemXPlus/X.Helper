using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.InteropServices.ComTypes;


public static class FileExtension
{
    /// <summary>
    /// 文件转为二进制流
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public static System.IO.Stream ToStream(this FileInfo file)
    {
        System.IO.Stream stream = null;
        // 打开文件  
        using (FileStream fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            // 读取文件的 byte[]  
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            // 把 byte[] 转换成 Stream  
            stream = new MemoryStream(bytes);
        }
        return stream;
    }
    /// <summary>
    /// 文件转为BASE64编码字符串
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public static string ToBase64(this FileInfo file)
    {
        string result = string.Empty;
        using (FileStream filestream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            byte[] bytes = new byte[filestream.Length];
            filestream.Read(bytes, 0, bytes.Length);
            result = Convert.ToBase64String(bytes);
        }
        return result;
    }

    #region 文件摘要/签名

    public static string GetSHA1(this FileInfo file)
    {
        string result = null;
        using (var sha1 = SHA1.Create())
        {
            using (var stream = System.IO.File.OpenRead(file.FullName))
            {
                var hash = sha1.ComputeHash(stream);
                result = BitConverter.ToString(hash).Replace("-", "").ToUpper();
            }
        }
        return result;
    }
    /// <summary>
    /// 计算文件哈希值SHA-256
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public static string GetSHA256(this FileInfo file)
    {
        string result = null;
        using (var sha256 = SHA256.Create())
        {
            using (var stream = System.IO.File.OpenRead(file.FullName))
            {
                var hash = sha256.ComputeHash(stream);
                result = BitConverter.ToString(hash).Replace("-", "").ToUpper();
            }
        }
        return result;
    }
    /// <summary>
    /// 计算文件哈希值SHA-512
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public static string GetSHA512(this FileInfo file)
    {
        string result = null;
        using (var sha512 = SHA512.Create())
        {
            using (var stream = System.IO.File.OpenRead(file.FullName))
            {
                var hash = sha512.ComputeHash(stream);
                result = BitConverter.ToString(hash).Replace("-", "").ToUpper();
            }
        }
        return result;
    }
    /// <summary>
    /// 计算文件MD5值
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public static string GetMD5(this FileInfo file)
    {
        string result = null;
        using (var md5 = MD5.Create())
        {
            using (var stream = System.IO.File.OpenRead(file.FullName))
            {
                var hash = md5.ComputeHash(stream);
                result = BitConverter.ToString(hash).Replace("-", "").ToUpper();
            }
        }
        return result;
    }

    #endregion
}

