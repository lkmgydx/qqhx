using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool
{
    class HasSomeObj
    {
        static List<int[]> ar = new List<int[]>();
        static HasSomeObj()
        {
            addColor(148, 247, 247);
            addColor(148, 251, 247);

            addColor(198, 227, 214);
            // addColor(214, 255, 255);
            addColor(231, 251, 247);
            // addColor(189, 227, 214);  冲突了
            // addColor(173, 231, 231);
            addColor(189, 227, 231);
            addColor(181, 243, 247);
            addColor(198, 227, 231);
            addColor(231, 247, 247);

            //包  绿色
            //  addColor(189 ,243 ,247);
            addColor(140, 251, 247);
            addColor(148, 227, 231);


            //长箱子 中间颜色
            addColor(198, 247, 247);
            addColor(231, 243, 247);
            // addColor(231, 255, 255);
            //addColor(181, 255, 255);
            addColor(206, 231, 231);
            addColor(214, 235, 231);

            //长箱子  边上颜色
            addColor(140, 227, 206);
            addColor(181, 239, 247);
            //addColor(173, 231, 231);
            addColor(90, 207, 198);
            addColor(156, 235, 222);
        }
        public static bool HasSome(Bitmap bmp)
        {
            for (int i = 0; i < ar.Count; i++)
            {
                if (hasSome2(bmp, ar[i][0], ar[i][1], ar[i][2]))
                {
                    //string rst = ar[i][0] + "," + ar[i][1] + "," + ar[i][2];
                    return true;
                }
            }
            return false;
        }
        private Bitmap bmp;


        private static void addColor(int r, int g, int b)
        {
            ar.Add(new int
                [] { r, g, b });
        }

        private static unsafe bool hasSome2(Bitmap bmp, int r, int g, int b)
        {
            BitmapData bitSmallData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            try
            {
                int stepXtemp = 1;
                int stepYtemp = 1;
                int stepSmall = bitSmallData.Stride / bitSmallData.Width;
                var smallPtr = (byte*)bitSmallData.Scan0;
                for (int t1 = 0; t1 < bmp.Width; t1++)
                {
                    for (int t2 = 0; t2 < bmp.Height; t2++)
                    {
                        int x = t2 * stepXtemp;
                        int y = t1 * stepYtemp;

                        var ptrTempSmall = bitSmallData.Stride * y + x * stepSmall;
                        //int tb = smallPtr[ptrTempSmall++];
                        //int tg = smallPtr[ptrTempSmall++];
                        //int tr = smallPtr[ptrTempSmall++];
                        //if (r == tr && g == tg && b == tb)
                        if (b == smallPtr[ptrTempSmall++] && g == smallPtr[ptrTempSmall++] && r == smallPtr[ptrTempSmall++])
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                bmp.UnlockBits(bitSmallData);
            }
            return false;
        }
    }
}
