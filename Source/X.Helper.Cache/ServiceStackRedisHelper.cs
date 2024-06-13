using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServiceStack.Redis;

namespace X.Helper.Cache
{
    // Install-Package ServiceStack -ProjectName xxx -Version 3.9.71

    public class ServiceStackRedisHelper : ICacheHelper
    {

        private PooledRedisClientManager RedisClientPool = null;
        private static readonly List<string> RedisServers = new List<string>();
        private static readonly List<string> RedisPorts = new List<string>();
        private static readonly List<string> RedisPassowrd = new List<string>();
        private static readonly Dictionary<double, ServiceStackRedisHelper> DbCache = new Dictionary<double, ServiceStackRedisHelper>();



        private static readonly object LockInstanceObject = new object();

        private static readonly object LockObject = new object();

        /// <summary>
        /// 默认超时时间（分钟）
        /// </summary>
        private static readonly double TIMEOUT = 20;

        #region 构造
        static ServiceStackRedisHelper()
        {
            #region 从CONFIG文件读取连接参数
            try
            {
                //RedisServer = X.Helper.Common.ConfigHelper.GetAppsetting(Config.RedisServer);
                //if (string.IsNullOrEmpty(RedisServer))
                //    RedisServer = "localhost";
                //RedisPort = X.Helper.Common.ConfigHelper.GetAppsetting(Config.RedisPort);
                //if (string.IsNullOrEmpty(RedisPort))
                //    RedisPort = "6379";
                //RedisPassword = X.Helper.Common.ConfigHelper.GetAppsetting(Config.RedisPassword);
                //if (string.IsNullOrEmpty(RedisPassword))
                //    RedisPassword = string.Empty;

                TIMEOUT = Config.DefaultTimeout;

                /*
                 * TODO 准备支持 服务器集群 读写分离
                 * 多台服务器、端口、密码在配置文件中以;分隔
                 * 端口、密码与服务器一一对应，如未匹配上则使用默认值 Port="6379" Password=""
                 * 服务器默认可读写，只读服务器需以[R]为前缀
                 * */

            }
            catch (Exception ex)
            {
                throw new TypeInitializationException(typeof(ServiceStackRedisHelper).FullName, ex);
            }
            #endregion

            //CreateRedisClientManager();
        }
        public ServiceStackRedisHelper() : this(Config.DefaultDatabaseIndex) { }
        public ServiceStackRedisHelper(long dbIndex)
        {
            var index = dbIndex;
            RedisClientPool = CreateRedisClientManager(index);
            _DbIndex = index;
            if (!DbCache.ContainsKey(index))
            {
                lock (LockInstanceObject)
                {
                    if (!DbCache.ContainsKey(index))
                    {
                        DbCache[index] = this;
                    }
                }
            }
        } 
        #endregion

        #region PRIVATE

        private static PooledRedisClientManager CreateRedisClientManager(long dbIndex)
        {
            #region 连接池
            var redisClientPool = new PooledRedisClientManager(new string[] { string.Format("{0}{1}{2}:{3}", Config.RedisPassword, string.IsNullOrEmpty(Config.RedisPassword) ? "" : "@", Config.RedisServer, Config.RedisPort) },
                new string[] { string.Format("{0}{1}{2}:{3}", Config.RedisPassword, string.IsNullOrEmpty(Config.RedisPassword) ? "" : "@", Config.RedisServer, Config.RedisPort) },
                Config.ServiceStackRedisConfig,
                dbIndex,
                null,
                null);
            return redisClientPool;
            #endregion
        }



        //private static RedisClient client { get; set; }

        /// <summary>
        /// 存储客户端
        /// </summary>
        private RedisClient NewClient
        {
            get
            {
                if (RedisClientPool == null)
                {
                    lock (LockObject)
                    {
                        if (RedisClientPool == null)
                            CreateRedisClientManager(_DbIndex);
                    }
                }

                return RedisClientPool.GetClient() as RedisClient;


                //if (string.IsNullOrEmpty(RedisPassword))
                //{
                //    return new RedisClient(RedisServer, RedisPort);
                //}
                //else
                //{
                //    return new RedisClient(RedisServer, RedisPort, RedisPassword);
                //}
            }
        }

        /// <summary>
        /// 键是否存在
        /// </summary>
        /// <param name="client">客户端</param>
        /// <param name="key">存储键</param>
        /// <returns>键内容数量</returns>
        private long ExistsKey(RedisClient client, string key)
        {
            return client.Exists(key);
        }



        /// <summary>
        /// 写入内容
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="client">客户端</param>
        /// <param name="key">存储键</param>
        /// <param name="obj">存储值</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>是否成功</returns>
        private bool SetValue<T>(RedisClient client, string key, T obj, double? timeout = null) where T : class
        {
            return client.Set<T>(key, obj, DateTime.Now.AddMinutes(timeout ?? TIMEOUT));
        }


