using HXmain.HXInfo.NPC.监牢;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain.HXInfo.Map
{
    public class Map监牢 : MapBase
    {
        public static Map监牢 M = new Map监牢();
        private Map监牢()
        {
            Name = "监牢";
            MapKey = "3359A9AA974621F3F92C596AA391B7A6,29BEB49F52A9AC02105AC47D607992A7";
        }

        protected override void initNPC()
        {
            base.initNPC();
            小洁 = new NPC_小洁();
            捉迷藏的小孩1 = new NPC_捉迷藏的小孩(new System.Drawing.Point(15, 47));
            捉迷藏的小孩2 = new NPC_捉迷藏的小孩(new System.Drawing.Point(41, 25));
            捉迷藏的小孩3 = new NPC_捉迷藏的小孩(new System.Drawing.Point(59, 43));
            捉迷藏的小孩4 = new NPC_捉迷藏的小孩(new System.Drawing.Point(55, 80));
            捉迷藏的小孩5 = new NPC_捉迷藏的小孩(new System.Drawing.Point(38, 98));
            捉迷藏的小孩6 = new NPC_捉迷藏的小孩(new System.Drawing.Point(7, 90));
        }

        public NPC_小洁 小洁 { get; set; }

        public NPC_捉迷藏的小孩 捉迷藏的小孩1 { get; private set; }
        public NPC_捉迷藏的小孩 捉迷藏的小孩2 { get; private set; }
        public NPC_捉迷藏的小孩 捉迷藏的小孩3 { get; private set; }
        public NPC_捉迷藏的小孩 捉迷藏的小孩4 { get; private set; }
        public NPC_捉迷藏的小孩 捉迷藏的小孩5 { get; private set; }
        public NPC_捉迷藏的小孩 捉迷藏的小孩6 { get; private set; }

    }
}
