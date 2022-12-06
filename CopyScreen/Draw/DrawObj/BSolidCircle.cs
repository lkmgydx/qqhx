using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyScreen.Draw.DrawObj
{
    /// <summary>
    /// 实心圆绘制
    /// </summary>
    class BSolidCircle : BaseRectangeDraw
    {
        public override void render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(DrawColor), DrawRectangle);
        }
    }
}
