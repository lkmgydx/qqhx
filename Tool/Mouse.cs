namespace HFrameWork.SystemInput
{
    using HFrameWork.SystemDll;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public sealed class Mouse
    {
        private static string create = (DateTime.Now.ToShortTimeString() + " -> " + HYL.Infomation);
        private static int hHook;
        private static HookProcDelegate hProc;
        private static int lastClickTime = 0;
        private static List<int> li = new List<int>();
        private const int MOUSE_LEFTDOWN = 0x201;

        public static void sleep(int v)
        {
            try
            {
                Thread.Sleep(v);
            }
            catch
            {

            }
        }

        public const int WM_LBUTTONDOWN = 513; // 鼠标左键按下
        public const int WM_LBUTTONUP = 514; // 鼠标左键抬起
        public const int WM_RBUTTONDOWN = 516; // 鼠标右键按下
        public const int WM_RBUTTONUP = 517; // 鼠标右键抬起
        public const int WM_MBUTTONDOWN = 519; // 鼠标中键按下
        public const int WM_MBUTTONUP = 520; // 鼠标中键抬起


        private const int MOUSE_LEFTUP = 0x202;
        private const int MOUSE_MIDDOWN = 0x207;
        private const int MOUSE_MIDUP = 520;
        private const int MOUSE_MOVE = 0x200;
        private const int MOUSE_RIGHTDOWN = 0x204;
        private const int MOUSE_RIGHTTUP = 0x205;
        private const int MOUSE_WHHEEL = 0x20a;
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        private const int MOUSEEVENTF_LEFTDOWN = 2;
        private const int MOUSEEVENTF_LEFTUP = 4;
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x20;
        private const int MOUSEEVENTF_MIDDLEUP = 0x40;
        private const int MOUSEEVENTF_MOVE = 1;
        private const int MOUSEEVENTF_RIGHTDOWN = 8;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        private const int MOUSEEVENTF_WHEEL = 0x800;
        private static Point point = new Point(0, 0);
        private const int WH_MOUSE_LL = 14;
        private static int WIN_HEIGHT = Screen.PrimaryScreen.Bounds.Height;
        private static int WIN_WIDTH = Screen.PrimaryScreen.Bounds.Width;

        public static event MouseEventHandler OnMouseDoubleClick;

        public static event MouseEventHandler OnMouseLeftDown;

        public static event MouseEventHandler OnMouseLeftUp;

        public static event MouseEventHandler OnMouseMoveEvent;

        public static event MouseEventHandler OnMouseRightDown;

        public static event MouseEventHandler OnMouseRightUp;

        public static event MouseEventHandler OnOnMouseMidDown;

        public static event MouseEventHandler OnOnMouseMidUp;

        public static event MouseEventHandler OnOnMouseWheel;

        public static void HideMouse()
        {
            User32.ShowCursor(false);
        }

        public static void ShowMouse()
        {
            User32.ShowCursor(true);
        }

        static Mouse()
        {
            IsStopMouse = false;
            //SetHook();
            cacheLocation();
        }

        public static void dbclick()
        {
            User32.mouse_event(6, 0, 0, 0, 0);
            User32.mouse_event(6, 0, 0, 0, 0);
        }

        public static void leftclick(bool moveToZero = false)
        {
            User32.mouse_event(6, 0, 0, 0, 0);
            if (moveToZero)
                move(0, 0);
        }

        public static void leftdown()
        {
            User32.mouse_event(2, 0, 0, 0, 0);
        }

        public static void leftup()
        {
            User32.mouse_event(4, 0, 0, 0, 0);
        }

        private static Stack<Point> _cachePointList = new Stack<Point>();
        public static void cacheLocation()
        {
            _cachePointList.Push(Location());
        }

        /// <summary>
        /// 是否清除此记录点
        /// </summary>
        /// <param name="pop"></param>
        public static void reventLocation(bool pop = true)
        {
            if (_cachePointList.Count == 1 || !pop)
            {
                move(_cachePointList.Peek());
            }
            else
            {
                move(_cachePointList.Pop());
            }
        }

        public static Point Location()
        {
            User32.GetCursorPos(ref point);
            return point;
        }

        public static void middledown()
        {
            User32.mouse_event(0x20, 0, 0, 0, 0);
        }

        public static void middletup()
        {
            User32.mouse_event(0x40, 0, 0, 0, 0);
        }

        private static int MouseHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (wParam > 0)
            {
                MouseHookStruct struct2 = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
                MouseEventArgs e = new MouseEventArgs(MouseButtons.None, 0, struct2.pt.x, struct2.pt.y, 0);
                switch (wParam)
                {
                    case 0x200:
                        if (OnMouseMoveEvent != null)
                        {
                            OnMouseMoveEvent(create, e);
                        }
                        break;

                    case 0x201:
                        if (OnMouseLeftDown != null)
                        {
                            OnMouseLeftDown(create, e);
                        }
                        if (OnMouseDoubleClick != null)
                        {
                            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                            timer.Tick += new EventHandler(Mouse.t_Tick);
                        }
                        break;

                    case 0x202:
                        if (OnMouseLeftUp != null)
                        {
                            OnMouseLeftUp(create, e);
                        }
                        break;

                    case 0x204:
                        if (OnMouseRightDown != null)
                        {
                            OnMouseRightDown(create, e);
                        }
                        break;

                    case 0x205:
                        if (OnMouseRightUp != null)
                        {
                            OnMouseRightUp(create, e);
                        }
                        break;

                    case 0x207:
                        if (OnOnMouseMidDown != null)
                        {
                            OnOnMouseMidDown(create, e);
                        }
                        break;

                    case 520:
                        if (OnOnMouseMidUp != null)
                        {
                            OnOnMouseMidUp(create, e);
                        }
                        break;

                    case 0x20a:
                        if (OnOnMouseWheel != null)
                        {
                            OnOnMouseWheel(create, e);
                        }
                        break;
                }
            }
            return User32.CallNextHookEx(hHook, nCode, wParam, lParam);
        }

        public static void move(Point pt)
        {
            move(pt.X, pt.Y);
        }

        public static void move(int x, int y)
        {
            //Log("实际移动位置-  x:" + x + " y:" + y);
            //float fx = 65535f / Screen.PrimaryScreen.Bounds.Width;
            //float fy = 65535f / Screen.PrimaryScreen.Bounds.Height;
            //int realX = (int)Math.Ceiling(x * fx);
            //int realY = (int)Math.Ceiling(y * fy);
            
            User32.SetCursorPos(x, y);
           // User32.mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, realX, realY, 0, 0);
        }

        public static void moveR(int x, int y)
        {
            Point t = Location();
            move(t.X + x, t.Y + y);
        }

        public static void rightclick()
        {
            //User32.mouse_event(0x18, 0, 0, 0, 0);
            rigthdown();
            sleep(100);
            rightup();
        }

        public static void rightDbclick()
        {
            User32.mouse_event(0x18, 0, 0, 2, 0);
            sleep(10);
        }

        public static void rightup()
        {
            User32.mouse_event(0x10, 0, 0, 0, 0);
        }

        public static void rigthdown()
        {
            User32.mouse_event(8, 0, 0, 0, 0);
        }

        public static int SetHook()
        {
            if (hHook == 0)
            {
                hProc = new HookProcDelegate(Mouse.MouseHookProc);
                hHook = User32.SetWindowsHookEx(14, hProc, IntPtr.Zero, 0);
            }
            return hHook;
        }

        private static void t_Tick(object sender, EventArgs e)
        {
            lastClickTime = Environment.TickCount;
        }

        public static void UnHook()
        {
            if (hHook != 0)
            {
                User32.UnhookWindowsHookEx(hHook);
            }
            hHook = 0;
        }

        public static void wheel(int i)
        {
            User32.mouse_event(0x800, 0, 0, -i, 0);
        }

        public static void DrawMousePosition(Point pt)
        {
            System.IntPtr p = (IntPtr)User32.GetDC(IntPtr.Zero);// '取得屏幕
            Graphics g = Graphics.FromHdc(p);
            //g.FillEllipse(Brushes.Red, new RectangleF(pt.X - 50, pt.Y - 50, 100, 100));
            g.DrawEllipse(Pens.Red, new RectangleF(pt.X - 50, pt.Y - 50, 100, 100));
            g.DrawEllipse(Pens.Red, new RectangleF(pt.X - 30, pt.Y - 30, 60, 60));
            g.DrawEllipse(Pens.Red, new RectangleF(pt.X - 10, pt.Y - 10, 20, 20));
            g.Dispose();
        }

        public static void DrawMousePosition()
        {
            DrawMousePosition(Location());
        }

        public static bool IsStopMouse
        {
            get; set;
        }
    }
}

