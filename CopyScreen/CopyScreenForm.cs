using HFrameWork.SystemInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace qqhx
{
    public partial class CopyScreenForm : Form
    {
        Bitmap bmp = null;


        public delegate void onSucSelect(Rectangle rec);
        public event onSucSelect onSucSelectEventHandler;


        public delegate void onSucSelect2(Point start, Point end);
        public event onSucSelect2 onSucSelectEventHandler2;

        private static Point SmallImgMargin = new Point(30, 30);//小图间距

        private static Size SmallImgSize = new Size(150, 150);//小图大小

        private Rectangle drawRec = Rectangle.Empty;
        public CopyScreenForm()
        {
            InitializeComponent();
            DoubleBuffered = true;
            bmp = CopyPriScreenFull();
            //bmp = CopyPriScreen();
        }

        BufferedGraphics hostBuffer;
        Graphics g;
        Graphics gg;
        private void CopyScreenForm_Load(object sender, EventArgs e)
        {
            this.Location = Point.Empty;
            this.Size = bmp.Size;
            this.BackgroundImage = bmp;
            g = CreateGraphics();
            hostBuffer = BufferedGraphicsManager.Current.Allocate(g, DisplayRectangle); //自己管理这块缓冲用的画布
            gg = hostBuffer.Graphics;
        }

        private void CopyScreenForm_MouseDoubleClick(object sender, MouseEventArgs e)
        {
             this.Close();
        }

        private bool isDraw = false;
        private Point StartPoint = Point.Empty;
        private Point EndPoint = Point.Empty;
        private void CopyScreenForm_MouseDown(object sender, MouseEventArgs e)
        {
            isDraw = true;
            StartPoint = new Point(e.X, e.Y);
            EndPoint = Point.Empty;
        }

        private void CopyScreenForm_MouseUp(object sender, MouseEventArgs e)
        {
            isDraw = false;
        }

        /// <summary>
        /// 是否有选择区域
        /// </summary>
        private bool hasSelectRange = false;

        private static readonly Size drawSize = new Size(50, 50);

        private void refreshRec(MouseEventArgs e)
        {
            if (needReset)
            {
                isDraw = true;
            }
            if (isDraw)
            {
                Cursor = Cursors.Arrow;
                EndPoint = new Point(e.X, e.Y);
                drawRec = new Rectangle(StartPoint, new Size(EndPoint.X - StartPoint.X, EndPoint.Y - StartPoint.Y));
            }

            hasSelectRange = true;

            var drawPoint = isDraw ? EndPoint : Mouse.Location();
            var rec = new Rectangle(drawPoint.X - drawSize.Width / 2, drawPoint.Y - drawSize.Height / 2, drawSize.Width, drawSize.Height);

            Rectangle rt = new Rectangle(drawPoint.X + SmallImgMargin.X, drawPoint.Y + SmallImgMargin.Y, SmallImgSize.Width, SmallImgSize.Height);

            if (rt.X + rt.Width > Width)
            {
                rt.X = rt.X - rt.Width - 100;
            }
            if (rt.Y + rt.Height > Height)
            {
                rt.Y = rt.Y - rt.Height - 100;
            }


            gg.DrawImage(bmp, Point.Empty);//原背景

            gg.DrawImage(bmp, rt, new Rectangle(drawPoint.X - 25, drawPoint.Y - 25, 50, 50), GraphicsUnit.Pixel);//指针范围的小图

            gg.DrawRectangle(Pens.Black, rt);
            gg.DrawString(Mouse.Location().ToString(), new Font("宋体", 12), Brushes.Green, Point.Empty);//左上角画上坐标
            gg.DrawLine(Pens.Red, new Point(rt.X + 75, rt.Y + 25), new Point(rt.X + 75, rt.Y + 150 - 25)); //十字架，横线
            gg.DrawLine(Pens.Red, new Point(rt.X + 25, rt.Y + 75), new Point(rt.X + 150 - 25, rt.Y + 75));//十字架，竖线

            gg.DrawRectangle(Pens.Red, drawRec);//画个边框
            hostBuffer.Render(g); //画

            if (needReset)
            {
                isDraw = false;
                needReset = false;
            }
        }

        private void CopyScreenForm_MouseMove(object sender, MouseEventArgs e)
        {
            refreshRec(e);
        }

        bool needReset = false;
        private void CopyScreenForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Close();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.hasSelectRange = false;
                this.Close();
            }
            else if (e.KeyCode == Keys.Up)
            {
                Mouse.moveR(0, -1);
            }
            else if (e.KeyCode == Keys.Down)
            {
                Mouse.moveR(0, 1);
            }
            else if (e.KeyCode == Keys.Left)
            {
                Mouse.moveR(-1, 0);
            }
            else if (e.KeyCode == Keys.Right)
            {
                Mouse.moveR(1, 0);
            }
        }

        private void CopyScreenForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void CopyScreenForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (hasSelectRange)
            {
                if (drawRec == Rectangle.Empty)
                {
                    return;
                }
                try
                {
                    Clipboard.SetImage(bmp.Clone(drawRec, System.Drawing.Imaging.PixelFormat.Format24bppRgb));

                }
                catch (Exception)
                {
                     
                }
                 Task.Factory.StartNew(() =>
                {
                    //var back = (Bitmap)this.BackgroundImage;
                    //back = back.Clone(drawRec, back.PixelFormat);
                    //back.Save("d:/vv.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                    onSucSelectEventHandler?.Invoke(drawRec);
                    onSucSelectEventHandler2?.Invoke(drawRec.Location, (drawRec.Location + drawRec.Size));
                });
            }
        }

        public static Bitmap CopyPriScreenFull()
        {
            if (Screen.AllScreens.Length == 1)
            {
                return CopyPriScreen();
            }
            Screen[] screens;
            screens = Screen.AllScreens;
            int noofscreens = screens.Length, maxwidth = 0, maxheight = 0;
            for (int i = 0; i < noofscreens; i++)
            {
                if (maxwidth < (screens[i].Bounds.X + screens[i].Bounds.Width)) maxwidth = screens[i].Bounds.X + screens[i].Bounds.Width;
                if (maxheight < (screens[i].Bounds.Y + screens[i].Bounds.Height)) maxheight = screens[i].Bounds.Y + screens[i].Bounds.Height;
            }
            Rectangle bounds = new Rectangle(0, 0, maxwidth, maxheight);
            Bitmap image = new Bitmap(bounds.Width, bounds.Height);
            using (Graphics g = Graphics.FromImage(image))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
            }
            return image;
        }

        public static Bitmap CopyPriScreen()
        {
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            Bitmap image = new Bitmap(bounds.Width, bounds.Height);
            using (Graphics g = Graphics.FromImage(image))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
            }
            return image;
        }
    }
}
