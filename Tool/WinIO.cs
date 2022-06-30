namespace HFrameWork.SystemDll
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    internal class WinIO
    {
        public enum VKKey
        {
            absolute = 0x8000,
            leftdown = 2,
            leftup = 4,
            middledown = 0x20,
            middleup = 0x40,
            move = 1,
            rightdown = 8,
            rightup = 0x10,
            virtualdesk = 0x4000,
            VK_A = 0x41,
            VK_ADD = 0x6b,
            VK_B = 0x42,
            VK_BACK = 8,
            VK_C = 0x43,
            VK_CANCEL = 3,
            VK_CAPITAL = 20,
            VK_CLEAR = 12,
            VK_CONTROL = 0x11,
            VK_D = 0x44,
            VK_DECIMAL = 110,
            VK_DELETE = 0x2e,
            VK_DIVIDE = 0x6f,
            VK_DOWN = 40,
            VK_E = 0x45,
            VK_END = 0x23,
            VK_ESCAPE = 0x1b,
            VK_EXECUTE = 0x2b,
            VK_F = 70,
            VK_F1 = 0x70,
            VK_F10 = 0x79,
            VK_F11 = 0x7a,
            VK_F12 = 0x7b,
            VK_F2 = 0x71,
            VK_F3 = 0x72,
            VK_F4 = 0x73,
            VK_F5 = 0x74,
            VK_F6 = 0x75,
            VK_F7 = 0x76,
            VK_F8 = 0x77,
            VK_F9 = 120,
            VK_G = 0x47,
            VK_H = 0x48,
            VK_HELP = 0x2f,
            VK_HOME = 0x24,
            VK_I = 0x49,
            VK_INSERT = 0x2d,
            VK_J = 0x4a,
            VK_K = 0x4b,
            VK_L = 0x4c,
            VK_LBUTTON = 1,
            VK_LEFT = 0x25,
            VK_M = 0x4d,
            VK_MBUTTON = 4,
            VK_MENU = 0x12,
            VK_N = 0x4e,
            VK_NEXT = 0x22,
            VK_NULTIPLY = 0x6a,
            VK_NUM0 = 0x30,
            VK_NUM1 = 0x31,
            VK_NUM2 = 50,
            VK_NUM3 = 0x33,
            VK_NUM4 = 0x34,
            VK_NUM5 = 0x35,
            VK_NUM6 = 0x36,
            VK_NUM7 = 0x37,
            VK_NUM8 = 0x38,
            VK_NUM9 = 0x39,
            VK_NUMLOCK = 0x90,
            VK_NUMPAD0 = 0x60,
            VK_NUMPAD1 = 0x61,
            VK_NUMPAD2 = 0x62,
            VK_NUMPAD3 = 0x63,
            VK_NUMPAD4 = 100,
            VK_NUMPAD5 = 0x65,
            VK_NUMPAD6 = 0x66,
            VK_NUMPAD7 = 0x67,
            VK_NUMPAD8 = 0x68,
            VK_NUMPAD9 = 0x69,
            VK_O = 0x4f,
            VK_P = 80,
            VK_PAUSE = 0x13,
            VK_PRINT = 0x2a,
            VK_PRIOR = 0x21,
            VK_Q = 0x51,
            VK_R = 0x52,
            VK_RBUTTON = 2,
            VK_RETURN = 13,
            VK_RIGHT = 0x27,
            VK_S = 0x53,
            VK_SCROLL = 0x91,
            VK_SELECT = 0x29,
            VK_SEPARATOR = 0x6c,
            VK_SHIFT = 0x10,
            VK_SNAPSHOT = 0x2c,
            VK_SPACE = 0x20,
            VK_SUBTRACT = 0x6d,
            VK_T = 0x54,
            VK_TAB = 9,
            VK_U = 0x55,
            VK_UP = 0x26,
            VK_V = 0x56,
            VK_W = 0x57,
            VK_X = 0x58,
            VK_Y = 0x59,
            VK_Z = 90,
            wheel = 0x800,
            xdown = 0x80,
            xup = 0x100
        }

        public class WinIo
        {
            public const int KBC_KEY_CMD = 100;
            public const int KBC_KEY_DATA = 0x60;

            [DllImport("WinIo64.dll")]
            public static extern bool GetPhysLong(IntPtr pbPhysAddr, byte pdwPhysVal);
            [DllImport("WinIo64.dll")]
            public static extern bool GetPortVal(IntPtr wPortAddr, out int pdwPortVal, byte bSize);
            public static void Initialize()
            {
                if (InitializeWinIo())
                {
                    KBCWait4IBE();
                    IsInitialize = true;
                }
                else
                {
                    MessageBox.Show("failed");
                }
            }

            [DllImport("WinIo64.dll")]
            public static extern bool InitializeWinIo();
            private static void KBCWait4IBE()
            {
                int pdwPortVal = 0;
                do
                {
                    GetPortVal((IntPtr) 100, out pdwPortVal, 1);
                }
                while ((pdwPortVal & 2) > 0);
            }

            [DllImport("WinIo64.dll")]
            public static extern byte MapPhysToLin(byte pbPhysAddr, uint dwPhysSize, IntPtr PhysicalMemoryHandle);
            [DllImport("user32.dll")]
            public static extern int MapVirtualKey(uint Ucode, uint uMapType);
            public static void MykeyDown(Keys vKeyCoad)
            {
                if (IsInitialize)
                {
                    int num = 0;
                    num = MapVirtualKey((uint) vKeyCoad, 0);
                    KBCWait4IBE();
                    SetPortVal(100, (IntPtr) 210, 1);
                    KBCWait4IBE();
                    SetPortVal(0x60, (IntPtr) 0x60, 1);
                    KBCWait4IBE();
                    SetPortVal(100, (IntPtr) 210, 1);
                    KBCWait4IBE();
                    SetPortVal(0x60, (IntPtr) num, 1);
                }
            }

            public static void MykeyUp(Keys vKeyCoad)
            {
                if (IsInitialize)
                {
                    int num = 0;
                    num = MapVirtualKey((uint) vKeyCoad, 0);
                    KBCWait4IBE();
                    SetPortVal(100, (IntPtr) 210, 1);
                    KBCWait4IBE();
                    SetPortVal(0x60, (IntPtr) 0x60, 1);
                    KBCWait4IBE();
                    SetPortVal(100, (IntPtr) 210, 1);
                    KBCWait4IBE();
                    SetPortVal(0x60, (IntPtr) (num | 0x80), 1);
                }
            }

            public static void MyMouseDown(int vKeyCoad)
            {
                int num = 0;
                num = MapVirtualKey((byte) vKeyCoad, 0);
                KBCWait4IBE();
                SetPortVal(100, (IntPtr) 0xd3, 1);
                KBCWait4IBE();
                SetPortVal(0x60, (IntPtr) (num | 0x80), 1);
            }

            public static void MyMouseUp(int vKeyCoad)
            {
                int num = 0;
                num = MapVirtualKey((byte) vKeyCoad, 0);
                KBCWait4IBE();
                SetPortVal(100, (IntPtr) 0xd3, 1);
                KBCWait4IBE();
                SetPortVal(0x60, (IntPtr) (num | 0x80), 1);
            }

            [DllImport("WinIo64.dll")]
            public static extern bool SetPhysLong(IntPtr pbPhysAddr, byte dwPhysVal);
            [DllImport("WinIo64.dll")]
            public static extern bool SetPortVal(uint wPortAddr, IntPtr dwPortVal, byte bSize);
            public static void Shutdown()
            {
                if (IsInitialize)
                {
                    ShutdownWinIo();
                }
                IsInitialize = false;
            }

            [DllImport("WinIo64.dll")]
            public static extern void ShutdownWinIo();
            [DllImport("WinIo64.dll")]
            public static extern bool UnmapPhysicalMemory(IntPtr PhysicalMemoryHandle, byte pbLinAddr);

            private static bool IsInitialize
            {
                get; set;
            }
        }
    }
}

