using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool
{

    public class LockTopRun : LockRun
    {

        public void SubRun(Action t)
        {
            lock (lockObj) { }//先尝试拿到顶层，再释放
            t();
        }
    }
}
