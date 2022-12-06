using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CopyScreen
{
    public class Utils
    {
        /// <summary>
        /// 任意四个点组成的矩形
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns></returns>
        public static Rectangle getRec(Point pt1, Point pt2)
        {
            int px = Math.Min(pt1.X, pt2.X);
            int py = Math.Min(pt1.Y, pt2.Y);
            int width = Math.Abs(pt1.X - pt2.X);
            int height = Math.Abs(pt1.Y - pt2.Y);
            return new Rectangle(px, py, width, height);
        }

        /// <summary>
        /// 灰度图片
        /// </summary>
        /// <param name="bmp"></param>
        public static void filter(Bitmap bmp)
        {
            // Lock the bitmap's bits.  
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                  bmp.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            int step = Math.Abs(bmpData.Stride) / bmpData.Width;

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            // Set every third value to 255. A 24bpp bitmap will look red.  
            for (int counter = 0; counter < rgbValues.Length; counter += step)
            {
                rgbValues[counter] = (byte)(rgbValues[counter] * 0.7);
                rgbValues[counter + 1] = (byte)(rgbValues[counter + 1] * 0.7);
                rgbValues[counter + 2] = (byte)(rgbValues[counter + 2] * 0.7);
            }


            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);
        }

        /// <summary>
        /// 提取指定颜色
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Bitmap filter(Bitmap bmp, Color color)
        {
            // Lock the bitmap's bits.  
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                 bmp.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            int step = Math.Abs(bmpData.Stride) / bmpData.Width;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            // Set every third value to 255. A 24bpp bitmap will look red.  
            for (int counter = 0; counter < rgbValues.Length; counter += step)
            {
                if (rgbValues[counter] != color.B || rgbValues[counter + 1] != color.G || rgbValues[counter + 2] != color.R)
                {
                    rgbValues[counter] = 0;
                    rgbValues[counter + 1] = 0;
                    rgbValues[counter + 2] = 0;
                    rgbValues[counter + 3] = 0;
                }
            }

            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);
            return bmp;
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
    }
}
