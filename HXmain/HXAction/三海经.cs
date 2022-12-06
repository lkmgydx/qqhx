using HFrameWork.SystemInput;
using HXmain.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;
using WSTools.WSLog;
using WSTools.WSThread;

namespace HXmain.HXAction
{
    public class 三海经
    {
        public static bool 三第经日常(MainGame game)
        {

            if (game == null)
            {
                return false;
            }
            game.Active();
            HSearchPoint hs = null;

            BaseAction.waitSucFn(() =>
            {
                hs = game.findImg(Resources.dia_山海经_o);
                if (hs.Success)
                {
                    return hs.Success;
                }
                else
                {
                    if (game.CloseWin.HasCoseWin)
                    {
                        game.CloseWin.closeAll();
                    }
                    game.SendKey(System.Windows.Forms.Keys.F8);
                    game.sleep(200);
                    int i = 0;
                    while (!game.CloseWin.HasCoseWin)
                    {
                        i++;
                        game.sleep(500);
                        if (i > 10)
                        {
                            Log.logWarnForce("超过2秒未检测到窗口,重试" + game.CloseWin.HasCoseWin);
                            break;
                        }
                    }
                }
                return false;
            }, 0);

            if (hs != null && hs.Success)
            {
                mainDo(game, hs);
                return true;
            }
            return false;
        }

        private static void mainDo(MainGame game, HSearchPoint hs)
        {
            game.MouseClick(hs.CenterPoint.X + 175, hs.CenterPoint.Y + 34);
            game.clickImg(Resources.免费拓印, 60, 40, true);
            game.clickImg(Resources.黄金拓印, 60, 40, true);
            game.SendKey(System.Windows.Forms.Keys.F8);
        }
    }
}
