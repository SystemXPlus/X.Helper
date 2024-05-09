using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

public static class Image
{

    [Obsolete("方法有缺陷，现在修改为内部实际调用Object的ToByteArray扩展方法", false)]
    public static byte[] ToByteArray(this System.Drawing.Image image)
    {
        return ((object)image).ToByteArray();
        //ImageFormat format = image.RawFormat;
        //using (MemoryStream ms = new MemoryStream())
        //{
        //    if (format.Equals(ImageFormat.Jpeg))
        //    {
        //        image.Save(ms, ImageFormat.Jpeg);
        //    }
        //    else if (format.Equals(ImageFormat.Png))
        //    {
        //        image.Save(ms, ImageFormat.Png);
        //    }
        //    else if (format.Equals(ImageFormat.Bmp))
        //    {
        //        image.Save(ms, ImageFormat.Bmp);
        //    }
        //    else if (format.Equals(ImageFormat.Gif))
        //    {
        //        image.Save(ms, ImageFormat.Gif);
        //    }
        //    else if (format.Equals(ImageFormat.Icon))
        //    {
        //        image.Save(ms, ImageFormat.Icon);
        //    }
        //    byte[] buffer = new byte[ms.Length];
        //    //Image.Save()会改变MemoryStream的Position，需要重新Seek到Begin
        //    ms.Seek(0, SeekOrigin.Begin);
        //    ms.Read(buffer, 0, buffer.Length);
        //    return buffer;
        //}
    }

    public static string ToBase64(this System.Drawing.Image image)
    {
        return Convert.ToBase64String(image.ToByteArray());
    }

    /// <summary>
    /// 从文件加载图像IMAGE对象
    /// </summary>
    /// <param name="fileInfo"></param>
    /// <returns></returns>
    public static System.Drawing.Image ImageFromFile(this FileInfo fileInfo)
    {
        System.Drawing.Image image = null;
        using (FileStream fs = System.IO.File.OpenRead(fileInfo.FullName))
        {
            int filelength = 0;
            filelength = (int)fs.Length;
            System.Byte[] bytes = new System.Byte[filelength];
            fs.Read(bytes, 0, filelength);
            image = System.Drawing.Image.FromStream(fs);
        }
        return image;
    }

    #region 图像裁剪

    /// <summary>
    /// 直接生成缩略图
    /// </summary>
    /// <param name="source"></param>
    /// <param name="width">缩略图宽度</param>
    /// <param name="height">缩略图高度</param>
    /// <param name="quality">图像质量（JPEG压缩）</param>
    /// <param name="type">缩略图裁剪类型</param>
    /// <returns></returns>
    public static System.Drawing.Image GetThumbnail(this System.Drawing.Image source, int width, int height, int quality = 80, CutType type = CutType.CutTopAutoZoom)
    {
        if (source == null)
            return null;
        int customWidth, customHeight, left, top;
        SetCutSize(source, type, ref width, ref height, out customWidth, out customHeight, out left, out top);
        return CreateThumbnail(source, customWidth, customHeight, left, top, width, height, quality);
    }

