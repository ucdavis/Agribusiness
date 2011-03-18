using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agribusiness.Web
{
    public interface IPictureService
    {
        byte[] Crop(byte[] img, int x, int y, int width, int height);
        byte[] Resize(byte[] img, int width, int height);

        byte[] MakeMainProfile(byte[] img);
        byte[] MakeThumbnail(byte[] img);
    }
}
