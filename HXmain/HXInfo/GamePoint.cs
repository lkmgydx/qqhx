using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain.HXInfo
{
    /// <summary>
    /// 坐标点移动信息
    /// </summary>
    public class GamePoint
    {
        public int X { get; set; }
        public int Y { get; set; }

        public bool Left { get; set; } = true;
        public bool Rigth { get; set; } = true;
        public bool Top { get; set; } = true;
        public bool Bottom { get; set; } = false;
    }
}
