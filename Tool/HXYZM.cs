using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool
{
    public class HXYZM
    {
        public static Point getYZM(Bitmap bmp)
        {
            Point p = Point.Empty;
            new ImgData(bmp).filter();
            return p;
        }


    }


    class ImgData
    {
        private Bitmap Bmp;
        public ImgData(Bitmap bmp)
        {
            Bmp = bmp;
        }

        public void filter()
        {
            // for(int i=0;i<bmp)
        }

        public void saveBig(int si)
        {
            Bitmap bt = new Bitmap(Bmp.Width * si, Bmp.Height * si);
            for (int y = 0; y < Bmp.Height; y++)
            {
                for (int x = 0; x < Bmp.Width; x++)
                {

                }
            }
        }
    }
}