    /// <summary>
    /// 可根据等比缩放后的图像选定区域生成缩略图
    /// </summary>
    /// <param name="source"></param>
    /// <param name="zoomWidth">等比缩放后宽度（只考虑等比缩放尺寸）</param>
    /// <param name="areaLeft">选定区域左边距</param>
    /// <param name="areaTop">选定区域顶边距</param>
    /// <param name="areaWidth">选定区域宽度</param>
    /// <param name="areaHeight">选定区域高度</param>
    /// <param name="destinationWidth">生成缩略图宽度</param>
    /// <param name="destinationHeight">生成缩略图宽度</param>
    /// <param name="quality">图像质量（JPEG压缩）</param>
    /// <returns></returns>
    public static System.Drawing.Image GetThumbnail(this System.Drawing.Image source, int zoomWidth, int areaLeft, int areaTop, int areaWidth, int areaHeight, int destinationWidth, int destinationHeight, int quality = 80)
    {
        if (source == null)
            return null;
        int sourceWidth = source.Width, sourceHeight = source.Height;
        int cutLeft, cutTop, cutWidth, cutHeight;
        if (zoomWidth == sourceWidth)
        {
            cutLeft = areaLeft; cutTop = areaTop;
            cutWidth = areaWidth; cutHeight = areaHeight;
        }
        else
        {
            double scale = (double)sourceWidth / (double)zoomWidth;
            cutLeft = (int)(scale * (double)areaLeft);
            cutTop = (int)(scale * (double)areaTop);
            cutWidth = (int)(scale * (double)areaWidth);
            cutHeight = (int)(scale * (double)areaHeight);
        }
        //将选定区域复制出来
        Bitmap bmpSource = new Bitmap(source);
        Bitmap bmpResult = bmpSource.Clone(new Rectangle(cutLeft, cutTop, cutWidth, cutHeight), bmpSource.PixelFormat);

        return bmpResult.GetThumbnail(destinationWidth, destinationHeight, quality);
    }

    /// <summary>
    /// 生成缩略图保存到指定路径
    /// </summary>
    /// <param name="sourcePath"></param>
    /// <param name="savePath">缩略图保存路径</param>
    /// <param name="width">缩略图宽度</param>
    /// <param name="height">缩略图高度</param>
    /// <param name="quality">图像质量（JPEG压缩）</param>
    /// <param name="type">缩略图裁剪类型</param>
    public static void GetThumbnail(this string sourcePath, string savePath, int width, int height, int quality = 80, CutType type = CutType.CutCenterAutoZoom)
    {
        if (!System.IO.File.Exists(sourcePath))
            throw new FileNotFoundException("文件不存在");
        using (FileStream fs = new FileStream(sourcePath, FileMode.Open))
        {
            System.Drawing.Image source = System.Drawing.Image.FromStream(fs);
            var thumb = source.GetThumbnail(width, height, quality, type);
            thumb.Save(savePath);
        }
    }

    /// <summary>
    /// 根据缩放尺寸及设置的裁剪区域生成指定大小缩略图并保存到指定路径
    /// </summary>
    /// <param name="sourcePath"></param>
    /// <param name="savePath">缩略图保存路径</param>
    /// <param name="zoomWidth">等比缩放后宽度（程序只考虑等比缩放尺寸）</param>
    /// <param name="areaLeft">选定区域左边距</param>
    /// <param name="areaTop">选定区域顶边距</param>
    /// <param name="areaWidth">选定区域宽度</param>
    /// <param name="areaHeight">选定区域高度</param>
    /// <param name="destinationWidth">生成缩略图宽度</param>
    /// <param name="destinationHeight">生成缩略图宽度</param>
    /// <param name="quality">图像质量（JPEG压缩）</param>
    public static void GetThumbnail(this string sourcePath, string savePath, int zoomWidth, int areaLeft, int areaTop, int areaWidth, int areaHeight, int destinationWidth, int destinationHeight, int quality = 80)
    {
        if (!System.IO.File.Exists(sourcePath))
            throw new FileNotFoundException("文件不存在");
        using (FileStream fs = new FileStream(sourcePath, FileMode.Open))
        {
            System.Drawing.Image source = System.Drawing.Image.FromStream(fs);
            var thumb = source.GetThumbnail(zoomWidth, areaLeft, areaTop, areaWidth, areaHeight, destinationWidth, destinationHeight, quality);
            thumb.Save(savePath);
        }
    }

