
using HFrameWork.SystemInput;
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
    public partial class Form2 : Form
    {
        WSTools.WSThread.WsSyn syn;
        public Form2()
        {
            InitializeComponent();
            syn = new WSTools.WSThread.WsSyn(this);
        }

        StratFormLocation lc = new StratFormLocation();

        private void Form2_Load(object sender, EventArgs e)
        {
            return;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            while (true)
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();

                dic.Add("deviceId", "ddffeeee");
                try
                {
                    //string f = await ht.PostAsync(dic);
                    var f = "";
                    this.richTextBox1.AppendText(DateTime.Now.ToShortTimeString() + " : " + f + "\r\n");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            WSTools.WSThread.WsThread.Run(dl);
        }


        private HSearchPoint waitImg(string path, string waitMsg = "等待中...", string sucMsg = "匹配成功!", bool isFind = true, int waitTime = 500)
        {
            while (true)
            {
                var suc = Tool.ImageTool.findImageFromScreen((Bitmap)Bitmap.FromFile(path));
                if (suc.Success == isFind)
                {
                    Console.WriteLine(sucMsg);
                    return suc;
                }
                else
                {
                    Console.WriteLine(waitMsg);
                }
                Thread.Sleep(waitTime);
            }
        }


        private void inputText(string pwd)
        {
            pwd = "123456789";
            for (int i = 0; i < pwd.Length; i++)
            {
                Keys key = (Keys)Enum.Parse(typeof(Keys), Convert.ToByte(pwd[i]).ToString());
                KeyBoard.down_up(key);
                Thread.Sleep(200);
            }
        }

        private void waitDo(string img, string msg = "等待操作")
        {
            var suc = waitImg(img, msg);
            Mouse.move(suc.CenterPoint);
            KeyBoard.sleep(100);
            Mouse.leftclick();
        }

        ChannelWaitDo cw = new ChannelWaitDo();

        private void dl()
        {

            cw.onSucHandler += Cw_onSucHandler;
            cw.AddImg("D:/qqhximg/dl/登陆帐号.bmp", "等待帐号登陆", 3000);
            cw.AddImg("D:/qqhximg/dl/启动.bmp", "等待点击启动", 6000);
            cw.AddImg("D:/qqhximg/dl/qq帐号登陆.bmp", "等待【点击QQ帐号登陆】", 5000);
            cw.AddImg("D:/qqhximg/dl/人物登陆确定.bmp", "等待点击确认", 1000);
            BitFindInfo bi = new BitFindInfo();
            bi.bmp = (Bitmap)Image.FromFile("D:/qqhximg/dl/16加人物选择.bmp");
            bi.isDbClick = true;
            bi.Msg = "等等选择人物";
            bi.WaitTime = 10000;
            bi.Fp = new Point(228, 464);
            cw.AddAction(bi);

            cw.Start();

        }

        private void Cw_onSucHandler(HSearchPoint hs, BitFindInfo bf)
        {
            syn.Run(() =>
            {
                try
                {
                    this.textBoxX1.Text += bf.Msg + "\r\n";
                    this.toolLabState.Text = bf.Msg;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("异常：" + ex.Message);
                }

            });
        }


        private void Cw_onSucExitHandler(HSearchPoint hs, BitFindInfo bf)
        {
            WSTools.WSThread.WsThread.Run(() =>
            {
                ChannelWaitDo cw = new ChannelWaitDo();
                cw.onSucHandler += Cw_onSucHandler;
                cw.AddImg("D:/qqhximg/dl/登陆帐号.bmp", "等待帐号登陆", 3000);
                cw.AddImg("D:/qqhximg/dl/启动.bmp", "等待点击启动", 6000);
                cw.AddImg("D:/qqhximg/dl/qq帐号登陆.bmp", "等待【点击QQ帐号登陆】", 5000, true);
                cw.AddImg("D:/qqhximg/dl/qq帐号登陆.bmp", "等待【点击QQ帐号登陆】", 5000, true);
                cw.Start();
            });
        }

        private void clickPwd(int[] pwd)
        {
            for (int i = 0; i < pwd.Length; i++)
            {
                Mouse.move(0, 0);
                var suc = waitImg("d:/pwd" + pwd[i] + ".bmp", "查找密码:", pwd[i].ToString());
                Mouse.move(suc.CenterPoint);
                Mouse.leftclick();
                Mouse.sleep(500);
            }
        }
        Process p = new Process();
        private void button3_Click(object sender, EventArgs e)
        {
            // p.StartInfo.FileName = @"E:\Program Files\腾讯游戏\QQ华夏\WeGameLauncher\launcher.exe";
            try
            {
                p.StartInfo.FileName = lc.RunPath;
                p.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(lc.RunPath);
            }
            catch (Exception ex)
            {
                WSTools.WSWinFn.ShowError("请确认启动文件配置路径正确!", ex);
            }
            p.Start();
            WSTools.WSThread.WsThread.Run(dl);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                lc.RunPath = openFileDialog1.FileName;
                lc.save();
            }
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            cw.Stop();
        }


        //  private static string gURL = "https://translate.google.cn/translate_a/single";




    }
}
