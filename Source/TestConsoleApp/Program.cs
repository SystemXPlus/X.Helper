using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TestConsoleApp
{
    internal class Program
    {
        static void TestMethod()
        {
            //TODO TEST METHOD
            Extension.String.Test();
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