    #region 裁剪参数枚举
    /// <summary>
    /// 图片裁剪方式
    /// </summary>
    public enum CutType
    {
        /// <summary>
        /// 按裁剪框宽度等比缩放，若缩放后图像高度小于裁剪框高度，自动将裁剪框高度缩小至与图像高度相等
        /// </summary>
        ZoomByWidth,
        /// <summary>
        /// 按裁剪框高度等比缩放，若缩放后图像宽度小于裁剪框宽度，自动将裁剪框宽度缩小至与图像宽度相等
        /// </summary>
        ZoomByHeight,
        /// <summary>
        /// 按裁剪框大小缩放（如果源图像宽高比与裁剪框宽高比不一致会造成变形）
        /// </summary>
        ZoomByCustom,
        /// <summary>
        /// 根据裁剪框大小裁剪左上角部分，若图像高度或宽度小于裁剪框高度或宽度，图像会被放大
        /// </summary>
        CutLeftTop,
        /// <summary>
        /// 根据裁剪框大小裁剪左下角部分，若图像高度或宽度小于裁剪框高度或宽度，图像会被放大
        /// </summary>
        CutLeftBottom,
        /// <summary>
        /// 根据裁剪框大小裁剪右上角部分，若图像高度或宽度小于裁剪框高度或宽度，图像会被放大
        /// </summary>
        CutRightTop,
        /// <summary>
        /// 根据裁剪框大小裁剪右下角部分，若图像高度或宽度小于裁剪框高度或宽度，图像会被放大
        /// </summary>
        CutRightBottom,
        /// <summary>
        /// 根据裁剪框大小裁剪中间部分，若图像高度或宽度小于裁剪框高度或宽度，图像会被放大；反之不缩放源图像裁剪出源图像中间部分
        /// </summary>
        CutCenter,
        /// <summary>
        /// 根据裁剪框大小将原图等比缩放至对应大小后裁剪中间部分，根据源图像长宽比确定基于裁剪框高度或宽度自动缩放
        /// </summary>
        CutCenterAutoZoom,
        /// <summary>
        /// 根据裁剪框大小将原图等比绽放至对应大小后裁剪顶部，根据源图像长宽比确定基于裁剪框高度或宽度自动缩放
        /// </summary>
        CutTopAutoZoom
    }
    #endregion




    #region 根据裁剪类型计算裁减尺寸

