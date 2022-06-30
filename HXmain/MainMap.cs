using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain
{
    public class MainMap
    {
        private Bitmap BMP;
        public MainMap(Bitmap bmp)
        {
            BMP = bmp;
        }

        public Bitmap getImg(Rectangle rec)
        {
            try
            {
                return BMP.Clone(rec, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);
            }
            catch (Exception)
            {

            }
            return (Bitmap)BMP.Clone();
        }
    }
}
