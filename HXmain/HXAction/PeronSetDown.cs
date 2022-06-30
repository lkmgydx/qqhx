using HFrameWork.SystemInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tool;
using WSTools.WSLog;

namespace HXmain.HXAction
{
    public class PeronSetDown
    {
        public static Rectangle rec = new Rectangle(615, 782, 15, 15);
        public static bool isSetdown(MainGame gm)
        {
            using (Bitmap bmp = gm.getImg(rec))
            {
                return isSetdown(bmp);
            }
        }

        /// <summary>
        /// 是否是坐下状态
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static bool isSetdown(Bitmap bmp)
        {
            using (Bitmap bt = Properties.Resources.p_setDown1)
            {
                HSearchPoint hs = ImageTool.findEqImg(bmp, bt);
                return hs.Success;
            }
        }

        /// <summary>
        /// 打坐
        /// </summary>
        /// <param name="mainGame"></param>
        internal static void SetDown(MainGame mainGame)
        {
            if (!isSetdown(mainGame))
            {
                mainGame.Active();
                KeyBoard.sleep(100);
                KeyBoard.down_upCtrl(Keys.Z);
            }
        }
    }
}
