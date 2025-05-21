using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Helper.Http.Helper
{
    public static class Common
    {
        public static bool IsIso88591Encoded(string input)
        {
            try
            {
                if(input == null)
                {
                    return false;
                }
                // 尝试将字符串解码为ISO-8859-1编码的字节
                byte[] bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(input);
                // 再次尝试将这些字节编码回字符串
                string decodedString = Encoding.GetEncoding("ISO-8859-1").GetString(bytes);
                // 检查原始字符串和重新编码后的字符串是否相同
                return input == decodedString;
            }
            catch (EncoderFallbackException)
            {
                // 如果在解码或编码过程中发生异常，则说明不是ISO-8859-1编码
                return false;
            }
        }
    }
}
