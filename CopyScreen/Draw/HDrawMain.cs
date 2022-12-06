using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CopyScreen.Utils;
using static CopyScreen.Config;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using CopyScreen.Draw.DrawObj;
using CopyScreen.Draw.Enums;

namespace CopyScreen.Draw
{
    class HDrawMain : HObj
    {
        /// <summary>
        /// 原始截图
        /// </summary>
        Bitmap orgBackBmp;
        /// <summary>
        /// 灰度处理后的截图
        /// </summary>
        Bitmap grayBackBmp;

        DrawTool dt = new DrawTool();

        /// <summary>
        /// 缩放图片
        /// </summary>
        private DrawZoomImg draw_ZoomImg = new DrawZoomImg();
        /// <summary>
        /// 绘制的主要区域
        /// </summary>
        private HRectange draw_mainRec = new HRectange();


        private HRectange draw_Left1 = new HRectange();

        /// <summary>
        /// 是否存在选择区域
        /// </summary>
        public bool hasSelectRange { get; protected set; }

        /// <summary>
        /// 绘制缓存
        /// </summary>
        BufferedGraphics hostBuffer;
        /// <summary>
        /// 实际绘制
        /// </summary>
        Graphics g;


        public HDrawMain(Form form) : this(form, false, Color.Black)
        {
        }
        public HDrawMain(Form form, bool filterColor, Color color)
        {
            if (filterColor)
            {
                orgBackBmp = filter(CopyPriScreenFull(), color);
            }
            else
            {
                orgBackBmp = CopyPriScreenFull();
            }

            DrawMainRec = new Rectangle(0, 0, orgBackBmp.Width, orgBackBmp.Height);
            MaxRec = DrawMainRec;
            grayBackBmp = orgBackBmp.Clone(DrawMainRec, orgBackBmp.PixelFormat);
            filter(grayBackBmp);
            BackBmp = grayBackBmp.Clone(DrawMainRec, orgBackBmp.PixelFormat);
            g = form.CreateGraphics();
            g.SmoothingMode = SmoothingMode.HighQuality;
            hostBuffer = BufferedGraphicsManager.Current.Allocate(g, MaxRec); //自己管理这块缓冲用的画布
            G = hostBuffer.Graphics;
            G.SmoothingMode = SmoothingMode.HighQuality;

            form.Controls.Add(dt);
            dt.Visible = false;

            //draw_mainRec.DrawColor = Color.Red;
            //draw_mainRec.Model = PointModel.Point;

            draw_ZoomImg.bmp = orgBackBmp;
            draw_ZoomImg.Margin = new Point(30, 50);

            onCursorChangeEvent += (f) =>
            {
                form.Cursor = f;
            };
        }

        public override void render()
        {

            if (Model != DrawModel.Nomal)
            {
                hasSelectRange = true;
                G.FillRectangle(Brushes.Black, DrawMainRec);

                G.DrawImage(grayBackBmp, Point.Empty);//原背景
                G.DrawImage(orgBackBmp, DrawMainRec, DrawMainRec, GraphicsUnit.Pixel);

                //绘制最终截图矩形
                G.DrawRectangle(Pens.Red, DrawMainRec);//画个边框

                //shift键按下或者鼠标按下时，显示缩放的小图
                if (isShift || !isMouseUp)
                {
                    draw_ZoomImg.DrawPoint = HFrameWork.SystemInput.HMouse.Location();
                    draw_ZoomImg.render(G);
                }

                if (isMouseUp)
                {
                    draw_Left1.DrawColor = Color.Green;
                    draw_Left1.DrawRectangle = new Rectangle(DrawMainRec.X + 10, DrawMainRec.Y + DrawMainRec.Height, 20, 50);
                    draw_Left1.render(G);
                    dt.Location = new Point(DrawMainRec.X + 10, DrawMainRec.Y + DrawMainRec.Height + 10);
                    dt.Visible = true;
                }
                else
                {
                    dt.Visible = false;
                }

            }
            draw_mainRec.DrawRectangle = DrawMainRec;
            draw_mainRec.render(G);

            base.render();
            hostBuffer.Render(g);
        }

        internal override Bitmap CloneImg(Rectangle drawMainRec)
        {
            return orgBackBmp.Clone(drawMainRec, orgBackBmp.PixelFormat);
        }
    }
}
