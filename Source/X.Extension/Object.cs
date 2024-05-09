using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

public static class Object
{

    #region 常用

    public static bool IsNull(this object obj)
    {
        return obj == null;
    } 

    public static bool IsNotNull(this object obj)
    {
        return obj != null;
    }

    #endregion

    public static byte[] ToByteArray(this object obj)
    {
        byte[] buff;
        using (MemoryStream ms = new MemoryStream())
        {
            IFormatter iFormatter = new BinaryFormatter();
            iFormatter.Serialize(ms, obj);
            buff = ms.GetBuffer();
        }
        return buff;
    }

    /// <summary>
    /// 获取OBJECT对象中属性为值类型或者字符类型的属性与属性值的键值对
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Dictionary<string, string> GetObjectPropertyValueList(this object obj)
    {
        try
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            Type t = obj.GetType();
            PropertyInfo[] info = t.GetProperties();
            foreach (PropertyInfo i in info)
            {
                if (i.PropertyType.IsValueType || i.PropertyType.Name.Equals("string", StringComparison.CurrentCultureIgnoreCase))
                {
                    dic.Add(i.Name, i.GetValue(obj, null).ToString());
                }
            }
            return dic;
        }
        catch
        {
            return new Dictionary<string, string>();
        }
    }

}

