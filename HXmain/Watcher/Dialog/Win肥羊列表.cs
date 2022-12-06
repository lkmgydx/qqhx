using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HXmain.Properties;
using Tool;

namespace HXmain.Watcher.Dialog
{
    class Win肥羊列表 : ImageWatcher
    {
        Font_已选中 yx = new Font_已选中();
        public Point offSetP = new Point(-210, 0);
        internal bool Dajie(MainGame game)
        {
            return WatcherClick<Font_打劫>(game);
        }

        public List<HSearchPoint> get一品(MainGame game)
        {
            return game.findImgAll(Resources.font_打劫_1品);
        }

        public List<HSearchPoint> get四品(MainGame game)
        {
            return game.findImgAll(new List<Bitmap> { Resources.font_打劫4品1, Resources.font_打劫4品1_1 });
        }

        public List<HSearchPoint> get三品(MainGame game)
        {
            return game.findImgAll(new List<Bitmap> { Resources.font_打劫３品１});
        }

        public bool Select一品(MainGame game)
        {
            return WatcherClick<Font_一品>(game, offSetP);
        }

        public bool Select四品(MainGame game)
        {
            return WatcherClick<Font_四品1>(game, offSetP);
        }

        public bool Select三品(MainGame game)
        {
            return WatcherClick<Font_三品>(game, offSetP);
        }

        public bool HasSelect
        {
            get
            {
                return WinSearchPoint.Success && yx.Watch(WatcherGameBmp).Success;
            }
        }

        protected override Bitmap WatcherBmp
        {
            get
            {
                return Resources.win_肥羊列表;
            }
        }

        class Font_已选中 : ImageWatcher
        {
            protected override Bitmap WatcherBmp
            {
                get
                {
                    return Resources.font_已选中;
                }
            }


            protected override List<Bitmap> WatcherBmpList
            {
                get
                {
                    return new List<Bitmap>() { Resources.font_已选中2, Resources.font_打劫选中２ };
                }
            }
        }

        class Font_打劫 : ImageWatcher
        {
            protected override Bitmap WatcherBmp
            {
                get
                {
                    return Resources.font_前往打劫;
                }
            }
        }


        class Font_一品 : ImageWatcher
        {
            protected override Bitmap WatcherBmp
            {
                get
                {
                    return Resources.font_打劫_1品;
                }
            }
        }

        class Font_三品 : ImageWatcher
        {
            protected override Bitmap WatcherBmp
            {
                get
                {
                    return Resources.font_打劫３品１;
                }
            }
        }

        class Font_四品1 : ImageWatcher
        {
            protected override Bitmap WatcherBmp
            {
                get
                {
                    return Resources.font_打劫4品1;
                }
            }

            protected override List<Bitmap> WatcherBmpList
            {
                get
                {
                    return new List<Bitmap>() { Resources.font_打劫4品1_1 };
                }
            }
        }
    }
}
