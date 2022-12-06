using HFrameWork.SystemInput;
using HXmain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace HXmain.HXAction
{
    public class CloseGameWins
    {
        private MainGame game;
        public CloseGameWins(MainGame game)
        {
            this.game = game;
        }

        /// <summary>
        /// 是否有可关闭的窗口
        /// </summary>
        /// <returns></returns>
        public bool HasCoseWin
        {
            get
            {
                return game.findImg(Resources.close).Success;
            }
        }

        public void closeAll()
        {
            game.TopRun(() =>
            {
                Mouse.cacheLocation();
                HSearchPoint hs;
                do
                {
                    hs = game.clickImg(Resources.close);
                } while (hs.Success);
                Mouse.reventLocation();
            }, true);
        }
    }
}
