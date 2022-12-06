using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CopyScreen
{
    /// <summary>
    ///  整体配置
    /// </summary>
    internal class Config
    {
        /// <summary>
        /// 整体配置
        /// </summary>
        static Config()
        {
            if (Screen.AllScreens.Length == 1)
            {
                ScreenSize = Screen.AllScreens[0].Bounds.Size;
                return;
            }
            Screen[] screens;
            screens = Screen.AllScreens;
            int noofscreens = screens.Length, maxwidth = 0, maxheight = 0;
            for (int i = 0; i < noofscreens; i++)
            {
                if (maxwidth < (screens[i].Bounds.X + screens[i].Bounds.Width)) maxwidth = screens[i].Bounds.X + screens[i].Bounds.Width;
                if (maxheight < (screens[i].Bounds.Y + screens[i].Bounds.Height)) maxheight = screens[i].Bounds.Y + screens[i].Bounds.Height;
            }
            ScreenSize = new Size(maxwidth, maxheight);
        }

        /// <summary>
        /// 屏幕大小
        /// </summary>
        public static Size ScreenSize { get; }

        /// <summary>
        /// 小图间距
        /// </summary>
        public static Point SmallImgMargin { get; set; } = new Point(800, 800);
        /// <summary>
        /// //小图大小
        /// </summary>
        public static Size SmallImgSize { get; set; } = new Size(150, 150);

        /// <summary>
        /// 边上8个点宽度
        /// </summary>
        public static int _8PointWidth { get; set; } = 5;

        /// <summary>
        /// 边上8个点高度
        /// </summary>
        public static int _8PointHeight { get; set; } = 5;

    }
}
