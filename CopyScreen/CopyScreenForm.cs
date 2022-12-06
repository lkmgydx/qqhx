using CopyScreen;
using CopyScreen.Draw;
using HFrameWork.SystemInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CopyScreen.Utils;
using static CopyScreen.Config;

namespace qqhx
{
    public partial class CopyScreenForm : Form
    {

        public delegate void onSucSelect2(Point start, Point end, Rectangle rec, Bitmap img);
        public event onSucSelect2 onSucSelectEventHandler2;


        HDrawMain drawObj;

        public Color FilterColor { get; set; }
        public bool EnableFilterColor { get; set; }

        public bool isColorModel { get; set; }

        public CopyScreenForm()
        {
            InitializeComponent();
            DoubleBuffered = true;
            TopMost = true;
            Size = ScreenSize;
        }


        private void CopyScreenForm_Load(object sender, EventArgs e)
        {
            Location = Point.Empty;
            //drawObj = new HDrawMain(this, false, Color.Red);
            drawObj = new HDrawMain(this, EnableFilterColor, FilterColor);

            TopMost = false;
            BackgroundImage = drawObj.BackBmp;
        }

        bool waitClose = false;
        private void CopyScreenForm_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            waitClose = true;
            this.Close();
        }



        private void CopyScreenForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (waitClose)
            {
                return;
            }
            drawObj.onMouseUp(e);
        }

        private void CopyScreenForm_MouseDown(object sender, MouseEventArgs e)
        {
            drawObj.OnMouseDown(e);
        }

        private void CopyScreenForm_MouseMove(object sender, MouseEventArgs e)
        {
            drawObj.OnMouseMove(e);
        }

        private void ReDrawAll(MouseEventArgs e)
        {
            //refreshRec(e);
            drawObj.render();
            //hostBuffer.Render(g); //画 
        }

        private void CopyScreenForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            drawObj.isShift = e.Shift;
        }

        private void CopyScreenForm_KeyUp(object sender, KeyEventArgs e)
        {
            drawObj.onKeyUp(e);
            // refreshRec(EndPoint, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 1));
        }

        /// <summary>
        /// 鼠标按键处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyScreenForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Close();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            else if (e.KeyCode == Keys.Up)
            {
                HMouse.moveR(0, -1);
            }
            else if (e.KeyCode == Keys.Down)
            {
                HMouse.moveR(0, 1);
            }
            else if (e.KeyCode == Keys.Left)
            {
                HMouse.moveR(-1, 0);
            }
            else if (e.KeyCode == Keys.Right)
            {
                HMouse.moveR(1, 0);
            }
        }

        /// <summary>
        /// 窗体关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyScreenForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (drawObj.hasSelectRange)
            {
                if (drawObj.DrawMainRec == Rectangle.Empty)
                {
                    return;
                }

                using (var bmp = drawObj.CloneImg(drawObj.DrawMainRec))
                {
                    try
                    {
                        Application.DoEvents();
                        Clipboard.SetImage(bmp);
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                    try
                    {
                        onSucSelectEventHandler2?.Invoke(drawObj.DrawMainRec.Location, (drawObj.DrawMainRec.Location + drawObj.DrawMainRec.Size), drawObj.DrawMainRec, bmp);

                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                }
            }
        }

        private void 保存sToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var img = drawObj.CloneImg(drawObj.DrawMainRec);
                img.Save(saveFileDialog1.FileName);
                Close();
            }
        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (drawObj.hasSelectRange && drawObj.DrawMainRec != Rectangle.Empty)
            {
                保存sToolStripMenuItem.Enabled = true;
            }
            else
            {
                保存sToolStripMenuItem.Enabled = false;
            }
        }

        private void 退出EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CopyScreenForm_Paint(object sender, PaintEventArgs e)
        {
            drawObj.render();
        }
    }
}
