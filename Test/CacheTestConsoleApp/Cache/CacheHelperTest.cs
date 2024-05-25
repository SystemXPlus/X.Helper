using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using X.Helper.Cache;

namespace CacheTestConsoleApp_NET461.Cache
{
    public class CacheHelperTest
    {
        private ICacheHelper Helper;
        public CacheHelperTest(ICacheHelper helper)
        {
            Helper = helper;
        }

        public void Test()
        {
            var key = $"testKey_{Helper.DbIndex}_{Helper.GetType()}";
            var value = $"testValue_{Helper.DbIndex}_{Helper.GetType()}";

            Helper.SetValue(key, value);
            Console.WriteLine($"key:{key}\t{Helper.GetValue<string>(key)}");

            var dbIndex = 5;
            Helper = Helper.GetDatabase(dbIndex);
            key = $"testKey_{Helper.DbIndex}_{Helper.GetType()}";
            value = $"testValue_{Helper.DbIndex}_{Helper.GetType()}";
            Helper.SetValue(key, value);

            Console.WriteLine($"key:{key}\t{Helper.GetValue<string>(key)}");
            Console.WriteLine($"key:{key}\t{Helper.GetValue<string>(key)}");
        }
    }
}
