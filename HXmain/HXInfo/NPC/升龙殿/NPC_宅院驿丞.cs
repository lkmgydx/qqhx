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
    public class NPC_宅院驿丞 : NPCBase
    {
        public NPC_宅院驿丞() : base(MapBase.升龙殿, "宅院驿丞", new System.Drawing.Point(45, 103))
        {

        }
        public bool 排行榜任务(MainGame game)
        {
            MoveTo(game);
            List<BaseAction> li = new List<BaseAction>();
            li.Add(DoToDaySLD(game));
            li.Add(get和宅院驿丞对话(game));
            li.Add(get排行榜第一(game));
            li.Add(get投鲜花(game));
            li.Add(get投鲜花(game));
            li.Add(get投鲜花(game));
            li.Add(get投鲜花(game));
            BaseAction ba = get投鲜花(game);
            ba.AfterWaitTime = 500;
            li.Add(ba);
            BaseAction.DoActions(li);
            game.clickImg(Resources.结束对话);
            return true;
        }

        public bool 我要打劫(MainGame game)
        {
            MoveTo(game);
            var ad = new NpcAskDo(game, Resources.npc_升龙殿_宅院驿丞310211406, "宅院驿丞任务...");
            ad.AddSearchBmp(Resources.font_趁火打劫);
            ad.AddSearchBmp(Resources.font_肥羊列表);
            ad.AddOnSucBmp(Resources.font_肥羊列表, () => { return true; });

            ad.ExitBmp = Resources.act_钱善_exit;
            ad.addWaitDoImage(Resources.自动捐满Suc, 30000);

            bool suc = ad.Start();
            if (!suc)
            {
                return false;
            }
            BathDo bd = new BathDo(game);
            bd.AddSearchBmp(Resources.font_领取荣誉);
            return bd.Start();
        }

        private BaseAction get排行榜第一(MainGame game)
        {
            BaseAction action = BaseAction.getAction(game, Resources.font_排行_名字);
            action.FlotPoint = new System.Drawing.Point(45, 18);
            return action;
        }

        private BaseAction get和宅院驿丞对话(MainGame game)
        {
            BaseAction ba = new BaseAction();
            ba.AfterWaitTime = 1000;
            ba.Bmp = Resources.font_排行榜;
            ba.MatchOp = 95;
            ba.isCenter = true;
            ba.ReverMousePostion = false;
            ba.game = game;
            return ba;
        }

        private BaseAction DoToDaySLD(MainGame game)
        {
            BaseAction ba = new BaseAction();
            ba.AfterWaitTime = 500;
            ba.Bmp = Resources.npc_升龙殿_宅院驿丞310211406;
            ba.MatchOp = 95;
            ba.FlotPoint = new System.Drawing.Point(30, 80);
            ba.ActionType = ActionType.RigthDbClickNPC;
            ba.ReverMousePostion = false;
            ba.game = game;
            return ba;
        }

        private BaseAction get投鲜花(MainGame game)
        {
            BaseAction ba = BaseAction.getAction(game, Resources.font_投掷鲜花);
            ba.MatchOp = 95;
            ba.BeforActionTime = 300;
            ba.isCenter = true; ;
            return ba;
        }
    }
}
