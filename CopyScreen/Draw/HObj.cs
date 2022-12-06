 
using CopyScreen.Draw.Baseinfo;
using CopyScreen.Draw.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CopyScreen.Draw
{
    internal abstract class HObj
    {
        public Bitmap OrgBmp { get; protected set; }
        public Bitmap BackBmp { get; protected set; }
        public virtual Graphics G { get;  set; }

        private HDraw hDraw = new HDraw();

        public bool isMouseUp { get; set; }
        public bool isShift { get; set; }

        /// <summary>
        /// 绘制主框
        /// </summary>
        public Rectangle DrawMainRec
        {
            get;
            protected set;
        }

        public Rectangle MaxRec { get; protected set; }

        /// <summary>
        /// 托动矩形初始位置
        /// </summary>
        public Point DrawMoveStart;
        /// <summary>
        /// 矩形8边框
        /// </summary>
        protected Side8 R8 = new Side8(Rectangle.Empty);
        /// <summary>
        /// 绘制模式
        /// </summary>
        public DrawModel Model { get; protected set; }

        public DrawModelDetail DrawDetailModel { get; protected set; }

        internal virtual void OnMouseDown(MouseEventArgs e)
        {
            isMouseUp = false;
            if (e.Button == MouseButtons.Left)
            {
                if (Model == DrawModel.Nomal)
                {
                    Model = DrawModel.MouseDown;
                    //StartPoint = new Point(e.X, e.Y);
                    hDraw.Start(e.X, e.Y);
                }
                else if (Model == DrawModel.MouseUp)
                {

                    if (R8.lt.Contains(e.X, e.Y))
                    {
                        DrawDetailModel = DrawModelDetail.LeftTop;
                    }
                    else if (R8.rt.Contains(e.X, e.Y))
                    {
                        DrawDetailModel = DrawModelDetail.RightTop;
                    }
                    else if (R8.lb.Contains(e.X, e.Y))
                    {
                        DrawDetailModel = DrawModelDetail.LeftBottom;
                    }
                    else if (R8.rb.Contains(e.X, e.Y))
                    {
                        DrawDetailModel = DrawModelDetail.RightBottom;
                    }
                    else if (R8.top.Contains(e.X, e.Y))
                    {
                        DrawDetailModel = DrawModelDetail.Top;
                    }
                    else if (R8.left.Contains(e.X, e.Y))
                    {
                        DrawDetailModel = DrawModelDetail.Left;
                    }
                    else if (R8.right.Contains(e.X, e.Y))
                    {
                        DrawDetailModel = DrawModelDetail.Right;
                    }
                    else if (R8.bottom.Contains(e.X, e.Y))
                    {
                        DrawDetailModel = DrawModelDetail.Bottom;
                    }
                    else if (DrawMainRec.Contains(e.X, e.Y))
                    {
                        DrawDetailModel = DrawModelDetail.Move;
                        DrawMoveStart = new Point(e.X, e.Y);
                        //TStartPoint = StartPoint;
                        //TendPoinnt = EndPoint;
                    }
                    else
                    {
                        DrawDetailModel = DrawModelDetail.Undefined;
                    }
                }
            }
        }

        internal virtual void onMouseUp(MouseEventArgs e)
        {
            onCursorChangeEvent?.Invoke(Cursors.Default);
            if (Model == DrawModel.MouseDown)
            {
                Model = DrawModel.MouseUp;
            }
            else if (Model == DrawModel.MouseUp)
            {
                DrawDetailModel = DrawModelDetail.Undefined;
            }
            isMouseUp = true;
            render();
        }

        public delegate void CursorEventHandler(Cursor cursor);

        public event CursorEventHandler onCursorChangeEvent;

        public event EventHandler onRefresh;

        public virtual void render()
        {
            onRefresh?.Invoke(this, null);
        }


        internal virtual void OnMouseMove(MouseEventArgs e)
        {
            if (isShift)
            {
                onCursorChangeEvent?.Invoke(Cursors.Default);
                render();
                return;
            }

            if (Model == DrawModel.MouseDown)
            {
                onCursorChangeEvent?.Invoke(Cursors.Arrow);
                //EndPoint = new Point(e.X, e.Y);
                hDraw.End(e.X, e.Y);
            }
            else if (Model == DrawModel.MouseUp)
            {
               
                if (R8.lt.Contains(e.X, e.Y))
                {
                    onCursorChangeEvent?.Invoke(Cursors.SizeNWSE);
                }
                else if (R8.rt.Contains(e.X, e.Y))
                {
                    onCursorChangeEvent?.Invoke(Cursors.SizeNESW);
                }
                else if (R8.lb.Contains(e.X, e.Y))
                {
                    onCursorChangeEvent?.Invoke(Cursors.SizeNESW);
                }
                else if (R8.rb.Contains(e.X, e.Y))
                {
                    onCursorChangeEvent?.Invoke(Cursors.SizeNWSE);
                }
                else if (R8.top.Contains(e.X, e.Y))
                {
                    onCursorChangeEvent?.Invoke(Cursors.SizeNS);
                }
                else if (R8.left.Contains(e.X, e.Y))
                {
                    onCursorChangeEvent?.Invoke(Cursors.SizeWE);
                }
                else if (R8.right.Contains(e.X, e.Y))
                {
                    onCursorChangeEvent?.Invoke(Cursors.SizeWE);
                }
                else if (R8.bottom.Contains(e.X, e.Y))
                {
                    onCursorChangeEvent?.Invoke(Cursors.SizeNS);
                }
                else if (R8.mainRec.Contains(e.X, e.Y))
                {
                    onCursorChangeEvent?.Invoke(Cursors.SizeAll);
                }
                else
                {
                    onCursorChangeEvent?.Invoke(Cursors.Default);
                }

                if (DrawDetailModel == DrawModelDetail.Undefined)
                {
                    return;
                }


                //if (DrawDetailModel == DrawModelDetail.Right)
                //{
                //    EndPoint = new Point(e.X, EndPoint.Y);
                //}
                switch (DrawDetailModel)
                {
                    case DrawModelDetail.Undefined: return;


                    //case DrawModelDetail.LeftTop: StartPoint = new Point(e.X, e.Y); break;
                    //case DrawModelDetail.Top: StartPoint = new Point(StartPoint.X, e.Y); break;
                    //case DrawModelDetail.RightTop: StartPoint = new Point(StartPoint.X, e.Y); EndPoint = new Point(e.X, EndPoint.Y); break;

                    //case DrawModelDetail.Left: StartPoint = new Point(e.X, StartPoint.Y); break;
                    //case DrawModelDetail.Right: EndPoint = new Point(e.X, EndPoint.Y); break;

                    //case DrawModelDetail.LeftBottom: StartPoint = new Point(e.X, StartPoint.Y); EndPoint = new Point(EndPoint.X, e.Y); break;
                    //case DrawModelDetail.Bottom: EndPoint = new Point(EndPoint.X, e.Y); break;
                    //case DrawModelDetail.RightBottom: EndPoint = new Point(e.X, e.Y); break;
                    case DrawModelDetail.Move:
                        {
                            //int fx = e.X - DrawMoveStart.X;
                            //int fy = e.Y - DrawMoveStart.Y;
                            //StartPoint = new Point(TStartPoint.X + fx, TStartPoint.Y + fy);
                            //EndPoint = new Point(TendPoinnt.X + fx, TendPoinnt.Y + fy);
                            break;
                        }

                        //default: return;
                }
            }
            else
            {
                return;
            }

            //DrawMainRec = Utils.getRec(StartPoint, EndPoint);
            DrawMainRec = hDraw.MainRec;
            R8 = new Side8(DrawMainRec);
            render();
            //refreshRec(EndPoint, e);
        }

        internal abstract Bitmap CloneImg(Rectangle drawMainRec);

        internal virtual void onKeyUp(KeyEventArgs e)
        {
            if (e.Shift)
            {
                return;
            }
            isShift = false;
            render();
        }
    }
}
