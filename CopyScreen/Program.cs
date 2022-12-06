
using qqhx;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Tool;

namespace CopyScreen
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //string path1 = @"d:\vv.bmp";
            //var bt = Utils.filter(Image.FromFile(path1) as Bitmap, Color.FromArgb(255, 138, 0));

            //bt.Save("D:\\xb.bmp");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CopyScreenForm());
        }
    }
}
