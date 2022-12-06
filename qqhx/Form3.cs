using HFrameWork.SystemDll;
using HFrameWork.SystemInput;
using HXmain.HXAction;
using HXmain.HXInfo;
using HXmain.HXInfo.Map;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tool;

namespace qqhx
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            drawT = new Thread(DrawText);
            drawT.IsBackground = true;
        }

        public HXmain.MainGame game { get; set; }
        private void button1_Click(object sender, EventArgs e)
        {
            if (game != null)
            {
                try
                {
                    lab_str.Text = game.getString(Convert.ToInt32(this.textBox1.Text, 16));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("获取异常!" + ex.Message);
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string path = game.savePic_MapStr();
            ProcessStartInfo psi = new ProcessStartInfo("Explorer.exe");
            psi.Arguments = "/e,/select," + path;
            Process.Start(psi);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Rectangle rec = new Rectangle(game.Width / 4, game.Height / 4, game.Width / 2, game.Height / 2);
            Bitmap bt = game.getImg(rec);
            this.pictureBox1.Image = bt;
        }

        private ImageSource ims = new ImageSource(@"D:\qqhximg\some");
        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            ObjectPick.startSQ(game);
            return;
            //Rectangle rec = new Rectangle(game.Width / 4, game.Height / 4, game.Width / 2, game.Height / 2);
            //using (var bt = game.getImg(rec))
            //{
            //    this.pictureBox1.Image = (Bitmap)bt.Clone();
            //    if (ObjectPick.hasObjectWaitPick(game))
            //    {

            //    }
            //    return;
            //    HSearchPoint hs = ImageTool.findImageFromCenter(ims.Bmps, bt);
            //    if (hs.Success)
            //    {
            //        //MessageBox.Show("SUC");
            //        WSTools.WSHook.MouseHook.MouseMove(game.Width / 4 + game.X + hs.CenterPoint.X, game.Height / 4 + game.Y + hs.CenterPoint.Y);
            //        WSTools.WSHook.MouseHook.MouseRigtClick();
            //        return;
            //    }
            //}
            //MessageBox.Show("false");
        }


        private void button6_Click_1(object sender, EventArgs e)
        {
            Text = "当前选择目标:" + game.getCurrentSelectName();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Text = "当前选择目标:" + game.CurrentLocation.ToString();
        }

        HSearchPoint hss = null;

        int ffi = 0;
        private void button8_Click(object sender, EventArgs e)
        {
            if (hss == null)
            {
                WuPingLanInfo wi = new WuPingLanInfo(game);
                // hss = wi.WuPing();
            }
            string[] ss = ipAddressInput1.Value.Split('.');
            int x = Convert.ToInt32(ss[0]);
            int y = Convert.ToInt32(ss[1]);
            int width = Convert.ToInt32(ss[2]);
            int heigth = Convert.ToInt32(ss[3]);
            Bitmap bt = game.getImg(new Rectangle(hss.Point.X + x, hss.Point.Y + y, width, heigth));
            //string t = ImageTool.getImg8Base64(bt);
            //bool suc = ImageTool.is255Suc(WuPingLanInfo.Empty_WUPING_BT, bt);
            //if (ffi++ % 2 == 0)
            //{
            //    this.textBox2.Text = t;
            //}
            //else
            //{
            //    textBox3.Text = t;
            //}
            this.pictureBox1.Image = bt;
            // 363 , 294 
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ToDaySlD.升龙殿投票NPC任务(game);
            //BaseAction.DoBaseAction(ToDaySlD.get投鲜花(game));
            return;
            //HSearchPoint hs = ImageTool.findLikeImg(MapBase.FONT_排行榜, game.getImg(), 95);
            //if (hs.Success)
            //{
            //    game.MouseClick(hs.CenterPoint);
            //    // game.MouseRigthClick(new Point(hs.Point.X + 30, hs.Point.Y + 80));
            //}
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ToDaySlD.升龙殿官职晋升NPC任务(game);
            //BaseAction ac = ToDaySlD.getGG(game);
            //ac.ReverMousePostion = false;
            //BaseAction.DoBaseAction(ac);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            ToDaySlD.升龙殿钱善NPC任务(game);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            ToDaySlD.Start升龙殿(game);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            HXmain.HXAction.三海经.三第经日常(game);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            TiLian.TiLianSome(game);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            TiLian.show提练Win(game);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            WuPing.openWuPing(game);
        }
        Graphics g;
        private string text = "A";
        private void DrawText()
        {
            while (true)
            {
                if (g == null)
                {
                    g = Graphics.FromHwnd(game.MainProcess.MainWindowHandle);
                }
                g.DrawString(text, new Font("宋体", 12), Brushes.Red, 100, 100);
            }
        }

        Thread drawT;
        private void Draw()
        {
            drawT.Start();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (text != "A")
            {
                text = text + "_" + "A";
                return;
            }
            Draw(); 
        }

        private void button18_Click(object sender, EventArgs e)
        {
            WSTools.WSThread.WsThread.Run(() =>
            {
                HXmain.HXAction.ToDaySlD.升龙殿钱善NPC任务1_自动捐满(this.game);
            });
        }

        private void button19_Click(object sender, EventArgs e)
        {

        }
    }
}
