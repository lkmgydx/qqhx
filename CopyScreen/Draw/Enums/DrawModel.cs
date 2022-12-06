using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyScreen.Draw.Enums
{
    /// <summary>
    /// 0.普通模式,
    /// 1.点击第一个点，等待第二个点松开 
    /// 2.松开第二个点，等待确定退出
    /// </summary>
    public enum DrawModel
    {
        /// <summary>
        /// 普通模式，等待点击
        /// </summary>
        Nomal,
        /// <summary>
        /// 点击第一个点，等待第二个点松开
        /// </summary>
        MouseDown,
        /// <summary>
        /// 松开第二个点，等待确定退出
        /// </summary>
        MouseUp
    }

    /// <summary>
    /// 鼠标各方位模式
    /// </summary>
    public enum DrawModelDetail
    {
        Undefined, Left, Top, Right, Bottom, LeftTop, RightBottom, RightTop, LeftBottom, Move
    }
}
