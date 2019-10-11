using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace LeoTools.FileHelper
{
    /// <summary>
    /// 图片操作类
    /// </summary>
    /// <author> Write by 云创天成 QQ:2943075966 </author>
    public class ImageHelper
    {

   

        /// <summary>
        /// 生成500*600 和750*422的图片
        /// </summary>
        /// <param name="id">指定文件夹</param>
        //public static void LoadNewImgs(string directory)
        //{
        //    var img2 = directory + "430x430.jpg";
        //    //750-422  先生成750*750 再取中间值
        //    Bitmap bit = (Bitmap)Bitmap.FromFile(img2, false);

        //    //改图片大小
        //    //var bit750 = ImageHelper.KiResizeImage(bit, 750, 750);

        //    var bit750 = ZoomPicture(bit, 750, 750);
        //    GraphicsPath path750 = new GraphicsPath();
        //    Point[] p = {
        //                    new Point(0,164),
        //                    new Point(750,164),
        //                    new Point(750,586),
        //                    new Point(0,586)
        //                };
        //    path750.AddLines(p);
        //    Bitmap newBit = null;
        //    ImageHelper.BitmapCrop(bit750, path750, out newBit);
        //    newBit.Save(directory + "750x422.jpg");

        //    //500-600    先生成600*600 再取中间值
        //    var bit600 = ImageHelper.ZoomPicture(bit, 600, 600);
        //    GraphicsPath path600 = new GraphicsPath();
        //    Point[] p2 = {
        //                    new Point(50,0),
        //                    new Point(550,0),
        //                    new Point(550,600),
        //                    new Point(50,600)
        //                };
        //    path600.AddLines(p2);
        //    Bitmap newBit2 = null;
        //    ImageHelper.BitmapCrop(bit600, path600, out newBit2);
        //    newBit2.Save(directory+"500x600.jpg");
        //}


        /// <summary>
        /// 通过计算生成指定像素的图片  (要求图片宽高相等)
        /// </summary>
        /// <param name="id">指定文件夹</param>
        public static void LoadPxImgs(string dirName, string fileName, int outWidth, int outHeight)
        {
            //750-422  先生成750*750 再取中间值
            Bitmap bit = (Bitmap)Bitmap.FromFile(dirName + fileName, false);
            var zoompx = 0;
            if (outWidth > outHeight)
            {
                zoompx = outWidth;
            }
            else
            {
                zoompx = outHeight;
            }
            //像素等比例缩放 例如 450*450缩放为750*750
            var zoomImg = ZoomPicture(bit, zoompx, zoompx);
            GraphicsPath path = new GraphicsPath();  //通用剪切画笔算法
            Point[] p = {
                            new Point((zoompx-outWidth)/2,(zoompx-outHeight)/2),
                            new Point(outWidth+(zoompx-outWidth)/2,(zoompx-outHeight)/2),
                            new Point(outWidth+(zoompx-outWidth)/2,outHeight+(zoompx-outHeight)/2),
                            new Point((zoompx-outWidth)/2,outHeight+(zoompx-outHeight)/2)
                        };
            path.AddLines(p);
            Bitmap newBit = null;
            ImageHelper.BitmapCrop(zoomImg, path, out newBit);
            newBit.Save(dirName + outWidth.ToString() + "x" + outHeight.ToString() + ".jpg");
        }


        /// <summary>
        /// 图片裁剪
        /// </summary>
        /// <param name="bitmap">原图</param>
        /// <param name="path">裁剪路径</param>
        /// <param name="outputBitmap">输出图</param>
        /// <returns></returns>
        public static Bitmap BitmapCrop(Bitmap bitmap, GraphicsPath path, out Bitmap outputBitmap)
        {
            RectangleF rect = path.GetBounds();
            int left = (int)rect.Left;
            int top = (int)rect.Top;
            int width = (int)rect.Width;
            int height = (int)rect.Height;
            Bitmap image = (Bitmap)bitmap.Clone();
            outputBitmap = new Bitmap(width, height);
            for (int i = left; i < left + width; i++)
            {
                for (int j = top; j < top + height; j++)
                {
                    //判断坐标是否在路径中   
                    if (path.IsVisible(i, j))
                    {
                        //复制原图区域的像素到输出图片   
                        outputBitmap.SetPixel(i - left, j - top, image.GetPixel(i, j));
                        //设置原图这部分区域为透明   
                        image.SetPixel(i, j, Color.FromArgb(0, image.GetPixel(i, j)));
                    }
                    else
                    {
                        outputBitmap.SetPixel(i - left, j - top, Color.FromArgb(0, 255, 255, 255));
                    }
                }
            }
            bitmap.Dispose();
            return image;
        }

        /// <summary>
        /// 修改图片分辨率
        /// </summary>
        /// <param name="bmp">原始Bitmap</param>
        /// <param name="newW">新的宽度</param>
        /// <param name="newH">新的高度</param>
        /// <returns>处理以后的Bitmap</returns>
        public static Bitmap KiResizeImage(Bitmap bmp, int newW, int newH)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);

                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();

                return b;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 按比例缩放图片  
        /// </summary>
        /// <param name="SourceImage"></param>
        /// <param name="TargetWidth"></param>
        /// <param name="TargetHeight"></param>
        /// <returns></returns>
        public static Bitmap ZoomPicture(Image SourceImage, int TargetWidth, int TargetHeight)
        {
            int IntWidth; //新的图片宽  
            int IntHeight; //新的图片高  
            try
            {
                System.Drawing.Imaging.ImageFormat format = SourceImage.RawFormat;
                System.Drawing.Bitmap SaveImage = new System.Drawing.Bitmap(TargetWidth, TargetHeight);
                Graphics g = Graphics.FromImage(SaveImage);
                g.Clear(Color.White);

                //计算缩放图片的大小  

                if (SourceImage.Width > TargetWidth && SourceImage.Height <= TargetHeight)//宽度比目的图片宽度大，长度比目的图片长度小  
                {
                    IntWidth = TargetWidth;
                    IntHeight = (IntWidth * SourceImage.Height) / SourceImage.Width;
                }
                else if (SourceImage.Width <= TargetWidth && SourceImage.Height > TargetHeight)//宽度比目的图片宽度小，长度比目的图片长度大  
                {
                    IntHeight = TargetHeight;
                    IntWidth = (IntHeight * SourceImage.Width) / SourceImage.Height;
                }
                else if (SourceImage.Width <= TargetWidth && SourceImage.Height <= TargetHeight) //长宽比目的图片长宽都小  
                {
                    IntHeight = SourceImage.Width;
                    IntWidth = SourceImage.Height;
                }
                else//长宽比目的图片的长宽都大  
                {
                    IntWidth = TargetWidth;
                    IntHeight = (IntWidth * SourceImage.Height) / SourceImage.Width;
                    if (IntHeight > TargetHeight)//重新计算  
                    {
                        IntHeight = TargetHeight;
                        IntWidth = (IntHeight * SourceImage.Width) / SourceImage.Height;
                    }
                }
                g.DrawImage(SourceImage, (TargetWidth - IntWidth) / 2, (TargetHeight - IntHeight) / 2, IntWidth, IntHeight);
                SourceImage.Dispose();

                return SaveImage;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

    }
}
