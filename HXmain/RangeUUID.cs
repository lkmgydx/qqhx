using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain
{
    public class RangeUUID
    {
        public static RangeUUID 系统聊天显示可见 = new RangeUUID(37, 581, 10, 8, new string[] { "8C435DC9565EE52F519F4BA13F81A31E", "9CDC35F53A6CA33021D425D44485FE85" });
        public static RangeUUID 人物聊天显示可见 = new RangeUUID(37, 704, 9, 11, new string[] { "EB1F1A8BBD2F9362E4CA9F81F4E5D3E8", "8E20245154EF5F8FDD0836F8053D870F" });
        //REC
        public Rectangle Rec { get; private set; }
        /// <summary>
        /// 唯一值
        /// </summary>
        public string UUID { get; private set; }

        string[] uuids = new string[] { };

        public bool inUUIDS(string uuid)
        {
            for (int i = 0; i < uuids.Length; i++)
            {
                if (uuids[i] == uuid)
                {
                    return true;
                }
            }
            return false;
        }

        public RangeUUID(int x, int y, int width, int height, string uuid) : this(x, y, width, height, new string[] { uuid })
        {
        }

        public RangeUUID(int x, int y, int width, int height, string[] uuid)
        {
            Rec = new Rectangle(x, y, width, height);
            if (uuid.Length == 0)
            {
                throw new Exception("uuid 大小为0");
            }
            UUID = uuid[0];
            uuids = uuid;
        }
    }
}