        /// <summary>
        /// 读取内容
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="client">客户端</param>
        /// <param name="key">存储键</param>
        /// <returns>内容</returns>
        private T GetValue<T>(RedisClient client, string key) where T : class
        {
            if (ExistsKey(client, key) < 1)
            {
                return null;
            }
            return client.Get<T>(key);
        }
        /// <summary>
        /// 读取内容
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="client">客户端</param>
        /// <param name="key">存储键</param>
        /// <param name="timeout">自动延期（分钟） 该值大于0时若当时过期时间剩余不足设置定一半时自动延期</param>
        /// <returns>内容</returns>
        private T GetValue<T>(RedisClient client, string key, double? timeout = 0) where T : class
        {
            if (ExistsKey(client, key) < 1)
            {
                return null;
            }
            if (timeout != null && timeout > 0)
            {
                //自动延期
                var ttl = client.Ttl(key);
                if (ttl > 0 && ttl < (timeout / 2) * 60)    //TTL>0时才执行自动延期。 -1 为永不过期 -2为KEY不存在
                {
                    client.ExpireEntryAt(key, DateTime.Now.AddMinutes(timeout ?? TIMEOUT));
                }
            }
            return client.Get<T>(key);
        }
        #endregion

        #region Inherit form ICacheHelper

        private long _DbIndex = Config.DefaultDatabaseIndex;

        public long DbIndex
        {
            get { return _DbIndex; }
        }

        public ICacheHelper GetDatabase(long? index = 0)
        {
            //返回包含指定indexDB对象的实例 
            var dbIndex = index ?? Config.DefaultDatabaseIndex;
            if (!DbCache.ContainsKey(dbIndex))
            {
                lock (LockInstanceObject)
                {
                    if (!DbCache.ContainsKey(dbIndex))
                    {
                        var instance = new ServiceStackRedisHelper();
                        instance.RedisClientPool = CreateRedisClientManager(dbIndex);
                        instance._DbIndex = dbIndex;
                        DbCache[dbIndex] = instance;
                    }
                }
            }
            return DbCache[dbIndex];
        }

        /// <summary>
        /// 键是否存在
        /// </summary>
        /// <param name="key">存储键</param>
        /// <returns>键内容数量</returns>
        public bool ExistsKey(string key)
        {
            using (var client = NewClient)
            {
                return ExistsKey(client, key) > 0;
            }
        }


        /// <summary>
        /// 移除键
        /// </summary>
        /// <param name="key">存储键</param>
        /// <returns>是否成功</returns>
        public bool RemoveKey(string key)
        {
            using (var client = NewClient)
            {
                if (ExistsKey(client, key) < 1)
                {
                    return false;
                }
                return client.Remove(key);
            }
        }


        public void SetExpiredTime(string key, long expireTime)
        {
            using (var client = NewClient)
            {
                client.ExpireEntryAt(key, DateTime.Now.AddMinutes(expireTime));
            }
        }

        /// <summary>
        /// 写入内容
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">存储键</param>
        /// <param name="obj">存储值</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>是否成功</returns>
        public bool SetValue<T>(string key, T obj, double? timeout = null) where T : class
        {
            using (var client = NewClient)
            {
                return SetValue<T>(client, key, obj, timeout);
            }
        }
        /// <summary>
        /// 批量写入
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="dic">字典（键,实体）</param>
        public void SetValue<T>(IDictionary<string, T> dic) where T : class
        {
            using (var client = NewClient)
            {
                client.SetAll<T>(dic);
            }
        }

        /// <summary>
        /// 读取内容
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">存储键</param>
        /// <returns>内容</returns>
        public T GetValue<T>(string key) where T : class
        {
            using (var client = NewClient)
            {
                if (ExistsKey(client, key) < 1)
                {
                    return null;
                }
                return GetValue<T>(client, key);
            }
        }
        /// <summary>
        /// 读取内容
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">存储键</param>
        /// <param name="timeout">若读取数据时剩余过期时间不足该值的一半则自动续期</param>
        /// <returns></returns>
        public T GetValue<T>(string key, double timeout) where T : class
        {
            using (var client = NewClient)
            {
                if (ExistsKey(client, key) < 1)
                {
                    return null;
                }
                return GetValue<T>(client, key, timeout);
            }
        }

        /// <summary>
        /// 读取内容
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">存储键</param>
        /// <param name="func">存储值委托</param>
        /// <param name="timeout">超时时间</param>
        /// <param name="autoRenew">自动续期？该值为TRUE时若在剩余过期时间不足一半时读取数据则自动续期</param>
        /// <returns>内容</returns>
        public T GetValue<T>(string key, Func<T> func, double? timeout = null, bool autoRenew = false) where T : class
        {
            using (var client = NewClient)
            {
                if (ExistsKey(client, key) < 1)
                {
                    T obj = null;

                    if (func != null)
                    {
                        obj = func();

                        if (obj != null)
                        {
                            SetValue<T>(client, key, obj, timeout);
                        }
                    }

                    return obj;
                }
                else
                {
                    if (autoRenew)
                    {
                        return GetValue<T>(client, key, timeout);
                    }
                    else
                    {
                        return GetValue<T>(client, key);
                    }
                };
            }
        }

