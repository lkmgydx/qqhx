using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace qqhx
{
    class MProcessRead
    {
        public Process Process { get; set; }
        public MProcessRead(Process p)
        {
            this.Process = p;
        }

        public string ReadData(int address)
        {
            string data = MemoeryOpt.ReadMemoryValueStr(address, Process.Id);
            // MemoeryOpt.ReadMemoryValue(Address, MainProcess.Id)
            // return MemoeryOpt.ReadMemoryValueStr(address, Process.Id);
            //  return "";
            return data;
        }
    }
}
