using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace TestConsoleApp
{
    internal class Program
    {
        static void TestMethod()
        {
            //TODO TEST METHOD
            //Common.ConfigHelper.Test();
            //Cache.ServiceStackRedisHelperTest.Test();
            //var method = X.Helper.Http.Enums.HttpMethod.POST;
            //Console.WriteLine(method.ToString());

            //Extension.EnumExtensionTest.TestGetEnumTypeFromString();

            //Http.HttpTest.HttpGetTest();
            //Http.HttpTest.HttpGetWithCookieTest();
            //Http.HttpTest.HttpGet301Test();
            //Http.HttpTest.HttpPostTest();
            //Http.HttpTest.HttpDownloadFileTest();
            //Http.HttpTest.HttpDownloadFileWithCookieTest();
            //TestTemp();
            var path = @"d:\abc\test\abc.text";
            var filename = path.Substring(path.LastIndexOf('\\') + 1);
            var fileextensionname = path.Substring(path.LastIndexOf('.') + 1);
            Console.WriteLine(filename);
            Console.WriteLine(fileextensionname);
        }

        static void TestTemp()
        {
            var str = @"d:\test\";
            var fileInfo = new System.IO.FileInfo(str);
            Console.WriteLine(fileInfo.Name);
            var directoryInfo = new System.IO.DirectoryInfo(str);
            Console.WriteLine(directoryInfo.Name);
        }

        #region Main with stopwatch
        static void Main(string[] args)
        {
            var stopWatch = Stopwatch.StartNew();
            TestMethod();
            stopWatch.Stop();


            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\t\t\t\t┌───────────────────────────────────────────────────────┐");
            Console.WriteLine($"\t\t\t\t│ 结束时间：{DateTime.Now:yyyy:MM:dd HH:mm:ss:ffff}\t\t\t│");
            Console.WriteLine("\t\t\t\t├───────────────────────────────────────────────────────┤");
            Console.WriteLine($"\t\t\t\t│ 执行耗时：{stopWatch.Elapsed}\t\t\t\t│");
            Console.WriteLine("\t\t\t\t├───────────────────────────────────────────────────────┤");
            Console.WriteLine("\t\t\t\t│ Press any key to exit ......                          │");
            Console.WriteLine("\t\t\t\t└───────────────────────────────────────────────────────┘");
            Console.ResetColor();
            Console.ReadLine();
        }
        #endregion
    }
}
