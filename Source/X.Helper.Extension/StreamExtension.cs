using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

[Obsolete("该类未经过严格测试，请谨慎使用", false)]
public static class StreamExtension
    {
    /// <summary>
    /// Stream对象转换为字节数组
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public static byte[] ToByteArray(this System.IO.Stream stream)
    {
        byte[] bytes = new byte[stream.Length];
        stream.Read(bytes, 0, bytes.Length);

        // 设置当前流的位置为流的开始  
        stream.Seek(0, SeekOrigin.Begin);
        return bytes;
    }

    /// <summary>
    /// Stream对象保存到文件
    /// </summary>
    /// <param name="stream">Stream对象</param>
    /// <param name="filePath">文件完整路径</param>
    public static void ToFile(this System.IO.Stream stream, string filePath)
    {
        // 把 Stream 转换成 byte[]  
        byte[] bytes = new byte[stream.Length];
        stream.Read(bytes, 0, bytes.Length);
        // 设置当前流的位置为流的开始  
        stream.Seek(0, SeekOrigin.Begin);

        // 把 byte[] 写入文件  
        using (FileStream fs = new FileStream(filePath, FileMode.Create))
        {
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                bw.Write(bytes);
            }
        }
    }
    /// <summary>
    /// 网络返回的Stream转为本地MemoryStream
    /// </summary>
    /// <param name="stream"></param>
    public static System.IO.Stream ToMemoryStream(this System.IO.Stream stream)
    {
        MemoryStream memoryStream = new MemoryStream();

        //将基础流写入内存流
        const int bufferLength = 1024;
        byte[] buffer = new byte[bufferLength];
        int actual = stream.Read(buffer, 0, bufferLength);
        while (actual > 0)
        {
            // 读、写过程中，流的位置会自动走。
            memoryStream.Write(buffer, 0, actual);
            actual = stream.Read(buffer, 0, bufferLength);
        }
        memoryStream.Position = 0;

        return memoryStream;
    }
}
