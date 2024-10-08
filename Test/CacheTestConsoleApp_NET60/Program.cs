﻿using System;
using System.Diagnostics;

namespace CacheTestConsoleApp_NET60
{
    internal class Program
    {
        static void TestMethod()
        {
            //TODO TEST METHOD
            Cache.ServiceStackRedisHelperTest.Test();
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
