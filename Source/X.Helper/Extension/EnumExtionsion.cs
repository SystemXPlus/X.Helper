﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

public static class EnumExtionsion
{
    /// <summary>
    /// 返回指定枚举值使用Display(Name="name")配置的DISPLAYNAME
    /// </summary>
    /// <param name="eum"></param>
    /// <param name="returnObjectNameWhenNotDisplayName"></param>
    /// <returns></returns>
    public static string GetDisplayName(this Enum eum, bool returnObjectNameWhenNotDisplayName = false)
    {
        var type = eum.GetType(); 
        var field = type.GetField(eum.ToString()); 
        var objs = field.GetCustomAttributes(typeof(DisplayAttribute), false);
        if (null == objs || objs.Length.Equals(0))
            return returnObjectNameWhenNotDisplayName ? eum.ToString() : string.Empty;
        return ((DisplayAttribute)objs[0]).Name;
    }

    /// <summary>
    /// 返回指定枚举值使用Display(Name="name")配置的DISPLAYNAME
    /// </summary>
    /// <param name="eum"></param>
    /// <param name="returnObjectNameWhenNotDescription"></param>
    /// <returns></returns>
    public static string GetDescription(this Enum eum, bool returnObjectNameWhenNotDescription = false)
    {
        var type = eum.GetType();
        var field = type.GetField(eum.ToString());
        var objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (null == objs || objs.Length.Equals(0))
            return returnObjectNameWhenNotDescription ? eum.ToString() : string.Empty;
        return ((DescriptionAttribute)objs[0]).Description;
    }
}

