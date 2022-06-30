using HFrameWork.SystemInput;
using HXmain.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;
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
                hs = game.findImg(Resources.dia_山海经);
                if (hs.Success)
                {
                    return hs.Success;
                }
                else
                {
                    game.SendKey(System.Windows.Forms.Keys.F8);
                    game.sleep(200);
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
            // Mouse.cacheLocation();

             game.MouseClick(hs.CenterPoint.X + 175, hs.CenterPoint.Y + 34);
            //game.MouseClick(hs.CenterPoint.X , hs.CenterPoint.X);
            //Point pt = game.ToScreenPoint(hs.CenterPoint);
            //Mouse.move(pt);
            //Mouse.moveR(175, 34);
            //Mouse.leftclick();
            //Mouse.sleep(500);

            //免费拓印
            var hssh = game.findImg(Resources.免费拓印);
            if (hssh.Success)
            {
                game.MouseClick(hssh.Point.X + 60, hssh.Point.Y + 40);
            }
            hssh = game.findImg(Resources.黄金拓印);
            if (hssh.Success)
            {
                game.MouseClick(hssh.Point.X + 60, hssh.Point.Y + 40);
            }
            game.SendKey(System.Windows.Forms.Keys.F8);
           // Mouse.reventLocation();
        }
    }
}
