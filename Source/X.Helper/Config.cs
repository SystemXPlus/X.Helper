using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.Helper.Interface;

namespace X.Helper
{
    public static class Config
    {
        private static readonly IConfigHelper helper;

        static Config()
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
