using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Helper.Common.Interface
{
    public interface IConfigHelper
    {
        string GetAppsetting(string key);
        string GetAppsetting(string key, string defaultValue);


        T GetAppsetting<T>(string key) where T : class, new();

        T GetAppsetting<T>(string key, T defaultValue) where T : class, new();

    }
}
