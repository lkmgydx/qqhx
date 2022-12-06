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
    public class ToDaySlD
    {
        private static Point[] ZYpath = new Point[] {
            new Point(48,71),
            new Point(44,83),
            new Point(45,90),
             new Point(41,96),
                 new Point(42,101), //宅院
        };

        private static Point[] GZpath = new Point[] {
                  new Point(43,92),
                  new Point(48,85),
                  new Point(52,78),
                    new Point(53,75),
                    new Point(55,74),
                   new Point(56,73)//官职
        };

        private static Point[] QSpath = new Point[] {
                 new Point(52,69),
                 new Point(49,65),
                new Point(48,63), //钱善
        };

        public static void Start升龙殿(MainGame game)
        {
            game.closeAllGameWin();
            game.isXUNHUAN = false;
            if (needZY(game))
            {
                game.runPath(ZYpath, () =>
                {
                    Mouse.sleep(2000);
                    game.closeAllGameWin();
                    升龙殿投票NPC任务(game);

                });
            }
            else
            {
                do2(game);
            }
        }


        private static void do2(MainGame game)
        {
            game.closeAllGameWin();
            game.runPath(GZpath, () =>
            {
                Mouse.sleep(2000);
                升龙殿官职晋升NPC任务(game);
                do3(game);
            });
        }

        private static void do3(MainGame game)
        {
            game.closeAllGameWin();
            game.runPath(QSpath, () =>
            {
                Mouse.sleep(2000);
                升龙殿钱善NPC任务(game);
                升龙殿钱善NPC任务_自动捐满(game);
            });
        }

        static System.Drawing.Rectangle dc = new System.Drawing.Rectangle(620, 140, 450, 600);


        private static bool ClickBmpCenter(MainGame game, Bitmap bmp)
        {
            using (bmp)
            {
                var hs = game.findImg(bmp);
                game.MouseClick(hs.CenterPoint);
                return hs.Success;
            }
        }

        public static void 升龙殿钱善NPC任务3(MainGame game)
        {
            NpcAskDo ns = new NpcAskDo(game, Resources.npc_钱善, "对话钱善..");
            ns.ExitBmp = Resources.act_钱善_exit;
            ns.AddSearchBmp(Resources.font_我要出力);
            ns.AddSearchBmp(Resources.font_我要出力);
            ns.AddSearchBmp(Resources.font_一键完成);
            ns.AddSearchBmp(Resources.font_花费5官银重置任务);
            ns.AddSearchBmp(Resources.font_花费5官银重置任务2);
            ns.AddSearchBmp(Resources.font_花费20官银重置任务);
            ns.AddSearchBmp(Resources.font_花费20官银2);
            ns.onExit += (bool suc) =>
            {
                Console.WriteLine("对话钱善.." + suc);
            };
            ns.Start();
        }

        public static void 升龙殿官职晋升NPC任务1(MainGame game)
        {
            NpcAskDo np = new NpcAskDo(game, Resources.NPC_官职晋升, "官职任务");
            np.ExitBmp = Resources.font_我知道了;
            np.AddSearchBmp(Resources.font_官职任务);
            np.AddSearchBmp(Resources.font_一键完成);
            np.Start();
        }

        public static void 升龙殿钱善NPC任务1_自动捐满(MainGame game)
        {
            var ad = new NpcAskDo(game, Resources.npc_钱善, "钱善自动捐满任务...");
            ad.AddSearchBmp(Resources.font_我要出钱);
            ad.AddSearchBmp(Resources.font_自动捐满);

            ad.ExitBmp = Resources.自动捐满Suc;
            ad.addWaitDoImage(Resources.自动捐满Suc, 30000);
            ad.onExit += (bool suc) =>
            {
                if (suc)
                {
                    BathDo bd = new BathDo(game);
                    bd.AddSearchBmp(Resources.font_领取荣誉);
                    bd.Start();
                }
            };
            ad.Start();
        }

        public static void 升龙殿钱善NPC任务2(MainGame game)
        {
            BaseAction ac = BaseAction.getNpcAction(game, Resources.npc_钱善);
            ac.FlotPoint = new System.Drawing.Point(0, 70);
            ac.ActionType = ActionType.RigthDbClick;
            ac.ActionName = "对话钱善";
            bool waitExit = true;
            WsThread.Run(() =>
            {
                BathDo bd = new BathDo(game, 1000 * 60 * 5);//最多5分钟
                bd.ExitBmp = Resources.act_钱善_exit;
                bd.AddSearchBmp(Resources.font_我要出力);
                bd.AddSearchBmp(Resources.font_我要出力);
                bd.AddSearchBmp(Resources.font_一键完成);
                bd.AddSearchBmp(Resources.font_花费5官银重置任务);
                bd.AddSearchBmp(Resources.font_花费5官银重置任务2);
                bd.AddSearchBmp(Resources.font_花费20官银重置任务);
                bd.AddSearchBmp(Resources.font_花费20官银2);
                bd.onExit += ((bool suc) =>
                {
                    waitExit = false;
                });
                bd.Start();

            });
            while (true)
            {
                if (!waitExit)
                {
                    return;
                }
                RunInfo info = BaseAction.DoBaseAction(ac);
                if (info.IsSuc)
                {
                    Mouse.sleep(1000);
                }
            }
        }

        public static void 升龙殿钱善NPC任务(MainGame game)
        {
            List<BaseAction> li = new List<BaseAction>();
            for (int i = 0; i < 100; i++)
            {
                BaseAction ac = BaseAction.getNpcAction(game, Resources.npc_钱善);
                ac.FlotPoint = new System.Drawing.Point(0, 70);
                ac.ActionType = ActionType.RigthDbClick;
                ac.ActionName = "对话钱善";
                li.Add(ac);

                ac = BaseAction.get对话Action(game, Resources.font_我要出力, "我要出力");
                ac.SameImg.Add(Resources.font_一键完成);
                ac.SameImg.Add(Resources.font_花费5官银重置任务);
                ac.SameImg.Add(Resources.font_花费5官银重置任务2);
                ac.SameImg.Add(Resources.font_花费20官银重置任务);
                ac.SameImg.Add(Resources.font_花费20官银2);
                ac.RunWaitTime = 3000;

                li.Add(ac);
                li.Add(ac);


                BaseAction bac = BaseAction.getCenterAction(game, Resources.act_钱善_exit);
                bac.AfterWaitTime = 300;
                bac.ActionName = "退出";
                bac.beforSucSkipThis = true;
                bac.RunWaitTime = 1000;
                bac.SucStop = true; //找到则退出
                bac.beforSucSkipThis = true;
                bac.isCenter = false;
                // bac.FlotPoint = new System.Drawing.Point(10, 96);
                li.Add(bac);
            }
            BaseAction.DoActions(li);
        }

        public static void 升龙殿钱善NPC任务_自动捐满(MainGame game)
        {
            List<BaseAction> li = new List<BaseAction>();
            for (int i = 0; i < 4; i++)
            {
                BaseAction ac = BaseAction.getNpcAction(game, Resources.npc_钱善);
                ac.FlotPoint = new System.Drawing.Point(0, 70);
                ac.ActionType = ActionType.RigthDbClick;
                ac.ActionName = "对话钱善";
                li.Add(ac);

                li.Add(BaseAction.get对话Action(game, Resources.font_我要出钱, "我要出钱"));
                li.Add(BaseAction.get对话Action(game, Resources.font_自动捐满, "自动捐满"));
                ac = BaseAction.get对话Action(game, Resources.自动捐满Suc, "等待自动捐满成功");
                ac.beforFalsSkipThis = false;
                ac.RunWaitTime = 30000;
                li.Add(ac);

                BaseAction bac = BaseAction.getCenterAction(game, Resources.font_领取荣誉);
                bac.AfterWaitTime = 300;
                bac.ActionName = "领取荣誉退出";
                bac.beforSucSkipThis = false;
                bac.beforFalsSkipThis = true;
                bac.RunWaitTime = 1000;
                bac.SucStop = true; //找到则退出
                bac.isCenter = false;
                li.Add(bac);
            }
            BaseAction.DoActions(li);
        }


        public static void 升龙殿官职晋升NPC任务(MainGame game)
        {
            game.MouseMove(0, 0);
            List<BaseAction> li = new List<BaseAction>();
            for (int i = 0; i < 10; i++)
            {
                li.Add(getGG(game));
                BaseAction ba = BaseAction.getCenterAction(game, Resources.font_官职任务, "官职任务");
                ba.SameImg.Add(Resources.font_一键完成);
                li.Add(ba);
                li.Add(ba);

                BaseAction action = BaseAction.getCenterAction(game, Resources.font_我知道了, "是否为我知道了");
                action.notFindWaitTime = 500;
                action.beforSucSkipThis = true;
                action.SucStop = true;
                li.Add(action);
            }
            BaseAction.DoActions(li);
            game.SendKey(System.Windows.Forms.Keys.Escape);
        }




        public static BaseAction getGG(MainGame game)
        {
            return BaseAction.getNpcAction(game, Resources.NPC_官职晋升);
        }

        public static void 升龙殿投票NPC任务(MainGame game)
        {
            if (needZY(game))
            {
                List<BaseAction> li = new List<BaseAction>();
                li.Add(DoToDaySLD(game));
                li.Add(get和宅院驿丞对话(game));
                li.Add(get排行榜第一(game));
                li.Add(get投鲜花(game));
                li.Add(get投鲜花(game));
                li.Add(get投鲜花(game));
                li.Add(get投鲜花(game));
                BaseAction ba = get投鲜花(game);
                ba.AfterWaitTime = 500;
                li.Add(ba);
                try
                {
                    BaseAction.DoActions(li);
                }
                catch (Exception ex)
                {
                    Log.logErrorForce("宅院异常", ex);
                }
                
            }

            game.closeAllGameWin();
            升龙殿投票NPC任务_打劫(game);
        }

        public static void 升龙殿投票NPC任务_打劫(MainGame game)
        {
            if (!needZY_DJ(game))
            {
                return;
            }
            var info = iniAndGetDay(game);
            int maxCount = 10;
            while (true)
            {
                maxCount--;
                if (maxCount <= 0)
                {
                    return;
                }
                List<BaseAction> li = new List<BaseAction>();
                li.Add(DoToDaySLD(game));
                BaseAction.DoActions(li);
                game.MouseClick(452, 366);
                game.sleep(500);
                if (game.findImg(Resources.打劫0).Success)
                {
                    info.ZY_DJ_ALL_Count = info.ZY_DJ_Count;
                    game.closeAllGameWin();
                    return;
                }
                game.clickImg(Resources.font_肥羊列表);
                game.sleep(500);

                List<HSearchPoint> hs = game.findImgAll(Resources.font_打劫4品1);
                if (hs.Count == 0)
                {
                    return;
                }
                int f = new Random().Next(0, hs.Count);
                game.MouseClick(hs[f].Point.X - 210, hs[f].Point.Y);
                if (game.clickImg(Resources.font_前往打劫).Success)
                {
                    info.ZY_DJ_Count++;
                }
                game.closeAllGameWin();
            }
        }

        private static BaseAction DoToDaySLD(MainGame game)
        {
            BaseAction ba = new BaseAction();
            ba.AfterWaitTime = 500;
            ba.Bmp = Resources.npc_升龙殿_宅院驿丞310211406;
            ba.MatchOp = 95;
            ba.FlotPoint = new System.Drawing.Point(30, 80);
            ba.ActionType = ActionType.RigthDbClick;
            ba.game = game;
            return ba;
        }

        private static BaseAction get和宅院驿丞对话(MainGame game)
        {
            BaseAction ba = new BaseAction();
            ba.AfterWaitTime = 1000;
            ba.Bmp = Resources.font_排行榜;
            ba.MatchOp = 95;
            ba.isCenter = true;
            ba.game = game;
            return ba;
        }

        private static BaseAction get排行榜第一(MainGame game)
        {
            BaseAction action = BaseAction.getAction(game, Resources.font_排行_名字);
            action.FlotPoint = new System.Drawing.Point(45, 18);
            return action;
        }

        private static Dictionary<string, dicInfo> di = new Dictionary<string, dicInfo>();

        /// <summary>
        /// 是否已经自动完成宅院任务
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public static bool needZY(MainGame game)
        {
            var c = iniAndGetDay(game);
            return c.ZY_Count < 5;
        }

        public static bool needZY_DJ(MainGame game)
        {
            var info = iniAndGetDay(game);
            return info.ZY_DJ_Count == info.ZY_DJ_ALL_Count;
        }

        private static dicInfo iniAndGetDay(MainGame game)
        {
            var key = game.Name + DateTime.Now.DayOfYear;
            var dicInfo = new dicInfo()
            {
                Name = key,
                ZY_DJ_ALL_Count = 5
            };
            if (!di.ContainsKey(key))
            {
                di.Add(key, dicInfo);
            }
            return di[key];
        }

        struct dicInfo
        {
            /// <summary>
            /// 名称
            /// </summary>
            public string Name;
            /// <summary>
            /// 宅院鲜花次数
            /// </summary>
            public int ZY_Count;
            /// <summary>
            /// 宅院打劫成功次数
            /// </summary>
            public int ZY_DJ_Count;
            /// <summary>
            /// 宅院总共打劫数次
            /// </summary>
            public int ZY_DJ_ALL_Count;
        }

        private static BaseAction get投鲜花(MainGame game)
        {
            var info = iniAndGetDay(game);
            BaseAction ba = BaseAction.getAction(game, Resources.font_投掷鲜花);
            ba.MatchOp = 95;
            ba.BeforActionTime = 300;
            ba.isCenter = true; ;
            ba.HXAction = () =>
            {
                info.ZY_Count++;
            };
            return ba;
        }
    }
}
