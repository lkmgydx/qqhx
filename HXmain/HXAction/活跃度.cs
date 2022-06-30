using HXmain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace HXmain.HXAction
{
    class 活跃度
    {
        /// <summary>
        /// 游戏助手窗口 ,145,75
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        static HSearchPoint hasYXZSWin(MainGame game)
        {
            return ImageTool.findEqImg(Resources.win_游戏助手, game.getImg());
        }

        public void doAction()
        {

        }
    }
}
