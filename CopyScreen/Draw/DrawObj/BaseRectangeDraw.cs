 
using CopyScreen.Draw.Baseinfo;
using CopyScreen.Draw.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyScreen.Draw.DrawObj
{
    abstract class BaseRectangeDraw : BaseDraw, IOPRegionPoints
    {
        public Side8 R8 { get; private set; }
        public BaseRectangeDraw()
        {
            DrawColor = Color.Red;
        }

        private List<Point> _liRegionPoints = new List<Point>();

        private Rectangle _drawRectangel;
        /// <summary>
        /// 操作的矩形区域
        /// </summary>
        public virtual Rectangle DrawRectangle
        {
            get { return _drawRectangel; }
            set
            {
                _drawRectangel = value;
                _liRegionPoints = new List<Point>(PointsUtils.getAll(DrawRectangle));
                R8 = new Side8(value, Config._8PointWidth, Config._8PointHeight);
            }
        }

        private Color _color;
        /// <summary>
        /// 绘制的颜色
        /// </summary>
        public virtual Color DrawColor { get { return _color; } set { _color = value; DrawPen = new Pen(value); DrawBrush = new SolidBrush(value); } }

        protected Pen DrawPen { get; private set; }

        protected Brush DrawBrush { get; private set; }

        /// <summary>
        /// 是否在范围内
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool isContain(Point p)
        {
            return _liRegionPoints.Contains(p);
        }

        /// <summary>
        /// 获取当前坐标点模式
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual DrawModelDetail getModel(Point e)
        {
            if (R8.lt.Contains(e.X, e.Y))
            {
                return DrawModelDetail.LeftTop;
            }
            else if (R8.rt.Contains(e.X, e.Y))
            {
                return DrawModelDetail.RightTop;
            }
            else if (R8.lb.Contains(e.X, e.Y))
            {
                return DrawModelDetail.LeftBottom;
            }
            else if (R8.rb.Contains(e.X, e.Y))
            {
                return DrawModelDetail.RightBottom;
            }
            else if (R8.top.Contains(e.X, e.Y))
            {
                return DrawModelDetail.Top;
            }
            else if (R8.left.Contains(e.X, e.Y))
            {
                return DrawModelDetail.Left;
            }
            else if (R8.right.Contains(e.X, e.Y))
            {
                return DrawModelDetail.Right;
            }
            else if (R8.bottom.Contains(e.X, e.Y))
            {
                return DrawModelDetail.Bottom;
            }
            return DrawModelDetail.Undefined;
        }

        public virtual Point[] getRegionPoints()
        {
            return _liRegionPoints.ToArray();
        }
    }
}