    /// <summary>
    /// 根据裁剪类型设置源图像缩放大小或裁剪框大小以及裁剪框位置
    /// </summary>
    /// <param name="sourceImagePath">原始图像路径</param>
    /// <param name="cutType">裁剪类型</param>
    /// <param name="width">裁剪框宽度</param>
    /// <param name="height">裁剪框高度</param>
    /// <param name="customWidth">源图像缩放宽度</param>
    /// <param name="customHeight">源图像缩放高度</param>
    /// <param name="left">裁剪框左边距</param>
    /// <param name="top">裁前框顶边距</param>
    private static void SetCutSize(System.Drawing.Image source, CutType cutType, ref int width, ref int height, out int customWidth, out int customHeight, out int left, out int top)
    {
        //if (!File.Exists(sourceImagePath))
        //{
        //    customWidth = 0;
        //    customHeight = 0;
        //    left = 0; top = 0;
        //    return;
        //}
        ////加载图像文件
        //Image sourceImage = Image.FromStream(new MemoryStream(File.ReadAllBytes(sourceImagePath)));
        System.Drawing.Image sourceImage = source;
        //获取源文件尺寸
        int sourceWidth = sourceImage.Width;
        int sourceHeight = sourceImage.Height;
        //初始化缩放后图像尺寸
        int cutWidth = 0;
        int cutHeight = 0;
        //初始化裁剪框位置
        int cutLeft = 0;
        int cutTop = 0;
        //根据裁剪类计算源图像缩放大小及裁剪区域
        switch (cutType)
        {
            case CutType.ZoomByWidth:
                cutWidth = width;
                cutHeight = (int)(sourceHeight * ((double)cutWidth / (double)sourceWidth));
                //若缩放后高度小于参数指定的裁剪高度。将裁剪高度设置为当前缩放后高度。防止生成图像出现多余部分
                if (cutHeight < height)
                    height = cutHeight;
                cutLeft = 0;
                cutTop = (cutHeight - height) / 2;
                break;
            case CutType.ZoomByHeight:
                cutHeight = height;
                cutWidth = (int)(sourceWidth * ((double)cutHeight / (double)sourceHeight));
                //若缩放后宽度小于参数指定的裁剪宽度。将裁剪宽度设置为当前缩放后宽度。防止生成图像出现多余部分
                if (cutWidth < width)
                    width = cutWidth;
                cutLeft = (cutWidth - width) / 2;
                cutTop = 0;
                break;
            case CutType.ZoomByCustom:
                cutWidth = width;
                cutHeight = height;
                cutLeft = 0;
                cutTop = 0;
                break;
            case CutType.CutLeftTop:
                SetZoomSize(sourceWidth, sourceHeight, width, height, ref cutWidth, ref cutHeight);
                cutLeft = 0;
                cutTop = 0;
                break;
            case CutType.CutLeftBottom:
                SetZoomSize(sourceWidth, sourceHeight, width, height, ref cutWidth, ref cutHeight);
                cutLeft = 0;
                cutTop = cutHeight - height;
                break;
            case CutType.CutRightTop:
                SetZoomSize(sourceWidth, sourceHeight, width, height, ref cutWidth, ref cutHeight);
                cutLeft = cutWidth - width;
                cutTop = 0;
                break;
            case CutType.CutRightBottom:
                SetZoomSize(sourceWidth, sourceHeight, width, height, ref cutWidth, ref cutHeight);
                cutLeft = cutWidth - width;
                cutTop = cutHeight - height;
                break;
            case CutType.CutCenter:
                SetZoomSize(sourceWidth, sourceHeight, width, height, ref cutWidth, ref cutHeight);
                cutLeft = (cutWidth - width) / 2;
                cutTop = (cutHeight - height) / 2;
                break;
            case CutType.CutCenterAutoZoom:
                //源图宽度或宽度小于裁剪框宽度或高度时将源图进行缩放处理
                //20160324  这里要不要调整一下。目前如果图像尺寸比例不一致的情况下会被裁剪掉一部分
                //如果调整过来的庆碰到尺寸比例不一致的情况会有部分留白。
                //if ((double)sourceWidth / (double)sourceHeight > (double)width / (double)height)
                //{
                //    //源图宽高比大于要裁剪的宽高比时。将源图尺寸以要裁剪的高度为准等比缩放
                //    cutHeight = height;
                //    cutWidth = (int)(sourceWidth * ((double)cutHeight / (double)sourceHeight));
                //}
                //else
                //{
                //    //源图宽高比小于要裁剪的宽高比时。将源图尺寸以裁剪框的宽度为准等比缩放
                //    cutWidth = width;
                //    cutHeight = (int)(sourceHeight * ((double)cutWidth / (double)sourceWidth));

                //}
                if ((double)sourceWidth / (double)sourceHeight > (double)width / (double)height)
                {
                    //源图宽高比大于要裁剪的宽高比时。将源图尺寸以要裁剪的高度为准等比缩放
                    cutWidth = width;
                    cutHeight = (int)(sourceHeight * ((double)cutWidth / (double)sourceWidth));
                }
                else
                {
                    //源图宽高比小于要裁剪的宽高比时。将源图尺寸以裁剪框的宽度为准等比缩放
                    cutHeight = height;
                    cutWidth = (int)(sourceWidth * ((double)cutHeight / (double)sourceHeight));


                }
                cutLeft = (cutWidth - width) / 2;
                cutTop = (cutHeight - height) / 2;
                break;
            case CutType.CutTopAutoZoom:
                //源图宽度或宽度小于裁剪框宽度或高度时将源图进行缩放处理
                if ((double)sourceWidth / (double)sourceHeight > (double)width / (double)height)
                {
                    //源图宽高比大于要裁剪的宽高比时。将源图尺寸以要裁剪的高度为准等比缩放
                    cutHeight = height;
                    cutWidth = (int)(sourceWidth * ((double)cutHeight / (double)sourceHeight));
                }
                else
                {
                    //源图宽高比小于要裁剪的宽高比时。将源图尺寸以裁剪框的宽度为准等比缩放
                    cutWidth = width;
                    cutHeight = (int)(sourceHeight * ((double)cutWidth / (double)sourceWidth));
                }
                cutLeft = 0;
                cutTop = 0;
                break;
        }
        customWidth = cutWidth;
        customHeight = cutHeight;
        left = cutLeft;
        top = cutTop;
    }

