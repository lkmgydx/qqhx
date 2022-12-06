using HXmain.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain.Watcher.Dialog
{
    public class Win宅院驿丞 : ImageWatcher
    {
        static Font_今日剩余打劫 sjdy = new Font_今日剩余打劫();
        static Font_今日打劫剩余2 dj_sy2 = new Font_今日打劫剩余2();
        static dh_确定 dd = new dh_确定();

        protected override Bitmap WatcherBmp
        {
            get
            {
                return Resources.dia_宅院驿丞;
            }

        }

        public bool Ask排行榜(MainGame game)
        {
            return WatcherClick<dh_排行榜>(game);
        }


        /// <summary>
        /// 是否能够打劫
        /// </summary>
        public bool CanDJ
        {
            get
            {
                return WinSearchPoint.Success && !sjdy.Watch(WatcherGameBmp).Success;
            }
        }

        public int DJCount
        {
            get
            {
                if (WinSearchPoint.Success)
                {
                    if (dj_sy2.Watch(WatcherGameBmp).Success)
                    {
                        return 2;
                    }
                }
                return -1;
            }
        }

        public bool AskDjOKClose(MainGame game)
        {
            return WatcherClick<dh_确定>(game);
        }

        public bool Ask趁火打劫(MainGame game)
        {
            return WatcherClick<dh_趁火打劫>(game);
        }

        public bool Ask_趁火打劫_肥羊列表(MainGame game)
        {
            return WatcherClick<dh_肥羊列表>(game, new Point(0, 10));
        }


        public class dh_排行榜 : ImageWatcher
        {
            protected override Bitmap WatcherBmp
            {
                get
                {
                    return Resources.font_排行榜;
                }
            }

        }

        public class dh_趁火打劫 : ImageWatcher
        {
            protected override Bitmap WatcherBmp
            {
                get
                {
                    return Resources.font_趁火打劫;
                }
            }
        }

        public class dh_肥羊列表 : ImageWatcher
        {
            protected override Bitmap WatcherBmp
            {
                get
                {
                    return Resources.font_肥羊列表;
                }
            }
        }

        public class Font_今日剩余打劫 : ImageWatcher
        {
            protected override Bitmap WatcherBmp
            {
                get
                {
                    return Resources.font_今天剩余打劫0;
                }
            }
        }


        public class Font_今日打劫剩余2 : ImageWatcher
        {
            protected override Bitmap WatcherBmp
            {
                get
                {
                    return Resources.font_打劫剩余2;
                }
            }
        }


        class dh_确定 : ImageWatcher
        {
            protected override Bitmap WatcherBmp
            {
                get
                {
                    return Resources.font_宅院驿丞_确定;
                }
            }
        }
    }
}
