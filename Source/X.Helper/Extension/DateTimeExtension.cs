using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public static class DateTimeExtension
{
    
    /// <summary>
    /// 格式化指定时间到现在的时间差
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string FormatToNow(this DateTime dt)
    {
        TimeSpan ts = DateTime.Now - dt;
        if (ts.Days > 7)
        {
            return dt.ToString("yyyy-MM-dd");
        }
        else if (ts.Days > 0)
        {
            return string.Format("{0}天前", ts.Days);
        }
        else if (ts.Hours > 0)
        {
            return string.Format("{0}小时前", ts.Hours);
        }
        else if (ts.Minutes > 0)
        {
            return string.Format("{0}分钟前", ts.Minutes);
        }
        else
        {
            return "刚刚";
        }
    }

    /// <summary>
    /// 格式化从现在开始到指定时间的时间差
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string FormatFromNow(this DateTime dt)
    {
        TimeSpan ts = dt - DateTime.Now;
        if (ts.Days>30)
        {
            return string.Format("{0}天后", ts.Days);
        }
        else if (ts.TotalDays > 28)
        {
            return "4周后";
        }
        else if (ts.TotalDays > 21)
        {
            return "3周后";
        }
        else if (ts.TotalDays > 14)
        {
            return "2周后";
        }else if (ts.TotalDays > 7)
        {
            return "1周后";
        }else if (ts.TotalDays > 1)
        {
            return string.Format("{0}天后", ts.Days);
        }else if(ts.Hours>1)
        {
            return string.Format("{0}小时后", ts.Hours);
        }else if (ts.Minutes > 1)
        {
            return string.Format("{0}分钟后", ts.Minutes);
        }else if (ts.Seconds > 1)
        {
            return string.Format("{0}秒后", ts.Seconds);
        }else
        {
            return "已结束";
        }
    }

}

