namespace HFrameWork.SystemDll
{
    using System;
    using System.Runtime.InteropServices;

    public class Shell32
    {
        [DllImport("shell32.dll")]
        public static extern void SHChangeNotify(HChangeNotifyEventID wEventId, HChangeNotifyFlags uFlags, IntPtr dwItem1, IntPtr dwItem2);
    }
}

