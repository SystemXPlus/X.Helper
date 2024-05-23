using ServiceStack.Redis;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Helper.Cache
{
    public static class Config
    {
        static Config()
        {
            //静态构造初始化配置

            ServiceStackRedisConfig = new RedisClientManagerConfig
            {
                MaxReadPoolSize = 100,
                MaxWritePoolSize = 100,
                AutoStart = true,
            };

            StackExchangeRedisConfig = new ConfigurationOptions
            {
                EndPoints = { $"{RedisServer}:{RedisPort}" },
                Password = RedisPassword, // 如果有密码
                AbortOnConnectFail = false,
                ConnectRetry = 3,
                SyncTimeout = 5000,
                AsyncTimeout = 5000,
                ConnectTimeout = 5000,
            };
        }

        #region 公共配置
        private static double _DefaultTimeout = 20;
        /// <summary>
        /// 默认过期时间（分钟）
        /// </summary>
        public static double DefaultTimeout
        {
            get
            {
                return _DefaultTimeout;
            }
            set
            {
                if (value < -1)
                    throw new ArgumentOutOfRangeException("DefaultTimeout", new Exception("不能设置小于-1的过期时间"));
                _DefaultTimeout = value;
            }
        }

        private static int _DefaultDatabaseIndex = 0;
        public static int DefaultDatabaseIndex
        {
            get
            {
                return _DefaultDatabaseIndex;
            }
            set
            {
                var index = 0;
                if (value >= 0 && value <= 15)
                    index = value;
                _DefaultDatabaseIndex = index;
            }
        }
        #endregion

        #region RedisServer连接配置

#if NET461
        private static readonly string RedisServerConfigName = "RedisServer";
        private static readonly string RedisPortConfigName = "RedisPort";
        private static readonly string RedisPasswordConfigName = "RedisPassword";
#endif
#if NETSTANDARD2_0_OR_GREATER || NET6_0_OR_GREATER
                private static readonly string  RedisServerConfigName = "Cache:RedisServer";
                private static readonly string  RedisPortConfigName = "Cache:RedisPort";
                private static readonly string  RedisPasswordConfigName = "Cache:RedisPassword";
#endif

        private static string _RedisServer;

        public static string RedisServer
        {
            get
            {
                if (string.IsNullOrEmpty(_RedisServer))
                {
                    var server = X.Helper.Common.ConfigHelper.GetAppsetting(RedisServerConfigName);
                    if (string.IsNullOrEmpty(server))
                    {
                        server = "localhost";
                    }
                    _RedisServer = server;
                }
                return _RedisServer;
            }
            set { _RedisServer = value; }
        }

        private static string _RedisPort;

        public static string RedisPort
        {
            get
            {
                if (string.IsNullOrEmpty(_RedisPort))
                {
                    var port = X.Helper.Common.ConfigHelper.GetAppsetting(RedisPortConfigName);
                    if (string.IsNullOrEmpty(port))
                    {
                        port = "6379";
                    }
                    _RedisPort = port;
                }
                return _RedisPort;
            }
            set { _RedisPort = value; }
        }

        private static string _RedisPassword = null;

        public static string RedisPassword
        {
            get
            {
                if (null == _RedisPassword)
                {
                    var password = X.Helper.Common.ConfigHelper.GetAppsetting(RedisPasswordConfigName);
                    if (string.IsNullOrEmpty(password))
                        password = string.Empty;
                    _RedisPassword = password;
                }
                return _RedisPassword;
            }
            set { _RedisPassword = value; }
        }

        #endregion

        #region ServiceStack.Redis

        public static readonly RedisClientManagerConfig ServiceStackRedisConfig;

        #endregion

        #region StackExchange.Redis
        public static readonly ConfigurationOptions StackExchangeRedisConfig;
        #endregion
    }
}
