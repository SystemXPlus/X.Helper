using ServiceStack.Script;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace X.Helper.Cache
{
    /// <summary>
    /// 缓存HELPER类接口
    /// </summary>
    public interface ICacheHelper
    {
        long DbIndex{get;}
        /// <summary>
        /// 设置要使用的数据库索引
        /// </summary>
        /// <param name="index">索引号：0-15（默认0）</param>
        /// <returns></returns>
        ICacheHelper GetDatabase(long? index = null);

        /// <summary>
        /// 查询指定KEY是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool ExistsKey(string key);
        /// <summary>
        /// 移除指定KEY
        /// </summary>
        /// <param name="key">KEY NAME, Supports wildcard:*</param>
        /// <returns></returns>
        bool RemoveKey(string key);
        /// <summary>
        /// 设置指定KEY的对象
        /// </summary>
        /// <typeparam name="T">typeof cache object</typeparam>
        /// <param name="key">KEY</param>
        /// <param name="obj">缓存对象</param>
        /// <param name="timeout">缓存超时时间</param>
        /// <returns></returns>
        bool SetValue<T>(string key, T obj, double? timeout = null) where T : class;
        /// <summary>
        /// 批量写入
        /// </summary>
        /// <typeparam name="T">typeof cache object</typeparam>
        /// <param name="dic">KEY和缓存对象的字典</param>
        /// <returns></returns>
        void SetValue<T>(IDictionary<string, T> dic) where T : class;

        T GetValue<T>(string key) where T : class;

        T GetValue<T>(string key, double timeout) where T : class;

        T GetValue<T>(string key, Func<T> func, double? timeout = null, bool autoRenew = false) where T : class;
    }
}
