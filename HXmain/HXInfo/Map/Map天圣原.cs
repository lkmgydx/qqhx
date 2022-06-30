using HXmain.HXInfo.CacheData;
using HXmain.HXInfo.NPC;
using HXmain.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain.HXInfo.Map
{
    public class Map天圣原 : MapBase
    {
        public static Map天圣原 Map = new Map天圣原();

        public Map天圣原()
        {
            this.Name = "天圣原";
            this.MapKey = "DFBD9B077E613D03A7775A5C5CC8F578";
            this.MaxWidth = 500;
            this.MaxHeigth = 800;
        }

        protected override void initNPC()
        {
            NPC_驿站车夫 = new NPCBase(Map, "驿站车夫", new Point(171, 359));
            NPC_驿站车夫2 = new NPCBase(Map, "驿站车夫2", new Point(201, 304));
            NPC_驿站车夫3 = new NPCBase(Map, "驿站车夫3", new Point(141, 333));
        }

        private void initNPCs()
        {
            NPC_驿站车夫 = new NPCBase(Map, "驿站车夫", new Point(171, 359));
            NPC_驿站车夫2 = new NPCBase(Map, "驿站车夫2", new Point(201, 304));
            NPC_驿站车夫3 = new NPCBase(Map, "驿站车夫3", new Point(141, 333));
            //NPC_寄售商人 = new NPC(Map, "寄售商人", new Point(175, 327), Resources.NPC_寄售商人);
            //NPC_清源长老 = new NPC(Map, "清源长老", new Point(164, 347), Resources.NPC_清源长老);
            //NPC_武器商 = new NPC(Map, "清源长老", new Point(194, 336), Resources.NPC_武器商);
            //NPC_驿站车夫 = new NPC(Map, "驿站车夫", new Point(171, 359), Resources.NPC_武器商);
            //NPC_驿站车夫2 = new NPC(Map, "驿站车夫2", new Point(201, 304), Resources.NPC_驿站车夫);
            //NPC_驿站车夫3 = new NPC(Map, "驿站车夫3", new Point(141, 341), Resources.NPC_驿站车夫);
        }

        public NPCBase NPC_寄售商人 { get; private set; }


        public NPCBase NPC_清源长老 { get; private set; }

        public NPCBase NPC_武器商 { get; private set; }

        public NPCBase NPC_驿站车夫 { get; private set; }

        public NPCBase NPC_驿站车夫2 { get; private set; }

        public NPCBase NPC_驿站车夫3 { get; private set; }
    }
}
