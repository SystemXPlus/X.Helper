using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CacheTestConsoleApp_NET461.Cache
{
    public static class ServiceStackRedisHelperTest
    {
        public static void Test()
        {
            X.Helper.Cache.ICacheHelper helper = new X.Helper.Cache.ServiceStackRedisHelper();
            var key = $"testKey_{helper.DbIndex}_{helper.GetType()}";
            var value = $"testValue_{helper.DbIndex}_{helper.GetType()}";

            helper.SetValue(key, value);
            Console.WriteLine($"key:{key}\t{helper.GetValue<string>(key)}");

            var dbIndex = 5;
            helper = helper.GetDatabase(dbIndex);
            key = $"testKey_{helper.DbIndex}_{helper.GetType()}";
            value = $"testValue_{helper.DbIndex}_{helper.GetType()}";
            helper.SetValue(key, value);

            Console.WriteLine($"key:{key}\t{helper.GetValue<string>(key)}");
            Console.WriteLine($"key:{key}\t{helper.GetValue<string>(key)}");

        }
    }
}
