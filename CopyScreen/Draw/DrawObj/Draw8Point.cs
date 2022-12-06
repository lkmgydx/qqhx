using CopyScreen.Draw; 
using CopyScreen.Draw.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyScreen.Draw.DrawObj
{
    /// <summary>
    /// 绘制8个点
    /// </summary>
    internal class Draw8Point : BaseRectangeDraw
    {
        public PointModel Model { get; set; }

       
        public override void render(Graphics gg)
        {

            if (Model == PointModel.Point)
            {
                gg.FillEllipse(DrawBrush, R8.lt);// 上左
                gg.FillEllipse(DrawBrush, R8.t); //上中
                gg.FillEllipse(DrawBrush, R8.rt); //上右

                gg.FillEllipse(DrawBrush, R8.l); //中左
                gg.FillEllipse(DrawBrush, R8.r); //中右

                gg.FillEllipse(DrawBrush, R8.lb); //下左
                gg.FillEllipse(DrawBrush, R8.b); //下中
                gg.FillEllipse(DrawBrush, R8.rb); //下右
            }
            else if (Model == PointModel.Rectange)
            {
                gg.FillRectangle(DrawBrush, R8.lt);// 上左
                gg.FillRectangle(DrawBrush, R8.t); //上中
                gg.FillRectangle(DrawBrush, R8.rt); //上右

                gg.FillRectangle(DrawBrush, R8.l); //中左
                gg.FillRectangle(DrawBrush, R8.r); //中右

                gg.FillRectangle(DrawBrush, R8.lb); //下左
                gg.FillRectangle(DrawBrush, R8.b); //下中
                gg.FillRectangle(DrawBrush, R8.rb); //下右
            }
            else if (Model == PointModel.EmptyPoint)
            {
                gg.FillRectangle(new SolidBrush(Color.FromArgb(1,0,0,0 )), R8.lt);
                gg.DrawEllipse(DrawPen, R8.lt);// 上左
                gg.DrawEllipse(DrawPen, R8.t); //上中
                gg.DrawEllipse(DrawPen, R8.rt); //上右

                gg.DrawEllipse(DrawPen, R8.l); //中左
                gg.DrawEllipse(DrawPen, R8.r); //中右

                gg.DrawEllipse(DrawPen, R8.lb); //下左
                gg.DrawEllipse(DrawPen, R8.b); //下中
                gg.DrawEllipse(DrawPen, R8.rb); //下右
            }
        }



        //private Rectangle getRec8(int x, int y)
        //{
        //    return new Rectangle(x - Width / 2, y - Height / 2, Width, Height);
        //}
    }
}
