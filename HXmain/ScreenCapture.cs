﻿namespace HXmain
{
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using WSTools.WSLog;

    public class ScreenCapture
    {
        private int _hBottomPix;
        private int _hLeftPix;
        private int _hRightPix;
        private int _hTopPix;
        private Bitmap _image;
        private Size _imageSize;
        private IntPtr _intPtr;
        private HUser32.RECT _rect;
        private Point _ZeroPoint;

        public ScreenCapture()
        {
            initConfig();
        }

        public ScreenCapture(IntPtr handle) : this()
        {
            this.Handle = handle;
        }

        private void initConfig()
        {
            this._imageSize = Size.Empty;
            this._ZeroPoint = new Point(0, 0);
            this._rect = new HUser32.RECT();
        }

        private void getScreenCaputureUserCopy()
        {
            Graphics.FromImage(this._image).CopyFromScreen(new Point(this._hLeftPix, this._hTopPix), this._ZeroPoint, this._imageSize);
        }

        private void getScreenCaputureUserHandle()
        {
            long start = Environment.TickCount;
            IntPtr windowDC = HUser32.GetWindowDC(this._intPtr);
            IntPtr hDC = HGDI32.CreateCompatibleDC(windowDC);
            IntPtr hObject = HGDI32.CreateCompatibleBitmap(windowDC, this._imageSize.Width, this._imageSize.Height);
            IntPtr ptr4 = HGDI32.SelectObject(hDC, hObject);
            HGDI32.BitBlt(hDC, 0, 0, this._imageSize.Width, this._imageSize.Height, windowDC, 0, 0, 0xcc0020);
            HGDI32.SelectObject(hDC, ptr4);
            HGDI32.DeleteDC(hDC);
            HUser32.ReleaseDC(this._intPtr, windowDC);
            this._image = Image.FromHbitmap(hObject);
            HGDI32.DeleteObject(hObject);
            long time = Environment.TickCount - start;
            if (time / 1000 > 1)
            {
                Log.logErrorForce("getScreenCaputureUserHandle获取图片时间超过1秒!" + time + "s");
            }
        }


        public Bitmap getImg(Rectangle rec)
        {
            long start = Environment.TickCount;
            IntPtr windowDC = HUser32.GetWindowDC(this._intPtr);
            IntPtr hDC = HGDI32.CreateCompatibleDC(windowDC);
            IntPtr hObject = HGDI32.CreateCompatibleBitmap(windowDC, rec.Width, rec.Height);
            IntPtr ptr4 = HGDI32.SelectObject(hDC, hObject);
            HGDI32.BitBlt(hDC, 0, 0, rec.Width, rec.Height, windowDC, rec.X, rec.Y, 0xcc0020);
            HGDI32.SelectObject(hDC, ptr4);
            HGDI32.DeleteDC(hDC);
            HUser32.ReleaseDC(this._intPtr, windowDC);
            Bitmap bmp = null;
            try
            {
                bmp = Image.FromHbitmap(hObject);
            }
            catch (Exception ex)
            {
                Log.logError(ex);
                bmp = new Bitmap(1, 1);
            }
            HGDI32.DeleteObject(hObject);
            long time = Environment.TickCount - start;
            if (time / 1000 > 1)
            {
                Log.logErrorForce("getImg获取图片时间超过1秒!" + time + "s");
            }
            return bmp;
        }

        private object initLock = new object();


        int priTime = 0;
        private void initRange()
        {
            if (Environment.TickCount - priTime < 100)
            {
                return;
            }
            initConfig();
            HUser32.GetWindowRect(this._intPtr, ref this._rect);
            if (this._rect.right != 0)
            {
                this._hLeftPix = this._rect.left;
                this._hRightPix = this._rect.right;
                this._hTopPix = this._rect.top;
                this._hBottomPix = this._rect.bottom;
                this._imageSize = new Size(this._hRightPix - this._hLeftPix, this._hBottomPix - this._hTopPix);
            }
            priTime = Environment.TickCount;
        }

        private void init()
        {
            initRange();
            if (this._rect.right != 0)
            {
                if (this.ImageModle == CaptureImageModle.MemeryCopy)
                {
                    this.getScreenCaputureUserHandle();
                }
                else
                {
                    this.getScreenCaputureUserCopy();
                }
            }
        }

        public IntPtr Handle
        {
            get
            {
                return this._intPtr;
            }
            set
            {
                this._intPtr = value;
                this.init();
            }
        }

        public int HBottom
        {
            get
            {
                initRange();
                return this._hBottomPix;
            }
        }

        public Bitmap HImage
        {
            get
            {
                this.init();
                return this._image;
            }
        }

        public int HLeft
        {
            get
            {
                initRange();
                return this._hLeftPix;
            }
        }

        public int HRight
        {
            get
            {
                initRange();
                return this._hRightPix;
            }
        }

        public int HTop
        {
            get
            {
                initRange();
                return this._hTopPix;
            }
        }

        public CaptureImageModle ImageModle { get; set; }

        public Size ImageSize
        {
            get
            {
                return this._imageSize;
            }
        }

        public enum CaptureImageModle
        {
            MemeryCopy,
            ScreenCopy
        }

        private class HGDI32
        {
            [DllImport("gdi32.dll")]
            public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hObjectSource, int nXSrc, int nYSrc, int dwRop);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);
            [DllImport("gdi32.dll")]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        }

        public class HUser32
        {
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hWnd);
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
            [DllImport("user32.dll")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }
        }
    }
}

