using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyScreen.Draw.Baseinfo
{
    /// <summary>
    /// 操作的区域
    /// </summary>
    interface IOPRegionPoints
    {
        Point[] getRegionPoints();
    }
}
