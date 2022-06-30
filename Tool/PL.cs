namespace Tool
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct PL
    {
        public int[][] pl;
        public PP[,] pp;
        public PL(int[][] pl, PP[,] pp)
        {
            this.pl = pl;
            this.pp = pp;
        }
    }
}