    /// <summary>
    /// 根据源图像大小和裁剪框大小设置图像缩放尺寸
    /// </summary>
    /// <param name="sourceWidth">源图像宽度</param>
    /// <param name="sourceHeight">源图像高度</param>
    /// <param name="width">裁剪框宽度</param>
    /// <param name="height">裁剪框高度</param>
    /// <param name="cutWidth">缩放后宽度</param>
    /// <param name="cutHeight">缩放后高度</param>
    private static void SetZoomSize(int sourceWidth, int sourceHeight, int width, int height, ref int cutWidth, ref int cutHeight)
    {
        if (sourceWidth < width || sourceHeight < height)
        {
            //源图宽度或宽度小于裁剪框宽度或高度时将源图进行缩放处理
            if ((double)sourceWidth / (double)sourceHeight > (double)width / (double)height)
            {
                //源图宽高比大于要裁剪的宽高比时。将源图尺寸以要裁剪的高度为准等比缩放
                cutHeight = height;
                cutWidth = (int)(sourceWidth * ((double)cutHeight / (double)sourceHeight));
            }
            else
            {
                //源图宽高比小于要裁剪的宽高比时。将源图尺寸以裁剪框的宽度为准等比缩放
                cutWidth = width;
                cutHeight = (int)(sourceHeight * ((double)cutWidth / (double)sourceWidth));
            }
        }
        else
        {
            cutWidth = sourceWidth; ;
            cutHeight = sourceHeight; ;
        }
    }


    #endregion

