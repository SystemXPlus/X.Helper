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
                result = defaultValue;
            return result;
        }

        [Obsolete("Web.Config/App.Config don't support reading appsetting to entity", true)]
        public T GetAppsetting<T>(string key) where T : class, new()
        {
            throw new NotImplementedException();
        }
        [Obsolete("Web.Config/App.Config don't support reading appsetting to entity", true)]
        public T GetAppsetting<T>(string key, T defaultValue) where T : class, new()
        {
            throw new NotImplementedException();
        }
    }
#endif
}
