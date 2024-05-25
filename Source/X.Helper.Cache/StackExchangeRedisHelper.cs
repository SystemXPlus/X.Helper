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


        public bool SetValue<T>(string key, T obj, double? timeout = null) where T : class
        {
            TimeSpan? expiry = null ;
            if (timeout != null)
                expiry = TimeSpan.FromMinutes(timeout.Value);
            if (obj is string || obj.GetType().IsValueType)
            {
                return Database.StringSet(key, obj?.ToString(), expiry);
            }
            else
            {
                var str = X.Helper.Json.Serialize(obj);
                return Database.StringSet(key, str, expiry);
            }
        }

        public void SetValue<T>(IDictionary<string, T> dic) where T : class
        {
            if (dic == null || dic.Count == 0)
                return;
            var tmpdic = new Dictionary<RedisKey, RedisValue>();
            int i = 0;
            foreach(var item in dic)
            {
                string key = item.Key;
                string value;
                if (item.Value is string || item.Value.GetType().IsValueType)
                {
                    value = item.Value.ToString();
                }
                else
                {
                    value = X.Helper.Json.Serialize(item.Value);
                }
                tmpdic.Add(key, value);
                i++;
            }
            Database.StringSet(tmpdic.ToArray());
        }


        public T GetValue<T>(string key) where T : class
        {
            throw new NotImplementedException();
        }

        public T GetValue<T>(string key, double timeout) where T : class
        {
            throw new NotImplementedException();
        }

        public T GetValue<T>(string key, Func<T> func, double? timeout = null, bool autoRenew = false) where T : class
        {
            throw new NotImplementedException();
        }

        public bool RemoveKey(string key)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
