using HFrameWork.SystemInput;
using HXmain.HXInfo.Map;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tool;
using WSTools.WSLog;
using static HXmain.MainGame;

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
                game.AutoOneKey = true;
                //game.aUTOTiLianWuPing.TilianAll();
                game.runGamePath.onRoundEnd -= RunGamePath_onRoundEnd1;
                game.runGamePath.onRoundEnd += RunGamePath_onRoundEnd1;
                Log.logForce("自动开始.");
                //pt = PathPointUtil.getResourcePoint("阪泉小范围.txt");
                pt = PathPointUtil.getResourcePoint("阪泉.txt");

                //bool skip = false;
                //Rectangle rec = new Rectangle(153, 178, 90, 120);

                //LocationChangeEvent le = (rv, v) =>
                //{
                //    if (skip)
                //    {
                //        return;
                //    }
                //    skip = true;

                //    if (rec.Contains(game.CurrentLocation))
                //    {
                //        Log.logForce("cc stop");
                //        game.PowerStop();
                //        var pp = PathPointUtil.getResourcePoint("阪泉Boss走路.txt");
                //        var prst = new Point[pp.Length - 20];

                //        System.Array.Copy(pp, 0, prst, 0, prst.Length);
                //        game.isXUNHUAN = false;
                //        game.runPath(prst, () =>
                //        {
                //            Log.logForce("rr ");
                //            skip = false;
                //            Run(game);
                //        });
                //        Log.logForce("next r");
                //    }

                //    Log.logForce("eeeeeeeeeeeee near");
                //};

                //if (!PathPointUtil.isNear(pt, game.CurrentLocation))
                //{
                //    Log.logForce("no near");
                //    le(game.CurrentLocation, game.CurrentLocation);
                //    return;
                //}
                //game.onLocationChange -= le;
                //game.onLocationChange += le;
            }
            else if (name == "赤水")
            {
                game.isXUNHUAN = true;
                pt = PathPointUtil.getResourcePoint("赤水.txt");
                LocationChangeEvent lc = new LocationChangeEvent((a, b) =>
                {
                    if (game.IsSafeRegion && game.CurrentLocation.X < 100)
                    {
                        Log.logForce("安全区，退出...!" + a);
                        game.PowerStop();
                    }
                });
                game.onLocationChange += lc;
            }
            else if (name == "昆仑虚二层")
            {
                game.isXUNHUAN = false;
                pt = PathPointUtil.getResourcePoint("昆仑虚二层.txt");
                game.runPath(pt, () =>
                {
                    pt = PathPointUtil.getResourcePoint("昆仑虚二层last.txt");
                    game.isXUNHUAN = true;
                    game.runPath(pt);
                    game.onMapChangeEvent += (a, b) =>
                    {
                        if (b.Name == MapBase.Maps.诡误大泽.Name)
                        {
                            game.PowerStop();
                        }
                    };
                });
                return;
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
                WSTools.WSThread.WsThread.Run(() => { });
                game.isXUNHUAN = false;
                var start = Environment.TickCount;
                game.runPath(PathPointUtil.getResourcePoint("轩辕台一层.txt"), (() =>
                {
                    Log.logForce("轩辕台一层，运行完毕，耗时：" + (Environment.TickCount - start) / 60000.0 + "分");
                    //EmailTool.SendToQQ("轩辕台一层 运行完毕", "自动轩辕台FB");
                }));
            }
            else if (name == "香魂冢")
            {
                game.isXUNHUAN = false;
                if (game.isFbMode)
                {
                    pt = PathPointUtil.getResourcePoint("香魂冢1.txt");
                    game.runPath(pt, () =>
                    {
                        game.WaitGet();

                        pt = PathPointUtil.getResourcePoint("香魂冢2.txt");
                        game.runPath(pt, () =>
                        {
                            game.SetDown();
                            KeyBoard.down_upCtrl(Keys.Z);
                            game.WaitGet();
                            KeyBoard.down_upCtrl(Keys.Z);
                            pt = PathPointUtil.getResourcePoint("香魂冢3.txt");
                            game.runPath(pt, () =>
                            {
                                game.SetDown();
                                KeyBoard.down_upCtrl(Keys.Z);
                                game.WaitGet();
                                KeyBoard.down_upCtrl(Keys.Z);
                                pt = PathPointUtil.getResourcePoint("香魂冢4.txt");
                                game.runPath(pt, () =>
                                {
                                    game.WaitGet();
                                });
                            });

                        });
                    });
                }
                else if (game.isFbMode1)
                {
                    pt = PathPointUtil.getResourcePoint("香魂冢1.txt");
                    game.runPath(pt, () =>
                    {
                        game.SendKey(System.Windows.Forms.Keys.L);
                        System.Threading.Thread.Sleep(1000 * 60 * 5);
                        game.SendKey(System.Windows.Forms.Keys.L);
                    });
                }
                else if (game.isFbMode2)
                {
                    pt = PathPointUtil.getResourcePoint("香魂冢2.txt");
                    game.runPath(pt, () =>
                    {
                        game.SendKey(System.Windows.Forms.Keys.L);
                        System.Threading.Thread.Sleep(1000 * 60 * 5);
                        game.SendKey(System.Windows.Forms.Keys.L);
                    });
                }
                else if (game.isFbMode3)
                {

                    pt = PathPointUtil.getResourcePoint("香魂冢3.txt");
                    game.runPath(pt, () =>
                    {
                        game.SendKey(System.Windows.Forms.Keys.L);
                        System.Threading.Thread.Sleep(1000 * 60 * 5);
                        game.SendKey(System.Windows.Forms.Keys.L);
                    });
                }
                else if (game.isFbMode4)
                {
                    Log.logForce("model 4");
                    try
                    {
                        pt = PathPointUtil.getResourcePoint("香魂冢4.txt");
                    }
                    catch (Exception ex)
                    {
                        Log.logErrorForce("失败", ex);
                    }

                    Log.logForce("数量:" + pt.Length);
                    game.runPath(pt, () =>
                    {
                        game.SendKey(System.Windows.Forms.Keys.L);
                        System.Threading.Thread.Sleep(1000 * 60 * 5);
                        game.SendKey(System.Windows.Forms.Keys.L);
                    });
                }
                else
                {
                    pt = PathPointUtil.getResourcePoint("香魂冢.txt");
                    if (isRevernt)
                    {
                        pt = pt.Reverse().ToArray();
                    }
                    game.runPath(pt);
                }
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
            else if (map == MapBase.Maps.行商之路)
            {
                game.isXUNHUAN = false;
                pt = PathPointUtil.getResourcePoint("运镖.txt");
            }
            else if (map == MapBase.Maps.魔化鬼墓)
            {
                game.isXUNHUAN = false;
                pt = PathPointUtil.getResourcePoint("魔化鬼墓.txt");
            }
            else if (map == MapBase.Maps.从雨沼泽)
            {
                game.isXUNHUAN = false;
                Rectangle r = new Rectangle(100, 80, 100, 200);

                //if (r.Contains(game.CurrentLocation))
                //{

                //}
                //else
                //{
                //    game.isXUNHUAN = true;
                //    game.runGamePath.onRoundEnd += RunGamePath_onRoundEnd;
                //    pt = PathPointUtil.getResourcePoint("从雨沼泽31257578.txt");
                //}
                pt = PathPointUtil.getResourcePoint("从雨BOSS.txt");
                game.runPath(pt, () =>
                {
                    game.AutoOneKey = true;
                    game.SendKey(Keys.Home);
                    game.SetDown();
                });
                return;
            }

            //执行pt动作
            if (pt == null)
            {
                Log.logErrorForce("无路径可执行，退出");
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
