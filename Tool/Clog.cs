using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool
{
    public class Clog
    {
        public static void log(string msg)
        {
            System.Console.WriteLine(DateTime.Now.ToString("hh:mm:ss") + " - " + msg);
        }
    }
}
