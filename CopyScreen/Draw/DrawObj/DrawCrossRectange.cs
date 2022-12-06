
using CopyScreen.Draw.Baseinfo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyScreen.Draw.DrawObj
{
    internal class DrawCrossRectange : BaseDraw
    {
        /// <summary>
        /// 左上角位置
        /// </summary>
        public Point LeftTopPoint { get; set; }
        /// <summary>
        /// 长度
        /// </summary>
        public int Width { get; set; } = 150;
        /// <summary>
        /// 间距
        /// </summary>
        public int MarginX { get; set; } = 75;

        public int MarginY { get; set; } = 25;
        public override void render(Graphics g)
        {
            g.DrawLine(Pens.Red, new Point(LeftTopPoint.X + MarginX, LeftTopPoint.Y + MarginY), new Point(LeftTopPoint.X + MarginX, LeftTopPoint.Y + Width - MarginY)); //十字架，横线
            g.DrawLine(Pens.Red, new Point(LeftTopPoint.X + MarginY, LeftTopPoint.Y + MarginX), new Point(LeftTopPoint.X + Width - MarginY, LeftTopPoint.Y + MarginX));//十字架，竖线

        }
    }
}
