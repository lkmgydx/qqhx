using HXmain.HXAction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace HXmain.Watcher
{
    /// <summary>
    /// 基本的观察抽象类
    /// </summary>
    public abstract class BaseWatcher : IWatcher
    {

        public void WatchDo<T>(System.Drawing.Bitmap b, T t) where T : BaseAction
        {
            var v = Watch(b);
            if (v.Success)
            {
                BaseAction.DoBaseAction(t);
            }
        }

        /// <summary>
        /// 观察整个界面
        /// </summary>
        public abstract HSearchPoint Watch(System.Drawing.Bitmap bmp);

        /// <summary>
        /// 当成功观察到时，触发事件
        /// </summary>
        /// <param name="suc"></param>
        /// <param name="hs"></param>
        public delegate void WatchEventHandler(bool suc, HSearchPoint hs);
        /// <summary>
        /// 成功观测到
        /// </summary>
        /// <param name="hs"></param>
        public delegate void WatchSucEventHandler(HSearchPoint hs);

        /// <summary>
        /// 当观察到时触发
        /// </summary>
        public virtual event WatchEventHandler onWatchChange;
    }
}
