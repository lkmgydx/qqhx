using HFrameWork.SystemInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace HXmain.HXInfo
{
    public class HasSomeToHit
    {
        private MainGame game;

        private static readonly Point PersonPoint = new Point(80, 90);
        private static readonly Rectangle recStart = new Rectangle(80, 90, 10, 10);
        private static readonly Rectangle recEnd = new Rectangle(257, 90, 10, 10);

        /// <summary>
        /// 技能1
        /// </summary>
        private static readonly Rectangle JiNeng = new Rectangle(205, 782, 12, 12);

        private static string PersionUUID = null;
        public HasSomeToHit(MainGame game)
        {
            this.game = game;
        }

        //是否有怪可以攻击
        public bool HasHit()
        {
            game.MouseClick(PersonPoint);
            game.sleep(30);
            game.SendKey(System.Windows.Forms.Keys.Oemtilde);
            game.sleep(30);
            if (PersionUUID == null)
            {
                PersionUUID = game.UUIDImageRec(recStart);
            }
            bool suc = true;
            if (PersionUUID == game.UUIDImageRec(recEnd))
            {
                suc = false;
            }
            return suc;
        }
    }
}
