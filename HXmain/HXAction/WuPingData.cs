using HXmain.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;
using WSTools.WSSerialize;

namespace HXmain.HXAction
{
    [Serializable]
    public class WuPingData
    {
        private static string _R = AppDomain.CurrentDomain.BaseDirectory;
        private static string WUPING_PATH = Directory.CreateDirectory(_R + "物品").FullName + "/";
        private static string WUPING_TEMP = Directory.CreateDirectory(WUPING_PATH + "temp").FullName + "/";

        /// <summary>
        /// 缓存文件名
        /// </summary>
        private static readonly string WUPING_BIN = AppDomain.CurrentDomain.BaseDirectory + "pageData.bin";
        private static Dictionary<string, WuPingData> All_Wuping = new Dictionary<string, WuPingData>();
        public static WuPingData getWuping(Bitmap bmp)
        {
            if (bmp == null) {
                return null;
            }
            string key = ImageTool.UUIDImg(bmp);
            foreach (var k in All_Wuping)
            {
                if (k.Value.ImgID == key)
                {
                    return k.Value;
                }
            }
           // All_Wuping.Add(key, new WuPingData());
            bmp.Save(WUPING_TEMP + key + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            return new WuPingData();
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        public bool isEmpty
        {
            get
            {
                return Name == "空";
            }
        }

        private static void addEmpt()
        {
            if (!All_Wuping.ContainsKey("空"))
            {
                string imgID = "EC79806407ED73AB4765CEB8FAC02D15";
                WuPingData wd = new WuPingData();
                wd.Name = "空";
                wd.ImgID = imgID;
                All_Wuping.Add(wd.Name, wd);
            }
        }

        private static void initLocalPath(string path, WuPingData config)
        {
            initPath(WUPING_PATH + path, config);
        }

        private static void initPath(string path, WuPingData config)
        {
            if (!File.Exists(WUPING_BIN))
            {
                addEmpt();
                BinSerialize.toFile(WUPING_BIN, All_Wuping);
            }
            All_Wuping = BinSerialize.fromFile(WUPING_BIN) as Dictionary<string, WuPingData>;
            addEmpt();
            Directory.CreateDirectory(path);
            string[] files = Directory.GetFiles(path);
            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    string filep = files[i];
                    Bitmap bt = new Bitmap(files[i]);
                    WuPingData wd = new WuPingData();
                    wd.ImgID = ImageTool.UUIDImg(bt);
                    wd.Name = Path.GetFileNameWithoutExtension(filep);
                    wd.Level = config.Level;
                    wd.CanTiLian = config.CanTiLian;
                    wd.isZuangBei = config.isZuangBei;
                    wd.isYP = config.isYP;
                    wd.CanRigtUse = config.CanRigtUse;
                    wd.isThrow = config.isThrow;
                    bt.Dispose();
                    if (All_Wuping.ContainsKey(wd.Name))
                    {
                        All_Wuping.Remove(wd.Name);
                    }
                    All_Wuping.Add(wd.Name, wd);
                }
                catch (Exception ex)
                {
                    ex.StackTrace.ToString();
                }

            }
            BinSerialize.toFile(WUPING_BIN, All_Wuping);
        }
        static WuPingData()
        {
            WuPingData wd = new WuPingData();
            wd.CanTiLian = true;
            wd.Level = 1;
            wd.isZuangBei = true;
            initLocalPath("装备", wd);

            //药品初始化
            wd = new WuPingData();
            wd.isYP = true;
            wd.CanRigtUse = true;
            wd.isZuangBei = true;
            initLocalPath("药品", wd);

            //消耗品初始化 
            wd = new WuPingData();
            wd.CanRigtUse = true;
            initLocalPath("消耗", wd);

            //丢弃物品
            wd = new WuPingData();
            wd.isThrow = true;
            initLocalPath("丢弃物品", wd);

            //药品初始化
            wd = new WuPingData();
            initLocalPath("其它", wd);
            initLocalPath("战魂食物", wd);
            initLocalPath("进阶技能书", wd);
            initLocalPath("配方", wd);
            initLocalPath("晶石", wd);
            initLocalPath("修练", wd);
            initLocalPath("魂魄", wd);
            initLocalPath("宝石", wd);
            initLocalPath("矿石", wd);
        }

        /// <summary>
        /// 物品名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否为装备
        /// </summary>
        public bool isZuangBei { get; set; }
        /// <summary>
        /// 是否为药品
        /// </summary>
        public bool isYP { get; set; }
        /// <summary>
        /// 是否为丢弃物品
        /// </summary>
        public bool isThrow { get; set; }

        /// <summary>
        /// 档次
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// 是否要以提练
        /// </summary>
        public bool CanTiLian { get; private set; }
        /// <summary>
        /// 是否可以右键使用
        /// </summary>
        public bool CanRigtUse { get; private set; }

        /// <summary>
        /// 图片ID
        /// </summary>
        public string ImgID { get; private set; }
    }
}
