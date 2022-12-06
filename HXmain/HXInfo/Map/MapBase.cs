using HXmain.HXAction;
using HXmain.HXInfo.NPC;
using HXmain.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tool;
using WSTools.WSLog;

namespace HXmain.HXInfo.Map
{

    public class MapBase : AbsMap
    {
        private static List<MapBase> maps = new List<MapBase>();

        public sealed class Maps
        {
            public static Map昆仑墟 昆仑虚 { get; private set; }
            public static Map天圣原 天圣原 { get; private set; }
            public static MapBase 朝歌山一层 = new MapBase("朝歌山一层", "66C8A95CE928D1A4987B90F72A993D99");
            public static MapBase 朝歌山二层 = new Map朝歌山二层();
            public static MapBase 从雨沼泽 = new MapBase("从雨沼泽", "3D79497E761B7211CD301EA02F081AEC");
            public static Map升龙殿 升龙殿 { get; private set; } = new Map升龙殿();
            public static MapBase 校场入口 = new MapBase("校场入口", "5F5BAED81FB1EB231316AD015E0287D3");
            public static MapBase 香魂冢 = new MapBase("香魂冢", "58E0481A3C04B673EB4515E951BFCDDE");
            public static MapBase 行商之路 = new MapBase("行商之路", "B9CFAE348E70DA00860E18DCEF51083F");

            public static MapBase 魔化鬼墓 = new MapBase("魔化鬼墓", "BDDCDFE588E99DB5D3B6E79C0E48EC55,DD999A66509D313497BCFB7DE0994FD3");

            public static MapBase 逆水庭 = new MapBase("逆水庭", "63566909CC1094D4EF7B9BED2AE2478B");
            public static MapBase 诡误大泽 = new MapBase("诡误大泽", "FB7D65CAE5821E6BD93356425AAEC1BA");
            public static MapBase 昆仑虚二层 = new MapBase("昆仑虚二层", "86626BE0C31EB91ED2E7118C14E8378487703949");

            public static Map北郡5 北郡5 { get; private set; } = Map北郡5.m5;
            public static Map监牢 监牢 { get; private set; } = Map监牢.M;
            static Maps()
            {
                maps.Add(Maps.升龙殿);
                maps.Add(天圣原 = Map天圣原.Map);
                maps.Add(昆仑虚 = Map昆仑墟.Map);
                maps.Add(诡误大泽);
                maps.Add(昆仑虚二层);
                maps.Add(香魂冢);
                maps.Add(从雨沼泽);
                maps.Add(魔化鬼墓);
                maps.Add(逆水庭);
                maps.Add(北郡5);
                maps.Add(监牢);

                maps.Add(new MapBase("香魂冢", "58E0481A3C04B673EB4515E951BFCDDE"));
                maps.Add(new MapBase("北郡（一）", "C2874F58270A13BFE38A5502D08A0447"));
                maps.Add(new MapBase("北郡（二）", "C1ECABE6E3F7691449754A922690DDE1"));
                maps.Add(new MapBase("北郡（三）", "2FE1EB8D846CECB2E47B26CEB00AA75B"));
                maps.Add(new MapBase("北郡（四）", "13ABC4E78869E13482EE0E9AE91257B5"));
                //maps.Add(new MapBase("北郡（五）", "3E9369BF883A94C80254C5E91D976506"));
                maps.Add(new MapBase("北郡（六）", "21AD8E0400786F524CCC59D898E7C8DF"));
                maps.Add(new MapBase("北郡（七）", "7525A705ED7BB0AEDF2933884F024227"));
                maps.Add(new MapBase("北郡（八）", "A4030FC8F2194877FAD3FFA0452AF7A3"));

                maps.Add(new MapBase("东离草原", "85B8ACEBACCDE7EBEFCDF193545D664E"));

                maps.Add(new MapBase("南郡", "B80E29E2CF811E85AB172516DE4513D4"));
                maps.Add(new MapBase("宁海州", "8258D2EA27BF3B5E09C26AA3F2E62F16"));
                maps.Add(new MapBase("巨人野", "F2F2BAC7F96284838CC0E37030E2D8CA"));
                maps.Add(new MapBase("断壁谷", "86E1367D07C2089140AFDD51AB961691"));
                maps.Add(new MapBase("昆仑", "16218A5869F8C4431369932E56593092"));

                //maps.Add(new MapBase("昆仑墟", "D0A342CA36078BB92D141241309D0B41"));//v
                maps.Add(new MapBase("暮秀雨林", "2906D317C858EEFA967E4602125AB2A5"));
                maps.Add(new MapBase("林海", "D6F681FE1473A9975AF4C54A12E7BDFB"));
                maps.Add(new MapBase("琅琊盆地", "F5F64FA503A5CD45A018B3682EB48123"));
                maps.Add(new MapBase("秋枫原", "74243FCDDD2518BD937A576565D5E427"));
                maps.Add(new MapBase("西烈荒原", "9DAAB62156C497908F50E9FE1E9F8338"));
                maps.Add(new MapBase("轩辕", "56EBBE00696DD8C10EB68940843D653E"));
                maps.Add(new MapBase("阪泉", "C87F58DA2AD46439ECAB8818B46190B3"));
                //maps.Add(new MapBase("监牢", "3359A9AA974621F3F92C596AA391B7A6,29BEB49F52A9AC02105AC47D607992A7"));
                maps.Add(new MapBase("轩辕台一层", "D4781A025988583C842CE692FEFF1CC5"));
                maps.Add(new MapBase("赤水", "8924AFF3685DE081C6EE8CA0BDA4EAB1"));
                maps.Add(校场入口);
                maps.Add(new MapBase("训练校场", "89F2CA82B6CD03EAE4CA4BB412C1BE57"));
                maps.Add(new MapBase("轩辕台入口", "803956063DD127BFDB09B88B97EECA95"));
                maps.Add(new MapBase("诡墓", "C3A16EE4494282E607745137F271ABED"));
                maps.Add(new MapBase("奇兵洞一层", "472D0C245F66FC5423148D00E077D36A"));

                maps.Add(朝歌山一层);
                maps.Add(朝歌山二层);
                maps.Add(行商之路);
                for (int i = 0; i < maps.Count; i++)
                {
                    maps[i].initNPC();
                }
            }
        }
        public readonly static MapBase STATIC_BASE_MAP = new MapBase();


