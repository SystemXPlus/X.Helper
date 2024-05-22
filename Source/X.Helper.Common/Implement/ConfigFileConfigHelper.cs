using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Helper.Common.Implement
{
#if NET461
    public class ConfigFileConfigHelper : Interface.IConfigHelper
    {
        public string GetAppsetting(string key)
        {
            var result = System.Configuration.ConfigurationManager.AppSettings[key];
            return result;

        }

        public string GetAppsetting(string key, string defaultValue)
        {
            var result = GetAppsetting(key);
            if(string.IsNullOrEmpty(result))
                return defaultValue;
            return result;
        }
    }
#endif
}
