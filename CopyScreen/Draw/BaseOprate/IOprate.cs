using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CopyScreen.Draw.BaseOprate
{
    /// <summary>
    /// 操作接口
    /// </summary>
    interface IOprate
    {
        void OnMouseDown(MouseEventArgs e);

        void onMouseUp(MouseEventArgs e);

        void OnMouseMove(MouseEventArgs e);
        void onKeyUp(KeyEventArgs e);
    }
}
