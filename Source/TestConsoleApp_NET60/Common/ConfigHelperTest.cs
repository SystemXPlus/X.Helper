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
            var str2 = X.Helper.Common.ConfigHelper.GetAppsetting("database:connstring");
            Console.WriteLine(str2);
            var str3 = X.Helper.Common.ConfigHelper.GetAppsetting("testmodel:sex", "TestKey3Value");
            Console.WriteLine(str3);
            var str4 = X.Helper.Common.ConfigHelper.GetAppsetting("testmodel");
            Console.WriteLine(str4);

            var setting = X.Helper.Common.ConfigHelper.GetAppsetting<AppsettingModel>("testmodel", new AppsettingModel { Name = "Tom", Age = 16, Sex = false });
            var setting2 = X.Helper.Common.ConfigHelper.GetAppsetting<AppsettingModel>("testmodel", new AppsettingModel { Name = "Tom", Age = 16, Sex = false });
            var setting3 = X.Helper.Common.ConfigHelper.GetAppsetting<AppsettingModel>("testmodel3", new AppsettingModel { Name="Tom",Age=16,Sex=false});
            var setting4= X.Helper.Common.ConfigHelper.GetAppsetting<AppsettingModel2>("testmodel", new AppsettingModel2 { Name = "Tom", Age = 16, Sex = false });
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(setting));
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(setting2));
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(setting3));
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(setting4));

        }
    }

    public class AppsettingModel
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public bool Sex { get; set; }
    }

    public class AppsettingModel2
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public bool Sex { get; set; }
    }
}
