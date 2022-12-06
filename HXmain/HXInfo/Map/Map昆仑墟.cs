using HXmain.HXInfo.NPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain.HXInfo.Map
{
    public class Map昆仑墟 : MapBase
    {
        public static Map昆仑墟 Map = new Map昆仑墟();
        Map昆仑墟()
        {
            MaxWidth = 250;
            MaxHeigth = 387;
            MapKey = "D0A342CA36078BB92D141241309D0B41";
            Name = "昆仑墟";

            NPC_寒祁1 = new NPCBase(this, "寒祁", new System.Drawing.Point(48, 270));
            NPC_寒祁2 = new NPCBase(this, "寒祁", new System.Drawing.Point(90, 224));

            NPC_寒祁3 = new NPCBase(this, "寒祁", new System.Drawing.Point(0, 0));
            NPC_寒祁4 = new NPCBase(this, "寒祁", new System.Drawing.Point(159, 167));

            NPC_拯宣 = new NPCBase(this, "拯宣", new System.Drawing.Point(74, 198));

            NPC_施须长老 = new NPCBase(this, "拯宣", new System.Drawing.Point(35, 124));
        }

        public NPCBase NPC_寒祁1 { get; private set; }
        public NPCBase NPC_寒祁2 { get; private set; }


        public NPCBase NPC_寒祁3 { get; private set; }

        public NPCBase NPC_寒祁4 { get; private set; }

        public NPCBase NPC_拯宣 { get; private set; }

        public NPCBase NPC_施须长老 { get; private set; }
    }
}
