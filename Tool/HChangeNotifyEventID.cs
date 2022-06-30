namespace HFrameWork.SystemDll
{
    using System;

    [Flags]
    public enum HChangeNotifyEventID
    {
        SHCNE_ALLEVENTS = 0x7fffffff,
        SHCNE_ASSOCCHANGED = 0x8000000,
        SHCNE_ATTRIBUTES = 0x800,
        SHCNE_CREATE = 2,
        SHCNE_DELETE = 4,
        SHCNE_DRIVEADD = 0x100,
        SHCNE_DRIVEADDGUI = 0x10000,
        SHCNE_DRIVEREMOVED = 0x80,
        SHCNE_EXTENDED_EVENT = 0x4000000,
        SHCNE_FREESPACE = 0x40000,
        SHCNE_MEDIAINSERTED = 0x20,
        SHCNE_MEDIAREMOVED = 0x40,
        SHCNE_MKDIR = 8,
        SHCNE_NETSHARE = 0x200,
        SHCNE_NETUNSHARE = 0x400,
        SHCNE_RENAMEFOLDER = 0x20000,
        SHCNE_RENAMEITEM = 1,
        SHCNE_RMDIR = 0x10,
        SHCNE_SERVERDISCONNECT = 0x4000,
        SHCNE_UPDATEDIR = 0x1000,
        SHCNE_UPDATEIMAGE = 0x8000
    }
}

