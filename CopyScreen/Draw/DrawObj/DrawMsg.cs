 
using CopyScreen.Draw.Baseinfo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyScreen.Draw.DrawObj
{
    internal class DrawMsg : BaseDraw
    {
        /// <summary>
        /// 要绘制信息的矩形位置
        /// </summary>
        public Rectangle Rectangle;
        /// <summary>
        /// 显示的颜色信息
        /// </summary>
        public Color Color = Color.Red;
        /// <summary>
        /// 显示矩形信息
        /// </summary>
        public Rectangle DrawRectangle;

        /// <summary>
        /// 十六进制显示颜色
        /// </summary>
        public bool isX16Color { get; set; }

        public override void render(Graphics gg)
        {
            gg.FillRectangle(Brushes.Black, Rectangle);

            Point pt = Rectangle.Location;
            pt.Offset(0, 1);

            gg.DrawString("HYL FOR LMX - forever", new Font("宋体", 8), Brushes.Yellow, pt);
            pt.Offset(10, 12);
            //绘制黑色背景上的文字
            gg.DrawString($"X:{f(DrawRectangle.X)} Y:{f(DrawRectangle.Y)}", new Font("宋体", 10), Brushes.White, pt);//左上角画上坐标
            pt.Offset(0, 15);
            gg.DrawString($"W:{f(DrawRectangle.Width)} H:{f(DrawRectangle.Height)}", new Font("宋体", 10), Brushes.Green, pt);//左上角画上坐标
            pt.Offset(0, 15);
            if (isX16Color)
            {
                gg.DrawString($"RGB:[{ColorTranslator.ToHtml(Color)}]", new Font("宋体", 10), Brushes.White, pt);//左上角画上坐标
            }
            else
            {
                gg.DrawString($"RGB:[{Color.R}:{Color.G}:{Color.B}]", new Font("宋体", 10), Brushes.White, pt);//左上角画上坐标

            }
        }

        public string f(int obj)
        {
            return string.Format("{0,-5}", obj);
        }
    }
}
