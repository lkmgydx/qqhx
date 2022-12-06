using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tool
{
    public sealed class Tlhelp32
    {
        [DllImport("tlhelp32")]
        public static extern void CreateToolhelp32Snapshot(int p, int px);
    }
}
