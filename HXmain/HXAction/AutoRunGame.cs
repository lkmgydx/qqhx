using HXmain.HXInfo.Map;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;
using WSTools.WSLog;

namespace HXmain.HXAction
{
    public class AutoRunGame
    {
        public static void Run(MainGame game, bool isRevernt = false)
        {
            MapBase map = game.CurrentMap;
            string name = map.Name;
            //界面显示.关闭系统发言();
            // 界面显示.关闭人物世发言();
            Point[] pt = null;
            if (name == "巨人野")
            {
                game.isXUNHUAN = true;
                pt = PathPointUtil.getResourcePoint("巨人野.txt");
            }
            else if (name == "阪泉")
            {
                Log.logForce("阪泉自动");
                game.isXUNHUAN = true;
                //game.aUTOTiLianWuPing.TilianAll();
                game.runGamePath.onRoundEnd -= RunGamePath_onRoundEnd1;
                game.runGamePath.onRoundEnd += RunGamePath_onRoundEnd1;
                Log.logForce("自动开始.");
                pt = PathPointUtil.getResourcePoint("阪泉小范围.txt");
            }
            else if (name == "赤水")
            {
                game.isXUNHUAN = true;
                pt = PathPointUtil.getResourcePoint("赤水.txt");
            }
            else if (name == "昆仑墟")
            {
                pt = PathPointUtil.getResourcePoint("昆仑FB.txt");
            }
            else if (name == "轩辕")
            {
                pt = PathPointUtil.getResourcePoint("轩辕.txt");
            }
            else if (name == "轩辕台一层")
            {
                game.isXUNHUAN = false;
                game.runPath(PathPointUtil.getResourcePoint("轩辕台一层.txt"), new Action(() =>
                {
                    EmailTool.SendToQQ("轩辕台一层 运行完毕", "自动轩辕台FB");
                }));
            }
            else if (name == "香魂冢")
            {
                game.isXUNHUAN = false;
                pt = PathPointUtil.getResourcePoint("香魂冢.txt");
                if (isRevernt)
                {
                    pt = pt.Reverse().ToArray();
                }
                game.runPath(pt, new Action(() =>
                {
                    // EmailTool.SendToQQ("香魂冢 运行完毕", "自动香魂冢FB");
                }));
                return;
            }
            else if (name == "升龙殿")
            {
                ToDaySlD.Start升龙殿(game);
            }
            else if (name == "诡墓")
            {
                game.isXUNHUAN = false;
                pt = PathPointUtil.getResourcePoint("诡墓.txt");
            }
            else if (map == MapBase.行商之路)
            {
                game.isXUNHUAN = false;
                pt = PathPointUtil.getResourcePoint("运镖.txt");
            }
            else if (map == MapBase.从雨沼泽)
            {
                game.isXUNHUAN = false;
                Rectangle r = new Rectangle(122, 113, 183, 49);
                if (r.Contains(game.CurrentLocation))
                {
                    game.isXUNHUAN = true;
                    game.runGamePath.onRoundEnd += RunGamePath_onRoundEnd;
                    pt = PathPointUtil.getResourcePoint("从雨沼泽31257578.txt");
                }
                else
                {
                    pt = PathPointUtil.getResourcePoint("从雨BOSS.txt");
                }

            }

            //执行pt动作
            if (pt == null)
            {
                return;
            }
            if (isRevernt)
            {
                pt = pt.Reverse().ToArray();
            }
            game.runPath(pt);
        }

        private static void RunGamePath_onRoundEnd1(object sender, EventArgs e)
        {
            MainGame game = sender as MainGame;
            if (game == null)
            {
                return;
            }
            game.WuPingLanInfo.TilianAllForce();
            game.drawMouse();
        }

        private static void RunGamePath_onRoundEnd(object sender, EventArgs e)
        {
            MainGame game = sender as MainGame;
            if (game == null)
            {
                return;
            }
            game.runGamePath.onRoundEnd -= RunGamePath_onRoundEnd;
            game.WuPingLanInfo.TilianAllForce();
            game.drawMouse();
        }
    }
}
