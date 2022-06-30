using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleApplication2
{
    class Program
    {
        private static Bitmap bmp = (Bitmap)Image.FromFile(@"D:\qqhximg\_YZB_70.bmp");

        private static string hostIP = "192.168.1.26";

        private static void HugUP(int i)
        {
            IPAddress ip = IPAddress.Parse(hostIP);
            IPEndPoint ipEnd = new IPEndPoint(ip, 8060);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(ipEnd);
            }
            catch (SocketException e)
            {
                return;
            }

            string input = "DECODER-STOP-" + i + ".";
            byte[] data = Encoding.UTF8.GetBytes(input);
            socket.Send(data, data.Length, SocketFlags.None);

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }


        private static bool conn(int x, int y, Bitmap bmp)
        {
            if (getCount(x, y, bmp) == 2)
            {
                return true;
            }
            return false;
        }

        private static bool canDel(int x, int y, Bitmap bmp)
        {
            int count = 0;
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }
                    if (isCount(x + i, y + j, bmp))
                    {
                        count++;
                        if (count > 2)
                        {
                            return true;
                        }
                    }
                }

            return count > 2;
        }


        struct ConnInfo
        {
            public bool abc;//
            public bool abC;//

            public bool aBc;//
            public bool aBC;//

            public bool Abc;
            public bool AbC;

            public bool ABc;
            public bool ABC;
        }

        private static int getCount(int x, int y, Bitmap bmp)
        {
            int count = 0;
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }
                    if (isCount(x + i, y + j, bmp))
                    {
                        count++;
                    }
                }
            return count;
        }

        private static int getCountN(int x, int y, Bitmap bmp)
        {
            int count = 0;
            Console.WriteLine($"------------getCountN--------------");
            for (int i = 1; i <= 8; i++)
            {
                Console.WriteLine($"[{(i & 3) - 1}]   -[{i / 3}]");
                //if (isCount(x + i % 3 - 1, y + i / 3 - 1, bmp))
                //{
                //    count++;
                //}
            }
            Console.WriteLine($"------------getCountN【{count}】--------------");
            return count;
        }

        private static bool isCount(int x, int y, Bitmap bmp)
        {
            Console.WriteLine($"isCount[{x}][{y}]");
            return isSucPoint(x, y, bmp) && isBlack(x, y, bmp);
        }

        private static bool isSucPoint(int x, int y, Bitmap bmp)
        {
            return x >= 0 && x <= bmp.Width && y >= 0 && y <= bmp.Height;
        }


        private static bool isBlack(int x, int y, Bitmap bmp)
        {
            return bmp.GetPixel(x, y).Name == "ff000000";
        }

        static void Main(string[] args)
        {

            AsyncPortScan.Maint(new string[] { "192.168.1.1" });
            Console.Read();
            return;
            //切图思路
            //1.清除多余点

            // HugUP(1);
            // var bmp = Tool.ImageTool.CopyPriScreen(new Rectangle(100, 100, 100, 100));
            var bmp = (Bitmap)Bitmap.FromFile("d://vv.png");
            var bt = new Bitmap(bmp.Width, bmp.Height);

            #region 打断
            for (int y = 0; y < bmp.Height; y++)
                for (int x = 0; x < bmp.Width; x++)
                {
                    if (!isBlack(x, y, bmp))
                    {
                        continue;
                    }
                    int count = getCountN(x, y, bmp);
                    bool candel = canDel(x, y, bmp);

                    if (candel != (count > 2))
                    {
                        "".ToString();
                    }
                    if (candel)
                    {
                        bmp.SetPixel(x, y, Color.White);
                        continue;
                    }
                    bt.SetPixel(x, y, Color.Black);
                }

            bt.Save("h:\\vv.bmp");
            #endregion 

            //for (int y = 0; y < bmp.Height; y++)
            //    for (int x = 0; x < bmp.Width; x++)
            //    {
            //        if (!conn(x, y, bmp))
            //        {

            //        }
            //    }


            bmp = ConsoleApplication2.Properties.Resources.vv;
            var rst = Tool.ImageTool.findImageFromScreen(bmp);
            Tool.ImageTool.COMP_IMG_TIMES.ToString();
            Console.Write(rst);
            // AsyncPortScan.MainDo(new string[] { "192.168.1.141","1","128" });

            //var fv = Environment.TickCount;
            //HFrameWork.SystemInput.Mouse.DrawMousePosition();
            //var big = Tool.ImageTool.CopyPriScreen();

            //var small = new System.Drawing.Bitmap("d:/abc.bmp");
            //var f = Tool.ImageTool.findLikeImg(small, big);
            //Console.WriteLine(" suc:{0} big:{1} small:{2} Time:{3} Location:{4}", f.Success, big.Size, small.Size, f.Times, f.CenterPoint);
            //if (f.Success)
            //{
            //    big.Clone(new System.Drawing.Rectangle(f.Point, small.Size), System.Drawing.Imaging.PixelFormat.Format24bppRgb).Save("d://abc2.bmp");
            //}
            //else
            //{
            //    big.Save("d:/abc1.bmp");
            //}
        }
    }
}
