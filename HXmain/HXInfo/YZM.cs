using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain.HXInfo
{
    /// <summary>
    /// 验证码
    /// </summary>
    public class YZM
    {
        private static Rectangle YZM_REC = new Rectangle(448, 103, 100, 100);

        /// <summary>
        /// 获取验证图片
        /// </summary>
        /// <param name="gm"></param>
        /// <returns></returns>
        public static Bitmap getYZMBmp(MainGame gm)
        {
            if (gm == null)
            {
                return new Bitmap(1, 1);
            }
            return gm.getImg(YZM_REC);
        }
        public static bool hasYZM(MainGame gm)
        {
            if (gm == null) {
                return false;
            }
            using (Bitmap bmp = gm.getImg(YZM_REC))
            {
                return hasYZM(bmp);
            }
        }

        public static bool hasYZM(Bitmap bmp)
        {

            bool hasWhite = false;
            bool hasBlack = false;
            for (int i = 0; i < bmp.Width; i += 3)
                for (int j = 0; j < bmp.Height; j += 3)
                {
                    Color c = bmp.GetPixel(i, j);
                    if (c.Name == "ffffffff")
                    {
                        hasWhite = true;
                    }
                    else if (c.Name == "ff000000")
                    {
                        hasBlack = true;
                    }
                    else
                    {
                        return false;
                    }
                }
            return hasWhite && hasBlack;
        }
    }
}
