
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if NETSTANDARD2_0_OR_GREATER || NET6_0_OR_GREATER
using Microsoft.Extensions.Configuration;
#endif

namespace X.Helper.Implement
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
                return defaultValue;
            return result;
        }
    }
#endif
}
