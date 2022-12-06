using HXmain.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain.Watcher.Dialog
{
    class Win钱善 : ImageWatcher
    {

        public bool 我要出力(MainGame game)
        {
            return WatcherClick<dh_我要出力>(game);
        }


        protected override Bitmap WatcherBmp
        {
            get
            {
                return Resources.dia_钱善;
            }
        }

        public bool 一键完成(MainGame game)
        {
            return WatcherClick<dh_一键完成>(game);
        }

        public bool 花5官银重置(MainGame game)
        {
            return WatcherClick<dh_花5官银>(game);
        }

        public bool 花5官银重置2(MainGame game)
        {
            return WatcherClick<dh_花5官银2>(game);
        }

        public bool 花20官银重置(MainGame game)
        {
            return WatcherClick<dh_花费20官银>(game);
        }

        public bool 花20官银重置2(MainGame game)
        {
            return WatcherClick<dh_花费20官银2>(game);
        }

        class dh_我要出力 : ImageWatcher
        {
            protected override Bitmap WatcherBmp
            {
                get
                {
                    return Resources.font_我要出力;
                }
            }
        }

        class dh_一键完成 : ImageWatcher
        {
            protected override Bitmap WatcherBmp
            {
                get
                {
                    return Resources.font_一键完成;
                }
            }
        }

        class dh_花5官银 : ImageWatcher
        {
            protected override Bitmap WatcherBmp
            {
                get
                {
                    return Resources.font_钱善_花5官银;
                }
            }
        }

        class dh_花5官银2 : ImageWatcher
        {
            protected override Bitmap WatcherBmp
            {
                get
                {
                    return Resources.font_钱善_花5官银2;
                }
            }
        }

        class dh_花费20官银 : ImageWatcher
        {
            protected override Bitmap WatcherBmp
            {
                get
                {
                    return Resources.font_钱善_花20官银;
                }
            }
        }

        class dh_花费20官银2 : ImageWatcher
        {
            protected override Bitmap WatcherBmp
            {
                get
                {
                    return Resources.font_钱善_花20官银2;
                }
            }
        }
    }
}
