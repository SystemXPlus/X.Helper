using System;
using System.Diagnostics;

namespace TestConsoleApp_NET60
{
    internal class Program
    {
        static void TestMethod()
        {
            //TODO TEST METHOD
            var str = X.Helper.Config.GetAppsetting("database:connstring");
            Console.WriteLine(str);
            var str2 = X.Helper.Config.GetAppsetting("TestKye2");
            Console.WriteLine(str2);
            var str3 = X.Helper.Config.GetAppsetting("TestKey3", "TestKey3Value");
            Console.WriteLine(str3);
        }

        static void Main(string[] args)
        {
            var stopWatch = Stopwatch.StartNew();
            TestMethod();
            stopWatch.Stop();


            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\t\t\t\t┌───────────────────────────────────────────────────────┐");
            Console.WriteLine($"\t\t\t\t│ 结束时间：{DateTime.Now:yyyy:MM:dd HH:mm:ss:ffff}\t\t\t│");
            Console.WriteLine("\t\t\t\t├───────────────────────────────────────────────────────┤");
            Console.WriteLine($"\t\t\t\t│ 执行耗时：{stopWatch.Elapsed}\t\t\t\t│");
            Console.WriteLine("\t\t\t\t├───────────────────────────────────────────────────────┤");
            Console.WriteLine("\t\t\t\t│ Press any key to exit ......                          │");
            Console.WriteLine("\t\t\t\t└───────────────────────────────────────────────────────┘");
            Console.ReadLine();
        }
    }
}
