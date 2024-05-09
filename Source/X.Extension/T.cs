using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class T
{
    /// <summary>
    /// 创建一个指定类型的空的LIST
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    public static List<T> CreateEmptyList<T>(this T t) where T : class
    {
        return new List<T>();
    }


}
