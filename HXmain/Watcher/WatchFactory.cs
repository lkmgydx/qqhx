using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;
using static HXmain.Watcher.BaseWatcher;

namespace HXmain.Watcher
{
    class WatchFactory<T> where T : ImageWatcher, new()
    {
        public delegate void WatcherRunPonit(T iw, HSearchPoint hs);
        public delegate void WatcherRun(T iw);

        public static T getWatcher(WatcherRun run)
        {
            T wz = new T();
            wz.onWatchSucess += (e) =>
            {
                run?.Invoke(wz);
            };
            return wz;
        }

        public static T getWatcher(WatcherRunPonit run)
        {
            T wz = new T();
            wz.onWatchSucess += (e) =>
            {
                run?.Invoke(wz, e);
            };
            return wz;
        }

        public static T getWatcher(WatchSucEventHandler suc = null, WatchSucEventHandler notfind = null)
        {
            T iw = new T();
            if (suc != null || notfind != null)
            {
                iw.onWatchChange += (e, s) =>
               {
                   if (e)
                   {
                       suc?.Invoke(s);
                   }
                   else
                   {
                       notfind?.Invoke(s);
                   }
               };
            }
            return iw;
        }
    }
}
