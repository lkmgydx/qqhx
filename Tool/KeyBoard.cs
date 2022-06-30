namespace HFrameWork.SystemInput
{
    using HFrameWork.SystemDll;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    public class KeyBoard
    {
        private const Keys Alt = Keys.LMenu;
        private static string creat;
        private const Keys Ctrl = Keys.LControlKey;
        private static int hHook;
        private static HookProcDelegate KeyboardHookDelegate;
        private const int KEYEVENTF_EXTENDEDKEY = 1;
        private const int KEYEVENTF_KEYUP = 2;
        private static int lastKeyDownTime = 0;
        private static List<KeyboardHookStruct> likey = new List<KeyboardHookStruct>();
        private static List<Keys> preKeysList = new List<Keys>();
        private const Keys Shift = Keys.LShiftKey;
        private static StringBuilder st = new StringBuilder();
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int WM_SYSKEYDOWN = 260;
        private const int WM_SYSKEYUP = 0x105;
        private static List<int> wP = new List<int>();

        public static event KeyEventHandler OnKeyDownEvent;

        public static event KeyPressEventHandler OnKeyPressEvent;

        public static event KeyEventHandler OnKeyUpEvent;

        static KeyBoard()
        {
            SetHook();
            creat = DateTime.Now.ToShortTimeString() + " --> " + HYL.Infomation;
        }

        public static void down(Keys key)
        {
            User32.keybd_event(key, 0, 0, 0);
        }

        public static void down_up(params Keys[] key)
        {
            for (int i = 0; i < key.Length; i++)
            {
                down(key[i]);
            }
            for (int j = 0; j < key.Length; j++)
            {
                up(key[j]);
            }
        }

        public static void down_upAlt(params Keys[] key)
        {
            down_up(getKeys(key, new Keys[] { Keys.LMenu }));
        }

        public static void down_upCtrl(params Keys[] key)
        {
            down_up(getKeys(key, new Keys[] { Keys.LControlKey }));
        }

        public static void down_upCtrlAlt(params Keys[] key)
        {
            down_up(getKeys(key, new Keys[] { Keys.LControlKey, Keys.LMenu }));
        }

        public static void down_upCtrlAltShift(params Keys[] key)
        {
            down_up(getKeys(key, new Keys[] { Keys.LShiftKey, Keys.LControlKey, Keys.LMenu }));
        }

        public static void down_upCtrlShift(params Keys[] key)
        {
            down_up(getKeys(key, new Keys[] { Keys.LShiftKey, Keys.LControlKey }));
        }

        public static void down_upShift(params Keys[] key)
        {
            down_up(getKeys(key, new Keys[] { Keys.LShiftKey }));
        }

        private static Keys GetDownKeys(Keys key)
        {
            Keys none = Keys.None;
            foreach (Keys keys2 in preKeysList)
            {
                switch (keys2)
                {
                    case Keys.LControlKey:
                    case Keys.RControlKey:
                        none |= Keys.Control;
                        break;
                }
                if ((keys2 == Keys.LMenu) || (keys2 == Keys.RMenu))
                {
                    none |= Keys.Alt;
                }
                if ((keys2 == Keys.LShiftKey) || (keys2 == Keys.RShiftKey))
                {
                    none |= Keys.Shift;
                }
            }
            return (none | key);
        }

        private static Keys[] getKeys(Keys[] keys, params Keys[] key)
        {
            List<Keys> list = new List<Keys>();
            list.AddRange(key);
            list.AddRange(keys);
            return list.ToArray();
        }

        private static bool IsCtrlAltShiftKeys(Keys key)
        {
            if ((((key != Keys.LControlKey) && (key != Keys.RControlKey)) && ((key != Keys.LMenu) && (key != Keys.RMenu))) && ((key != Keys.LShiftKey) && (key != Keys.RShiftKey)))
            {
                return false;
            }
            return true;
        }

        private static int KeyboardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                KeyboardHookStruct struct2 = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                Keys vkCode = (Keys)struct2.vkCode;
                new KeyEventArgs(GetDownKeys(vkCode));
                if ((Environment.TickCount - lastKeyDownTime) > SystemInformation.DoubleClickTime)
                {
                    preKeysList.Clear();
                }
                if ((((OnKeyDownEvent != null) || (OnKeyPressEvent != null)) && ((wParam == 0x100) || (wParam == 260))) && (IsCtrlAltShiftKeys(vkCode) && (preKeysList.IndexOf(vkCode) == -1)))
                {
                    preKeysList.Add(vkCode);
                }
                lastKeyDownTime = Environment.TickCount;
                if ((OnKeyDownEvent != null) && ((wParam == 0x100) || (wParam == 260)))
                {
                    KeyEventArgs e = new KeyEventArgs(GetDownKeys(vkCode));
                    OnKeyDownEvent(creat, e);
                    if (e.SuppressKeyPress)
                    {
                        return 1;
                    }
                }
                if ((OnKeyPressEvent != null) && (wParam == 0x100))
                {
                    byte[] pbKeyState = new byte[0x100];
                    User32.GetKeyboardState(pbKeyState);
                    byte[] lpwTransKey = new byte[2];
                    if (User32.ToAscii(struct2.vkCode, struct2.scanCode, pbKeyState, lpwTransKey, struct2.flags) == 1)
                    {
                        KeyPressEventArgs args2 = new KeyPressEventArgs((char)lpwTransKey[0]);
                        OnKeyPressEvent(creat, args2);
                    }
                }
                else if ((OnKeyUpEvent != null) && ((wParam == 0x101) || (wParam == 0x105)))
                {
                    KeyEventArgs args3 = new KeyEventArgs(GetDownKeys(vkCode));
                    OnKeyUpEvent(creat, args3);
                    if (args3.SuppressKeyPress)
                    {
                        return 1;
                    }
                }
            }
            return User32.CallNextHookEx(hHook, nCode, wParam, lParam);
        }

        public static void SetHook()
        {
            KeyboardHookDelegate = new HookProcDelegate(KeyBoard.KeyboardHookProc);
            IntPtr moduleHandle = Kernel32.GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName);
            hHook = User32.SetWindowsHookEx(13, KeyboardHookDelegate, moduleHandle, 0);
        }

        public static void sleep(int time)
        {
            try
            {
                Thread.Sleep(time);
            }
            catch (Exception)
            {
                return;
            }
           
        }

        public void UnHook()
        {
            User32.UnhookWindowsHookEx(hHook);
        }

        public static void up(Keys key)
        {
            User32.keybd_event(key, 0, 2, 0);
        }

        public static bool IsStopKeyBoard
        {
            get; set;
        }

        [StructLayout(LayoutKind.Sequential)]
        private class KeyboardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }
    }
}

