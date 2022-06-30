using HXmain.HXInfo;
using System;
using System.Collections.Generic;
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
        Timer t_Main;
        MainGame game;

        public GameState(MainGame game)
        {
            this.game = game;
            t_Main = new System.Windows.Forms.Timer();
            t_Main.Interval = 2000;
            t_Main.Tick += T_Main_Tick;
        }

        private void T_Main_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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
    }
}
