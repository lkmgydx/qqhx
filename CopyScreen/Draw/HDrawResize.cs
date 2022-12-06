using CopyScreen.Draw.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyScreen.Draw
{
    class HDrawResize
    {
        private HDraw _hdraw = new HDraw();

        /// <summary>
        /// 要重置大小的矩形区域
        /// </summary>
        public HDraw StartDraw
        {
            get
            {
                return _hdraw;
            }
            set
            {
                if (value == null)
                {
                    _hdraw = new HDraw();
                }
                else
                {
                    _hdraw = value;
                }
                ResultDraw = _hdraw.Clone() as HDraw;
            }
        }

        private Point StartPoint;

        /// <summary>
        /// 结果hdraw;
        /// </summary>
        public HDraw ResultDraw { get; private set; } = new HDraw();

        public DrawModelDetail Model { get; set; }

        public HDrawResize() { }

        public HDrawResize(HDraw draw)
        {
            StartDraw = draw;
        }

        public void Start(int x, int y)
        {
            StartPoint = new Point(x, y);
        }

        public void End(int x, int y)
        {
            var ox = StartPoint.X;
            var oy = StartPoint.Y;
            switch (Model)
            {
                case DrawModelDetail.LeftTop: ResultDraw.Start(x, y); break;
                case DrawModelDetail.Top: ResultDraw.Start(ox, y); break;
                case DrawModelDetail.RightTop: ResultDraw.Start(ox, y); ResultDraw.End(x, oy); break;

                case DrawModelDetail.Left: ResultDraw.Start(x, oy); break;
                case DrawModelDetail.Right: ResultDraw.End(x, oy); break;

                case DrawModelDetail.LeftBottom: ResultDraw.Start(x, oy); ResultDraw.End(ox, y); break;
                case DrawModelDetail.Bottom: ResultDraw.End(ox, y); break;
                case DrawModelDetail.RightBottom: ResultDraw.End(x, y); break;
                case DrawModelDetail.Move:
                    {
                        int fx = StartDraw.MainRec.X - ox;
                        int fy = StartDraw.MainRec.Y - oy;
                        ResultDraw.Start(StartDraw.MainRec.X + x, StartDraw.MainRec.Y + y);
                        ResultDraw.End(StartDraw.MainRec.X + StartDraw.MainRec.Width + x, StartDraw.MainRec.Y + StartDraw.MainRec.Height + y);
                        break;
                    }

                    //default: return;
            }
        }
    }
}
