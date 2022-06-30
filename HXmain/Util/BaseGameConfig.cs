using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain.Util
{
    /// <summary>
    /// 游戏基本配置
    /// </summary>
    public sealed class BaseGameConfig
    {
        /// <summary>
        /// 是否直全屏获取
        /// </summary>
        public bool IsFromScreen { get; set; }
        /// <summary>
        /// 自动一技能
        /// </summary>
        public bool AutoOneKey { get; set; } = false;
        /// <summary>
        /// 自动复活
        /// </summary>
        public bool AutoLive { get; set; } = false;
        /// <summary>
        /// 自动组队
        /// </summary>
        public bool AutoZudui { get; set; } = false;
        /// <summary>
        /// 自动提练
        /// </summary>
        public bool AutoTilian { get; set; } = false;

        /// <summary>
        /// 未移动检测
        /// </summary>
        public bool StandCheck { get; set; } = false;

        /// <summary>
        /// 验证码检测
        /// </summary>
        public bool CheckYZM { get; set; } = true;

    }
}
