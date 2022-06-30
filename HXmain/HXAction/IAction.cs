using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain.HXAction
{
    interface IRun
    {
        /// <summary>
        /// 运行
        /// </summary>
        /// <returns></returns>
        bool run();
        /// <summary>
        /// 暂停
        /// </summary>
        void pause();
        /// <summary>
        /// 恢复
        /// </summary>
        void resume();
        /// <summary>
        /// 停止
        /// </summary>
        void stop();
    }
}
