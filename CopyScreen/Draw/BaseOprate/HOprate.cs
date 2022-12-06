
using CopyScreen.Draw.Baseinfo;
using CopyScreen.Draw.DrawObj;
using CopyScreen.Draw.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CopyScreen.Draw.BaseOprate
{
    abstract class HOprate : IOprate
    {
        public BaseRectangeDraw DrawObj { get; private set; }
        public HOprate(BaseRectangeDraw br)
        {
            DrawObj = br;
        }
        /// <summary>
        /// 缩放信息
        /// </summary>
        public HDrag HDrag_Zoom { get; set; } = new HDrag();

        /// <summary>
        /// 拖动信息
        /// </summary>
        public HDrag HDrag_Move { get; set; } = new HDrag();

        /// <summary>
        /// 鼠标是否松开
        /// </summary>
        protected bool isMouseUp { get; set; }

        /// <summary>
        /// 绘制主框
        /// </summary>
        public Rectangle DrawMainRec
        {
            get { return DrawObj.DrawRectangle; }
        }
        /// <summary>
        /// 操作模式
        /// </summary>
        public HOprateModel OprateModel { get; set; }

        /// <summary>
        /// 矩形8边框
        /// </summary>
        protected Side8 R8 = new Side8(Rectangle.Empty);
        /// <summary>
        /// 绘制模式
        /// </summary>
        public DrawModel DrawModel { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public DrawModelDetail DrawDetailModel { get; protected set; }

        public Cursor Cursor { get; private set; } = Cursors.Default;

        public virtual void OnMouseDown(MouseEventArgs e)
        {
            isMouseUp = false;
            if (DrawModel == DrawModel.Nomal)
            {
                DrawModel = DrawModel.MouseDown;
                HDrag_Move.Start = e.Location;
            }
            //else if (Model == DrawModel.MouseUp)
            //{

            //    if (R8.lt.Contains(e.X, e.Y))
            //    {
            //        DrawDetailModel = DrawModelDetail.LeftTop;
            //    }
            //    else if (R8.rt.Contains(e.X, e.Y))
            //    {
            //        DrawDetailModel = DrawModelDetail.RightTop;
            //    }
            //    else if (R8.lb.Contains(e.X, e.Y))
            //    {
            //        DrawDetailModel = DrawModelDetail.LeftBottom;
            //    }
            //    else if (R8.rb.Contains(e.X, e.Y))
            //    {
            //        DrawDetailModel = DrawModelDetail.RightBottom;
            //    }
            //    else if (R8.top.Contains(e.X, e.Y))
            //    {
            //        DrawDetailModel = DrawModelDetail.Top;
            //    }
            //    else if (R8.left.Contains(e.X, e.Y))
            //    {
            //        DrawDetailModel = DrawModelDetail.Left;
            //    }
            //    else if (R8.right.Contains(e.X, e.Y))
            //    {
            //        DrawDetailModel = DrawModelDetail.Right;
            //    }
            //    else if (R8.bottom.Contains(e.X, e.Y))
            //    {
            //        DrawDetailModel = DrawModelDetail.Bottom;
            //    }
            //    else if (DrawMainRec.Contains(e.X, e.Y))
            //    {
            //        DrawDetailModel = DrawModelDetail.Move;
            //        DrawMoveStart = new Point(e.X, e.Y);
            //        TStartPoint = StartPoint;
            //        TendPoinnt = EndPoint;
            //    }
            //    else
            //    {
            //        DrawDetailModel = DrawModelDetail.Undefined;
            //    }
            //}
        }

        public virtual void onMouseUp(MouseEventArgs e)
        {
            isMouseUp = true;
        }

        public virtual void OnMouseMove(MouseEventArgs e)
        {
            if (OprateModel != HOprateModel.None && !isMouseUp)
            {

                if (OprateModel == HOprateModel.Move)//移动模式
                {
                    HDrag_Move.End = e.Location;
                    Cursor = Cursors.Arrow;
                }
                else if (OprateModel == HOprateModel.Resize) //缩放模式
                {
                    //if(DrawModel)
                }
            }

            //else if (DrawModel == DrawModel.MouseUp)
            //{

            //    if (R8.lt.Contains(e.X, e.Y))
            //    {
            //        onCursorChangeEvent?.Invoke(Cursors.SizeNWSE);
            //    }
            //    else if (R8.rt.Contains(e.X, e.Y))
            //    {
            //        onCursorChangeEvent?.Invoke(Cursors.SizeNESW);
            //    }
            //    else if (R8.lb.Contains(e.X, e.Y))
            //    {
            //        onCursorChangeEvent?.Invoke(Cursors.SizeNESW);
            //    }
            //    else if (R8.rb.Contains(e.X, e.Y))
            //    {
            //        onCursorChangeEvent?.Invoke(Cursors.SizeNWSE);
            //    }
            //    else if (R8.top.Contains(e.X, e.Y))
            //    {
            //        onCursorChangeEvent?.Invoke(Cursors.SizeNS);
            //    }
            //    else if (R8.left.Contains(e.X, e.Y))
            //    {
            //        onCursorChangeEvent?.Invoke(Cursors.SizeWE);
            //    }
            //    else if (R8.right.Contains(e.X, e.Y))
            //    {
            //        onCursorChangeEvent?.Invoke(Cursors.SizeWE);
            //    }
            //    else if (R8.bottom.Contains(e.X, e.Y))
            //    {
            //        onCursorChangeEvent?.Invoke(Cursors.SizeNS);
            //    }
            //    else if (R8.mainRec.Contains(e.X, e.Y))
            //    {
            //        onCursorChangeEvent?.Invoke(Cursors.SizeAll);
            //    }
            //    else
            //    {
            //        onCursorChangeEvent?.Invoke(Cursors.Default);
            //    }

            //    if (DrawDetailModel == DrawModelDetail.Undefined)
            //    {
            //        return;
            //    }


            //    if (DrawDetailModel == DrawModelDetail.Right)
            //    {
            //        EndPoint = new Point(e.X, EndPoint.Y);
            //    }
            //    switch (DrawDetailModel)
            //    {
            //        case DrawModelDetail.Undefined: return;


            //        case DrawModelDetail.LeftTop: StartPoint = new Point(e.X, e.Y); break;
            //        case DrawModelDetail.Top: StartPoint = new Point(StartPoint.X, e.Y); break;
            //        case DrawModelDetail.RightTop: StartPoint = new Point(StartPoint.X, e.Y); EndPoint = new Point(e.X, EndPoint.Y); break;

            //        case DrawModelDetail.Left: StartPoint = new Point(e.X, StartPoint.Y); break;
            //        case DrawModelDetail.Right: EndPoint = new Point(e.X, EndPoint.Y); break;

            //        case DrawModelDetail.LeftBottom: StartPoint = new Point(e.X, StartPoint.Y); EndPoint = new Point(EndPoint.X, e.Y); break;
            //        case DrawModelDetail.Bottom: EndPoint = new Point(EndPoint.X, e.Y); break;
            //        case DrawModelDetail.RightBottom: EndPoint = new Point(e.X, e.Y); break;
            //        case DrawModelDetail.Move:
            //            {
            //                int fx = e.X - DrawMoveStart.X;
            //                int fy = e.Y - DrawMoveStart.Y;
            //                StartPoint = new Point(TStartPoint.X + fx, TStartPoint.Y + fy);
            //                EndPoint = new Point(TendPoinnt.X + fx, TendPoinnt.Y + fy);
            //                break;
            //            }

            //        default: return;
            //    }
            //}
            //else
            //{
            //    return;
            //}

            //DrawMainRec = Utils.getRec(StartPoint, EndPoint);
            //R8 = new Side8(DrawMainRec);
            //render();
        }

        public void onKeyUp(KeyEventArgs e)
        {

        }
    }
}
