using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HXmain.Util
{
    public class LockUtil
    {

        object lockCount = new object();
        public void getTopLock()
        {
            Monitor.Enter(lockCount);
        }

        public void ReleaseTopLock()
        {
            Monitor.Exit(lockCount);
        }

        public void getLock()
        {
            Monitor.Enter(lockCount);
            Monitor.Exit(lockCount);
        }

        public void reSetLock()
        {
            lockCount = new object();
        }
    }
}
