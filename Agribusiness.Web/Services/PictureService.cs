using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace Agribusiness.Web.Services
{
    public class PictureService : IPictureService
    {
        public byte[] Crop(byte[] img, int x, int y, int height, int width)
        {
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

        public byte[] Resize(byte[] img, int height, int width)
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
            return ms.GetBuffer();
        }

        public byte[] MakeThumbnail(byte[] img)
        {
            return Resize(img, 100, 80);
        }
    }
}