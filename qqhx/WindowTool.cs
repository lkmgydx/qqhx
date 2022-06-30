using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using u32 = WSTools.WSWinAPI.WinUser32;

namespace qqhx
{
    /// <summary>
    /// Windows窗口管理工具
    /// </summary>
    public sealed class WindowTool
    {
        /// <summary>
        /// 进程PTR转主窗口PTR
        /// </summary>
        /// <param name="mainPtr"></param>
        /// <returns></returns>
        public static IntPtr getMain(IntPtr mainPtr)
        {
            return u32.GetWindow(mainPtr, u32.GetWindowCmd.GW_CHILD);
        }

        /// <summary>
        /// 获取子窗口列表
        /// </summary>
        /// <param name="ptr"></param>
        /// <returns></returns>
        public static IntPtr[] getChildens(IntPtr ptr)
        {
            List<IntPtr> li = new List<IntPtr>();
            ptr = u32.GetWindow(ptr, u32.GetWindowCmd.GW_CHILD);
            while (true)
            {
                if (ptr == IntPtr.Zero)
                {
                    break;
                }
                li.Add(ptr);
                ptr = u32.GetWindow(ptr, u32.GetWindowCmd.GW_HWNDNEXT);
            }
            return li.ToArray();
        }

        /// <summary>
        /// 查找类名为指定名称的窗口
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static List<IntPtr> findWindow(IntPtr ptr, string className)
        {
            List<IntPtr> lirst = new List<IntPtr>();
            if (ptr == IntPtr.Zero)
            {
                return lirst;
            }
            IntPtr[] ptrs = getChildens(ptr);
            if (ptrs.Length == 0)
            {
                return lirst;
            }
            foreach (IntPtr pt in ptrs)
            {
                if (getWindowClass(pt) == className)
                {
                    lirst.Add(pt);
                }
                lirst.AddRange(findWindow(pt, className));
            }
            return lirst;
        }

        public static List<IntPtr> findWindow(string className)
        {
            return findWindow(u32.GetDesktopWindow(), className);
        }

        /// <summary>
        /// 获取窗口类名
        /// </summary>
        /// <param name="winPtr"></param>
        /// <returns></returns>
        public static string getWindowClass(IntPtr winPtr)
        {
            StringBuilder f = new StringBuilder(1000);
            u32.GetClassName(winPtr, f, 1000);
            return f.ToString();
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="intPtr"></param>
        public static void CloseWin(IntPtr intPtr)
        {
            u32.SendMessage(intPtr, 0x10, 0, 0);
        }
    }
}
