using HXmain.HXInfo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HXmain
{
    /// <summary>
    /// 游戏状态类
    /// </summary>
    public class GameState
    {
        /// <summary>
        /// 技能1
        /// </summary>
        private static readonly Rectangle JiNeng = new Rectangle(205, 782, 12, 12);

        MainGame game;

        public GameState(MainGame game)
        {
            this.game = game;
        }



        /// <summary>
        /// 是否有组队信息
        /// </summary>
        /// <returns></returns>
        public bool hasZuDuiinfo()
        {
            return HXZuDuics.hasZuDuiInfo(game);
        }

        /// <summary>
        /// 是否有验证码
        /// </summary>
        /// <returns></returns>
        public bool hasYZM()
        {
            return YZM.hasYZM(game);
        }

        private string JiNeng1Str;
        public bool hasUseSkill()
        {
            var JiNeng1StrN = game.UUIDImageRec(JiNeng);
            if (JiNeng1Str == null)
            {
                JiNeng1Str = JiNeng1StrN;
                game.sleep(200);
                return hasUseSkill();
            }
            if (JiNeng1Str == JiNeng1StrN)
            {
                return false;
            }
            else
            {
                JiNeng1Str = JiNeng1StrN;
                return true;
            }
        }
    }
}
