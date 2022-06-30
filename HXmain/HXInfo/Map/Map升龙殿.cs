using HXmain.HXInfo.NPC.升龙殿;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain.HXInfo.Map
{
    public class Map升龙殿 : MapBase
    {
        public static Map升龙殿 Map = new Map升龙殿();
        Map升龙殿()
        {
            MaxWidth = 100;
            MaxHeigth = 170;
            MapKey = "C940C5535FA5D4E627ECA2EEEDB0A659,60ABBCBB4577F10504612BE725898B80";
            Name = "升龙殿";
        }

        public void doLW(MainGame game)
        {
            Point pt = game.CurrentLocation;
            int a1 = Math.Abs(pt.X - NPC_宅院驿丞.Position.X) + Math.Abs(pt.Y - NPC_宅院驿丞.Position.Y);
            int a2 = Math.Abs(pt.X - NPC_官职晋升.Position.X) + Math.Abs(pt.Y - NPC_官职晋升.Position.Y);
            if (a1 < a2)
            {
                NPC_宅院驿丞.排行榜任务(game);
                NPC_官职晋升.领取俸禄(game);
                NPC_官职晋升.官职晋升任务(game);
                NPC_钱善.我要出力(game);
                NPC_钱善.我要出钱(game);
            }
            else
            {
                a1 = Math.Abs(pt.X - NPC_钱善.Position.X) + Math.Abs(pt.Y - NPC_钱善.Position.Y);
                if (a1 < a2)
                {
                    NPC_钱善.我要出力(game);
                    NPC_钱善.我要出钱(game);

                    NPC_官职晋升.领取俸禄(game);
                    NPC_官职晋升.官职晋升任务(game);
                }
                else
                {
                    NPC_官职晋升.领取俸禄(game);
                    NPC_官职晋升.官职晋升任务(game);

                    NPC_钱善.我要出力(game);
                    NPC_钱善.我要出钱(game);
                }
                NPC_宅院驿丞.排行榜任务(game);
            }
        }

        protected override void initNPC()
        {
            NPC_宅院驿丞 = new NPC_宅院驿丞();
            NPC_官职晋升 = new NPC_官职晋升();
            NPC_钱善 = new NPC_钱善();
        }

        public NPC_官职晋升 NPC_官职晋升
        {
            get; private set;
        }


        public NPC_钱善 NPC_钱善
        {
            get; private set;
        }


        public NPC_宅院驿丞 NPC_宅院驿丞
        {
            get; private set;
        }

    }
}