    /// <summary>
    /// 生成缩略图
    /// </summary>
    /// <param name="sourceImagePath">源始图片绝对路径</param>
    /// <param name="thumbnailImagePath">缩略图保存绝对路径</param>
    /// <param name="zoomWidth">裁剪之前将源图像缩放的宽度</param>
    /// <param name="zoomHeight">裁剪之前将源图像绽放的高度</param>
    /// <param name="left">裁剪区域左边距</param>
    /// <param name="top">裁剪区域右边距</param>
    /// <param name="width">裁剪区域宽度</param>
    /// <param name="height">裁剪区域高度</param>
    /// <param name="quality">图像质量（0-100　数值越大图像质量越高，默认50）</param>
    /// <returns>缩略图保存绝对路径</returns>
    private static System.Drawing.Image CreateThumbnail(System.Drawing.Image source, int zoomWidth, int zoomHeight, int left, int top, int width, int height, int quality = 50)
    {


        if (quality < 1)
            quality = 1;
        if (quality > 100)
            quality = 100;
        //加载图像文件
        //Image sourceImage = Image.FromStream(new MemoryStream(File.ReadAllBytes(sourceImagePath)));
        System.Drawing.Image sourceImage = source;
        //获取图像原始高度
        int sourceWidth = sourceImage.Width, sourceHeight = sourceImage.Height;
        if (!((zoomWidth == sourceWidth) && (zoomHeight == sourceHeight)))
        {
            sourceImage = sourceImage.GetThumbnailImage(zoomWidth, zoomHeight, null, IntPtr.Zero);
        }
        //复制出指定区域指定大小的图像
        using (Bitmap bmp = new Bitmap(sourceImage))
        {

            using (Bitmap bmp1 = new Bitmap(width, height, bmp.PixelFormat))
            {

                //throw new Exception(string.Format("left:{0} top:{1} width:{2} height:{3} sourceWidth:{4} sourceHeight:{5} zoomWidth:{6} zoomHeight:{7}", left, top, width, height,sourceWidth,sourceHeight,zoomWidth,zoomHeight));

                Bitmap bmp2 = new Bitmap(width, height, bmp.PixelFormat);


                Graphics g2 = Graphics.FromImage(bmp2);
                g2.FillRectangle(Brushes.White, new Rectangle(0, 0, width, height));


                if (left >= 0 && top >= 0)
                {
                    //throw new Exception(string.Format("left:{0} top:{1} width:{2} height:{3} sourceWidth:{4} sourceHeight:{5} zoomWidth:{6} zoomHeight:{7}", left, top, width, height, sourceWidth, sourceHeight, zoomWidth, zoomHeight));
                    bmp2 = bmp.Clone(new Rectangle(left, top, width, height), bmp.PixelFormat);
                }
                else
                {
                    //缩放后图像高宽小于要裁剪的目标高度时会出现裁剪区域负数造成内存不足异常）
                    int fillLeft = left > 0 ? left : 0 - left;
                    int fillTop = top > 0 ? top : 0 - top;

                    for (int x = 1; x < zoomWidth; x++)
                    {
                        for (int y = 1; y < zoomHeight; y++)
                        {
                            //throw new Exception(string.Format("x:{0} y:{1} fleft:{2} ftop:{3} width:{4} height:{5}", x, y, fillLeft, fillTop,bmp.Width,bmp.Height));
                            bmp2.SetPixel(x + fillLeft, y + fillTop, bmp.GetPixel(x, y));
                        }
                    }
                }


                //return bmp2 as Image;

                //如果缩略图文件已经存在则先删除文件
                //if (File.Exists(thumbnailImagePath))
                //    File.Delete(thumbnailImagePath);
                //设置缩略图图像质量
                List<System.Drawing.Imaging.Encoder> encoderList = new List<System.Drawing.Imaging.Encoder>();
                encoderList.Add(System.Drawing.Imaging.Encoder.Quality);
                List<EncoderParameter> encoderParameter = new List<EncoderParameter>();
                for (int i = 0; i < encoderList.Count; i++)
                {
                    encoderParameter.Add(new EncoderParameter(encoderList[i], (long)quality));
                }
                EncoderParameters encoderParameters = new EncoderParameters(encoderList.Count);
                encoderParameters.Param = encoderParameter.ToArray();
                //设置编码器
                ImageCodecInfo[] imageCodeInfoArray = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegImageCodecInfo = null;
                for (int i = 0; i < imageCodeInfoArray.Length; i++)
                {
                    if (imageCodeInfoArray[i].FormatDescription.Equals("JPEG"))
                    {
                        jpegImageCodecInfo = imageCodeInfoArray[i];
                        break;
                    }
                }
                MemoryStream ms = new MemoryStream();
                bmp2.Save(ms, jpegImageCodecInfo, encoderParameters);
                return System.Drawing.Image.FromStream(ms);
                //sourceImage.Dispose();
                //bmp2.Dispose();
            }
        }
    }


    #endregion

    #region 水印

    #region   /*************************************文字水印*************************************/

    /// <summary>
    /// 生成文字水印返回IMAGE对象
    /// </summary>
    /// <param name="text">水印文字文本</param>
    /// <param name="font">字体</param>
    /// <param name="brush">笔刷</param>
    /// <param name="point">水印位置</param>
    public static System.Drawing.Image WatermarkText(this System.Drawing.Image image, string text, Font font, Brush brush, Point point)
    {
        Bitmap bmp = new Bitmap(image);
        Graphics g = Graphics.FromImage(bmp);
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
        g.DrawString(text
            , font
            , brush
            , point);
        return bmp;
    }