        /// <summary>
        /// 删除所有匹配的KEY
        /// </summary>
        /// <param name="pattern"></param>
        public void RemoveByPattern(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                return;
            var temp = pattern.Replace("*", string.Empty)
                .Replace("?", string.Empty);
            if (string.IsNullOrEmpty(temp))
                return;
            using (var client = NewClient)
            {
                var list = client.SearchKeys(pattern);
                if (null != list)
                {
                    foreach (var item in list)
                    {
                        client.Remove(item);
                    }
                }
            }
        }

        #endregion


        #region 队列（FIFO）

        private static bool IsExistEnqueue(RedisClient client, string queueId, string value)
        {
            return false;
        }
        private static void Enqueue(RedisClient client, string queueId, string value)
        {
            client.EnqueueItemOnList(queueId, value);
        }

        private static string Dequeue(RedisClient client, string queueId)
        {
            return client.DequeueItemFromList(queueId);
        }

        private static void RemoveAllFromQueue(RedisClient client, string queueId)
        {
            client.RemoveAllFromList(queueId);
        }

        /// <summary>
        /// 入列
        /// </summary>
        /// <param name="queueId">队列ID</param>
        /// <param name="value">入列值（可以保存缓存对象的KEY）</param>
        public void Enqueue(string queueId, string value)
        {
            using (var client = NewClient)
            {
                Enqueue(client, queueId, value);
            }
        }

        public void Enqueue<T>(string queueId, string key, T obj, double? timeout = null) where T : class
        {
            using (var client = NewClient)
            {
                SetValue<T>(client, key, obj, timeout);
                Enqueue(client, queueId, key);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queueId"></param>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="timeout"></param>
        public void Enqueue<T>(string queueId, string key, Func<T> func, double? timeout = null) where T : class
        {
            var c = GetValue<T>(key, func, timeout);
            if (c != null)
                using (var client = NewClient)
                {
                    Enqueue(client, queueId, key);
                    //client.ExpireEntryAt(key, DateTime.Now.AddMinutes(timeout));
                }
        }

        /// <summary>
        /// 出列
        /// </summary>
        /// <param name="queueId">队列ID</param>
        /// <returns></returns>
        public string Dequeue(string queueId)
        {
            using (var client = NewClient)
            {
                return Dequeue(client, queueId);
            }
        }

        /// <summary>
        /// 出列
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="queueId">队列ID</param>
        /// <returns></returns>
        public T Dequeue<T>(string queueId) where T : class
        {
            using (var client = NewClient)
            {
                string key = Dequeue(client, queueId);
                if (!string.IsNullOrEmpty(key))
                    return GetValue<T>(client, key);
                return null;
            }
        }

        public List<string> GetAllItemFromQueue(string queueId)
        {
            using (var client = NewClient)
            {
                return client.GetAllItemsFromList(queueId);
            }
        }

        public void RemoveAllItemFromQueue(string queueId)
        {
            using (var client = NewClient)
            {
                RemoveAllFromQueue(client, queueId);
            }
        }

        public string DequeueEnqueue(string queueId)
        {
            using (var client = NewClient)
            {
                return Encoding.Default.GetString(client.RPopLPush(queueId, queueId));
            }
        }

        public string DequeueEnqueue(string fromQueueId, string toQueueId)
        {
            using (var client = NewClient)
            {
                return Encoding.Default.GetString(client.RPopLPush(fromQueueId, toQueueId));
            }
        }


        #endregion

        #region 栈（LIFO）

        private static void PushStack(RedisClient client, string stackId, string value)
        {
            client.PushItemToList(stackId, value);
        }

        private static string PopStack(RedisClient client, string stackId)
        {
            return client.PopItemFromList(stackId);
        }

        //  LPUSH/RPUSH LPOP/RPOP 

        /// <summary>
        /// 压栈
        /// </summary>
        /// <param name="stackId">栈ID</param>
        /// <param name="value"></param>
        public void PushStack(string stackId, string value)
        {
            using (var client = NewClient)
            {
                PushStack(client, stackId, value);
            }
        }
        /// <summary>
        /// 出栈
        /// </summary>
        /// <param name="stackId">栈ID</param>
        /// <returns></returns>
        public string PopStack(string stackId)
        {
            using (var client = NewClient)
            {
                return PopStack(client, stackId);
            }
        }

        public T PopStack<T>(string stackId) where T : class
        {
            using (var client = NewClient)
            {
                string key = PopStack(client, stackId);
                if (!string.IsNullOrEmpty(key))
                    return GetValue<T>(client, key);
                return null;
            }
        }


        #endregion
    }
}

