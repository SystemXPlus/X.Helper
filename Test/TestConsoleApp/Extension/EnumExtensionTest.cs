using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApp.Extension
{
    public static class EnumExtensionTest
    {
        public static void TestGetEnumTypeFromString()
        {
            var str1 = "One";
            var str2 = "one";

            Console.WriteLine(str1.GetEnumTypeFromString<TestEnumType>());
            
            Console.WriteLine(str2.GetEnumTypeFromString<TestEnumType>(returnDefaultValueOnFailed: true));
            Console.WriteLine(str2.GetEnumTypeFromString<TestEnumType>(true));
            Console.WriteLine(str2.GetEnumTypeFromString<TestEnumType>());

        }
    }

    public enum TestEnumType
    {
        None = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5
    }
}
