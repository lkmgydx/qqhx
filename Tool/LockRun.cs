using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool
{
    /// <summary>
    /// 锁定执行
    /// </summary>
    public class LockRun
    {
        protected object lockObj = new object();
        public virtual void Run(Action t)
        {
            lock (lockObj)
            {
                t();
            }
        }
    }
}