    /// <summary>
    /// 生成文字水印保存到指定路径
    /// </summary>
    /// <param name="saveImagePath">保存图片保存</param>
    /// <param name="text">水印文字文本</param>
    /// <param name="font">字体</param>
    /// <param name="brush">笔刷</param>
    /// <param name="point">水印位置</param>
    public static void WatermarkText(this System.Drawing.Image image, string saveImagePath, string text, Font font, Brush brush, Point point)
    {
        Bitmap bmp = new Bitmap(image);
        Graphics g = Graphics.FromImage(bmp);
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
        g.DrawString(text
            , font
            , brush
            , point);
        bmp.Save(saveImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
    }


    /// <summary>
    /// 生成文字水印
    /// </summary>
    /// <param name="sourceImagePath">原始图片路径</param>
    /// <param name="saveImagePath">保存图片保存</param>
    /// <param name="text">水印文字文本</param>
    /// <param name="font">字体</param>
    /// <param name="brush">笔刷</param>
    /// <param name="point">水印位置</param>
    private static void WatermarkText(string sourceImagePath, string saveImagePath, string text, Font font, Brush brush, Point point)
    {
        System.Drawing.Image image = null;
        using (FileStream fs = System.IO.File.OpenRead(sourceImagePath))
        {
            int filelength = 0;
            filelength = (int)fs.Length;
            System.Byte[] bytes = new System.Byte[filelength];
            fs.Read(bytes, 0, filelength);
            image = System.Drawing.Image.FromStream(fs);
        }
        Bitmap bmp = new Bitmap(image);
        Graphics g = Graphics.FromImage(bmp);
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
        g.DrawString(text
            , font
            , brush
            , point);
        bmp.Save(saveImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
    }
    #endregion


    #region /*************************************图片水印*************************************/

    /// <summary>
    /// 生成图像水印并保存到指定路径 
    /// </summary>
    /// <param name="image"></param>
    /// <param name="watermarkImage"></param>
    /// <param name="saveImagePath"></param>
    /// <param name="point"></param>
    /// <param name="quality">0-100 100最高质量 默认值100</param>
    /// <param name="transparency">0.1- 1 1为不透明 默认值1</param>
    public static void WatermarkImage(this System.Drawing.Image image, System.Drawing.Image watermarkImage
        , string saveImagePath
        , Point point
        , int quality = 100
        , float transparency = 1
        )
    {

        Graphics g = Graphics.FromImage(image);
        //设置高质量插值法
        //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
        //设置高质量,低速度呈现平滑程度
        //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        System.Drawing.Image watermark = watermarkImage;

        if (watermark.Height >= image.Height || watermark.Width >= image.Width)
            return;

        ImageAttributes imageAttributes = new ImageAttributes();
        ColorMap colorMap = new ColorMap();

        colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
        colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
        ColorMap[] remapTable = { colorMap };

        imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

        float transparencyDefault = 1F;
        if (transparency >= 0.1 && transparency <= 1)
            transparencyDefault = transparency;


        float[][] colorMatrixElements = {
                                                new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  0.0f,  0.0f,  transparencyDefault, 0.0f},
                                                new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                            };

        ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

        imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);



        g.DrawImage(watermark, new Rectangle(point.X, point.Y, watermark.Width, watermark.Height), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);

        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
        ImageCodecInfo ici = null;
        foreach (ImageCodecInfo codec in codecs)
        {
            if (codec.MimeType.IndexOf("jpeg") > -1)
                ici = codec;
        }
        EncoderParameters encoderParams = new EncoderParameters();
        long[] qualityParam = new long[1];
        if (quality < 0 || quality > 100)
            quality = 80;

        qualityParam[0] = quality;

        EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
        encoderParams.Param[0] = encoderParam;

        if (ici != null)
            image.Save(saveImagePath, ici, encoderParams);
        else
            image.Save(saveImagePath);

        g.Dispose();

        watermark.Dispose();
        imageAttributes.Dispose();
    }
    #endregion


    #endregion


}



