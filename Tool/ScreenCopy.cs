using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool
{
    /// <summary>
    /// 屏幕复制
    /// </summary>
    public class ScreenCopy : IScreenOperater
    {
        private static ScreenCopy SC = new ScreenCopy();

        public IntPtr Handler
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public static Image Copy(Rectangle rec)
        {
            return SC.getImage(rec);
        }

        public Bitmap getImage()
        {
            throw new NotImplementedException();
        }

        public Bitmap getImage(Rectangle rec)
        {
            if (rec.Width <= 0 || rec.Height <= 0)
            {
                return new Bitmap(0, 0);
            }
            Bitmap bmp = new Bitmap(rec.Width, rec.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(rec.Location, Point.Empty, rec.Size);
            }
            return bmp;
        }

        public void MouseDbClick()
        {
            throw new NotImplementedException();
        }

        public void MouseLeftClick()
        {
            throw new NotImplementedException();
        }

        public void MouseMove(Point point)
        {
            throw new NotImplementedException();
        }

        public void MouseMove(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void MouseMoveR(Point point)
        {
            throw new NotImplementedException();
        }

        public void MouseMoveR(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void MouseRigthClick()
        {
            throw new NotImplementedException();
        }
    }
}
