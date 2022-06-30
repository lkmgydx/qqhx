using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace ConsoleApplication2
{
    public class ImageM : IDisposable
    {
        Bitmap imgSmall = null;
        List<PointInfo> li = new List<PointInfo>();
        public ImageM(string path)
        {
            using (Bitmap bmp = (Bitmap)Image.FromFile(path))
            {
                imgSmall = bmp.Clone(new Rectangle(0, 0, bmp.Width, bmp.Height), PixelFormat.Format1bppIndexed);
            }
        }

        public List<Point[]> SplitImageAll()
        {
            PointInfo[][] pis = new PointInfo[imgSmall.Width][];
            for (int i = 0; i < imgSmall.Width; i++) //初始化整个结构
            {
                pis[i] = new PointInfo[imgSmall.Height];
                for (int j = 0; j < imgSmall.Height; j++)
                {
                    PointInfo pi = new PointInfo(i, j, imgSmall.GetPixel(i, j).Name == "ff000000");
                    pis[i][j] = pi;
                }
            }

            for (int i = 1; i < imgSmall.Width - 1; i++) //初始化整个结构
            {
                for (int j = 1; j < imgSmall.Height - 1; j++)
                {
                    pis[i][j].Right = pis[i + 1][j];
                    pis[i + 1][j].Left = pis[i][j];

                    pis[i][j].BottomRight = pis[i + 1][j + 1];
                    pis[i + 1][j + 1].LeftTop = pis[i][j];

                    pis[i][j].Bottom = pis[i][j + 1];
                    pis[i][j + 1].Top = pis[i][j];

                    pis[i][j].BottomLeft = pis[i - 1][j + 1];
                    pis[i - 1][j + 1].RightTop = pis[i][j];
                }
            }
            Point pt = findIndex(imgSmall);
            pis[pt.X][pt.Y].HasDeal = true;
            PointInfo[] ps = pis[pt.X][pt.Y].Next();
            if (ps.Length != 2)
            {

            }
            return null;
        }


        public List<Point[]> SplitImage()
        {
            file(imgSmall);
            //Format1bppIndexed
            return null;
        }



        private void file(Bitmap bmp)
        {

            using (Bitmap imgSmall = bmp.Clone(new Rectangle(0, 0, bmp.Width, bmp.Height), PixelFormat.Format1bppIndexed))
            {
                Point pt = findIndex(imgSmall);
                SearchSplitImg si = new SearchSplitImg(pt, imgSmall);
                si.getRoups();

                return;
            }
        }

        private Point findIndex(Bitmap imgSmall)
        {
            int maxWidth = imgSmall.Width;
            int maxHeghit = imgSmall.Height;
            int currentx = 0;
            int currenty = 0;
            for (int cuurent = 0; cuurent < maxWidth; cuurent++)
            {
                currenty = Math.Min(cuurent, maxHeghit);
                for (int i = 0; currenty >= 0; i++, currenty--)
                {
                    currentx = Math.Min(i, maxWidth);
                    currenty = Math.Min(currenty, maxHeghit);

                    Console.Write(currentx + ":" + currenty + " ");

                    if (imgSmall.GetPixel(currentx, currenty).Name == "ff000000")
                    {
                        return new Point(currentx, currenty);
                    }
                }
                Console.WriteLine("  ");
            }
            return new Point(-1, -1);
        }

        public void Dispose()
        {
            if (imgSmall != null)
            {
                imgSmall.Dispose();
            }
        }
    }
}
