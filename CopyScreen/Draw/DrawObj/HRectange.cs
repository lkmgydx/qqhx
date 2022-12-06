using CopyScreen.Draw.DrawObj;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyScreen.Draw
{
    /// <summary>
    /// 8边矩形绘制
    /// </summary>
    class HRectange : Draw8Point
    {
        public override void render(Graphics g)
        {
            g.DrawRectangle(DrawPen, DrawRectangle);
            base.render(g);
        }
    }
}
