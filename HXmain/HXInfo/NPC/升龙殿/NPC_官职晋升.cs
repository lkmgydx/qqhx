using HXmain.HXAction;
using HXmain.HXInfo.Map;
using HXmain.Properties;
using HXmain.Watcher.Dialog;
using HXmain.Watcher.NPCAsk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain.HXInfo.NPC.升龙殿
{
    public class NPC_官职晋升 : NPCBase
    {
        public NPC_官职晋升() : base(MapBase.Maps.升龙殿, "官职晋升", new System.Drawing.Point(57, 71))
        { }


        public bool 领取俸禄N(MainGame game)
        {
            return new NPC_升龙殿_官职晋升().领取俸禄(game);
        }

        public bool 官职晋升任务(MainGame game)
        {
            MoveTo(game);
            NpcAskDo np = new NpcAskDo(game, Resources.NPC_官职晋升, "官职任务");
            np.ExitBmp = Resources.font_我知道了;
            np.AddSearchBmp(Resources.font_官职任务);
            np.AddSearchBmp(Resources.font_一键完成);
            return np.Start();
        }

        public bool 领取俸禄(MainGame game)
        {
            MoveTo(game);
            NpcAskDo np = new NpcAskDo(game, Resources.NPC_官职晋升, "官职任务");
            np.ExitBmp = Resources.领取俸禄退出;
            np.AddSearchBmp(Resources.对话_领取俸禄);
            return np.Start();
        }

        public bool 官职任务(MainGame mainGame)
        {
            return new NPC_升龙殿_官职晋升().官职任务(mainGame);
        }
    }
}
