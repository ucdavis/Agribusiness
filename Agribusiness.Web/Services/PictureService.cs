using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Agribusiness.Web.Services
{
    public class PictureService : IPictureService
    {
        public byte[] Crop(byte[] img, int x, int y, int width, int height)
        {
            //using (var OriginalImage = Image.FromStream(new MemoryStream(img)))
            //{
            //    using (var bmp = new Bitmap(width, height))
            //    {
            //        bmp.SetResolution(OriginalImage.HorizontalResolution, OriginalImage.VerticalResolution);

            //        using (var Graphic = Graphics.FromImage(bmp))
            //        {
            //            Graphic.SmoothingMode = SmoothingMode.AntiAlias;
            //            Graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //            Graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            //            Graphic.DrawImage(OriginalImage, new Rectangle(0, 0, width, height), x, y, width, height, GraphicsUnit.Pixel);

            //            MemoryStream ms = new MemoryStream();
            //            bmp.Save(ms, OriginalImage.RawFormat);
            //            return ms.GetBuffer();
            //        }
            //    }
            //}            

            var s = new MemoryStream(img);
            var origImg = Image.FromStream(s);

            var bmp = new Bitmap(width, height);
            bmp.SetResolution(origImg.HorizontalResolution, origImg.VerticalResolution);

            var graphic = Graphics.FromImage(bmp);
            graphic.SmoothingMode = SmoothingMode.AntiAlias;
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphic.DrawImage(origImg, new Rectangle(0, 0, width, height), x, y, width, height, GraphicsUnit.Pixel);

            var ms = new MemoryStream();
            bmp.Save(ms, origImg.RawFormat);
            return ms.GetBuffer();
        }

        public byte[] Resize(byte[] img, int width, int height)
        {
            var s = new MemoryStream(img);
            var origImg = Image.FromStream(s);

            var newImg = new Bitmap(width, height);

            var rectangle = new Rectangle(0, 0, width, height);

            var graphic = Graphics.FromImage(newImg);
            graphic.CompositingQuality = CompositingQuality.HighQuality;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.DrawImage(origImg, rectangle);

            var ms = new MemoryStream();
            newImg.Save(ms, origImg.RawFormat);

            graphic.Dispose();
            newImg.Dispose();
            origImg.Dispose();

            return ms.GetBuffer();
        }

        /// <summary>
        /// Resizes image for main profile picture
        /// </summary>
        /// <remarks>
        /// 148 width x 185 height
        /// </remarks>
        /// <param name="img"></param>
        /// <returns></returns>
        public byte[] MakeMainProfile(byte[] img)
        {
            return Resize(img, 148, 185);
        }

        /// <summary>
        /// Resizes image for thumbnail
        /// </summary>
        /// <remarks>
        /// 80 width x 100 height
        /// </remarks>
        /// <param name="img"></param>
        /// <returns></returns>
        public byte[] MakeThumbnail(byte[] img)
        {
            return Resize(img, 80, 100);
        }
    }
}