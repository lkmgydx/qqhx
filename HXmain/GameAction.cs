using HXmain.HXAction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain
{
    public class GameAction
    {
        private MainGame game;
        public GameAction(MainGame game)
        {
            this.game = game;
        }

        public void TiLianSome()
        {
            TiLian.TiLianSome(game);
        }
    }
}
