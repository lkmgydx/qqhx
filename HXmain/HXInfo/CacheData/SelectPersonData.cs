using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSTools.WSSerialize;

namespace HXmain.HXInfo.CacheData
{
    [Serializable]
    public class SelectPersonData
    {

        /// <summary>
        /// 缓存文件名
        /// </summary>
        private static readonly string NPC_BIN = AppDomain.CurrentDomain.BaseDirectory + "bin/SelectPerson.bin";
        private static Dictionary<string, SelectPersonData> All_NPC = new Dictionary<string, SelectPersonData>();

        static SelectPersonData()
        {
            if (!File.Exists(NPC_BIN))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(NPC_BIN));
                BinSerialize.toFile(NPC_BIN, All_NPC);
            }
            All_NPC = BinSerialize.fromFile(NPC_BIN) as Dictionary<string, SelectPersonData>;
            BinSerialize.toFile(NPC_BIN, All_NPC);
        }

        public SelectPersonData()
        {
        }
        public SelectPersonData(string key, string v)
        {
            this.Key = key;
            this.Value = v;
        }

        public static void addNpcInfo(SelectPersonData info)
        {
            string key = info.Key;
            if (All_NPC.ContainsKey(key))
            {
                All_NPC.Remove(key);
            }
            All_NPC.Add(key, info);

        }

        public static void save()
        {
            BinSerialize.toFile(NPC_BIN, All_NPC);
        }

        public static SelectPersonData getSelectPersonInfo(string key)
        {
            if (All_NPC.ContainsKey(key))
            {
                return All_NPC[key];
            }
            return null;
        }

        public string Key;

        public string Value;

    }
}
