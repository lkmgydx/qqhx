using HXmain.HXInfo.NPC.升龙殿;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain.HXInfo.Map
{
    public class Map北郡5 : MapBase
    {
        public static Map北郡5 m5 = new Map北郡5();
        private Map北郡5()
        {
            this.Name = "北郡（五）";
            this.MapKey = "3E9369BF883A94C80254C5E91D976506";

        }

        protected override void initNPC()
        {
            base.initNPC();
            杂货店掌柜 = new NPC_杂货店掌柜();
            中级装备商 = new NPC_中级装备商();
        }

        public NPC_杂货店掌柜 杂货店掌柜 { get; private set; }

        public NPC_中级装备商 中级装备商 { get; private set; }
    }
}
