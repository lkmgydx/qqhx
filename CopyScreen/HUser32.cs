namespace HFrameWork.SystemDll
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public sealed class HUser32
    {
        /// <summary>
        /// 获取当前激活的win窗口
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// 鼠标移动到指定位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);

        /// <summary>
        /// 是否显示鼠标
        /// </summary>
        /// <param name="show"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public extern static bool ShowCursor(bool show);
        /// <summary>
        /// 获取指定进程ID
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);
        [DllImport("User32")]
        public static extern bool GetCursorPos(ref Point lpPoint);
        /// <summary>
        /// 传0时为获取SCREEN句柄
        /// </summary>
        /// <param name="ptr"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr ptr);
        [DllImport("user32")]
        public static extern int GetKeyboardState(byte[] pbKeyState);
        [DllImport("user32")]
        public static extern uint GetWindowLong(IntPtr hwnd, int nIndex);
        [DllImport("user32")]
        public static extern int keybd_event(Keys bVk, byte bScan, int dwFlags, int dwExtralnfo);
        [DllImport("user32")]
        public static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, long lParam);
        [DllImport("user32")]
        public static extern int SetLayeredWindowAttributes(IntPtr hwnd, int crKey, int bAlpha, int dwFlags);
        [DllImport("user32")]
        public static extern uint SetWindowLong(IntPtr hwnd, int nIndex, uint dwNewLong);


        public delegate int HookProcDelegate(int nCode, int wParam, IntPtr lParam);
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int SetWindowsHookEx(int idHook, HookProcDelegate lpfn, IntPtr hInstance, int threadId);
        [DllImport("user32")]
        public static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, int fuState);
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        /// <summary>
        /// 将创建指定窗口的线程设置到前台，并且激活该窗口
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("USER32.DLL")]
        public static extern bool ShowWindow(IntPtr hWnd, int key);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr CreateWindowEx(
           int dwExStyle,                                //窗口的扩展风格
           string lpszClassName,                         //指向注册类名的指针
           string lpszWindowName,                        //指向窗口名称的指针
           int style,                                    //窗口风格
           int x,                                        //窗口的水平位置
           int y,                                        //窗口的垂直位置
           int width,                                    //窗口的宽度
           int height,                                   //窗口的高度
           IntPtr hWndParent,                            //父窗口的句柄
           IntPtr hMenu,                                 //菜单的句柄或是子窗口的标识符
           IntPtr hInst,                                 //应用程序实例的句柄
           [MarshalAs(UnmanagedType.AsAny)] object pvParam//指向窗口的创建数据
           );


        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
    }
}

