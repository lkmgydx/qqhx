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

        public void closeAll()
        {
            Mouse.cacheLocation();
            HSearchPoint hs;
            do
            {
                hs = game.clickImg(Resources.close);
            } while (hs.Success);
            Mouse.reventLocation();
        }
    }
}
