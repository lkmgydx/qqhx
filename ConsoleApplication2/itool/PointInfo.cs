using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2.itool
{
    /// <summary>
    /// 点当前状态
    /// </summary>
    enum PointState {
        /// <summary>
        /// 未确定状态
        /// </summary>
        NotSure,
        /// <summary>
        /// 可以忽略的路径
        /// </summary>
        SkipPath,
        /// <summary>
        /// 有效路径
        /// </summary>
        RealPath
    }

    class PointInfo
    {
        public int X { get; set; }
        public int Y { get; set; }
        /// <summary>
        /// 是否为一个路径点
        /// </summary>
        public bool isBlackPath { get; set; }
        /// <summary>
        /// 点状态
        /// </summary>
        public PointState PointState { get; set; }
        /// <summary>
        /// 是否为相交点
        /// </summary>
        public bool isCross { get; set; } 
    }
}
