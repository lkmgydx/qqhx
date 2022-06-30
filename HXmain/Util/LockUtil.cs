using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain.Util
{
    public class LockUtil
    {
        System.Threading.Mutex mainM = new System.Threading.Mutex(false);
        System.Threading.Mutex mainSub = new System.Threading.Mutex(false);
        System.Threading.EventWaitHandle ew = new System.Threading.EventWaitHandle(true, System.Threading.EventResetMode.AutoReset);

        bool mainRun = false;

        int mTick = 0;
        object lockCount = new object();
        public void getTopLock()
        {
            lock (lockCount)
            {
                mTick++;
                mainRun = true;
                mainM.WaitOne();
            }

        }

        public void ReleaseTopLock()
        {
            mainM.ReleaseMutex();
            lock (lockCount)
            {
                mTick--;
                if (mTick != 0)
                {
                    return;
                }
            }
            ew.Set();
            mainRun = false;
        }

        public void getLock()
        {
            if (mainRun)
            {
                mainM.WaitOne();
                mainM.ReleaseMutex();
            }
            ew.WaitOne();
            mainSub.WaitOne();
        }

        public void reSetLock()
        {
            mainM = new System.Threading.Mutex(false);
            mainSub = new System.Threading.Mutex(false);
            ew = new System.Threading.EventWaitHandle(true, System.Threading.EventResetMode.AutoReset);
        }

        public void ReleaseLock()
        {
            mainSub.ReleaseMutex();
            if (mainRun)
            {
                return;
            }
            ew.Set();
        }
    }
}
