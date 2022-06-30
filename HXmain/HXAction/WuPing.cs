using HFrameWork.SystemInput;
using HXmain.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace HXmain.HXAction
{
    public class WuPing
    {
        public static WuPing EmptyWuPing = new WuPing(Resources.wuping_empty, "无");
        public string Name { get; private set; }
        public Bitmap Img { get; private set; }
        /// <summary>
        /// 物品数量
        /// </summary>
        public int count { get; set; }
        public WuPing(Bitmap bmp, string name)
        {
            Name = name;
            Img = bmp;
        }

        public static HSearchPoint openWuPing(MainGame game, bool lockPosition = true)
        {
            HSearchPoint hs = null;
            bool isMoved = !lockPosition;
            for (int i = 0; i < 5; i++)
            {
                hs = isWuping(game);
                if (hs.Success)
                {
                    if (lockPosition && !isMoved)
                    {
                        game.DrawPointToPoint(hs.CenterPoint, new Point(760, 300));
                        isMoved = true;
                        continue;
                    }
                    break;
                }
                else
                {
                    game.Active();
                    game.SendKey(System.Windows.Forms.Keys.B);
                    Mouse.sleep(200);
                }
            }
            return hs;
        }

        private static HSearchPoint isWuping(MainGame game)
        {
            using (Bitmap bmp = game.getImg())
            {
                return ImageTool.findLikeImg(Resources.win_wpl, bmp, 90);
            }
        }
    }
}
