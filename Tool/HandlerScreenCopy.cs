using HFrameWork.SystemDll;
using HFrameWork.SystemInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool
{
    public class HandlerScreenCopy : IScreenOperater
    {
        public IntPtr Handler { get; set; }

       

        public HandlerScreenCopy(IntPtr handler)
        {
            this.Handler = handler;
        }

        public Bitmap getImage()
        {
            User32.RECT rcc = getRange();
            return getImage(new Rectangle(Point.Empty, new Size(rcc.right - rcc.left, rcc.bottom - rcc.top)));
        }

        private User32.RECT getRange()
        {
            if (Handler == null)
            {
                throw new Exception("请先设置主Handler");
            }
            User32.RECT rcc = new User32.RECT();
            User32.GetWindowRect(Handler, ref rcc);
            return rcc;
        }

        public Bitmap getImage(Rectangle rect)
        {
            User32.RECT rcc = getRange();
            IntPtr windowDC = User32.GetWindowDC(Handler);
            IntPtr hDC = HGDI32.CreateCompatibleDC(windowDC);
            IntPtr hObject = HGDI32.CreateCompatibleBitmap(windowDC, rect.Width, rect.Height);
            if (hObject == IntPtr.Zero)
            {
                return new Bitmap(1, 1);
            }
            IntPtr ptr4 = HGDI32.SelectObject(hDC, hObject);
            HGDI32.BitBlt(hDC, 0, 0, rect.Width, rect.Height, windowDC, rect.X, rect.Y, 0xcc0020);
            HGDI32.SelectObject(hDC, ptr4);
            HGDI32.DeleteDC(hDC);
            User32.ReleaseDC(Handler, windowDC);
            Bitmap img = Image.FromHbitmap(hObject);
            HGDI32.DeleteObject(hObject);
            return img;
        }

        public void MouseMove(Point point)
        {
            User32.RECT rcc = getRange();
            Mouse.move(new Point(rcc.left + point.X, rcc.top + point.Y));
        }

        public void MouseMove(int x, int y)
        {
            MouseMove(new Point(x, y));
        }

        public void MouseMoveR(Point point)
        {
            MouseMoveR(point);
        }

        public void MouseMoveR(int x, int y)
        {
            MouseMoveR(x, y);
        }

        public void MouseLeftClick()
        {
            throw new NotImplementedException();
        }

        public void MouseRigthClick()
        {
            throw new NotImplementedException();
        }

        public void MouseDbClick()
        {
            throw new NotImplementedException();
        }
    }
}
