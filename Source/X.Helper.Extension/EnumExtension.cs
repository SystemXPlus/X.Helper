using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

public static class EnumExtension
{
    /// <summary>
    /// 返回指定枚举值使用Display特性(Name="name")配置的Name
    /// </summary>
    /// <param name="eum"></param>
    /// <param name="returnObjectNameWhenNotDisplayName"></param>
    /// <returns></returns>
    public static string GetDisplayName(this System.Enum eum, bool returnObjectNameWhenNotDisplayName = false)
    {
        var type = eum.GetType();
        var field = type.GetField(eum.ToString());
        var objs = field.GetCustomAttributes(typeof(DisplayAttribute), false);
        if (null == objs || objs.Length.Equals(0))
            return returnObjectNameWhenNotDisplayName ? eum.ToString() : string.Empty;
        return ((DisplayAttribute)objs[0]).Name;
    }

    /// <summary>
    /// 返回指定枚举值使用Display特性(Description="des")配置的Description
    /// </summary>
    /// <param name="eum"></param>
    /// <param name="returnObjectNameWhenNotDisplayName"></param>
    /// <returns></returns>
    public static string GetDisplayDescription(this System.Enum eum, bool returnObjectNameWhenNotDisplayName = false)
    {
        var type = eum.GetType();
        var field = type.GetField(eum.ToString());
        var objs = field.GetCustomAttributes(typeof(DisplayAttribute), false);
        if (null == objs || objs.Length.Equals(0))
            return returnObjectNameWhenNotDisplayName ? eum.ToString() : string.Empty;
        return ((DisplayAttribute)objs[0]).Description;
    }

    /// <summary>
    /// 返回指定枚举值使用特性Description("des")配置的Description
    /// </summary>
    /// <param name="eum"></param>
    /// <param name="returnObjectNameWhenNotDescription"></param>
    /// <returns></returns>
    public static string GetDescription(this System.Enum eum, bool returnObjectNameWhenNotDescription = false)
    {
        var type = eum.GetType();
        var field = type.GetField(eum.ToString());
        var objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (null == objs || objs.Length.Equals(0))
            return returnObjectNameWhenNotDescription ? eum.ToString() : string.Empty;
        return ((DescriptionAttribute)objs[0]).Description;
    }
    /// <summary>
    /// 返回与字符串值相对应的枚举值
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <param name="itemName">字符串</param>
    /// <param name="ignoreCase">是否区分大小写</param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public static T GetEnumTypeFromString<T>(this string itemName, bool ignoreCase = false, bool returnDefaultValueOnFailed = false) where T : struct, IConvertible
    {
        if (!typeof(T).IsEnum)
            throw new ArgumentException("T must be an enumerated type");
        if (Enum.TryParse(itemName, ignoreCase, out T result))
            return result;
        if(returnDefaultValueOnFailed)
            return default(T);
        else
            throw new KeyNotFoundException(itemName);
    }
}

