using HXmain.HXAction;
using HXmain.HXInfo.Map;
using HXmain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain.HXInfo
{
    public class AutoInXHZ
    {
        // 69.381 NPC1
        // 116.270
        // 123.143
        //119.107
        public static void run(MainGame game)
        {
            if (game == null)
            {
                return;
            }
            WSTools.WSThread.WsThread.PoolRun(() =>
            {
                using (var blh = Resources.npc_林海_卿阎)
                {
                    using (var bxh = Resources.font_进入香魂冢)
                    {
                        List<BaseAction> act = new List<BaseAction>();
                        BaseAction action = BaseAction.getNpcAction(game, blh);
                        act.Add(action);
                        while (game.MapName == "林海")
                        {
                            var hs = game.findImg(bxh);
                            if (hs.Success)
                            {
                                game.MouseClick(hs.CenterPoint);
                                game.sleep(100);
                            }
                            else {
                                BaseAction.DoActions(act);
                            } 
                        }
                        if (game.CurrentMap == MapBase.香魂冢)
                        {

                            // Tool.EmailTool.SendToQQ("成功进入香魂冢!", "抢FB");
                        }
                    }
                }

            });
        }
    }
}
