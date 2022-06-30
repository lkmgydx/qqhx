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
    public class TiLian
    {
        static HSearchPoint priPoint = null;

        static Point TilianFormLocation = new Point(270,500);

        public static bool TiLianSome(MainGame game)
        {
            HSearchPoint hs = show提练Win(game);
            bool suc = false;
            if (hs.Success)
            {
                Mouse.cacheLocation();
                suc = TilianPoint(hs, game);
                Mouse.reventLocation();
            }
            return suc;
        }

        public static void TiLianBack(MainGame game)
        {
            if (game == null)
            {
                return;
            }
            HSearchPoint hs = WuPing.openWuPing(game);
            if (!hs.Success)
            {
                return;
            }
        }

        public static HSearchPoint show提练Win(MainGame game)
        {
            HSearchPoint hs = null;
            for (int i = 0; i < 5; i++)
            {
                hs = isFind(game);
                if (hs.Success)
                {
                    game.DrawPointToPoint(hs.CenterPoint, TilianFormLocation);
                    break;
                }
                else
                {
                    if (game != null)
                    {
                        hs = show提练WinReal(game);
                    }
                }
            }
            if (is技能Win(game).Success)
            {
                if (game != null)
                {
                    game.Active();
                    game.SendKey(System.Windows.Forms.Keys.S);
                }
            }
            return hs;
        }

        private static HSearchPoint show提练WinReal(MainGame game)
        {
            HSearchPoint hsW = null;
            for (int i = 0; i < 5; i++)
            {
                hsW = show技能Win(game);
                if (hsW.Success)
                {
                    game.MouseMove(hsW.CenterPoint);
                    Mouse.ShowMouse();
                    Mouse.leftclick();
                    Mouse.sleep(200);
                    Mouse.moveR(-10, 90);
                    Mouse.sleep(200);
                    Mouse.leftclick();
                    Mouse.sleep(200);
                    bool isMoved = false;
                    game.SendKey(System.Windows.Forms.Keys.S);
                    for (int j = 0; j < 5; j++)
                    {
                        Mouse.reventLocation(false);
                        hsW = isPic(Resources.tl, game);
                        if (hsW.Success)
                        {
                            if (!isMoved)
                            {
                                game.DrawPointToPoint(hsW.CenterPoint, TilianFormLocation);
                                isMoved = true;
                                continue;
                            }
                            return hsW;
                        }
                        Mouse.sleep(200);
                    }
                    return hsW;
                }
            }
            return hsW;
        }

        public static HSearchPoint show技能Win(MainGame game)
        {
            HSearchPoint hs = null;
            for (int i = 0; i < 5; i++)
            {
                hs = is技能Win(game);
                if (hs.Success)
                {
                    return hs;
                }
                else
                {
                    if (game != null)
                    {
                        game.Active();
                        game.SendKey(System.Windows.Forms.Keys.S);
                        KeyBoard.sleep(200);
                    }
                }
            }
            return hs;
        }

        private static HSearchPoint is技能Win(MainGame game)
        {
            return isPic(Resources.工作技能Tab, game);
        }

        private static HSearchPoint isFind(MainGame game)
        {
            return isPic(Resources.tl, game);
        }

        private static HSearchPoint isPic(Bitmap res, MainGame game)
        {
            HSearchPoint hs = null;
            if (game == null)
            {
                hs = ImageTool.findImageFromScreen(res, 90);
            }
            else
            {
                using (Bitmap bmp = game.getImg())
                {
                    hs = ImageTool.findLikeImg(res, bmp, 90);
                }
            }
            return hs;
        }

        private static bool TilianPoint(HSearchPoint hs, MainGame game)
        {
            bool isSuc = false;
            if (!hs.Success)
            {
                return false;
            }
            priPoint = hs;
            Mouse.cacheLocation(); //缓存当前位置
            if (game != null)  //将鼠标移动到物品栏
            {
                game.MouseMove(hs.Point.X + 118, hs.Point.Y + 110);
            }
            else
            {
                Mouse.move(hs.Point.X + 118, hs.Point.Y + 110);
            }
            Mouse.sleep(100);
            Mouse.leftclick();  //装备放入物品栏
            Mouse.sleep(100);
            Mouse.reventLocation(false); //鼠标归位
            KeyBoard.sleep(100);
            if (game != null)
            {
                Rectangle rec = new Rectangle(hs.Point.X, hs.Point.Y, 230, 200);
                for (int i = 0; i < 5; i++)
                {
                    using (Bitmap bnow = game.getImg(rec))
                    {
                        HSearchPoint hst = ImageTool.findLikeImg(Resources.wuping_empty, bnow, 90);
                        if (hst.Success)
                        {
                            Mouse.sleep(300);
                            isSuc = false;
                        }
                        else
                        {
                            isSuc = true;
                            break;
                        }
                    }
                }
            }
            if (game != null)
            {
                game.MouseMove(hs.Point.X + 118, hs.Point.Y + 110 + 65);
            }
            else
            {
                Mouse.move(hs.Point.X + 118, hs.Point.Y + 110 + 65);
            }
            Mouse.sleep(100);
            Mouse.leftclick();
            Mouse.rightclick();
            Mouse.reventLocation();

            return isSuc;
        }
    }
}
