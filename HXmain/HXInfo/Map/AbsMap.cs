using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain.HXInfo.Map
{
    /// <summary>
    /// 抽像地图类
    /// </summary>
    public abstract class AbsMap
    {
        /// <summary>
        /// BOSS区域
        /// </summary>
        protected Rectangle Rectangle_Boss = Rectangle.Empty;
        /// <summary>
        /// 地图名称
        /// </summary>
        public string Name { get; protected set; }
        /// <summary>
        /// 地图主键
        /// </summary>
        public string MapKey { get; protected set; }

        /// <summary>
        /// 最高宽度
        /// </summary>
        public int MaxWidth { get; protected set; } = 800;
        /// <summary>
        /// 最大高度
        /// </summary>
        public int MaxHeigth { get; protected set; } = 600;

        public bool isInBossRange(MainGame game)
        {
            return Rectangle_Boss.Contains(game.CurrentLocation);
        }

        public virtual void GotoBoss(MainGame game, Action action = null)
        {
            var pt = PathPointUtil.getResourcePoint(Name + "Boss走路.txt");
            if (pt.Length == 0)
            {
                return;
            }
            game.runPath(pt, action);
        }
    }
}
