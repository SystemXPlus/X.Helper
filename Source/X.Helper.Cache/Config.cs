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

        public static int MaxReadPoolSize { get; set; } = 100;
        public static int MaxWritePoolSize { get; set; } = 100;
        public static bool AutoStart { get; set; } = true;
    }
}
