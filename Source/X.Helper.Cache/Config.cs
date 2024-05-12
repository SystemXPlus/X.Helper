using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Helper.Cache
{
    public static class Config
    {
        public static double Timeout { get; set; } = 20;

        private static int _DefaultDatabase = 0;
        public static int DefaultDatabase
        {
            get
            {
                return _DefaultDatabase;
            }
            set
            {
                var index = 0;
                if (value >= 0 && value <= 15)
                    index = value;
                _DefaultDatabase = index;
            }
        }

        private static string _RedisServer;

        public static string RedisServer
        {
            get
            {
                if (string.IsNullOrEmpty(_RedisServer))
                {
                    var server = System.Configuration.ConfigurationManager.AppSettings["RedisServer"];
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
                    var port = System.Configuration.ConfigurationManager.AppSettings["RedisPort"];
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
                    var password = System.Configuration.ConfigurationManager.AppSettings["RedisPassword"];
                    if (string.IsNullOrEmpty(password))
                        password = string.Empty;
                    _RedisPassword = password;
                }
                return _RedisPassword;
            }
            set { _RedisPassword = value; }
        }


        public static int MaxReadPoolSize { get; set; } = 100;
        public static int MaxWritePoolSize { get; set; } = 100;
        public static bool AutoStart { get; set; } = true;
    }
}
