using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool
{


    public sealed class OcrNum
    {
        private struct Hp
        {
            public int X { get; set; }
            public int Y { get; set; }

            public static Hp Empty = new Hp();
        }

        private static readonly int step = 5;

        private static Hp HasNum_One = ConvetToImagePoint(4);
        private static Hp HasNum_Two = ConvetToImagePoint(39);



        private static Hp Num_1 = ConvetToImagePoint(1);
        private static Hp Num_2 = ConvetToImagePoint(2);

        private static Hp Num_7 = ConvetToImagePoint(7);
        private static Hp Num_8 = ConvetToImagePoint(8);
        private static Hp Num_9 = ConvetToImagePoint(9);
        private static Hp Num_11 = ConvetToImagePoint(11);
        private static Hp Num_20 = ConvetToImagePoint(20);
        private static Hp Num_22 = ConvetToImagePoint(32);
        private static Hp Num_24 = ConvetToImagePoint(24);
        private static Hp Num_27 = ConvetToImagePoint(27);
        private static Hp Num_33 = ConvetToImagePoint(33);
        private static Hp Num_36 = ConvetToImagePoint(36);
        private static Hp Num_37 = ConvetToImagePoint(37);
        private static Hp Num_40 = ConvetToImagePoint(40);

        private static Hp NN_1 = ConvetToImagePoint(1);
        private static Hp NN_9 = ConvetToImagePoint(9);
        private static Hp NN_19 = ConvetToImagePoint(19);

        private static Hp NN_20 = ConvetToImagePoint(20);
        private static Hp NN_13 = ConvetToImagePoint(13);

        /// <summary>
        /// 查找主体
        /// </summary>
        private class FinPro
        {
            private int FLoat = 0;
            public Bitmap BitMap { get; set; }
            public FinPro(Bitmap bmp)
            {
                BitMap = bmp;
            }

            public int getNum6_9()
            {
                if (hasPoint(Num_37)) return 0;
                if (hasPoint(Num_40)) return 9;
                if (hasPoint(Num_7)) return 6;
                if (hasPoint(Num_2)) return 8;
                return -1;
            }

            public Hp getPoint()
            {
                Hp hp = new Hp();
                FLoat = 0;
                hp.X = getNumb();
                MoveNextNums();
                hp.Y = getNumb();
                return hp;
            }

            private void MonveNextNum()
            {
                FLoat += step + 1;
            }

            private void MoveNextNums()
            {
                FLoat += 26;
            }

            private int getNumb()
            {
                int n1 = getNumbFromImage_Single();
                if (n1 == -1)
                {
                    n1 = 0;
                }
                MonveNextNum();
                int n2 = getNumbFromImage_Single();
                if (n2 == -1)
                {
                    n2 = 0;
                }
                MonveNextNum();
                int n3 = getNumbFromImage_Single();
                return n1 * 100 + n2 * 10 + n3;
            }

            public int getNumbFromImage_Single()
            {

                if (hasPoint(HasNum_One) || hasPoint(HasNum_Two))
                {
                    if (hasPoint(Num_22)) return 9;
                    if (hasPoint(Num_20)) return 0;
                    if (hasPoint(Num_24)) return 4;
                    if (hasPoint(Num_8)) return 1;
                    if (hasPoint(Num_27)) return 2;
                    if (hasPoint(Num_33)) return 7;
                    if (hasPoint(Num_1)) return 5;
                    if (hasPoint(Num_9)) return 6;
                    if (hasPoint(Num_11)) return 8;
                    return 3;
                }
                else
                {
                    return 0;
                }
            }

            private bool hasPoint(Hp hp)
            {
                return BitMap.GetPixel(FLoat + hp.X, hp.Y).R > 200;
            }

            internal int getNum()
            {
                if (BitMap.Width == 5)
                {
                    return 0;
                }
                else
                {
                    if (hasPoint(NN_1)) return 5;
                    else if (hasPoint(NN_9)) return 4;
                    else if (hasPoint(NN_19)) return 0;
                    else if (hasPoint(NN_20)) return 8;
                    else if (hasPoint(NN_13)) return 2;
                    return 3;
                }
            }
        }



        public static Point getNumFromImage(Bitmap bmp)
        {
            Hp hp = new FinPro(bmp).getPoint();
            if (hp.X == -1 || hp.Y == -1)
            {
                return new Point(0, 0);
            }
            return new Point(hp.X, hp.Y);
        }


        public static int getNumFromImageBg(Image bmp)
        {
            SpitImg si = new SpitImg(bmp);
            int ints = getIntFromImage(si.listImg());
            return ints;
        }

        static int getIntFromImage(List<Bitmap> bmps)
        {
            List<int> li = new List<int>();
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < bmps.Count; i++)
            {
                Bitmap bmp = bmps[i];
                if (isDot(bmp))
                {
                    s.Append("1");
                }
                else
                {
                    s.Append(new FinPro(bmp).getNum());
                }
            }
            return int.Parse(s.ToString());
        }

        static string getNumFromImg(Bitmap bmp)
        {
            if (bmp == null) return "";
            return getNumFromImage_N(bmp).ToString();
        }

        private static int getNumFromImage_N(Bitmap bmp)
        {
            return new FinPro(bmp).getNum();
        }

        static bool isDot(Bitmap bmp)
        {
            return bmp.Width == 3 && bmp.Height == 8;
        }

        private class SpitImg
        {
            Bitmap bmp;
            public SpitImg(Image bmp)
            {
                this.bmp = (Bitmap)bmp;
            }

            public List<Bitmap> listImg()
            {
                List<Bitmap> li = new List<Bitmap>();

                int startX = getNextColorY(0);
                if (startX == -1)
                {
                    return li;
                }

                int prx = startX;
                for (int i = startX; i < this.bmp.Width; i++)//从可能有数字的地方开始切割图片
                {
                    i = getNextEmptyColumnY(i);
                    Bitmap btemp = this.bmp.Clone(new Rectangle(prx, 0, i - prx, bmp.Height), System.Drawing.Imaging.PixelFormat.DontCare);
                    li.Add(splitUP_DOWN(btemp));
                    //li[0].Save()
                    i = getNextColorY(i);
                    if (i == -1)
                    {
                        break;
                    }
                    prx = i;
                }

                for (int i = 0; i < li.Count; i++)
                {
                    li[i].Save("E:\\bmp_" + i + ".bmp");
                }
                return li;
            }

            private Bitmap splitUP_DOWN(Bitmap bmp)
            {
                int start = getTop_Down_hasColor(bmp);
                int end = getDown_Top_hasColor(bmp);
                if (end == -1 || start == end)
                {
                    return new Bitmap(0, 0);
                }
                return bmp.Clone(new Rectangle(0, start, bmp.Width, end - start), System.Drawing.Imaging.PixelFormat.DontCare);
            }
            /// <summary>
            /// 从上往下找到有颜色的点
            /// </summary>
            /// <returns></returns>
            private int getTop_Down_hasColor(Bitmap bmp)
            {
                for (int i = 0; i < bmp.Height; i++)
                {
                    for (int j = 0; j < bmp.Width; j++)
                    {
                        if (hasColor(bmp.GetPixel(j, i)))
                        {
                            return i;
                        }
                    }
                }
                return 1;
            }
            /// <summary>
            /// 从下往上找到有颜色的行
            /// </summary>
            /// <returns></returns>
            private int getDown_Top_hasColor(Bitmap bmp)
            {
                for (int i = bmp.Height - 1; i >= 0; i--)
                {
                    for (int j = 0; j < bmp.Width; j++)
                    {
                        if (hasColor(bmp.GetPixel(j, i)))
                        {
                            return i + 1;
                        }
                    }
                }
                return -1;
            }

            /// <summary>
            /// 下一列有颜色的列,如果没有或最后一列,将返回-1;
            /// </summary>
            /// <param name="x"></param>
            /// <returns></returns>
            private int getNextColorY(int x)
            {
                for (int i = x; i < this.bmp.Width - 1; i++) //查找第一列有点的位置
                {
                    if (hasColorY(i))
                    {
                        return i;
                    }
                }
                return -1;
            }

            private int getNextColorX(int y)
            {
                for (int i = y; i < this.bmp.Height - 1; i++) //查找第一列有点的位置
                {
                    if (hasColorX(i))
                    {
                        return i;
                    }
                }
                return -1;
            }

            private int getNextEmptyColumnY(int x)
            {
                for (int i = x + 1; i < this.bmp.Width - 1; i++)
                {
                    if (!hasColorY(i))
                    {
                        return i;
                    }
                }
                return this.bmp.Width;
            }

            private int getNextEmptyColumnX(int y)
            {
                for (int i = y + 1; i < this.bmp.Height - 1; i++)
                {
                    if (!hasColorY(i))
                    {
                        return i;
                    }
                }
                return this.bmp.Height;
            }


            private const byte EMPTY_COLOR = (byte)0;

            private bool hasColorY(int column)
            {
                for (int i = 0; i < this.bmp.Height; i++)
                {
                    if (hasColor(this.bmp.GetPixel(column, i)))
                    {
                        return true;
                    }
                }
                return false;
            }
            /// <summary>
            /// 是否为非黑色
            /// </summary>
            /// <param name="c"></param>
            /// <returns></returns>
            private bool hasColor(Color c)
            {
                return c.R + c.G + c.B > 30;
            }

            private bool hasColorX(int row)
            {
                for (int i = 0; i < this.bmp.Width; i++)
                {
                    Color c = this.bmp.GetPixel(i, row);
                    if (c.R + c.G + c.B > 10)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 转成图片坐标系统,不考虑x,y 为0情况
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private static Hp ConvetToImagePoint(int v)
        {
            int x = v % 5;
            x = x == 0 ? 5 : x;
            --x;
            int y = v / 5;
            if (v % 5 == 0)
            {
                y = y - 1;
            }
            Hp hp = new Hp();
            hp.X = x;
            hp.Y = y;
            return hp;
        }
    }
}
