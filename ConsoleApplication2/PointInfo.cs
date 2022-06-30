using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ConsoleApplication2
{
    /// <summary>
    /// 坐标方向
    /// </summary>
    public enum PointAction
    {
        LeftTop,
        Top,
        RightTop,
        Left,
        Right,
        BottomLeft,
        Bottom,
        BottomRight, Unknow
    }

    /// <summary>
    /// 位置信息
    /// </summary>
    public class PointInfo
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool Suc { get; set; }

        public PointInfo LeftTop { get; set; }
        public PointInfo Top { get; set; }

        public PointInfo RightTop { get; set; }
        public PointInfo Left { get; set; }

        public PointInfo Right { get; set; }
        public PointInfo BottomLeft { get; set; }
        public PointInfo Bottom { get; set; }
        public PointInfo BottomRight { get; set; }
        /// <summary>
        /// 坐标方向
        /// </summary>
        public PointAction PointAction { get; set; }

        public bool HasDeal { get; set; }

        public PointInfo[] Next()
        {
            List<PointInfo> li = new List<PointInfo>();
            if (!Suc)
            {
                return li.ToArray();
            }
            addPointInfo(li, LeftTop);
            addPointInfo(li, Top);
            addPointInfo(li, RightTop);
            addPointInfo(li, Left);
            addPointInfo(li, Right);
            addPointInfo(li, BottomLeft);
            addPointInfo(li, Bottom);
            addPointInfo(li, BottomRight);
            return li.ToArray();
        }

        private void addPointInfo(List<PointInfo> ps, PointInfo p)
        {
            if (p != null && p.Suc)
            {
                ps.Add(p);
            }
        }

        public PointInfo(int x, int y, bool isSuc)
        {
            X = x;
            Y = y;
            Suc = isSuc;
        }
    }
}
