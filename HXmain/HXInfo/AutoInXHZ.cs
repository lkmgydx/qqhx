using HFrameWork.SystemInput;
using HXmain.HXAction;
using HXmain.HXInfo.Map;
using HXmain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSTools.WSLog;

namespace HXmain.HXInfo
{
    public class AutoInXHZ
    {
        // 69.381 NPC1
        // 116.270
        // 123.143
        //119.107

        private static bool powerStop = false;

        public static async void run(MainGame game, bool autoRun = false)
        {
            if (game == null)
            {
                return;
            }
            powerStop = false;
            game.onPowerStop -= Game_onPowerStop;
            game.onPowerStop += Game_onPowerStop;
            int count = 0;
            await Task.Run(() =>
             {
                 using (var blh = Resources.npc_林海_卿阎)
                 {
                     using (var bxh = Resources.font_进入香魂冢)
                     {
                         List<BaseAction> act = new List<BaseAction>();
                         BaseAction action = BaseAction.getNpcAction(game, blh);
                         act.Add(action);
                         while (game.MapName == "林海" && !powerStop)
                         {
                             try
                             {
                                 Mouse.cacheLocation();
                                 game.MouseMove(0, 0);
                                 var hs = game.findImg(bxh);
                                 if (hs.Success)
                                 {
                                     count++;
                                     game.MouseClick(hs.CenterPoint);
                                     game.sleep(100);
                                 }
                                 BaseAction.DoActions(act);
                             }
                             catch (Exception ex)
                             {
                                 Log.logErrorForce("林海异常!", ex);
                             }
                             finally
                             {
                                 Mouse.reventLocation();
                             }
                         }
                         Log.logForce("exit in while....");
                     }
                 }
             });

            if (game.CurrentMap == MapBase.Maps.香魂冢)
            {
                Log.logForce("进入香魂冢总共尝试进入次数:" + count);
                // Tool.EmailTool.SendToQQ("成功进入香魂冢!", "抢FB");
                if (autoRun)
                {
                    game.AutoOneKey = true;
                    game.isXUNHUAN = false;
                    game.AutoRun();
                }
            }
        }

        private static void Game_onPowerStop()
        {
            powerStop = true;
            Log.logForce("强制停止 香魂冢");
        }
    }
}
