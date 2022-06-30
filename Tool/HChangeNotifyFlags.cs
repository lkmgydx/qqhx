namespace HFrameWork.SystemDll
{
    using System;

    [Flags]
    public enum HChangeNotifyFlags
    {
        SHCNF_DWORD = 3,
        SHCNF_FLUSH = 0x1000,
        SHCNF_FLUSHNOWAIT = 0x2000,
        SHCNF_IDLIST = 0,
        SHCNF_PATHA = 1,
        SHCNF_PATHW = 5,
        SHCNF_PRINTERA = 2,
        SHCNF_PRINTERW = 6
    }
}

