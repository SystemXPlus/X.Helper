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

#if NETSTANDARD2_0_OR_GREATER || NET6_0_OR_GREATER
        [Obsolete("指定key的节点不存在或者属性未与实体属性匹配时，返回的实体未匹配的属性均为默认值")]
        public static T GetAppsetting<T>(string key) where T : class, new()
        {
            return helper.GetAppsetting<T>(key);
        }
        public static T GetAppsetting<T>(string key, T defaultValue) where T : class, new()
        {
            return helper.GetAppsetting<T> (key, defaultValue);
        }
#endif
    }
}
