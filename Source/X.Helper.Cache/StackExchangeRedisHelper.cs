using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace X.Helper.Cache
{
    public class StackExchangeRedisHelper : ICacheHelper
    {
        private static readonly object LockInstanceObject = new object();

        private static readonly object LockObject = new object();
        /// <summary>
        /// 默认超时时间（分钟）
        /// </summary>
        private static readonly double TIMEOUT = 20;

        private static readonly Dictionary<double, StackExchangeRedisHelper> DbCache = new Dictionary<double, StackExchangeRedisHelper>();

        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(Config.StackExchangeRedisConfig);
        });

        private static ConnectionMultiplexer connection => lazyConnection.Value;

        private IDatabase Database;

        #region 构造
        static StackExchangeRedisHelper()
        {
            TIMEOUT = Config.DefaultTimeout;
        }

        public StackExchangeRedisHelper() : this(Config.DefaultDatabaseIndex) { }
        public StackExchangeRedisHelper(long dbIndex)
        {
            var index = dbIndex;
            Database = connection.GetDatabase((int)index);
            _DbIndex = index;
            if (!DbCache.ContainsKey(index))
            {
                lock (LockInstanceObject)
                {
                    if (!DbCache.ContainsKey(index))
                    {
                        DbCache[dbIndex] = this;
                    }
                }
            }
        }
        #endregion

        #region PRIVATE

        private TimeSpan GetTimeSpan(double? timeout)
        {
            TimeSpan expiry;
            if (timeout == null)
                timeout = TIMEOUT;
            expiry = TimeSpan.FromMinutes(timeout.Value);
            return expiry;
        }

        private void SetExpiry(string key, double timeout)
        {
            Database.KeyExpire(key, GetTimeSpan(timeout));
        }

        #endregion

        #region Inherit from ICacheHelper
        private long _DbIndex = 0;
        public long DbIndex
        {
            get { return _DbIndex; }
        }

        public ICacheHelper GetDatabase(long? index = null)
        {
            var dbIndex = index ?? Config.DefaultDatabaseIndex;
            if (!DbCache.ContainsKey(dbIndex))
            {
                lock (LockInstanceObject)
                {
                    if (!DbCache.ContainsKey(dbIndex))
                    {
                        Database = connection.GetDatabase((int)dbIndex);
                        _DbIndex = dbIndex;
                        DbCache[dbIndex] = this;
                    }
                }
            }
            return DbCache[dbIndex];
        }

        public bool ExistsKey(string key)
        {
            return Database.KeyExists(key);
        }

        /*
         * 
         * 因为之前使用ServiceStack.Redis直接使用的Set<T>将所有类型序列化后写入
         * 导致之前存入的string、decimal及值类型数据都带有引号
         * 如果要兼容之前的数据则要使用和之前一样的方法将所有类型数据序列化后再StringSet
         * 实际测试两种方法性能相差较小，一般项目可以忽略
         * 
         * */


        public bool SetValue<T>(string key, T obj, double? timeout = null) where T : class
        {
            var str = X.Helper.Json.Serialize(obj);
            return Database.StringSet(key, str, GetTimeSpan(timeout));
        }

        //public bool SetValue<T>(string key, T obj, double? timeout = null) where T : class
        //{
        //    TimeSpan? expiry = null;
        //    if (timeout == null)
        //        timeout = TIMEOUT;
        //    expiry = TimeSpan.FromMinutes(timeout.Value);
        //    if (obj is string || obj is decimal || obj.GetType().IsValueType)
        //    {
        //        var result = Database.StringSet(key, obj?.ToString(), expiry);
        //        return result;
        //    }
        //    else
        //    {
        //        var str = X.Helper.Json.Serialize(obj);
        //        return Database.StringSet(key, str, expiry);
        //    }
        //}


        public void SetValue<T>(IDictionary<string, T> dic) where T : class
        {
            if (dic == null || dic.Count == 0)
                return;
            var tmpdic = new Dictionary<RedisKey, RedisValue>();
            int i = 0;
            foreach (var item in dic)
            {
                string key = item.Key;
                string value = X.Helper.Json.Serialize(item.Value);
                tmpdic.Add(key, value);
                i++;
            }
            Database.StringSet(tmpdic.ToArray());
        }

        //public void SetValue<T>(IDictionary<string, T> dic) where T : class
        //{
        //    if (dic == null || dic.Count == 0)
        //        return;
        //    var tmpdic = new Dictionary<RedisKey, RedisValue>();
        //    int i = 0;
        //    foreach (var item in dic)
        //    {
        //        string key = item.Key;
        //        string value;
        //        if (item.Value is string || item.Value.GetType().IsValueType)
        //        {
        //            value = item.Value.ToString();
        //        }
        //        else
        //        {
        //            value = X.Helper.Json.Serialize(item.Value);
        //        }
        //        tmpdic.Add(key, value);
        //        i++;
        //    }
        //    Database.StringSet(tmpdic.ToArray());
        //}

        public T GetValue<T>(string key) where T : class
        {
            string value = Database.StringGet(key);
            if (string.IsNullOrEmpty(value))
                return default(T);
            return X.Helper.Json.Deserialize<T>(value);
        }

        //public T GetValue<T>(string key) where T : class
        //{
        //    string value = Database.StringGet(key);
        //    if (string.IsNullOrEmpty(value))
        //        return default(T);
        //    if (typeof(T) == typeof(string))
        //    {
        //        return (T)(object)value;
        //    }
        //    else if (typeof(T).IsValueType || typeof(T) == typeof(decimal))
        //    {
        //        var targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
        //        return (T)Convert.ChangeType(value, targetType);
        //    }
        //    else
        //    {
        //        return X.Helper.Json.Deserialize<T>(value);
        //    }
        //}

        public T GetValue<T>(string key, double timeout) where T : class
        {
            var result = GetValue<T>(key);
            Database.KeyExpire(key, GetTimeSpan(timeout));
            return result;
        }

        public T GetValue<T>(string key, Func<T> func, double? timeout = null, bool autoRenew = false) where T : class
        {
            if (!ExistsKey(key))
            {
                T obj = null;
                if (func != null)
                {
                    obj = func();
                    if (obj != null)
                    {
                        SetValue<T>(key, obj, timeout);
                    }
                }
                return obj;
            }
            else
            {
                var result = GetValue<T>(key);
                if (autoRenew && timeout!=null)
                    SetExpiry(key, (double)timeout);
                return result;
            }

        }

        public bool RemoveKey(string key)
        {
            return Database.KeyDelete(key);
        }

        #endregion
    }
}
