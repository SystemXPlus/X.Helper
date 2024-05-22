using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApp.Common
{
    public static class ConfigHelper
    {
        public static void Test()
        {
            var str = X.Helper.Common.ConfigHelper.GetAppsetting("TestKey");
            Console.WriteLine(str);
            var str2 = X.Helper.Common.ConfigHelper.GetAppsetting("TestKye2");
            Console.WriteLine(str2);
            var str3 = X.Helper.Common.ConfigHelper.GetAppsetting("TestKey3", "TestKey3Value");
            Console.WriteLine(str3);
        }
    }
}
