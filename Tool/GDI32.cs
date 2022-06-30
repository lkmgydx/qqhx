using HFrameWork.SystemDll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tool
{
    public sealed class GDI32
    {

        //[DllImport("gdi32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
       // public static extern int DrawText(IntPtr hdc, string lpchText, int count, GDI32_Rangle lprc, int format);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public extern static int DrawText(IntPtr hdc, string lpString, int nCount, ref User32.RECT lpRect, int uFormat);
        // int DrawTextEx(HDC, hdc, LPTSTR lpchText, int cchText, LPRECT lprc, UINT dwDTFormat, LPDRAWTEXTPARAMS lpDTParams)；
    }
}
