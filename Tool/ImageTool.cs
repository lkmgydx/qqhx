namespace Tool
{
    using HFrameWork.SystemDll;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    public class ImageTool
    {
        private static Bitmap _cacheBmp = null;
        private static string _cacheBmpNmae = null;
        public static HSearchPoint ErrorHSearchPoint = new HSearchPoint(false, Point.Empty, Size.Empty, 0);
        public static Point ErrorPoint = new Point(-1, -1);

        public static Bitmap cutSigleBmp(Bitmap bmp, Color color)
        {
            var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            Point[] pts = filterColor(data, color);
            bmp.UnlockBits(data);
            if (pts.Length == 0)
            {
                return new Bitmap(1, 1);
            }
            int minx = pts[0].X;
            int miny = pts[0].Y;
            int maxX = pts[0].X;
            int maxY = pts[0].Y;
            for (int i = 1; i < pts.Length; i++)
            {
                minx = Math.Min(minx, pts[i].X);
                miny = Math.Min(miny, pts[i].Y);
                maxX = Math.Max(maxX, pts[i].X);
                maxY = Math.Max(maxY, pts[i].Y);
            }
            if (maxX == minx || maxY == miny)
            {
                return new Bitmap(1, 1);
            }
            Bitmap rst = new Bitmap(maxX - minx + 1, maxY - miny + 1);
            for (int i = 0; i < pts.Length; i++)
            {
                rst.SetPixel(pts[i].X - minx, pts[i].Y - miny, color);
            }
            return rst;
        }

        public static Point[] cutSigleBmpPoints(Bitmap bmp, Color color)
        {
            var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            Point[] pts = filterColor(data, color);
            bmp.UnlockBits(data);
            if (pts.Length == 0)
            {
                return new Point[0];
            }
            int minx = pts[0].X;
            int miny = pts[0].Y;
            int maxX = pts[0].X;
            int maxY = pts[0].Y;
            for (int i = 1; i < pts.Length; i++)
            {
                minx = Math.Min(minx, pts[i].X);
                miny = Math.Min(miny, pts[i].Y);
                maxX = Math.Max(maxX, pts[i].X);
                maxY = Math.Max(maxY, pts[i].Y);
            }
            if (maxX == minx || maxY == miny)
            {
                return new Point[0];
            }
            for (int i = 0; i < pts.Length; i++)
            {
                pts[i].X = pts[i].X - minx;
                pts[i].Y = pts[i].Y - miny;
            }
            return pts;
        }

        public static unsafe HSearchPoint findSigleImg(Point[] imgSmallIn, Bitmap imgBigIn, Color filter)
        {
            long[] times = new long[10];
            int tickCount = Environment.TickCount; //初始时间计算

            using (Bitmap imgBig = imgBigIn.Clone(new Rectangle(0, 0, imgBigIn.Width, imgBigIn.Height), PixelFormat.Format24bppRgb))
            {
                BitmapData bitBigData = imgBig.LockBits(new Rectangle(0, 0, imgBig.Width, imgBig.Height), ImageLockMode.ReadWrite, imgBig.PixelFormat);


                Point[] mColor = filterColor(bitBigData, filter);

                //Bitmap bmp = new Bitmap(bitBigData.Width, bitBigData.Height);
                //for (int i = 0; i < mColor.Length; i++)
                //{
                //    bmp.SetPixel(mColor[i].X, mColor[i].Y, Color.Red);
                //}
                //bmp.Save("e:\\sss.bmp");

                int maxError = maxError = imgSmallIn.Length - imgSmallIn.Length * 90 / 100;

                if (imgSmallIn.Length == 0 || mColor.Length < imgSmallIn.Length)
                {
                    return new HSearchPoint(false, ErrorPoint, imgBigIn.Size, Environment.TickCount - tickCount);
                }

                Point[] dsArrary;
                int flex = 0;
                do
                {
                    dsArrary = new Point[imgSmallIn.Length];
                    Array.Copy(mColor, flex++, dsArrary, 0, dsArrary.Length);
                    int fx = dsArrary[0].X - imgSmallIn[0].X;//相对位置
                    int fy = dsArrary[0].Y - imgSmallIn[0].Y;
                    bool suc = true;
                    int terror = 0;
                    for (int i = 1; i < imgSmallIn.Length; i++)
                    {
                        if (dsArrary[i].X - fx != imgSmallIn[i].X || dsArrary[i].Y - fy != imgSmallIn[i].Y)
                        {
                            suc = false;
                            terror++;
                            if (terror >= maxError)
                                break;
                        }
                    }
                    if (suc)
                    {
                        imgBig.UnlockBits(bitBigData);
                        return new HSearchPoint(true, dsArrary[0], imgBig.Size, Environment.TickCount - tickCount);
                    }
                }
                while (mColor.Length - flex - imgSmallIn.Length >= 0);

                imgBig.UnlockBits(bitBigData);

                return new HSearchPoint(false, ErrorPoint, imgBig.Size, Environment.TickCount - tickCount);
            }
        }

        public static int COMP_IMG_TIMES = 0;
        public static int COMP_IMG_MAX_TIMES = 0;

        public static int COMP_IMG_3X3_TIMES = 0;

        public static int COMP_IMG_6X6_TIMES = 0;

        /// <summary>
        /// 核心找图算法
        /// </summary>
        /// <param name="imgSmallIn"></param>
        /// <param name="imgBigIn"></param>
        /// <param name="xCount"></param>
        /// <param name="yCount"></param>
        /// <param name="opset"></param>
        /// <returns></returns>
        public static unsafe HSearchPoint findEqImg(Bitmap imgSmallIn, Bitmap imgBigIn, int opt = 100)
        {
            COMP_IMG_TIMES = 0;
            long[] times = new long[10];
            int tickCount = Environment.TickCount; //初始时间计算

            if (opt > 100) //控制比对范围在10-100%之间
            {
                opt = 100;
            }
            else if (opt <= 10)
            {
                opt = 10;
            }
            //times[0] = Environment.TickCount - tickCount;

            if (imgSmallIn.Width > imgBigIn.Width) //大小图对调
            {
                Bitmap t = imgSmallIn;
                imgSmallIn = imgBigIn;
                imgBigIn = t;
            }
            //times[1] = Environment.TickCount - tickCount;

            using (Bitmap imgSmall = imgSmallIn.Clone(new Rectangle(0, 0, imgSmallIn.Width, imgSmallIn.Height), PixelFormat.Format24bppRgb))
            {

                //times[2] = Environment.TickCount - tickCount;
                var imgBig = imgBigIn.Clone(new Rectangle(0, 0, imgBigIn.Width, imgBigIn.Height), PixelFormat.Format24bppRgb);
                using (imgBig)
                {
                    //times[3] = Environment.TickCount - tickCount;
                    BitmapData bitSmallData = imgSmall.LockBits(new Rectangle(0, 0, imgSmall.Width, imgSmall.Height), ImageLockMode.ReadWrite, imgSmall.PixelFormat);
                    BitmapData bitBigData = imgBig.LockBits(new Rectangle(0, 0, imgBig.Width, imgBig.Height), ImageLockMode.ReadWrite, imgBig.PixelFormat);

                    //times[4] = Environment.TickCount - tickCount;
                    int maxY = bitBigData.Height - bitSmallData.Height;//最大最高
                    int maxX = bitBigData.Width - bitSmallData.Width;//最大宽度

                    int maxFindCount = maxY * maxX;
                    //int findCount = 0;
                    int find3x3Count = 0;
                    //int skip3x3Times = 0;

                    times[5] = Environment.TickCount - tickCount;
                    for (int i = 0; i <= maxY; i++)
                    {
                        for (int j = 0; j <= maxX; j++) //一行行开始比对
                        {
                            find3x3Count++;
                            if (!filter3X3N(bitBigData, bitSmallData, j, i, opt))
                            {
                                //skip3x3Times++;
                                continue;
                            }
                            //findCount++;
                            if (filterAll(bitBigData, bitSmallData, j, i, opt))
                            {
                                times[6] = Environment.TickCount - tickCount;
                                //for (int v = 0; v < times.Length; v++)
                                //{
                                //    if (times[v] != 0)
                                //        Console.WriteLine("{0} = {1}", v, times[v]);
                                //}
                                //Console.WriteLine("maxTime:" + maxFindCount);
                                //Console.WriteLine("find3x3Count times:" + find3x3Count);
                                //Console.WriteLine("skip3x3Times times:" + skip3x3Times);
                                //Console.WriteLine("findCount times:" + findCount);
                                imgBig.UnlockBits(bitBigData);
                                imgSmall.UnlockBits(bitSmallData);
                                return new HSearchPoint(true, new Point(j, i), imgSmall.Size, Environment.TickCount - tickCount);
                            }
                        }
                    }
                    imgBig.UnlockBits(bitBigData);
                    imgSmall.UnlockBits(bitSmallData);
                    return new HSearchPoint(false, ErrorPoint, imgSmall.Size, Environment.TickCount - tickCount);
                }
            }
        }

        /// <summary>
        /// 核心找图算法
        /// </summary>
        /// <param name="imgSmallIn"></param>
        /// <param name="imgBigIn"></param>
        /// <param name="xCount"></param>
        /// <param name="yCount"></param>
        /// <param name="opset"></param>
        /// <returns></returns>
        public static unsafe List<HSearchPoint> findEqImgAll(Bitmap imgSmallIn, Bitmap imgBigIn)
        {
            List<HSearchPoint> liRst = new List<HSearchPoint>();
            COMP_IMG_TIMES = 0;
            long[] times = new long[10];
            int tickCount = Environment.TickCount; //初始时间计算

            //times[0] = Environment.TickCount - tickCount;

            if (imgSmallIn.Width > imgBigIn.Width) //大小图对调
            {
                Bitmap t = imgSmallIn;
                imgSmallIn = imgBigIn;
                imgBigIn = t;
            }
            //times[1] = Environment.TickCount - tickCount;

            using (Bitmap imgSmall = imgSmallIn.Clone(new Rectangle(0, 0, imgSmallIn.Width, imgSmallIn.Height), PixelFormat.Format24bppRgb))
            {

                //times[2] = Environment.TickCount - tickCount;
                var imgBig = imgBigIn.Clone(new Rectangle(0, 0, imgBigIn.Width, imgBigIn.Height), PixelFormat.Format24bppRgb);
                using (imgBig)
                {
                    //times[3] = Environment.TickCount - tickCount;
                    BitmapData bitSmallData = imgSmall.LockBits(new Rectangle(0, 0, imgSmall.Width, imgSmall.Height), ImageLockMode.ReadWrite, imgSmall.PixelFormat);
                    BitmapData bitBigData = imgBig.LockBits(new Rectangle(0, 0, imgBig.Width, imgBig.Height), ImageLockMode.ReadWrite, imgBig.PixelFormat);

                    //times[4] = Environment.TickCount - tickCount;
                    int maxY = bitBigData.Height - bitSmallData.Height;//最大最高
                    int maxX = bitBigData.Width - bitSmallData.Width;//最大宽度

                    int maxFindCount = maxY * maxX;
                    //int findCount = 0;
                    int find3x3Count = 0;
                    //int skip3x3Times = 0;

                    times[5] = Environment.TickCount - tickCount;
                    for (int i = 0; i <= maxY; i++)
                    {
                        for (int j = 0; j <= maxX; j++) //一行行开始比对
                        {
                            find3x3Count++;
                            if (!filter3X3N(bitBigData, bitSmallData, j, i, 100))
                            {
                                //skip3x3Times++;
                                continue;
                            }
                            //findCount++;
                            if (filterAll(bitBigData, bitSmallData, j, i, 100))
                            {
                                times[6] = Environment.TickCount - tickCount;
                                //for (int v = 0; v < times.Length; v++)
                                //{
                                //    if (times[v] != 0)
                                //        Console.WriteLine("{0} = {1}", v, times[v]);
                                //}
                                //Console.WriteLine("maxTime:" + maxFindCount);
                                //Console.WriteLine("find3x3Count times:" + find3x3Count);
                                //Console.WriteLine("skip3x3Times times:" + skip3x3Times);
                                //Console.WriteLine("findCount times:" + findCount);
                               // imgBig.UnlockBits(bitBigData);
                                //imgSmall.UnlockBits(bitSmallData);

                                liRst.Add(new HSearchPoint(true, new Point(j, i), imgSmall.Size, Environment.TickCount - tickCount));
                            }
                        }
                    }
                    imgBig.UnlockBits(bitBigData);
                    imgSmall.UnlockBits(bitSmallData);
                    return liRst;
                }
            }
        }

        public static string getNum(string img)
        {

            //var APP_ID = "20375427";
            // var API_KEY = "MHUExwHYSu7KTdiK6VdlN46G";
            // var SECRET_KEY = "Fa5GMQAOumLPDKNAlmdsEhUfmzTunAOM";
            System.Net.WebClient wc = new System.Net.WebClient();

            // var client = new Baidu.Aip.Ocr.Ocr(API_KEY, SECRET_KEY);
            // client.Timeout = 60000;  // 修改超时时间
            //var image = File.ReadAllBytes("图片文件路径");
            // 调用通用文字识别, 图片参数为本地图片，可能会抛出网络等异常，请使用try/catch捕获
            // var result =  client.GeneralBasic(image);
            var result = "";
            // Console.WriteLine(result);
            //        // 如果有可选参数
            //        var options = new Dictionary<string, object>{
            //    {"language_type", "CHN_ENG"},
            //    {"detect_direction", "true"},
            //    {"detect_language", "true"},
            //    {"probability", "true"}
            //};
            //        // 带参数调用通用文字识别, 图片参数为本地图片
            //        result = client.GeneralBasic(image, options);
            Console.WriteLine(result);
            return "";
        }

        private static unsafe bool canGo(byte* pt)
        {
            return true;
        }

        private static unsafe Bitmap CopyImg(Bitmap bmp, int y, int height)
        {
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bitmapdata = bmp.LockBits(rect, ImageLockMode.ReadOnly, bmp.PixelFormat);
            byte* numPtr = (byte*)bitmapdata.Scan0;
            int num = bitmapdata.Stride / bitmapdata.Width;
            Bitmap bitmap = new Bitmap(bmp.Width, height);
            Rectangle rectangle2 = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData data2 = bitmap.LockBits(rectangle2, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            int num2 = data2.Stride / data2.Width;
            byte* numPtr2 = (byte*)data2.Scan0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    for (int k = 0; (k < num) && (k < 4); k++)
                    {
                        numPtr2[((i * data2.Stride) + (j * num2)) + k] = numPtr[(((y + i) * bitmapdata.Stride) + (j * num)) + k];
                    }
                }
            }
            bitmap.UnlockBits(data2);
            bmp.UnlockBits(bitmapdata);
            return bitmap;
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

        public static Bitmap CopyPriScreen(Rectangle rec)
        {
            Bitmap image = new Bitmap(rec.Width, rec.Height);
            using (Graphics g = Graphics.FromImage(image))
            {
                g.CopyFromScreen(rec.Location, Point.Empty, rec.Size);
            }
            return image;
        }

        public static Bitmap ZoomBmp(Bitmap bmp, int size)
        {
            Bitmap newBmp = new Bitmap(bmp.Width * size, bmp.Height * size);
            using (Graphics g = Graphics.FromImage(newBmp))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, 0, 0, newBmp.Width, newBmp.Height);
            }
            return newBmp;
        }
        public static void DrawMousePoint(Point p, int sleepTime)
        {
            Graphics graphics = getScreenGraphics();
            for (int i = 0; i < 10; i++)
            {
                graphics.DrawEllipse(Pens.Red, new Rectangle(p.X - (50 - (i * 5)), p.Y - (50 - (i * 5)), 100 - (i * 10), 100 - (i * 10)));
            }
            graphics.Flush();
        }

        public static unsafe bool Equals(Bitmap img0, Bitmap img1, int op)
        {
            if ((img0.Width == img1.Width) && (img0.Height == img1.Height))
            {
                if (img0 == img1)
                {
                    return true;
                }
                Rectangle rect = new Rectangle(0, 0, img0.Width, img0.Height);
                Rectangle rectangle2 = new Rectangle(0, 0, img1.Width, img1.Height);
                BitmapData bitmapdata = img1.LockBits(rectangle2, ImageLockMode.ReadWrite, img1.PixelFormat);
                BitmapData data2 = img0.LockBits(rect, ImageLockMode.ReadWrite, img0.PixelFormat);
                List<long> list = new List<long>();
                List<long> list2 = new List<long>();
                byte* numPtr = (byte*)data2.Scan0;
                byte* numPtr2 = (byte*)bitmapdata.Scan0;
                int num = data2.Stride / data2.Width;
                int num2 = bitmapdata.Stride / bitmapdata.Width;
                int num3 = 20;
                int num4 = (data2.Width <= num3) ? num : (((data2.Stride / num3) / num) * num);
                int num5 = (data2.Height <= num3) ? 1 : (data2.Height / num3);
                int num6 = (data2.Width <= num3) ? data2.Width : num3;
                int num7 = (data2.Height <= num3) ? data2.Height : num3;
                op = ((num6 * num7) * op) / 100;
                long num8 = 0L;
                for (int i = 0; i <= data2.Height; i++)
                {
                    for (int j = 0; j <= data2.Width; j++)
                    {
                        long num11 = (j * num2) + (i * bitmapdata.Stride);
                        if (((Math.Abs((int)(numPtr[0] - numPtr2[(int)num11])) <= 20) && (Math.Abs((int)(numPtr[1] - numPtr2[(int)(num11 + 1L)])) <= 20)) && (Math.Abs((int)(numPtr[2] - numPtr2[(int)(num11 + 2L)])) <= 20))
                        {
                            num8 = 0L;
                            for (int k = 0; k < num7; k++)
                            {
                                for (int m = 0; m < num6; m++)
                                {
                                    long item = ((((num4 * m) / num) * num2) + (j * num2)) + ((i + (k * num5)) * bitmapdata.Stride);
                                    long num15 = (num4 * m) + ((k * data2.Stride) * num5);
                                    if (num8 >= op)
                                    {
                                        img1.UnlockBits(bitmapdata);
                                        img0.UnlockBits(data2);
                                        return true;
                                    }
                                    if (((Math.Abs((int)(numPtr[(int)num15] - numPtr2[(int)item])) < 20) && (Math.Abs((int)(numPtr[(int)(num15 + 1L)] - numPtr2[(int)(item + 1L)])) < 20)) && (Math.Abs((int)(numPtr[(int)(num15 + 2L)] - numPtr2[(int)(item + 2L)])) < 20))
                                    {
                                        list.Add(num15);
                                        list2.Add(item);
                                        num8 += 1L;
                                    }
                                }
                            }
                        }
                    }
                }
                img1.UnlockBits(bitmapdata);
                img0.UnlockBits(data2);
            }
            return false;
        }

        public static HSearchPoint findCenterImg(string imgname, Bitmap img1, int op)
        {
            Bitmap bitmap;
            if (imgname == _cacheBmpNmae)
            {
                bitmap = _cacheBmp;
            }
            else
            {
                bitmap = (Bitmap)Image.FromFile(imgname);
                _cacheBmp = bitmap;
                _cacheBmpNmae = imgname;
            }
            return findLikeImg(bitmap, img1, op);
        }

        public static HSearchPoint findImageFromScreen(Bitmap bmp, int op = 100)
        {
            using (Bitmap bs = CopyPriScreenFull())
            {
                return findLikeImg(bmp, bs, op);
            }

        }

        public static HSearchPoint findImageFromScreen(Bitmap bmp, int w, int h, int op)
        {
            return findLikeImg(bmp, CopyPriScreenFull(), w, h, op);
        }

        public static HSearchPoint findImageFromScreen(Bitmap[] img, int op)
        {
            for (int i = 0; i < img.Length; i++)
            {
                if (img != null)
                {
                    HSearchPoint point = findLikeImg(img[i], CopyPriScreen(), op);
                    if (point.Success)
                    {
                        return point;
                    }
                }
            }
            return ErrorHSearchPoint;
        }

        public static HSearchPoint findImageFromScreenCenter(Bitmap[] img, int op)
        {
            for (int i = 0; i < img.Length; i++)
            {
                if (img != null)
                {
                    HSearchPoint point = findLikeImg(img[i], CopyPriScreen(), op);
                    if (point.Success)
                    {
                        return point;
                    }
                }
            }
            return ErrorHSearchPoint;
        }

        public static HSearchPoints findImgesAll(Bitmap[] img, Bitmap bmp)
        {
            bool suc = false;
            int tickCount = Environment.TickCount;
            List<Point> sucCenter = new List<Point>();
            List<Point> sucLeftUp = new List<Point>();

            for (int i = 0; i < img.Length; i++)
            {
                if (img != null)
                {
                    HSearchPoint p = findEqImg(img[i], bmp);
                    if (p.Success)
                    {
                        suc = true;
                        addPoint(sucLeftUp, p.Point);
                        addPoint(sucCenter, p.CenterPoint);
                    }
                }
            }
            return new HSearchPoints(suc, sucLeftUp, sucCenter, Environment.TickCount - tickCount);
        }

        private static void addPoint(List<Point> pts, Point pt)
        {
            foreach (Point p in pts)
            {
                if (p == pt)
                {
                    return;
                }
            }
            pts.Add(pt);
        }

        public static HSearchPoint findImageFromCenter(Bitmap[] img, Bitmap bmp)
        {
            for (int i = 0; i < img.Length; i++)
            {
                if (img != null)
                {
                    HSearchPoint point = findEqImg(img[i], bmp);
                    if (point.Success)
                    {
                        return point;
                    }
                }
            }
            return ErrorHSearchPoint;
        }

        /// <summary>
        /// 查找图片,模糊区域为 20*20矩阵
        /// </summary>
        /// <param name="imgSmallIn"></param>
        /// <param name="imgBigIn"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        public static HSearchPoint findLikeImg(Bitmap imgSmallIn, Bitmap imgBigIn, int op = 100)
        {
            return findLikeImg(imgSmallIn, imgBigIn, 20, 20, op);
        }

        public static bool hasSomeOne(Bitmap bmp)
        {
            if (bmp == null)
            {
                return false;
            }
            return HasSomeObj.HasSome(bmp);
        }

        private static unsafe Point[] filterColor(BitmapData bitData, Color color)
        {
            int stepBig = bitData.Stride / bitData.Width;
            List<Point> colorRec = new List<Point>();

            var bigPtr = (byte*)bitData.Scan0;

            if (stepBig == 3)
            {
                for (int j = 0; j < bitData.Height; j++)
                    for (int i = 0; i < bitData.Stride; i++)
                    {
                        var ptrTempBit = bitData.Stride * j + i;
                        if (bigPtr[ptrTempBit++] == color.B && bigPtr[ptrTempBit++] == color.G && bigPtr[ptrTempBit++] == color.R)
                        {
                            colorRec.Add(new Point(i / stepBig, j));
                        }
                    }
                return colorRec.ToArray();
            }
            else if (stepBig == 4)
            {
                for (int j = 0; j < bitData.Height; j++)
                    for (int i = 0; i < bitData.Stride; i++)
                    {
                        var ptrTempBit = bitData.Stride * j + i;
                        if (bigPtr[ptrTempBit++] == color.B && bigPtr[ptrTempBit++] == color.G && bigPtr[ptrTempBit++] == color.R && bigPtr[ptrTempBit++] == color.A)
                        {
                            colorRec.Add(new Point(i / stepBig, j));
                        }
                    }
            }

            return colorRec.ToArray();
        }


        private static unsafe bool filter3X3N(BitmapData bitBigData, BitmapData bitSmallData, int fx, int fy, int opt)
        {
            int stepSmall = bitSmallData.Stride / bitSmallData.Width;
            int stepBig = bitBigData.Stride / bitBigData.Width;

            var bigPtr = (byte*)bitBigData.Scan0;
            var smallPtr = (byte*)bitSmallData.Scan0;

            //未命中数量
            int missCountTemp = 0;
            //最大未命中数量
            int maxError = bitSmallData.Width * bitSmallData.Height * (100 - opt) / 100;
            maxError = 5;
            for (int y = 0; y < bitSmallData.Height; y++)
            {
                for (int x = 0; x < bitSmallData.Width; x++)
                {
                    COMP_IMG_TIMES++;
                    COMP_IMG_3X3_TIMES++;
                    var ptrTempSmall = bitSmallData.Stride * y + x * stepSmall;
                    var ptrTempBit = bitBigData.Stride * (y + fy) + (x + fx) * stepBig;
                    if (bigPtr[ptrTempBit++] != smallPtr[ptrTempSmall++] || bigPtr[ptrTempBit++] != smallPtr[ptrTempSmall++] || bigPtr[ptrTempBit++] != smallPtr[ptrTempSmall++])
                    {
                        if (missCountTemp >= maxError)
                        {
                            return false;
                        }
                        missCountTemp++;
                    }
                }
            }
            return true;
        }


        private static unsafe bool filter3X3(BitmapData bitBigData, BitmapData bitSmallData, int fx, int fy)
        {
            int stepXtemp = bitSmallData.Width / 4;
            int stepYtemp = bitSmallData.Height / 4;

            int stepSmall = bitSmallData.Stride / bitSmallData.Width;
            int stepBig = bitBigData.Stride / bitBigData.Width;

            //int missCountTemp = 0;
            var bigPtr = (byte*)bitBigData.Scan0;
            var smallPtr = (byte*)bitSmallData.Scan0;
            for (int t1 = 0; t1 < 3; t1++)
            {
                for (int t2 = 0; t2 < 3; t2++)
                {
                    int x = t2 * stepXtemp;
                    int y = t1 * stepYtemp;

                    var ptrTempSmall = bitSmallData.Stride * y + x * stepSmall;
                    var ptrTempBit = bitBigData.Stride * (y + fy) + (x + fx) * stepBig;
                    if (bigPtr[ptrTempBit++] != smallPtr[ptrTempSmall++] || bigPtr[ptrTempBit++] != smallPtr[ptrTempSmall++] || bigPtr[ptrTempBit++] != smallPtr[ptrTempSmall++])
                    {
                        return false;
                        //if (missCountTemp >= 6)
                        //{
                        //    return false;
                        //}
                        //missCountTemp++;
                    }
                }
            }
            return true;
        }

        private static unsafe bool filterMi(BitmapData bitBigData, BitmapData bitSmallData, int fx, int fy, int opt)
        {
            int stepXtemp = bitSmallData.Width / 4;
            int stepYtemp = bitSmallData.Height / 4;

            int stepSmall = bitSmallData.Stride / bitSmallData.Width;
            int stepBig = bitBigData.Stride / bitBigData.Width;

            int missCountTemp = 0;
            var bigPtr = (byte*)bitBigData.Scan0;
            var smallPtr = (byte*)bitSmallData.Scan0;
            for (int t1 = 0; t1 < 3; t1++)
            {
                for (int t2 = 0; t2 < 3; t2++)
                {
                    int x = t2 * stepXtemp;
                    int y = t1 * stepYtemp;

                    var ptrTempSmall = bitSmallData.Stride * y + x * stepSmall;
                    var ptrTempBit = bitBigData.Stride * (y + fy) + (x + fx) * stepBig;
                    if (bigPtr[ptrTempBit++] != smallPtr[ptrTempSmall++] || bigPtr[ptrTempBit++] != smallPtr[ptrTempSmall++] || bigPtr[ptrTempBit++] != smallPtr[ptrTempSmall++])
                    {
                        if (missCountTemp >= 6)
                        {
                            return false;
                        }
                        missCountTemp++;
                    }
                }
            }
            return true;
        }

        private static unsafe bool filterAll(BitmapData bitBigData, BitmapData bitSmallData, int fx, int fy, int opt)
        {

            int stepSmall = bitSmallData.Stride / bitSmallData.Width;
            int stepBig = bitBigData.Stride / bitBigData.Width;

            var bigPtr = (byte*)bitBigData.Scan0;
            var smallPtr = (byte*)bitSmallData.Scan0;

            //未命中数量
            int missCountTemp = 0;
            //最大未命中数量
            int maxError = bitSmallData.Width * bitSmallData.Height * (100 - opt) / 100;
            maxError = 5;

            for (int y = 0; y < bitSmallData.Height; y++)
            {
                for (int x = 0; x < bitSmallData.Width; x++)
                {
                    COMP_IMG_TIMES++;
                    COMP_IMG_6X6_TIMES++;
                    var ptrTempSmall = bitSmallData.Stride * y + x * stepSmall;
                    var ptrTempBit = bitBigData.Stride * (y + fy) + (x + fx) * stepBig;
                    if (bigPtr[ptrTempBit++] != smallPtr[ptrTempSmall++] || bigPtr[ptrTempBit++] != smallPtr[ptrTempSmall++] || bigPtr[ptrTempBit++] != smallPtr[ptrTempSmall++])
                    {
                        if (missCountTemp >= maxError)
                        {
                            return false;
                        }
                        missCountTemp++;
                    }
                }
            }
            return true;
        }

        public static unsafe HSearchPoint findSingle(Bitmap imgSmallIn, Bitmap imgBigIn, int xCount, int yCount, int opset)
        {
            return null;
            //int tickCount = Environment.TickCount;

            //using (Bitmap imgSmall = imgSmallIn.Clone(new Rectangle(0, 0, imgSmallIn.Width, imgSmallIn.Height), PixelFormat.Format16bppRgb555))
            //{
            //    using (Bitmap imgBig = imgBigIn.Clone(new Rectangle(0, 0, imgBigIn.Width, imgBigIn.Height), PixelFormat.Format16bppRgb555))
            //    {
            //        BitmapData bitSmallData = imgSmall.LockBits(new Rectangle(0, 0, imgSmall.Width, imgSmall.Height), ImageLockMode.ReadWrite, imgSmall.PixelFormat);
            //        BitmapData bitBigData = imgBig.LockBits(new Rectangle(0, 0, imgBig.Width, imgBig.Height), ImageLockMode.ReadWrite, imgBig.PixelFormat);
            //        byte* smallPtr = (byte*)bitSmallData.Scan0;
            //        byte* bigPtr = (byte*)bitBigData.Scan0;

            //        int stepSmall = bitSmallData.Stride / bitSmallData.Width;
            //        int stepBig = bitBigData.Stride / bitBigData.Width;


            //        int op = ((imgSmallIn.Width * imgSmall.Height) * (opset % 100)) / 100;
            //        // op = xLen * yLen;
            //        long missCount = 0L;
            //        int maxX = bitBigData.Height - bitSmallData.Height;
            //        int maxY = bitBigData.Width - bitSmallData.Width;
            //        long num20 = 0L;
            //        Point point = new Point(0, 0);
            //        int stepXtemp = bitSmallData.Width / 3;
            //        int stepYtemp = bitSmallData.Height / 3;
            //        for (int i = 0; i <= maxX; i++)
            //        {
            //            for (int j = 0; j <= maxY; j++)
            //            {
            //                long tempValue = (j * stepBig) + (i * bitBigData.Stride);
            //                if (((Math.Abs(stepSmall.r - bigPtr[tempValue]) <= 10) && (Math.Abs(pl.pp[0, 0].g - bigPtr[tempValue + 1]) <= 10)) && (Math.Abs((pl.pp[0, 0].b - bigPtr[tempValue + 2])) <= 10))
            //                {
            //                    if (!filter3X3(bitBigData, bitSmallData, j, i))
            //                    {
            //                        continue;
            //                    } 
            //                    missCount = 0L;
            //                    for (int k = 0; k < yLen; k++)
            //                    {
            //                        for (int m = 0; m < xLen; m++)
            //                        {
            //                            long num26 = (tempValue + (pl.pl[0][m] * stepBig)) + (pl.pl[1][k] * bitBigData.Stride);
            //                            if (missCount >= op)
            //                            {
            //                                imgBig.UnlockBits(bitBigData);
            //                                imgSmall.UnlockBits(bitSmallData);
            //                                return new HSearchPoint(true, new Point(j, i), imgSmall.Size, Environment.TickCount - tickCount);
            //                            }
            //                            if ((pl.pp[k, m].r == bigPtr[num26]) && (pl.pp[k, m].g == bigPtr[num26 + 1]) && (pl.pp[k, m].b == bigPtr[num26 + 2]))
            //                            {
            //                                missCount++;
            //                                if (((point.X != j) || (point.Y != i)) && (missCount > num20))
            //                                {
            //                                    num20 = missCount;
            //                                    point = new Point(j, i);
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        imgBig.UnlockBits(bitBigData);
            //        imgSmall.UnlockBits(bitSmallData);
            //        //if (false)
            //        //{
            //        //    Bitmap bitmap = new Bitmap(bitSmallData.Width, bitSmallData.Height);
            //        //    for (int num27 = 0; num27 < bitmap.Height; num27++)
            //        //    {
            //        //        for (int num28 = 0; num28 < bitmap.Width; num28++)
            //        //        {
            //        //            bitmap.SetPixel(num28, num27, imgBig.GetPixel(num28 + point.X, num27 + point.Y));
            //        //        }
            //        //    }
            //        //    bitmap.Save(@"C:\gl.bmp");
            //        //}
            //        return new HSearchPoint(false, ErrorPoint, imgSmall.Size, Environment.TickCount - tickCount);
            //    }
            //}
        }

        /// <summary>
        /// 核心找图算法
        /// </summary>
        /// <param name="imgSmallIn"></param>
        /// <param name="imgBigIn"></param>
        /// <param name="xCount"></param>
        /// <param name="yCount"></param>
        /// <param name="opset"></param>
        /// <returns></returns>
        public static unsafe HSearchPoint findLikeImg(Bitmap imgSmallIn, Bitmap imgBigIn, int xCount, int yCount, int opset)
        {
            return findEqImg(imgSmallIn, imgBigIn, opset);
            int tickCount = Environment.TickCount;

            if (imgSmallIn.Width > imgBigIn.Width)
            {
                Bitmap t = imgSmallIn;
                imgSmallIn = imgBigIn;
                imgBigIn = t;
            }

            using (Bitmap imgSmall = imgSmallIn.Clone(new Rectangle(0, 0, imgSmallIn.Width, imgSmallIn.Height), PixelFormat.Format8bppIndexed))
            {
                using (Bitmap imgBig = imgBigIn.Clone(new Rectangle(0, 0, imgBigIn.Width, imgBigIn.Height), PixelFormat.Format8bppIndexed))
                {
                    BitmapData bitSmallData = imgSmall.LockBits(new Rectangle(0, 0, imgSmall.Width, imgSmall.Height), ImageLockMode.ReadWrite, imgSmall.PixelFormat);
                    BitmapData bitBigData = imgBig.LockBits(new Rectangle(0, 0, imgBig.Width, imgBig.Height), ImageLockMode.ReadWrite, imgBig.PixelFormat);
                    byte* smallPtr = (byte*)bitSmallData.Scan0;
                    byte* bigPtr = (byte*)bitBigData.Scan0;

                    int stepSmall = bitSmallData.Stride / bitSmallData.Width;
                    int stepBig = bitBigData.Stride / bitBigData.Width;

                    int xLen = Math.Min(bitSmallData.Width, xCount);// (bitSmallData.Width < xCount) ? bitSmallData.Width : xCount;
                    int yLen = Math.Min(bitSmallData.Height, yCount);// (bitSmallData.Height < yCount) ? bitSmallData.Height : yCount;

                    float xFlex = bitSmallData.Width / xLen;
                    float yFlex = bitSmallData.Height / yLen;

                    int[][] dyArrary = new int[][] { new int[xLen], new int[yLen] };
                    PL pl = new PL(dyArrary, new PP[yLen, xLen]);
                    for (int i = 0; i < yLen; i++)
                    {
                        pl.pl[1][i] = (int)(i * yFlex);
                    }
                    for (int j = 0; j < xLen; j++)
                    {
                        pl.pl[0][j] = (int)(j * xFlex);
                    }
                    for (int i = 0; i < yLen; i++)
                    {
                        for (int j = 0; j < xLen; j++)
                        {
                            long stepTemp = (pl.pl[1][i] * bitSmallData.Stride) + (pl.pl[0][j] * stepSmall);
                            pl.pp[i, j] = new PP(smallPtr[(int)((byte*)stepTemp)], smallPtr[(int)((byte*)(stepTemp + 1))], smallPtr[(int)((byte*)(stepTemp + 2))]);
                        }
                    }

                    int op = 0;
                    if (opset > 95 || opset <= 0)
                    {
                        op = 95;
                    }
                    else
                    {
                        op = opset;
                    }
                    op = ((xLen * yLen) * (op % 100)) / 100;
                    long missCount = 0L;
                    int maxX = bitBigData.Height - bitSmallData.Height;
                    int maxY = bitBigData.Width - bitSmallData.Width;
                    long num20 = 0L;
                    Point point = new Point(0, 0);
                    int stepXtemp = bitSmallData.Width / 3;
                    int stepYtemp = bitSmallData.Height / 3;
                    for (int i = 0; i <= maxX; i++)
                    {
                        for (int j = 0; j <= maxY; j++)
                        {
                            long tempValue = (j * stepBig) + (i * bitBigData.Stride);
                            if (((Math.Abs(pl.pp[0, 0].r - bigPtr[tempValue]) <= 10) && (Math.Abs(pl.pp[0, 0].g - bigPtr[tempValue + 1]) <= 10)) && (Math.Abs((pl.pp[0, 0].b - bigPtr[tempValue + 2])) <= 10))
                            {
                                if (!filter3X3(bitBigData, bitSmallData, j, i))
                                {
                                    continue;
                                }
                                if (filterAll(bitBigData, bitSmallData, j, i, op))
                                {
                                    imgBig.UnlockBits(bitBigData);
                                    imgSmall.UnlockBits(bitSmallData);
                                    return new HSearchPoint(true, new Point(j, i), imgSmall.Size, Environment.TickCount - tickCount);
                                }
                                //missCount = 0L;
                                //for (int k = 0; k < yLen; k++)
                                //{
                                //    for (int m = 0; m < xLen; m++)
                                //    {
                                //        long num26 = (tempValue + (pl.pl[0][m] * stepBig)) + (pl.pl[1][k] * bitBigData.Stride);
                                //        if (missCount >= op)
                                //        {
                                //            imgBig.UnlockBits(bitBigData);
                                //            imgSmall.UnlockBits(bitSmallData);
                                //            return new HSearchPoint(true, new Point(j, i), imgSmall.Size, Environment.TickCount - tickCount);
                                //        }
                                //        if ((pl.pp[k, m].r == bigPtr[num26]) && (pl.pp[k, m].g == bigPtr[num26 + 1]) && (pl.pp[k, m].b == bigPtr[num26 + 2]))
                                //        {
                                //            missCount++;
                                //            if (((point.X != j) || (point.Y != i)) && (missCount > num20))
                                //            {
                                //                num20 = missCount;
                                //                point = new Point(j, i);
                                //            }
                                //        }
                                //    }
                                //}
                            }
                        }
                    }
                    imgBig.UnlockBits(bitBigData);
                    imgSmall.UnlockBits(bitSmallData);
                    return new HSearchPoint(false, ErrorPoint, imgSmall.Size, Environment.TickCount - tickCount);
                }
            }
        }

        private static unsafe bool compareAll(PL pl, int yLen, int xLen, long tempValue, int stepBig, byte* bigPtr, BitmapData bitBigData)
        {
            int max = xLen * yLen;
            int less = max * 3 / 100;
            int count = 0;
            for (int k = 0; k < yLen; k++)
            {
                for (int m = 0; m < xLen; m++)
                {
                    long num26 = (tempValue + (pl.pl[0][m] * stepBig)) + (pl.pl[1][k] * bitBigData.Stride);
                    if ((pl.pp[k, m].r != bigPtr[num26]) || (pl.pp[k, m].g != bigPtr[num26 + 1]) || (pl.pp[k, m].b != bigPtr[num26 + 2]))
                    {
                        count++;
                        if (count > less)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public Image[] getCutImages(Bitmap bmp)
        {
            return new Image[] { bmp };
        }

        public static Bitmap[] getCutList(Bitmap bmp, int maxWidth, int cutHeight)
        {
            bmp = ResizeWidth(bmp, maxWidth);
            bmp.Save(@"C:\after.bmp");
            List<Bitmap> list = new List<Bitmap>();
            if (bmp.Height > cutHeight)
            {
                int num = ((bmp.Height % cutHeight) == 0) ? (bmp.Height / cutHeight) : ((bmp.Height / cutHeight) + 1);
                for (int i = 0; i < num; i++)
                {
                    if (i == (num - 1))
                    {
                        list.Add(CopyImg(bmp, i * cutHeight, bmp.Height - (cutHeight * i)));
                    }
                    else
                    {
                        list.Add(CopyImg(bmp, i * cutHeight, cutHeight));
                    }
                }
            }
            else
            {
                list.Add(bmp);
            }
            return list.ToArray();
        }

        public static Graphics getScreenGraphics()
        {
            return Graphics.FromHdc(User32.GetDC(IntPtr.Zero));
        }

        public static unsafe Point like(Bitmap img0, Bitmap img1, int op)
        {
            Point point = new Point(0, 0);
            if (img0 == img1)
            {
                return point;
            }
            Rectangle rect = new Rectangle(0, 0, img0.Width, img0.Height);
            Rectangle rectangle2 = new Rectangle(0, 0, img1.Width, img1.Height);
            BitmapData bitmapdata = img1.LockBits(rectangle2, ImageLockMode.ReadWrite, img1.PixelFormat);
            BitmapData data2 = img0.LockBits(rect, ImageLockMode.ReadWrite, img0.PixelFormat);
            List<long> list = new List<long>();
            List<long> list2 = new List<long>();
            byte* numPtr = (byte*)data2.Scan0;
            byte* numPtr2 = (byte*)bitmapdata.Scan0;
            int num = data2.Stride / data2.Width;
            int num2 = bitmapdata.Stride / bitmapdata.Width;
            int num3 = 20;
            int num4 = (data2.Width <= num3) ? num : (((data2.Stride / num3) / num) * num);
            int num5 = (data2.Height <= num3) ? 1 : (data2.Height / num3);
            int num6 = (data2.Width <= num3) ? data2.Width : num3;
            int num7 = (data2.Height <= num3) ? data2.Height : num3;
            op = ((num6 * num7) * op) / 100;
            long num8 = 0L;
            new StringBuilder();
            new StringBuilder();
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            int num9 = (bitmapdata.Height - data2.Height) + 0x11;
            int num10 = 0;
            int num11 = bitmapdata.Width - data2.Width;
            int num12 = 0;
            for (int i = num10; i <= num9; i++)
            {
                for (int m = num12; m <= num11; m++)
                {
                    long num15 = (m * num2) + (i * bitmapdata.Stride);
                    if (((Math.Abs((int)(numPtr[0] - numPtr2[(int)num15])) < 20) && (Math.Abs((int)(numPtr[1] - numPtr2[(int)(num15 + 1L)])) < 20)) && (Math.Abs((int)(numPtr[2] - numPtr2[(int)(num15 + 2L)])) < 20))
                    {
                        num8 += 1L;
                    }
                    num8 = 0L;
                    list = new List<long>();
                    list2 = new List<long>();
                    for (int n = 0; n < num7; n++)
                    {
                        for (int num17 = 0; num17 < num6; num17++)
                        {
                            if ((n == (num7 - 1)) && (num17 == (num6 - 1)))
                            {
                                dictionary.Add((m + ":" + i).ToString(), (int)num8);
                            }
                            long item = ((((num4 * num17) / num) * num2) + (m * num2)) + ((i + (n * num5)) * bitmapdata.Stride);
                            long num19 = (num4 * num17) + ((n * data2.Stride) * num5);
                            if (num8 >= op)
                            {
                                point = new Point(m, i);
                                break;
                            }
                            if (((Math.Abs((int)(numPtr[(int)num19] - numPtr2[(int)item])) < 20) && (Math.Abs((int)(numPtr[(int)(num19 + 1L)] - numPtr2[(int)(item + 1L)])) < 20)) && (Math.Abs((int)(numPtr[(int)(num19 + 2L)] - numPtr2[(int)(item + 2L)])) < 20))
                            {
                                list.Add(num19);
                                list2.Add(item);
                                num8 += 1L;
                            }
                        }
                    }
                }
            }
            builder.Clear();
            builder2.Clear();
            for (int j = 0; j < list.Count; j++)
            {
                builder.Append(((byte)(numPtr + list[j])).ToString().PadRight(5)).Append(" ");
                builder.Append(((byte)(numPtr + (list[j] + 1L))).ToString().PadRight(5)).Append(" ");
                builder.Append(((byte)(numPtr + (list[j] + 2L))).ToString().PadRight(5)).Append(" ");
                numPtr[list[j]] = 0;
                numPtr[(int)(list[j] + 1L)] = 0;
                numPtr[(int)(list[j] + 2L)] = 0;
            }
            for (int k = 0; k < list2.Count; k++)
            {
                builder2.Append(((byte)(numPtr2 + list2[k])).ToString().PadRight(5)).Append(" ");
                builder2.Append(((byte)(numPtr2 + (list2[k] + 1L))).ToString().PadRight(5)).Append(" ");
                builder2.Append(((byte)(numPtr2 + (list2[k] + 2L))).ToString().PadRight(5)).Append(" ");
                numPtr2[list2[k]] = 0;
                numPtr2[(int)(list2[k] + 1L)] = 0;
                numPtr2[(int)(list2[k] + 2L)] = 0;
            }
            img1.UnlockBits(bitmapdata);
            img0.UnlockBits(data2);
            img1.Save(@"c:\v1.bmp");
            Dictionary<string, int>.Enumerator enumerator = dictionary.GetEnumerator();
            int num22 = 0;
            string key = "";
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, int> current = enumerator.Current;
                if (current.Value > num22)
                {
                    KeyValuePair<string, int> pair2 = enumerator.Current;
                    num22 = pair2.Value;
                    KeyValuePair<string, int> pair3 = enumerator.Current;
                    key = pair3.Key;
                }
            }
            key.ToString();
            img0.Save(@"c:\v2.bmp");
            return new Point(0, 0);
        }

        public static unsafe Point likeMaxPoint(Bitmap img0, Bitmap img1, int op)
        {
            Point point = new Point(0, 0);
            if (img0 == img1)
            {
                return point;
            }
            Rectangle rect = new Rectangle(0, 0, img0.Width, img0.Height);
            Rectangle rectangle2 = new Rectangle(0, 0, img1.Width, img1.Height);
            BitmapData bitmapdata = img1.LockBits(rectangle2, ImageLockMode.ReadWrite, img1.PixelFormat);
            BitmapData data2 = img0.LockBits(rect, ImageLockMode.ReadWrite, img0.PixelFormat);
            List<long> list = new List<long>();
            List<long> list2 = new List<long>();
            byte* numPtr = (byte*)data2.Scan0;
            byte* numPtr2 = (byte*)bitmapdata.Scan0;
            int num = data2.Stride / data2.Width;
            int num2 = bitmapdata.Stride / bitmapdata.Width;
            int num3 = 20;
            int num4 = (data2.Width <= num3) ? num : (((data2.Stride / num3) / num) * num);
            int num5 = (data2.Height <= num3) ? 1 : (data2.Height / num3);
            int num6 = (data2.Width <= num3) ? data2.Width : num3;
            int num7 = (data2.Height <= num3) ? data2.Height : num3;
            op = ((num6 * num7) * op) / 100;
            long num8 = 0L;
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            int num9 = bitmapdata.Height - data2.Height;
            int num10 = 0;
            int num11 = bitmapdata.Width - data2.Width;
            int num12 = 0;
            for (int i = num10; i <= num9; i++)
            {
                for (int j = num12; j <= num11; j++)
                {
                    long num15 = (j * num2) + (i * bitmapdata.Stride);
                    if (((Math.Abs((int)(numPtr[0] - numPtr2[(int)num15])) <= 20) && (Math.Abs((int)(numPtr[1] - numPtr2[(int)(num15 + 1L)])) <= 20)) && (Math.Abs((int)(numPtr[2] - numPtr2[(int)(num15 + 2L)])) <= 20))
                    {
                        num8 = 0L;
                        list.Clear();
                        list2.Clear();
                        for (int k = 0; k < num7; k++)
                        {
                            for (int m = 0; m < num6; m++)
                            {
                                if ((k == (num7 - 1)) && (m == (num6 - 1)))
                                {
                                    dictionary.Add((j + ":" + i).ToString(), (int)num8);
                                }
                                long item = ((((num4 * m) / num) * num2) + (j * num2)) + ((i + (k * num5)) * bitmapdata.Stride);
                                long num19 = (num4 * m) + ((k * data2.Stride) * num5);
                                if (num8 >= op)
                                {
                                    img1.UnlockBits(bitmapdata);
                                    img0.UnlockBits(data2);
                                    return new Point(j, i);
                                }
                                if (((Math.Abs((int)(numPtr[(int)num19] - numPtr2[(int)item])) < 20) && (Math.Abs((int)(numPtr[(int)(num19 + 1L)] - numPtr2[(int)(item + 1L)])) < 20)) && (Math.Abs((int)(numPtr[(int)(num19 + 2L)] - numPtr2[(int)(item + 2L)])) < 20))
                                {
                                    list.Add(num19);
                                    list2.Add(item);
                                    num8 += 1L;
                                }
                            }
                        }
                    }
                }
            }
            img1.UnlockBits(bitmapdata);
            img0.UnlockBits(data2);
            if (dictionary.Count == 0)
            {
                return new Point(-1, -1);
            }
            Dictionary<string, int>.Enumerator enumerator = dictionary.GetEnumerator();
            int num20 = 0;
            string key = "-1,-1";
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, int> current = enumerator.Current;
                if (current.Value > num20)
                {
                    KeyValuePair<string, int> pair2 = enumerator.Current;
                    num20 = pair2.Value;
                    KeyValuePair<string, int> pair3 = enumerator.Current;
                    key = pair3.Key;
                }
            }
            string[] strArray = key.Split(new char[] { ':' });
            return new Point(Convert.ToInt32(strArray[0]), Convert.ToInt32(strArray[1]));
        }

        public static unsafe Point pri_like100(Bitmap img0, Bitmap img1, int op)
        {
            if (img0 == img1)
            {
                return new Point(0, 0);
            }
            Rectangle rect = new Rectangle(0, 0, img0.Width, img0.Height);
            Rectangle rectangle2 = new Rectangle(0, 0, img1.Width, img1.Height);
            BitmapData bitmapdata = img1.LockBits(rectangle2, ImageLockMode.ReadWrite, img1.PixelFormat);
            BitmapData data2 = img0.LockBits(rect, ImageLockMode.ReadWrite, img0.PixelFormat);
            byte* numPtr = (byte*)data2.Scan0;
            byte* numPtr2 = (byte*)bitmapdata.Scan0;
            int num = (data2.Stride * 3) / 30;
            int num2 = data2.Height / 10;
            long num3 = 0L;
            for (int i = 0; i <= (bitmapdata.Height - data2.Height); i++)
            {
                for (int j = 0; j <= (bitmapdata.Width - data2.Width); j++)
                {
                    num3 = 0L;
                    for (int k = 0; k < 10; k++)
                    {
                        for (int m = 0; m < 10; m++)
                        {
                            if (num3 >= op)
                            {
                                img1.UnlockBits(bitmapdata);
                                img0.UnlockBits(data2);
                                return new Point(j, i);
                            }
                            long num8 = ((num * m) + (j * 3)) + ((i + (k * num2)) * bitmapdata.Stride);
                            long num9 = (num * m) + ((k * data2.Stride) * num2);
                            num9 += 1L;
                            num8 += 1L;
                            if (Math.Abs((int)(numPtr[(int)num9] - numPtr2[(int)num8])) < 20)
                            {
                                num9 += 1L;
                                num8 += 1L;
                                if (Math.Abs((int)(numPtr[(int)num9] - numPtr2[(int)num8])) < 20)
                                {
                                    num9 += 1L;
                                    num8 += 1L;
                                    if (Math.Abs((int)(numPtr[(int)num9] - numPtr2[(int)num8])) < 20)
                                    {
                                        num3 += 1L;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            img1.UnlockBits(bitmapdata);
            img0.UnlockBits(data2);
            return new Point(-1, -1);
        }

        public static unsafe List<Point> pri_like100list(Bitmap img0, Bitmap img1, int op)
        {
            List<Point> list = new List<Point>();
            if (img0 != img1)
            {
                Rectangle rect = new Rectangle(0, 0, img0.Width, img0.Height);
                Rectangle rectangle2 = new Rectangle(0, 0, img1.Width, img1.Height);
                BitmapData bitmapdata = img1.LockBits(rectangle2, ImageLockMode.ReadWrite, img1.PixelFormat);
                BitmapData data2 = img0.LockBits(rect, ImageLockMode.ReadWrite, img0.PixelFormat);
                byte* numPtr = (byte*)data2.Scan0;
                byte* numPtr2 = (byte*)bitmapdata.Scan0;
                int num = (data2.Stride * 3) / 30;
                int num2 = data2.Height / 10;
                long num3 = 0L;
                int x = 0;
                for (int i = 0; i < (bitmapdata.Height - data2.Height); i++)
                {
                    for (x = 0; x < (bitmapdata.Width - data2.Width); x++)
                    {
                        num3 = 0L;
                        for (int j = 0; j < 10; j++)
                        {
                            for (int k = 0; k < 10; k++)
                            {
                                if (num3 >= op)
                                {
                                    list.Add(new Point(x, i));
                                    x++;
                                    continue;
                                }
                                long num8 = ((num * k) + (x * 3)) + ((i + (j * num2)) * bitmapdata.Stride);
                                long num9 = (num * k) + ((j * data2.Stride) * num2);
                                num9 += 1L;
                                num8 += 1L;
                                if (Math.Abs((int)(numPtr[(int)num9] - numPtr2[(int)num8])) < 10)
                                {
                                    num9 += 1L;
                                    num8 += 1L;
                                    if (Math.Abs((int)(numPtr[(int)num9] - numPtr2[(int)num8])) < 10)
                                    {
                                        num9 += 1L;
                                        num8 += 1L;
                                        if (Math.Abs((int)(numPtr[(int)num9] - numPtr2[(int)num8])) < 10)
                                        {
                                            num3 += 1L;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                img1.UnlockBits(bitmapdata);
                img0.UnlockBits(data2);
            }
            return list;
        }

        public static unsafe Point prit_likeSmall(Bitmap img0, Bitmap img1, int op)
        {
            if (img0 == img1)
            {
                return new Point(0, 0);
            }
            Rectangle rect = new Rectangle(0, 0, img0.Width, img0.Height);
            Rectangle rectangle2 = new Rectangle(0, 0, img1.Width, img1.Height);
            BitmapData bitmapdata = img1.LockBits(rectangle2, ImageLockMode.ReadWrite, img1.PixelFormat);
            BitmapData data2 = img0.LockBits(rect, ImageLockMode.ReadWrite, img0.PixelFormat);
            byte* numPtr = (byte*)data2.Scan0;
            byte* numPtr2 = (byte*)bitmapdata.Scan0;
            int num = (data2.Stride * 3) / 30;
            int num2 = data2.Height / 10;
            long num3 = 0L;
            for (int i = 0; i < (bitmapdata.Height - data2.Height); i++)
            {
                for (int j = 0; j < (bitmapdata.Width - data2.Width); j++)
                {
                    num3 = 0L;
                    for (int k = 0; k < 10; k++)
                    {
                        for (int m = 0; m < 10; m++)
                        {
                            if (num3 >= op)
                            {
                                img1.UnlockBits(bitmapdata);
                                img0.UnlockBits(data2);
                                return new Point(j, i);
                            }
                            long num8 = ((num * m) + (j * 3)) + ((i + (k * num2)) * bitmapdata.Stride);
                            long num9 = (num * m) + ((k * data2.Stride) * num2);
                            if (numPtr[(int)num9] == numPtr2[(int)num8])
                            {
                                num3 += 1L;
                            }
                        }
                    }
                }
            }
            img1.UnlockBits(bitmapdata);
            img0.UnlockBits(data2);
            return new Point(-1, -1);
        }

        public static unsafe void QSaveImg(Bitmap bmp, string file)
        {
            StringBuilder builder = new StringBuilder();
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bitmapdata = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            byte* numPtr = (byte*)bitmapdata.Scan0;
            bool flag = (bitmapdata.Stride / bitmapdata.Width) == 4;
            for (int i = 0; i < bitmapdata.Height; i++)
            {
                builder.Append("\r\n");
                for (int j = 0; j < bitmapdata.Stride; j++)
                {
                    if (!flag || (((j + 1) % 4) != 0))
                    {
                        builder.Append(((byte)(numPtr + (j + (i * bitmapdata.Stride)))).ToString().PadRight(4));
                    }
                }
            }
            bmp.UnlockBits(bitmapdata);
        }

        public static unsafe Bitmap ResizeWidth(Bitmap bmp, int maxWidth)
        {
            Bitmap bitmap = new Bitmap(maxWidth, bmp.Height);
            if (bmp.Width > maxWidth)
            {
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                Rectangle rectangle2 = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                BitmapData bitmapdata = bmp.LockBits(rect, ImageLockMode.ReadOnly, bmp.PixelFormat);
                BitmapData data2 = bitmap.LockBits(rectangle2, ImageLockMode.ReadWrite, bitmap.PixelFormat);
                byte* numPtr = (byte*)bitmapdata.Scan0;
                byte* numPtr2 = (byte*)data2.Scan0;
                int num = bitmapdata.Stride / bitmapdata.Width;
                int num2 = data2.Stride / data2.Width;
                int num3 = (((int)((((float)bitmapdata.Width) / ((float)maxWidth)) / ((float)num))) * num) * num;
                for (int i = 0; i < bmp.Height; i++)
                {
                    for (int j = 0; j < maxWidth; j++)
                    {
                        for (int k = 0; k < num; k++)
                        {
                            int index = (((i * bitmapdata.Stride) + num3) + (j * num)) + k;
                            int num8 = ((i * data2.Stride) + (j * num2)) + k;
                            numPtr2[num8] = numPtr[index];
                            if (num < num2)
                            {
                                numPtr2[num8 + 1] = 0xff;
                            }
                        }
                    }
                }
                bmp.UnlockBits(bitmapdata);
                bitmap.UnlockBits(data2);
                return bitmap;
            }
            return bmp;
        }

        /// <summary>
        /// 图片转base64
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static string getImgBase64(Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Bmp);
                return Convert.ToBase64String(ms.GetBuffer());
            }
        }

        public static byte[] getImgByte(Bitmap bmp)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Bitmap temp = (Bitmap)bmp.Clone())
                {
                    temp.Save(ms, ImageFormat.Png);
                    return ms.GetBuffer();
                }
            }
        }

        public static bool is255Suc(byte[] bt1, Bitmap img)
        {
            int max = bt1.Length * 90 / 100;
            byte[] bt2 = getImgByte(img);
            int min = Math.Min(bt1.Length, bt2.Length);
            for (int i = 0; i < min; i++)
            {
                if (bt1[i] != bt2[i])
                {
                    max--;
                    if (max <= 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static string UUIDImg(Bitmap img)
        {
            if (img == null)
            {
                return "user hyl - 279241400@qq.com";
            }
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Bmp);
                return Md5.getMd5(ms.GetBuffer());
            }
        }
        public static string getImg255Base64(Bitmap img)
        {

            using (MemoryStream ms = new MemoryStream())
            {
                using (Bitmap temp = get255Img(img))
                {
                    temp.Save(ms, ImageFormat.Png);
                    return Convert.ToBase64String(ms.GetBuffer());
                }
            }
        }

        public static string getImg8Base64(Bitmap img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Bitmap temp = get8Img(img))
                {
                    temp.Save(ms, ImageFormat.Png);
                    return Convert.ToBase64String(ms.GetBuffer());
                }
            }
        }

        /// <summary>
        /// 获取黑白照
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static Bitmap get255Img(Bitmap bmp)
        {
            return bmp.Clone(new Rectangle(0, 0, bmp.Width, bmp.Height), PixelFormat.Format1bppIndexed);
        }

        public static Bitmap get8Img(Bitmap bmp)
        {
            return bmp.Clone(new Rectangle(0, 0, bmp.Width, bmp.Height), PixelFormat.Format8bppIndexed);
        }


        private static unsafe void saveIMG1(Image im, byte* ptrs0, PL pl, int fx, int fy, int xstep, int ystemp, string file)
        {
            int num = fx * xstep;
            int num2 = fy * ystemp;
            for (int i = 0; i < pl.pl[1].Length; i++)
            {
                for (int j = 0; j < pl.pl[0].Length; j++)
                {
                    long num5 = num + (pl.pl[0][j] * xstep);
                    long num6 = num2 + (pl.pl[1][i] * ystemp);
                    ptrs0[(int)(num5 + num6)] = 0xff;
                    ptrs0[(int)((num5 + num6) + 1L)] = 0xff;
                    ptrs0[(int)((num5 + num6) + 2L)] = 0xff;
                }
            }
            im.Save(file);
        }
    }
}

