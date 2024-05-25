using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheTestConsoleApp_NET60.Cache
{
    public static class ServiceStackRedisHelperTest
    {
        public static void Test()
        {
            var helper = new X.Helper.Cache.ServiceStackRedisHelper();
            var key = "testKey";
            var value = "testValue";

            helper.SetValue(key, value);
            Console.WriteLine($"key:\t{helper.GetValue<string>(key)}");

            var dbIndex = 5;
            var cacheHelper = helper.GetDatabase(dbIndex);
            cacheHelper.SetValue($"{key}{dbIndex}", value);

            Console.WriteLine($"key:\t{helper.GetValue<string>(key+dbIndex)}");
            Console.WriteLine($"key:\t{cacheHelper.GetValue<string>(key+dbIndex)}");

        }
    }
}
