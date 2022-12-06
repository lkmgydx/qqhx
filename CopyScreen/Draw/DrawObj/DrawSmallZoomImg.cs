
using CopyScreen.Draw.Baseinfo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyScreen.Draw.DrawObj
{
    internal class DrawSmallZoomImg : BaseDraw
    {
        public Rectangle Rectangle { get; set; }

        public Rectangle RectangleZoom { get; set; }

        public Bitmap Bitmap { get; set; }

        public override void render(Graphics gg)
        {
            gg.DrawImage(Bitmap, RectangleZoom, Rectangle, GraphicsUnit.Pixel);//指针范围的小图


            //绘制黑色边框
            gg.DrawRectangle(Pens.Black, RectangleZoom);
        }
    }
}
