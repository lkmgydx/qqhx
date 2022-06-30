using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain.HXAction
{
    public class 界面显示
    {
        private MainGame game;

        private static Point worldTurnPoint = new Point(304, 730);
        private static Point personTurnPoint = new Point(240, 730);


        public 界面显示(MainGame game)
        {
            if (game == null)
            {
                throw new Exception("游戏不能为空");
            }
            this.game = game;
        }

        public void 显示系统发言()
        {
            if (RangeUUID.系统聊天显示可见.inUUIDS(game.UUIDImageRec(RangeUUID.系统聊天显示可见.Rec)))
            {
                return;
            }
            else
            {
                game.MouseClick(worldTurnPoint);
            }
        }

        public void 关闭系统发言()
        {
            string imgStr = game.UUIDImageRec(RangeUUID.系统聊天显示可见.Rec);
            WSTools.WSLog.Log.logForce(imgStr);
            if (!RangeUUID.系统聊天显示可见.inUUIDS(imgStr))
            {

                // WSTools.WSWinFn.ShowInfo("不同，已关闭" + imgStr);
                return;
            }
            else
            {
                WSTools.WSWinFn.ShowInfo("相同点击");
                game.MouseClick(worldTurnPoint);
            }
        }

        public void 显示人物世界发言()
        {
            string imgStr = game.UUIDImageRec(RangeUUID.人物聊天显示可见.Rec);
            if (RangeUUID.人物聊天显示可见.inUUIDS(game.UUIDImageRec(RangeUUID.人物聊天显示可见.Rec)))
            {
                return;
            }
            else
            {
                game.MouseClick(personTurnPoint);
            }
        }

        public void 关闭人物世发言()
        {
            string imgStr = game.UUIDImageRec(RangeUUID.人物聊天显示可见.Rec);
            WSTools.WSLog.Log.logForce(imgStr);
            if (!RangeUUID.人物聊天显示可见.inUUIDS(imgStr))
            {
                return;
            }
            else
            {
                game.MouseClick(personTurnPoint);
            }
        }
    }
}
