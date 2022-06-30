namespace HFrameWork.SystemDll
{
    using System;
    using System.Diagnostics;

    public sealed class HYL
    {
        public static string CreateTime = "2014/12/27";
        public static string Infomation = "[279241400@QQ.com - hyl]";
        public static string Remark = "向智能迈进！";

        public static void ExpolorerPath(string path) {
            ProcessStartInfo psi = new ProcessStartInfo("Explorer.exe");
            psi.Arguments = "/e,/select," + path;
            Process.Start(psi);
        }
    }
}

