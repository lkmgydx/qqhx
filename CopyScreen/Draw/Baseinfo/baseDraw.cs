using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyScreen.Draw.Baseinfo
{
    /// <summary>
    /// 画图抽象类
    /// </summary>
    internal abstract class BaseDraw
    {
        public abstract void render(System.Drawing.Graphics g);
    }
}
