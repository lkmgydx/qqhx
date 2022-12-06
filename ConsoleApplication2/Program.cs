
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleApplication2
{
    class Program
    {
        //private static Bitmap bmp = (Bitmap)Image.FromFile(@"D:\qqhximg\_YZB_70.bmp");

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
            catch
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

        private static byte[] ImageToBytes(Image img)
        {
            MemoryStream ms = new MemoryStream();
            byte[] imagedata = null;
            img.Save(ms, ImageFormat.Bmp);
            imagedata = ms.GetBuffer();
            return imagedata;
        }

        //public static Rectangle Find(Image sourceImage, Image matchImage, double threshold = 0.8)
        //{
        //    var refMat = Mat.FromImageData(ImageToBytes(sourceImage), ImreadModes.AnyColor);//大图 
        //    var tplMat = Mat.FromImageData(ImageToBytes(matchImage), ImreadModes.AnyColor);//小图 
        //    using (Mat res = new Mat(refMat.Rows - tplMat.Rows + 1, refMat.Cols - tplMat.Cols + 1, MatType.CV_32FC1))
        //    {
        //        Mat gref = refMat.CvtColor(ColorConversionCodes.BGR2GRAY);
        //        Mat gtpl = tplMat.CvtColor(ColorConversionCodes.BGR2GRAY);
        //        Cv2.MatchTemplate(gref, gtpl, res, TemplateMatchModes.CCoeffNormed);
        //        Cv2.Threshold(res, res, 0.8, 1.0, ThresholdTypes.Tozero);
        //        double minval, maxval;
        //        OpenCvSharp.Point minloc, maxloc;
        //        Cv2.MinMaxLoc(res, out minval, out maxval, out minloc, out maxloc);
        //        if (maxval >= threshold)
        //        {
        //            return new Rectangle(maxloc.X, maxloc.Y, tplMat.Width, tplMat.Height);
        //        }
        //        return Rectangle.Empty;
        //    }
        //}

        private static void ResetList(List<Point> p)
        {
            if (p == null || p.Count <= 1)
            {
                return;
            }
            int x = -p[0].X;
            int y = -p[0].Y;

            for (int i = 0; i < p.Count; i++)
            {
                var pt = p[i];
                pt.Offset(x, y);
                p[i] = pt;
            }
        }

        private static bool isNext(Point p1, Point p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y;
        }

        static void Main(string[] args)
        {
            string file = @"D:\users\Administrator\Desktop\OpenCV\10.jpg";
            var dir = @"D:\users\Administrator\Desktop\OpenCV";
            var files = Directory.GetFiles(dir);
            Array.Sort(files, delegate (string x, string y)
            {
                FileInfo infox = new FileInfo(x);
                FileInfo infoy = new FileInfo(y);

                return  Convert.ToInt32( infox.Name.Replace(infox.Extension,"")) - Convert.ToInt32( (infoy.Name.Replace(infoy.Extension,"")));

            });
            var img = Image.FromFile(file);
            var pdf = new iTextSharp.text.Document(new iTextSharp.text.Rectangle(0, 0, img.Width, img.Height));
            img.Dispose();
            var write = PdfWriter.GetInstance(pdf, new FileStream("d:\\a.pdf", FileMode.Create));
            iTextSharp.text.Jpeg jpg = new iTextSharp.text.Jpeg(File.ReadAllBytes(file));
            pdf.Open();

            foreach (var item in files)
            {
                pdf.Add(new iTextSharp.text.Jpeg(File.ReadAllBytes(item)));
            } 
            pdf.Close();


            //string file = @"D:\qqhximg\324198250.bmp";

            //Bitmap bmp = Tool.ImageTool.filterBmp((Bitmap)Image.FromFile(file), Color.Black);
            //var info = Tool.ImageTool.filterBmpInfo((Bitmap)Image.FromFile(file), Color.Black);
            //var sucP = info.Points;
            //int maxLen = 0;
            //for (int i = 0; i < sucP.Length - 1; i++)
            //{
            //    var one = sucP[i];
            //    var next = sucP[i + 1];
            //    int t = 0;
            //    while (isNext(one, next))
            //    {
            //        i++;
            //        t++;
            //        if (t > maxLen)
            //        {
            //            maxLen = t;
            //        }
            //        one = next;
            //        next = sucP[i + 1];
            //    }
            //}
            //bmp.Save(@"d:\dddd.bmp");

            //string file = @"D:\qqhximg\font_钱善_花5官银.bmp";
            //Bitmap bmp = Image.FromFile(file) as Bitmap;

            //string file2 = @"D:\qqhximg\font_钱善_花5官银2.bmp";
            //Bitmap bmp2 = Image.FromFile(file2) as Bitmap;
            //try
            //{
            //    var f = Tool.ImageTool.filterBmpInfo(bmp, Color.Yellow);
            //    var f2 = Tool.ImageTool.filterBmpInfo(bmp2, Color.Yellow);

            //    var pt1 = f.OffSetPoints;
            //    var pt2 = f2.OffSetPoints;


            //    List<Point> pr = new List<Point>();
            //    for (int i = 0; i < pt1.Length; i++)
            //    {
            //        Point pt = pt1[i];
            //        if (!pt2.Contains(pt))
            //        {
            //            pr.Add(pt);
            //        }
            //    }

            //    for (int i = 0; i < pt2.Length; i++)
            //    {
            //        Point pt = pt2[i];
            //        if (!pt1.Contains(pt))
            //        {
            //            pr.Add(pt);
            //        }
            //    }
            //    "".ToString();
            //    //var m = Tool.ImageTool.filterBmp(bmp, Color.Yellow);
            //    // f.Save("d:\\xx.jpg");
            //}
            //catch (Exception ex)
            //{
            //    ex.ToString();
            //}

            //Dictionary<Color, List<System.Drawing.Point>> di = new Dictionary<Color, List<System.Drawing.Point>>();

            //for (int x = 0; x < bmp.Width; x++)
            //    for (int j = 0; j < bmp.Height; j++)
            //    {
            //        Color c = bmp.GetPixel(x, j);
            //        System.Drawing.Point p = new System.Drawing.Point(x, j);
            //        if (!di.ContainsKey(c))
            //        {

            //            di.Add(c, new List<System.Drawing.Point>() { p });
            //        }
            //        else
            //        {
            //            di[c].Add(p);
            //        }
            //    }

            // Find(Image.FromFile("c:\\v1.png"), Image.FromFile("c:\\v2.png"));
            //AsyncPortScan.Maint(new string[] { "192.168.1.1" });
            //Console.Read();
            return;
            //切图思路
            //1.清除多余点

            // HugUP(1);
            // var bmp = Tool.ImageTool.CopyPriScreen(new Rectangle(100, 100, 100, 100));



            //for (int y = 0; y < bmp.Height; y++)
            //    for (int x = 0; x < bmp.Width; x++)
            //    {
            //        if (!conn(x, y, bmp))
            //        {

            //        }
            //    }


            //bmp = ConsoleApplication2.Properties.Resources.vv;
            //var rst = Tool.ImageTool.findImageFromScreen(bmp);
            //Tool.ImageTool.COMP_IMG_TIMES.ToString();
            //Console.Write(rst);
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
