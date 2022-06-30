using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace qqhx
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            syn = new WSTools.WSThread.WsSyn(this);
        }

        M m = new M();
        WSTools.WSThread.WsSyn syn;
        private void button1_Click(object sender, EventArgs e)
        {
            //run(1, true);
            run(2, false,1000,2000);
            run(3, false,2000,3000);

        }

        private void run(int i, bool isMain, int startRadom = 100, int endRadom = 200)
        {
            ProgressBar p = Controls["progressBar" + i] as ProgressBar;
            Label l = Controls["label" + i] as Label;

            if (p == null)
            {
                return;
            }
            int start = 0;
            Task.Run(() =>
            {
                Random r = new Random(Environment.TickCount);
                while (start < 100)
                {
                    int re;
                    if (isMain)
                    {
                        re = r.Next(600, 2000);
                        log("[" + i + "] 获取【锁】:" + re);
                        m.getMainLock();
                        log("[" + i + "] 获取【锁】成功!" + re, true);
                    }
                    else
                    {
                        re = r.Next(startRadom, endRadom);
                        log("[" + i + "] 获取锁:" + re);
                        m.getLock();
                        log("[" + i + "] 获取锁成功:" + re, true);
                    }

                    syn.Run(() =>
                    {
                        this.Text = "[" + i + "] 正在运行中...";
                    });
                    start++;
                    log("[" + i + "] 正在运行中 休整时间:" + re);
                    System.Threading.Thread.Sleep(re);
                    syn.Run(() =>
                    {
                        p.Value = start;
                    });
                    if (isMain)
                    {
                        log("[" + i + "] 准备释放【锁】");
                        m.ReleaseMainLock();
                        log("[" + i + "] 释放完毕【锁】");
                    }
                    else
                    {
                        log("[" + i + "] 准备释放锁");
                        m.ReleaseLock();
                        log("[" + i + "] 释放完毕");
                    }
                }
                log("[" + i + "] 运行完毕");
            });
        }

        private void log(string msg, bool red = true)
        {
            syn.Run(() =>
            {
                if (red)
                {
                    this.richTextBox1.ForeColor = Color.Red;
                }
                this.richTextBox1.Text += "\r\n" + msg;
                this.richTextBox1.ForeColor = Color.Black;
                this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length - 1;
                this.richTextBox1.ScrollToCaret();
            });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                m.getMainLock();
                progressBar1.Value+=10;
                log("p1+++++++++++++++++++");
                System.Threading.Thread.Sleep(3000);
                m.ReleaseMainLock();
            });

        }
    }
}
