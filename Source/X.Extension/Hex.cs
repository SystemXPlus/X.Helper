using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class HexTypeExtension
{
    public static string ToHex62(this long value, string key = null)
    {
        if (key.IsNullOrEmpty() || key.Length != 62)
            key = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        long exponent = key.Length;
        string result = string.Empty;
        while (value > 0)
        {
            long index = value % exponent;
            result = key[(int)index] + result;
            value = (value - index) / exponent;
        }
        return result;
    }

    public static long ToHex10FromHex62(this string str, string key = null)
    {
        try
        {
            if (str.IsNullOrEmpty())
                return 0L;
            if (key.IsNullOrEmpty() || key.Length != 62)
                key = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            long exponent = key.Length;
            long result = 0L;
            for (int i = 0; i < str.Length; i++)
            {
                if (!key.Contains(str[i].ToString()))
                    throw new ArgumentException(string.Format("未预料的字符：{0} 位置：{1}", str[i], i));
                int index = 0;
                for (int j = 0; j < exponent; j++)
                {
                    if (key[j].Equals(str[str.Length - i - 1]))
                        index = j;
                }
                result += (long)Math.Pow(62, i) * index;
            }
            return result;
        }
        catch
        {
            return 0L;
        }
    }

}
