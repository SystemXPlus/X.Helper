
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Helper.Common
{
    public static partial class Utility
    {


        #region UTC
        /// <summary>
        /// 获取当前时间戳（UTC时间 毫秒）
        /// </summary>
        /// <returns></returns>
        public static long GetTimestamp_UTC_Milliseconds()
        {
            DateTime now = DateTime.UtcNow;
            return GetTimestamp_UTC_Milliseconds(now);
        }

        /// <summary>
        /// 获取时间戳（UTC时间 毫秒）
        /// </summary>
        /// <returns></returns>
        public static long GetTimestamp_UTC_Milliseconds(DateTime dateTime)
        {
            DateTime now = dateTime.ToUniversalTime();
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = now - epoch;
            long timestamp = (long)timeSpan.TotalMilliseconds;
            return timestamp;
        }

        /// <summary>
        /// 获取当前时间戳（UTC时间 秒）
        /// </summary>
        /// <returns></returns>
        public static long GetTimestamp_UTC_seconds()
        {
            DateTime now = DateTime.UtcNow;
            return GetTimestamp_UTC_seconds(now);
        }
        /// <summary>
        /// 获取时间戳（UTC时间 秒）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long GetTimestamp_UTC_seconds(DateTime dateTime)
        {
            DateTime now = dateTime.ToUniversalTime();
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = now - epoch;
            long timestamp = (long)timeSpan.TotalSeconds;
            return timestamp;
        } 
        #endregion
    }
}
