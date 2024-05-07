using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApp.Extension
{
    public class String
    {
        public static void Test()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                {"strABC        ","ABC" },
                {"strEmpty      ",string.Empty},
                {"strWhiteSpace1", "" },
                {"strWhiteSpace2", "    " },
                {"strNull       ", null }
            };

            foreach (var kv in dic)
            {
                Console.WriteLine($"{kv.Key}\tIsNullOrEmpty:\t\t{kv.Value.IsNullOrEmpty()}");
                Console.WriteLine($"{kv.Key}\tIsNotNullOrEmpty:\t\t{kv.Value.IsNotNullOrEmpty()}");
                Console.WriteLine($"{kv.Key}\tIsNullOrWhiteSpace:\t\t{kv.Value.IsNullOrWhiteSpace()}");
                Console.WriteLine($"{kv.Key}\tIsNotNullOrWhiteSpace:\t\t{kv.Value.IsNotNullOrWhiteSpace()}");
            }
            Console.WriteLine();
            var str = "123456";
            Console.WriteLine($"{str}\r\n32BIT MD5:\t{str.ToMD5()}");
            Console.WriteLine($"32BIT MD5_L:\t{str.ToMD5(false,false)}");
            Console.WriteLine($"16BIT MD5:\t{str.ToMD5(false)}");
            Console.WriteLine($"16BIT MD5L:\t{str.ToMD5(false,false)}");
            Console.WriteLine();
            Console.WriteLine($"{str} BASE64:\t\n{str.ToBase64()}");
            Console.WriteLine($"{str} BASE64_ASCII:\t\n{str.ToBase64(Encoding.ASCII)}");
            Console.WriteLine($"{str} BASE64_UTF32:\t\n{str.ToBase64(Encoding.UTF32)}");
        }
    }
}
