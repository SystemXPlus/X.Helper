
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if NETSTANDARD2_0_OR_GREATER || NET6_0_OR_GREATER
using Microsoft.Extensions.Configuration;
using X.Helper.Common.Interface;
#endif

namespace X.Helper.Common.Implement
{
#if NETSTANDARD2_0_OR_GREATER || NET6_0_OR_GREATER
    public class JsonFileConfigHelper : Interface.IConfigHelper
    {
        private static IConfiguration configuration { get; set; }

        static JsonFileConfigHelper()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            configuration = builder.Build();
        }

        public string GetAppsetting(string key)
        {
            return configuration[key];
        }

        public string GetAppsetting(string key, string defaultValue)
        {
            var result = GetAppsetting(key);
            if(string.IsNullOrEmpty(result))
                result = defaultValue;
            return result;
        }

        public T GetAppsetting<T>(string key) where T : class, new()
        {
            T result = new T();
            configuration.Bind(key, result);
            return result;
        }

        public T GetAppsetting<T>(string key, T defaultValue) where T : class, new()
        {
            configuration.Bind(key, defaultValue);
            return defaultValue;
        }
    }
#endif
}
