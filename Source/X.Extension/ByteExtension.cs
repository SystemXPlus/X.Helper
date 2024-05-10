using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public static class ByteExtension
{
    /// <summary>
    /// 字节数组转换为Image对象
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static System.Drawing.Image ToImage(this byte[] bytes)
    {
        MemoryStream ms = new MemoryStream(bytes);
        System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
        return image;
    }

    /// <summary>
    /// Convert Byte[] to a picture and Store it in file
    /// 这个方法问题挺多。ELSE IF 要改成SWITCH CASE。另外扩展名应该在文件路径中就已经传过来了。这里是否有必要重新更改扩展名？
    /// </summary>
    /// <param name="fileFullPath"></param>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static void ToFile(this byte[] bytes,string fileFullPath)
    {
        string file = fileFullPath;
        System.Drawing.Image image = bytes.ToImage();
        ImageFormat format = image.RawFormat;
        if (format.Equals(ImageFormat.Jpeg))
        {
            file += ".jpeg";
        }
        else if (format.Equals(ImageFormat.Png))
        {
            file += ".png";
        }
        else if (format.Equals(ImageFormat.Bmp))
        {
            file += ".bmp";
        }
        else if (format.Equals(ImageFormat.Gif))
        {
            file += ".gif";
        }
        else if (format.Equals(ImageFormat.Icon))
        {
            //file += ".icon";
        }
        //System.IO.FileInfo info = new System.IO.FileInfo(file);
        //System.IO.Directory.CreateDirectory(info.Directory.FullName);
        System.IO.File.WriteAllBytes(file, bytes);
        //return file;
    }

    /// <summary>  
    /// 将字节数组转换成Object对象  
    /// </summary>  
    /// <param name="buff">被转换byte数组</param>  
    /// <returns>转换完成后的对象</returns>  
    public static object ToObject(this byte[] bytes)
    {
        object obj;
        using (MemoryStream ms = new MemoryStream(bytes))
        {
            IFormatter iFormatter = new BinaryFormatter();
            obj = iFormatter.Deserialize(ms);
        }
        return obj;
    }

    /// <summary>
    /// 字节数组转换为Bitmap对象
    /// </summary>
    /// <param name="Bytes"></param>
    /// <returns></returns>
    public static Bitmap ToBitmap(this byte[] Bytes)
    {
        MemoryStream stream = null;
        try
        {
            stream = new MemoryStream(Bytes);
            return new Bitmap(stream);
        }
        catch (ArgumentNullException ex)
        {
            throw ex;
        }
        catch (ArgumentException ex)
        {
            throw ex;
        }
        finally
        {
            stream.Close();
        }
    }

    /// <summary>
    /// 字节数组转换为Stream对象
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static System.IO.Stream BytesToStream(this byte[] bytes)
    {
        System.IO.Stream stream = new MemoryStream(bytes);
        return stream;
    }

}

