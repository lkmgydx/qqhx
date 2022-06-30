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
    public class 签到
    {
        /// <summary>
        /// 签到窗口 ,145,75
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        static HSearchPoint hasQDWin(MainGame game)
        {
            return ImageTool.findEqImg(Resources.win_每日签到, game.getImg());
        }
        public static void 开始签到(MainGame game)
        {
            if (game == null)
            {
                return;
            }
            Mouse.cacheLocation();
            HSearchPoint search = hasQDWin(game);
            HSearchPoint qd = null;
            if (!search.Success)
            {
                qd = ImageTool.findEqImg(Resources.签到, game.getImg());
                if (!qd.Success)
                {
                    Clog.log("未找到签到框，退出!");
                    Mouse.reventLocation();
                    return;
                }
                game.MouseMove(qd.CenterPoint);
                Mouse.leftclick();
            }
            bool suc = BaseAction.waitSucFn(() =>
            {
                search = hasQDWin(game);
                return search.Success;
            }, 5000);
            if (!suc)
            {
                Clog.log("未找到签到框，退出!");
            }
            else
            {
                Point ptStart = new Point(search.CenterPoint.X + 147, search.CenterPoint.Y + 75);
                //签到框大小 ，46*50
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        game.MouseMove(ptStart.X + (j * 46), ptStart.Y + (i * 50));
                        Mouse.leftclick();
                        Mouse.sleep(100);
                    }
                }
                game.MouseMove(qd.CenterPoint);
                Mouse.leftclick();
            }
            Mouse.reventLocation();
        }
    }
}
