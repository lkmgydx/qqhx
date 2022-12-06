using CopyScreen.Draw.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyScreen.Draw
{
    /// <summary>
    /// 拖拽支持类
    /// </summary>
    class HDraw : ICloneable
    {
        Point StartPoint { get; set; }
        Point EndPoint { get; set; }

        bool changed = false;
        Rectangle _ChacheMainRec = new Rectangle();

        /// <summary>
        /// 主绘制区域
        /// </summary>
        public Rectangle MainRec
        {
            get
            {
                if (changed)
                {
                    _ChacheMainRec = Utils.getRec(StartPoint, EndPoint);
                }
                return _ChacheMainRec;
            }
        }

        public void Start(int x, int y)
        {
            StartPoint = new Point(x, y);
            changed = true;
        }

        public void End(int x, int y)
        {
            EndPoint = new Point(x, y);
            changed = true;
        }

        public object Clone()
        {
            HDraw d = new HDraw();
            d.StartPoint = StartPoint;
            d.EndPoint = EndPoint;
            d.changed = true;
            return d;
        }
    }
}
