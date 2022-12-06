using HXmain.HXAction;
using HXmain.Properties;
using HXmain.Watcher.Dialog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSTools.WSLog;
using WSTools.WsWebPlanWebReference;

namespace HXmain.Watcher.NPCAsk
{
    public class NPC_升龙殿_宅院驿丞 : ImageWatcher
    {
        public void newCHDJ(MainGame game)
        {
            bool exit = false;
            var zy_ack_npc = getNpcWatcher<NPC_升龙殿_宅院驿丞>(game);

            int sydj_count = -1;
            var win_宅院驿丞 = getWatcher<Win宅院驿丞>((s) =>
            {
                if (s.Ask趁火打劫(game))
                {
                    return;
                }
                if (!s.CanDJ)
                {
                    exit = true;
                    game.closeAllGameWin();
                    return;
                }
                if (s.Ask_趁火打劫_肥羊列表(game))
                {
                    sydj_count = s.DJCount;
                    Log.logForce("打劫次数:::" + sydj_count);
                }
                else
                {
                    s.AskDjOKClose(game);
                }

            });

            Dictionary<Point, bool> canClickSome = new Dictionary<Point, bool>();
 

            var win_肥羊列表 = getWatcher<Win肥羊列表>((s, h) =>
            {
                if (s.HasSelect)
                {
                    s.Dajie(game);
                    return;
                }

                var r = s.get一品(game);
                if (r.Count != 0)
                {
                    foreach (var t in r)
                    {
                        if (canClickSome.ContainsKey(t.CenterPoint))
                        {
                            if (!canClickSome[t.CenterPoint])
                            {
                                continue;
                            }
                            else
                            {
                                var p = t.CenterPoint;
                                p.Offset(s.offSetP);
                                game.MouseClick();
                                return;
                            }
                        }
                    }
                }
                if (s.Select一品(game) || s.Select三品(game) || s.Select四品(game)) { };
            });


            BaseAction.waitSucFn(() =>
            {
                using (var b = game.getImg())
                {
                    lock (this)
                    {
                        Log.logForce("start ....");
                        win_宅院驿丞.Watch(b);
                        win_肥羊列表.Watch(b);
                        zy_ack_npc.Watch(b);
                    }
                }
                return exit || game.ESCSTOP;
            }, 30000);
        }

        public void newPHB(MainGame game)
        {
            bool exit = false;
            var zc = getWatcher<NPC_升龙殿_宅院驿丞>((s, su) =>
            {
                var tp = su.CenterPoint;
                tp.Offset(0, 70);
                game.MouseRigthClick(tp);
            });



            var winZYYC = getWatcher<Win宅院驿丞>((s) =>
            {
                s.Ask排行榜(game);
            });

            var winPHB = getWatcher<Win宅院排行榜>((s) =>
            {
                if (!s.hasSY)
                {
                    exit = true;
                    game.closeAllGameWin();
                    return;
                }
                if (!s.hasSelect)
                {
                    s.SelectOne(game);
                }
                s.Send鲜花(game);
            });

            BaseAction.waitSucFn(() =>
            {
                using (var b = game.getImg())
                {
                    winPHB.Watch(b);
                    winZYYC.Watch(b);
                    zc.Watch(b);
                }
                return exit || game.ESCSTOP;
            }, 30000);
        }

        protected override Bitmap WatcherBmp
        {
            get
            {
                return Resources.npc_升龙殿_宅院驿丞310211406;
            }

        }
    }
}
