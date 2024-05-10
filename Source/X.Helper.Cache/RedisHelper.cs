using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;

namespace X.Helper.Cache
{
    // Install-Package ServiceStack -ProjectName xxx -Version 3.9.71

    public class RedisHelper
    {
        private static string RedisServer = "localhost";
        private static int RedisPort = 6379;
        private static string RedisPassword = string.Empty;
        private static PooledRedisClientManager RedisClientPool = null;

        static RedisHelper()
        {
            #region 从CONFIG文件读取连接参数
            try
            {
                RedisServer = System.Configuration.ConfigurationManager.AppSettings["RedisServer"];
                if (string.IsNullOrEmpty(RedisServer))
                    RedisServer = "localhost";
                int.TryParse(System.Configuration.ConfigurationManager.AppSettings["RedisPort"], out RedisPort);
                if (RedisPort == 0)
                    RedisPort = 6379;
                RedisPassword = System.Configuration.ConfigurationManager.AppSettings["RedisPassword"];
                if (string.IsNullOrEmpty(RedisPassword))
                    RedisPassword = string.Empty;
            }
            catch { throw new Exception("REDIS连接参数配置失败"); }
            #endregion

            CreateRedisClientManager();
        }

        private static void CreateRedisClientManager()
        {
            #region 连接池
            RedisClientManagerConfig config = new RedisClientManagerConfig
            {
                MaxReadPoolSize = 100,
                MaxWritePoolSize = 100
            };
            RedisClientPool = new PooledRedisClientManager(new string[] { string.Format("{0}{1}{2}:{3}", RedisPassword, string.IsNullOrEmpty(RedisPassword) ? "" : "@", RedisServer, RedisPort) },
                new string[] { string.Format("{0}{1}{2}:{3}", RedisPassword, string.IsNullOrEmpty(RedisPassword) ? "" : "@", RedisServer, RedisPort) },
                config);

            #endregion
        }

        /// <summary>
        /// 默认超时时间（分钟）
        /// </summary>
        private const double TIMEOUT = 20;

        private static object LockObject = new object();

        private static RedisClient client { get; set; }

        /// <summary>
        /// 存储客户端
        /// </summary>
        private static RedisClient NewClient
        {
            get
            {
                if (RedisClientPool == null)
                {
                    lock (LockObject)
                    {
                        if (RedisClientPool == null)
                            CreateRedisClientManager();
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
        private static long ExistsKey(RedisClient client, string key)
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
        /// <param name="minutes">超时时间</param>
        /// <returns>是否成功</returns>
        private static bool SetValue<T>(RedisClient client, string key, T obj, double minutes = TIMEOUT) where T : class
        {
            return client.Set<T>(key, obj, DateTime.Now.AddMinutes(minutes));
        }


        /// <summary>
        /// 读取内容
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="client">客户端</param>
        /// <param name="key">存储键</param>
        /// <returns>内容</returns>
        private static T GetValue<T>(RedisClient client, string key) where T : class
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
        /// <param name="minutes">自动延期（分钟） 该值大于0时若当时过期时间剩余不足设置定一半时自动延期</param>
        /// <returns>内容</returns>
        private static T GetValue<T>(RedisClient client, string key, double minutes = TIMEOUT) where T : class
        {
            if (ExistsKey(client, key) < 1)
            {
                return null;
            }
            if (minutes > 0)
            {
                //自动延期
                var ttl = client.Ttl(key);
                if (ttl > 0 && ttl < (minutes / 2) * 60)    //TTL>0时才执行自动延期。 -1 为永不过期 -2为KEY不存在
                {
                    client.ExpireEntryAt(key, DateTime.Now.AddMinutes(minutes));
                }
            }
            return client.Get<T>(key);
        }

        /// <summary>
        /// 键是否存在
        /// </summary>
        /// <param name="key">存储键</param>
        /// <returns>键内容数量</returns>
        public static bool ExistsKey(string key)
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
        public static bool RemoveKey(string key)
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

        /// <summary>
        /// 写入内容
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">存储键</param>
        /// <param name="obj">存储值</param>
        /// <param name="minutes">超时时间</param>
        /// <returns>是否成功</returns>
        public static bool SetValue<T>(string key, T obj, double minutes = TIMEOUT) where T : class
        {
            using (var client = NewClient)
            {
                return SetValue<T>(client, key, obj, minutes);
            }
        }
        /// <summary>
        /// 批量写入
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="dic">字典（键,实体）</param>
        public static void SetValue<T>(IDictionary<string, T> dic) where T : class
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
        public static T GetValue<T>(string key) where T : class
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
        public static T GetValue<T>(string key, double timeout) where T : class
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
        /// <param name="minutes">超时时间</param>
        /// <param name="AUTO_RENEW">自动续期？该值为TRUE时若在剩余过期时间不足一半时读取数据则自动续期</param>
        /// <returns>内容</returns>
        public static T GetValue<T>(string key, Func<T> func, double minutes = TIMEOUT, bool AUTO_RENEW = false) where T : class
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
                            SetValue<T>(client, key, obj, minutes);
                        }
                    }

                    return obj;
                }
                else
                {
                    if (AUTO_RENEW)
                    {
                        return GetValue<T>(client, key, minutes);
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
        public static void Enqueue(string queueId, string value)
        {
            using (var client = NewClient)
            {
                Enqueue(client, queueId, value);
            }
        }

        public static void Enqueue<T>(string queueId, string key, T obj, double minutes = TIMEOUT) where T : class
        {
            using (var client = NewClient)
            {
                SetValue<T>(client, key, obj, minutes);
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
        /// <param name="minutes"></param>
        public static void Enqueue<T>(string queueId, string key, Func<T> func, double minutes = TIMEOUT) where T : class
        {
            var c = GetValue<T>(key, func, minutes);
            if (c != null)
                using (var client = NewClient)
                {
                    Enqueue(client, queueId, key);
                    //client.ExpireEntryAt(key, DateTime.Now.AddMinutes(minutes));
                }
        }

        /// <summary>
        /// 出列
        /// </summary>
        /// <param name="queueId">队列ID</param>
        /// <returns></returns>
        public static string Dequeue(string queueId)
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
        public static T Dequeue<T>(string queueId) where T : class
        {
            using (var client = NewClient)
            {
                string key = Dequeue(client, queueId);
                if (!string.IsNullOrEmpty(key))
                    return GetValue<T>(client, key);
                return null;
            }
        }

        public static List<string> GetAllItemFromQueue(string queueId)
        {
            using (var client = NewClient)
            {
                return client.GetAllItemsFromList(queueId);
            }
        }

        public static void RemoveAllItemFromQueue(string queueId)
        {
            using (var client = NewClient)
            {
                RemoveAllFromQueue(client, queueId);
            }
        }

        public static string DequeueEnqueue(string queueId)
        {
            using (var client = NewClient)
            {
                return Encoding.Default.GetString(client.RPopLPush(queueId, queueId));
            }
        }

        public static string DequeueEnqueue(string fromQueueId, string toQueueId)
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
        public static void PushStack(string stackId, string value)
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
        public static string PopStack(string stackId)
        {
            using (var client = NewClient)
            {
                return PopStack(client, stackId);
            }
        }

        public static T PopStack<T>(string stackId) where T : class
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

