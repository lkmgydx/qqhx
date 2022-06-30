using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain.HXAction
{
    public class RunInfo
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuc { get; set; }
        /// <summary>
        /// 是否全部退出
        /// </summary>
        public bool IsExit { get; set; }
        /// <summary>
        /// 是否重试
        /// </summary>
        public bool IsRetry { get; set; }
        /// <summary>
        /// 最大重试次数
        /// </summary>
        public bool MaxTestTime { get; set; }

        public override string ToString()
        {
            return "suc:" + IsSuc + "\r\n" + "isExit:" + IsExit;
        }
    }
}
