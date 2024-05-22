using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApp_NET60.Common
{
    public static class ConfigHelperTest
    {
        public static void Test()
        {
            var str = X.Helper.Common.ConfigHelper.GetAppsetting("database:connstring");
            Console.WriteLine(str);
            var str2 = X.Helper.Common.ConfigHelper.GetAppsetting("TestKye2");
            Console.WriteLine(str2);
            var str3 = X.Helper.Common.ConfigHelper.GetAppsetting("TestKey3", "TestKey3Value");
            Console.WriteLine(str3);
        }
    }
}
