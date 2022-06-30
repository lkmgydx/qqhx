using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain.HXInfo.Map
{
    public class MapPoints
    {
        /// <summary>
        /// 最大宽度
        /// </summary>
        public int MaxWidth { get; private set; }
        /// <summary>
        /// 最大高度
        /// </summary>
        public int MaxHeigth { get; private set; }

        private bool[][] Zb;

        private bool[][] realCanGo;

        /// <summary>
        /// 坐标系统
        /// </summary>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeigth"></param>
        public MapPoints(int maxWidth, int maxHeigth)
        {
            this.MaxHeigth = maxHeigth;
            this.MaxWidth = maxWidth;
            Zb = new bool[this.MaxHeigth][];
            realCanGo = new bool[this.MaxHeigth][];
            for (int i = 0; i < maxHeigth; i++)
            {
                Zb[i] = new bool[maxWidth];
                realCanGo[i] = new bool[maxWidth];
            }
        }

        public bool this[int x, int y]
        {
            get
            {
                if (realCanGo[x][y])
                {
                    return true;
                }
                return Zb[x][y];
            }
            set
            {
                if (Zb[x][y])
                {
                    return;
                }
                Zb[x][y] = value;
            }
        }

        public static bool CanRigth(MainGame game)
        {
            MapBase map = game.CurrentMap;
            if (map.MapPoints != null)
            {
                return map.MapPoints[game.CurrentLocation.X + 1, game.CurrentLocation.Y];
            }
            return true;
        }

        public static bool CanUp(MainGame game)
        {
            MapBase map = game.CurrentMap;
            if (map.MapPoints != null)
            {
                return map.MapPoints[game.CurrentLocation.X, game.CurrentLocation.Y - 1];
            }
            return true;
        }

        public static bool CanBottom(MainGame game)
        {
            MapBase map = game.CurrentMap;
            if (map.MapPoints != null)
            {
                return map.MapPoints[game.CurrentLocation.X, game.CurrentLocation.Y + 1];
            }
            return true;
        }

        public static bool CanLeft(MainGame game)
        {
            MapBase map = game.CurrentMap;
            if (map.MapPoints != null)
            {
                return map.MapPoints[game.CurrentLocation.X - 1, game.CurrentLocation.Y];
            }
            return true;
        }
    }
}
