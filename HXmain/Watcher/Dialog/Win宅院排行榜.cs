using HXmain.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace HXmain.Watcher.Dialog
{
    public class Win宅院排行榜 : ImageWatcher
    {
        Font_已选中 fy = new Font_已选中();
        Font_今日剩余 sy = new Font_今日剩余();
        /// <summary>
        /// 
        /// </summary>
        public bool hasSelect
        {
            get
            {
                return WinSearchPoint.Success && fy.Watch(WatcherGameBmp).Success;
            }
        }

        /// <summary>
        /// 是否有剩余
        /// </summary>
        public bool hasSY
        {
            get
            {
                return WinSearchPoint.Success && !sy.Watch(WatcherGameBmp).Success;
            }
        }


        public bool SelectOne(MainGame game)
        {
            if (WinSearchPoint.Success)
            {
                Point pt = WinSearchPoint.CenterPoint;
                pt.Offset(30, 60);
                game.MouseClick(pt);
                return true;
            }
            return false;
        }

        public bool Send鲜花(MainGame game)
        {
            return WatcherClick<Font_鲜花>(game);
        }

        protected override Bitmap WatcherBmp
        {
            get
            {
                return Resources.dia_宅院排行榜;
            }
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

        static List<Bitmap> li = new List<Bitmap>() { Resources.font_已选中2 };
        protected override List<Bitmap> WatcherBmpList
        {
            get
            {
                return li;
            }
        }
    }

    class Font_今日剩余 : ImageWatcher
    {
        protected override Bitmap WatcherBmp
        {
            get
            {
                return Resources.font_今日剩余0;
            }
        }

        protected override List<Bitmap> WatcherBmpList
        {
            get
            {
                return new List<Bitmap>() { Resources.font_今日打劫剩余0 };
            }
        }
    }

    class Font_鲜花 : ImageWatcher
    {
        protected override Bitmap WatcherBmp
        {
            get
            {
                return Resources.font_投掷鲜花;
            }
        }
    }
}
