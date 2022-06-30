using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool
{
    public interface IScreenOperater
    {
        IntPtr Handler { get; set; }

        Bitmap getImage();
        Bitmap getImage(Rectangle rect);

        void MouseMove(Point point);

        void MouseMove(int x, int y);

        void MouseMoveR(Point point);
        void MouseMoveR(int x, int y);
    }
}
