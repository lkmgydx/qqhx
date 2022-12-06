using HXmain.HXAction;
using HXmain.HXInfo.Map;
using HXmain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain.HXInfo.NPC.升龙殿
{
    public class NPC_中级装备商 : NPCBase
    {
        //中级装备商 41,78
        public NPC_中级装备商() : base(MapBase.Maps.北郡5, "中级装备商", new System.Drawing.Point(64, 83))
        { }

        public void AskSome(MainGame game)
        {
            game.MovePoint(this.Position);
            game.MouseRigthClick(game.Width / 2, game.Height / 2);
        }
    }
}
