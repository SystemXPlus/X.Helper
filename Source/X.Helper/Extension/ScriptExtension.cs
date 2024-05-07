using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using MSScriptControl;
using System.IO;

public static class ScriptExtension
{



    /// <summary>
    /// 执行指定文件中的JS代码中的方法。返回执行结果
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="functionName"></param>
    /// <param name="pams"></param>
    /// <returns></returns>
    //public static string ExecuteScript(this string filePath, string functionName, object[] pams)
    //{
    //    //MSSCriptControl.DLL需要更改设置。将“嵌入互操作类型”设置为False
    //    //读取文件时未处理编码，所以传入的文件需要UTF-8编码。
    //    //使用此方法时项目只能使用X86
    //    if (!File.Exists(filePath))
    //    { Console.WriteLine("file not found"); return string.Empty; }
    //    StreamReader reader = new StreamReader(filePath);
    //    string scriptCode = reader.ReadToEnd();
    //    ScriptControlClass sc = new ScriptControlClass();
    //    sc.Language = "javascript";
    //    sc.AddCode(scriptCode);
    //    object obj = sc.Run(functionName, ref pams);
    //    return obj.ToString();
    //}

    //public static string ExecuteScript(this FileInfo file,string functionName,object[] pams)
    //{
    //    return ExecuteScript(file.FullName, functionName, pams);
    //}





}

