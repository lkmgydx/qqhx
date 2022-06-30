using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool
{
    public sealed class WinTool
    {
        /// <summary>
        /// 返回数值
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int getNumFromText(System.Windows.Forms.TextBox text)
        {
            int rst = 0;
            try
            {
                rst = int.Parse(text.Text.Trim());
            }
            catch
            {

            }
            return rst;
        }
    }
}
