using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;
using WSTools.WSLog;

namespace HXmain.HXInfo
{
    public class HXZuDuics
    {
        private static Rectangle rec = new Rectangle(770, 782, 15, 15);

        public static bool hasZuDuiInfo(MainGame gm)
        {
            using (Bitmap bmp = gm.getImg(rec))
            {
                return hasZuDuiInfo(bmp);
            }
        }

        /// <summary>
        /// 是否有组队信息
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static bool hasZuDuiInfo(Bitmap bmp)
        {
            using (Bitmap bt = Properties.Resources.zudui)
            {
                HSearchPoint hs = ImageTool.findEqImg(bmp, bt);
                return !hs.Success;
            }
        }
    }
}
