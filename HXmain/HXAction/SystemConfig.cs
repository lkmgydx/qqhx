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
    public class SystemConfig
    {
        private static string CHECKID = "1F5D436F3BE0793D449953E2268406D5";
        private static Size CHECKSIZE = new Size(5, 5);
        //890  785
        // 620 350
        public static HSearchPoint openConfig(MainGame game)
        {
            if (game == null)
            {
                return null;
            }
            var hs = game.findImg(Resources.win_游戏设置);
            if (hs.Success)
            {
                return hs;
            }
            game.MouseClick(890, 785);//点击设置
                                      //game.MouseMove(890, 785);
                                      //Mouse.sleep(200);
                                      //Mouse.leftclick();
            var suc = BaseAction.waitSucFn(() =>
             {
                 hs = game.findImg(Resources.config_游戏设置);
                 return hs.Success;
             }, 3000);

            if (!suc)
            {
                return HSearchPoint.Empty;
            }

            game.MouseClick(620, 350); //点击游戏设置
            //game.MouseMove(620, 350); //点击游戏设置
            //game.MouseClick();

            BaseAction.waitSucFn(() =>
            {
                hs = game.findImg(Resources.win_游戏设置);
                return hs.Success;
            }, 3000);
            return hs;
        }

        /// <summary>
        /// 隐藏附近人员
        /// </summary>
        /// <param name="game"></param>
        /// <param name="hide"></param>
        /// <returns></returns>
        public static bool setHidePerson(MainGame game, bool hide = true)
        {
            game.Active();
            HSearchPoint hs = openConfig(game);
            if (!hs.Success)
            {
                return false;
            }
            Point p = new Point(hs.CenterPoint.X + 85, hs.CenterPoint.Y + 100);

            using (var img = game.getImg(new System.Drawing.Rectangle(p, CHECKSIZE)))
            {
                if (hide == (CHECKID == ImageTool.UUIDImg(img)))//选 中状态
                {
                    game.MouseClick(new Point(hs.CenterPoint.X + 78, hs.CenterPoint.Y + 328));
                    return true;
                }
            }
            game.MouseClick(p);
            BaseAction.waitSucFn(() =>
            {
                using (var img = game.getImg(new Rectangle(p, CHECKSIZE)))
                {
                    return hide == (CHECKID == ImageTool.UUIDImg(img));
                }
            }, 1000);
            game.MouseClick(hs.CenterPoint.X + 78, hs.CenterPoint.Y + 328);
            return true;
        }

    }
}
