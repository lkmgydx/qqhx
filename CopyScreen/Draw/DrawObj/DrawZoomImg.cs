
using CopyScreen.Draw.Baseinfo;
using CopyScreen.Draw.DrawObj;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CopyScreen.Config;

namespace CopyScreen.Draw
{
    /// <summary>
    /// 绘制缩放图片
    /// </summary>
    internal class DrawZoomImg : BaseDraw
    {
        /// <summary>
        /// 要绘制的坐标点
        /// </summary>
        public Point DrawPoint { get; set; }


        public Bitmap bmp
        {
            get { return DrawSmallZoomImg.Bitmap; }
            set { DrawSmallZoomImg.Bitmap = value; }
        }
        /// <summary>
        /// 与原始点的间距
        /// </summary>
        public Point Margin { get; set; } = new Point(10, 50);

        public Size DrawSize = new Size(50, 50);
        /// <summary>
        /// 矩形大小
        /// </summary>
        public Size RecSize = new Size(150, 150);

        private Size ScreenMaxSize { get; set; } = ScreenSize;


        DrawSmallZoomImg DrawSmallZoomImg = new DrawSmallZoomImg();

        DrawMsg DrawMsg = new DrawMsg();
        DrawCrossRectange DrawRectange = new DrawCrossRectange();

        public override void render(Graphics gg)
        {
            //放大的小图矩形范围
            Rectangle rec_ZoomSmall = new Rectangle(DrawPoint.X + Margin.X, DrawPoint.Y + Margin.Y, SmallImgSize.Width, SmallImgSize.Height);

            if (rec_ZoomSmall.X + rec_ZoomSmall.Width > ScreenMaxSize.Width)
            {
                rec_ZoomSmall.X = rec_ZoomSmall.X - rec_ZoomSmall.Width - 100;
            }
            if (rec_ZoomSmall.Y + rec_ZoomSmall.Height > ScreenMaxSize.Height)
            {
                rec_ZoomSmall.Y = rec_ZoomSmall.Y - rec_ZoomSmall.Height - 100;
            }

            //画缩略图
            DrawSmallZoomImg.RectangleZoom = rec_ZoomSmall;
            DrawSmallZoomImg.Rectangle = new Rectangle(DrawPoint.X - DrawSize.Width / 2, DrawPoint.Y - DrawSize.Height / 2, DrawSize.Width, DrawSize.Height);
            DrawSmallZoomImg.render(gg);


            //绘制上方文字黑色背景
            DrawMsg.Color = bmp.GetPixel(DrawPoint.X, DrawPoint.Y);
            DrawMsg.Rectangle = new Rectangle(rec_ZoomSmall.X, rec_ZoomSmall.Y - 20 - 16, 150, 60);
            DrawMsg.DrawRectangle = new Rectangle(DrawPoint, RecSize);
            DrawMsg.render(gg);

            //绘制 红色十字架
            DrawRectange.LeftTopPoint = rec_ZoomSmall.Location;
            DrawRectange.render(gg);
        }
    }
}
