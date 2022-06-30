using HXmain.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace HXmain.HXInfo
{
    class HXDieAutoLive
    {
        public static HSearchPoint hasDie(Bitmap bmp)
        {
            return ImageTool.findLikeImg(Resources.die_ok_btn, bmp, 70);
        }
    }
}
