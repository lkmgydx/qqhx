using HXmain.HXAction;
using HXmain.HXInfo.Map;
using HXmain.Properties;
using HXmain.Watcher.NPCAsk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain.HXInfo.NPC.升龙殿
{
    public class NPC_钱善 : NPCBase
    {
        public NPC_钱善() : base(MapBase.Maps.升龙殿, "钱善", new System.Drawing.Point(51, 62))
        {
        }

        public bool 我要出力N(MainGame game)
        {
            return new NPC_升龙殿_钱善().我要出力(game);
        }

        public bool 我要出力(MainGame game)
        {
            MoveTo(game);
            NpcAskDo ns = new NpcAskDo(game, Resources.npc_钱善, "对话钱善..");
            ns.ExitBmp = Resources.act_钱善_exit_明天再来;
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
            return ns.Start();
        }

        public bool 我要出钱(MainGame game)
        {
            MoveTo(game);
            var ad = new NpcAskDo(game, Resources.npc_钱善, "钱善自动捐满任务...");
            ad.AddSearchBmp(Resources.font_我要出钱);
            ad.AddSearchBmp(Resources.font_自动捐满);

            ad.ExitBmp = Resources.自动捐满Suc;
            ad.addWaitDoImage(Resources.自动捐满Suc, 30000);

            bool suc = ad.Start();
            if (!suc)
            {
                return false;
            }
            BathDo bd = new BathDo(game);
            bd.AddSearchBmp(Resources.font_领取荣誉);
            return bd.Start();
        }
    }
}