        static MapBase()
        {
            Maps.天圣原.ToString();
        }
        public static Bitmap NCP_升龙殿_宅院驿丞_BMP = Resources.npc_升龙殿_宅院驿丞310211406;

        public static Bitmap FONT_排行榜 = Resources.font_排行榜;
        //private static Dictionary<string, MapBase> MapCache = new Dictionary<string, MapBase>();

        protected Image CompareIMG = null;

        private List<NPCBase> _allNPC = new List<NPCBase>();
        public List<NPCBase> ALLNPC
        {
            get
            {
                return _allNPC;
            }
            private set
            {
                _allNPC = value;
                if (_allNPC == null)
                {
                    _allNPC = new List<NPCBase>();
                }
            }
        }

        protected virtual void initNPC() { }

        public MapBase()
        {
            MapKey = "000000000";
            Name = "默认";
        }

        public MapBase(string name, string key)
        {
            Name = name;
            MapKey = key;
        }

        private MapBase(string name, Image img)
        {
            this.Name = name;
            this.CompareIMG = img;
            this.MapKey = ImageTool.UUIDImg(img as Bitmap);
        }
        /// <summary>
        /// 坐标系统
        /// </summary>
        public MapPoints MapPoints { get; set; }

        private static Rectangle _MapRectangle = new Rectangle(903, 45, 77, 10);//new Rectangle(900, 42, 77, 14);
        /// <summary>
        /// 地图坐标范围 903 45 77 10
        /// </summary>
        /// 
        public static Rectangle MapRectangle { get { return _MapRectangle; } set { _MapRectangle = value; } }

        public static MapBase getCurrentMap(Image img)
        {
            string imgStr = ImageTool.UUIDImg(img as Bitmap);

            int index = maps.FindIndex((i) =>
             {
                 return i.MapKey.IndexOf(imgStr) != -1;
             });
            if (index != -1)
            {
                return maps[index];
            }
            MapBase mb = new MapBase();
            mb.MapKey = imgStr;
            mb.Name = imgStr;
            maps.Add(mb);
            return mb;
        }

        private static Rectangle centerRange = new Rectangle(300, 160, 618, 434);
        List<Bitmap> sqw = new List<Bitmap>() {
            Resources.w1,Resources.w2,Resources.w3,Resources.w4,Resources.w5,Resources.w6,Resources.w7,Resources.w8,Resources.w9,Resources.w10,Resources.w11,Resources.w12
        };

        /// <summary>
        /// 是否为安全区
        /// </summary>
        private static Rectangle REC_SAFEREGION = new Rectangle(848, 42, 37, 13);
        private static string REC_SAFEREGION_STRING = "56861203ACBCA5E8249D54976F75B21F";

        public bool isSafeRegion(MainGame game)
        {
            return game.UUIDImageRec(REC_SAFEREGION) == REC_SAFEREGION_STRING;
        }

        public void hasWuping(MainGame game)
        {
            List<HSearchPoint> rst = new List<HSearchPoint>();
            using (var img = game.getImg(centerRange))
            {
                for (int i = 0; i < sqw.Count; i++)
                {
                    rst.AddRange(ImageTool.findEqImgAll(sqw[i], img));
                }
            }
            if (rst.Count != 0)
            {
                using (var g = game.getGriphs())
                {
                    for (int i = 0; i < rst.Count; i++)
                    {
                        g.FillRectangle(Brushes.Red, new RectangleF(rst[i].Point, rst[i].SearchSize));
                    }
                }

            }
        }
    }
}
