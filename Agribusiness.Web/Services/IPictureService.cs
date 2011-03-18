using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agribusiness.Web
{
    public interface IPictureService
    {
        byte[] Crop(byte[] img, int x, int y, int height, int width);
        byte[] Resize(byte[] img, int height, int width);

        byte[] MakeThumbnail(byte[] img);
    }
}
