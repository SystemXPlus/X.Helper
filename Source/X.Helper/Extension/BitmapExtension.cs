using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

public static class BitmapExtension
{
    public static byte[] ToByteArray(this Bitmap bmp)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            bmp.Save(ms, bmp.RawFormat);
            byte[] byteImage = new Byte[ms.Length];
            byteImage = ms.ToArray();
            return byteImage;
        }
    }
}

