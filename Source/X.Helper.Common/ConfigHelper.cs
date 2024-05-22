using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.Helper.Common.Interface;

namespace X.Helper.Common
{
    public static class ConfigHelper
    {
        private static readonly IConfigHelper helper;

        static ConfigHelper()
        {
#if NET461
            helper = new Implement.ConfigFileConfigHelper();
#else
            helper = new Implement.JsonFileConfigHelper();
#endif
        }

        public static string GetAppsetting(string key)
        {
            return helper.GetAppsetting(key);
        }

        public static string GetAppsetting(string key, string defaultValue)
        {
            return helper.GetAppsetting(key, defaultValue);
        }
    }
}
