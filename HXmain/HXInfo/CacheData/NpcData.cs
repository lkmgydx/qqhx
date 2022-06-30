using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSTools.WSSerialize;

namespace HXmain.HXInfo.CacheData
{
    [Serializable]
    public class NpcData
    {
        /// <summary>
        /// 缓存文件名
        /// </summary>
        private static readonly string NPC_BIN = AppDomain.CurrentDomain.BaseDirectory + "NpcData.bin";
        private static Dictionary<string, NpcData> All_NPC = new Dictionary<string, NpcData>();

        static NpcData()
        {
            if (!File.Exists(NPC_BIN))
            {
                BinSerialize.toFile(NPC_BIN, All_NPC);
            }
            All_NPC = BinSerialize.fromFile(NPC_BIN) as Dictionary<string, NpcData>;
            BinSerialize.toFile(NPC_BIN, All_NPC);
        }


        public static void addNpcInfo(NpcData info)
        {
            string key = info.MapName + "_" + info.Name;
            if (All_NPC.ContainsKey(key))
            {
                All_NPC.Remove(key);
            }
            All_NPC.Add(key, info);
            BinSerialize.toFile(NPC_BIN, All_NPC);
        }

        public static NpcData getNpcInfo(string mapName, string npcName)
        {
            string key = mapName + "_" + npcName;
            if (All_NPC.ContainsKey(key))
            {
                return All_NPC[key];
            }
            return null;
        }

        public string MapName { get; set; }
        public string Name { get; set; }
        public Point[] NPCPoint { get; set; }
    }
}
