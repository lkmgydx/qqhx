using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace HXmain.Watcher
{
    public interface IWatcher
    {
        /// <summary>
        /// 观察整个界面
        /// </summary>
        HSearchPoint Watch(System.Drawing.Bitmap bmp);
    }
}
