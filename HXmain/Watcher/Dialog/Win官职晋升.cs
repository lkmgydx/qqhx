using HXmain.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain.Watcher.Dialog
{
    class Win官职晋升 : ImageWatcher
    {

        public bool Ask领取俸禄(MainGame game)
        {
            bool suc = WatcherClick<Font_领取俸禄1>(game);
            if (suc)
            {
                return waitSucFn(() =>
                {
                    WatcherGameBmp = game.getImg();
                    return WatcherClick<Font_领取俸禄2>(game);
                }, 3000);
            }
            else
            {
                return WatcherClick<Font_领取俸禄2>(game);
            }
        }

        public bool Ask晋升官职(MainGame game)
        {
            WatcherClick<Font_官职任务>(game);
            return WatcherClick<Font_一键完成>(game);
        }

        protected override Bitmap WatcherBmp
        {
            get
            {
                return Resources.dia_官职晋升;
            }
        }


        class Font_晋升官职 : ImageWatcher
        {
            protected override Bitmap WatcherBmp
            {
                get
                {
                    return Resources.dia_官职晋升;
                }
            }
        }

        class Font_领取俸禄1 : ImageWatcher
        {
            protected override Bitmap WatcherBmp
            {
                get
                {
                    return Resources.font_领取俸禄1;
                }
            }
        }

        class Font_领取俸禄2 : ImageWatcher
        {
            protected override Bitmap WatcherBmp
            {
                get
                {
                    return Resources.font_领取俸禄2;
                }
            }
        }

        class Font_官职任务 : ImageWatcher
        {
            protected override Bitmap WatcherBmp
            {
                get
                {
                    return Resources.font_官职任务;
                }
            }
        }

        class Font_一键完成 : ImageWatcher
        {
            protected override Bitmap WatcherBmp
            {
                get
                {
                    return Resources.font_一键完成;
                }
            }
        }
    }
}
