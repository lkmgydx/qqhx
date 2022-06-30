using HXmain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain.HXAction
{
    public class Qinyuan
    {
        public static void Start清源(MainGame game)
        {
            if (game == null)
            {
                return;
            }
            List<BaseAction> li = new List<BaseAction>();
            BaseAction ba = BaseAction.getNpcAction(game, Resources.NPC_清源长老);

        }
    }
}
