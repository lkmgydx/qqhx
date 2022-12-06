using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;
using WSTools.WSLog;

namespace HXmain.HXAction
{
    public class AUTOTiLianWuPing
    {
        private MainGame game;

        private WuPingLanInfo wuPingLanInfo;

        System.Timers.Timer tm = new System.Timers.Timer();

        /// <summary>
        /// 自动提练启用状态
        /// </summary>
        public bool isAutoTiLian { get { return this.tm.Enabled; } }

        /// <summary>
        /// 自动提练状态改变
        /// </summary>
        public event BooleanEvent onAutoTilainChangeEvent;


        public AUTOTiLianWuPing(MainGame game)
        {
            this.game = game;
            tm.Interval = 30 * 1000;//一分钟检查一次
            game.onPowerStop += Game_onPowerStop;
            tm.Elapsed += Tm_Elapsed;
            wuPingLanInfo = new WuPingLanInfo(game);
        }

        private bool isE = false;

        ///// <summary>
        ///// 提练物品
        ///// </summary>
        ///// <returns></returns>
        //public List<WuPingData> TilianAll(int canTiLianCount = 5, int emptcont = 2)
        //{
        //    try
        //    {
        //        if (wuPingLanInfo.getCanTiLianCount().Count > 5 || wuPingLanInfo.getEmpty().Count <= emptcont)
        //        {
        //            return wuPingLanInfo.TilianAllForce();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.logErrorForce("提练异常!", ex);
        //    }
        //    finally
        //    {
        //        game.closeAllGameWin();
        //    }
        //    Log.logForce("空物品");
        //    return new List<WuPingData>();
        //}


        private void Tm_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (isE)
            {
                return;
            }
            isE = true;

            game.TopRun(() =>
            {
                var item = new WuPingLanInfo(game);
                var wu = item.WuPings();
                var canTilians = 0;
                foreach (var i in wu)
                {
                    if (i.CanTiLian)
                    {
                        canTilians++;
                    }
                }
                Log.logForce("自动数量:" + canTilians);
                item.TilianAllForce();
                game.closeAllGameWin();
            }, true);
        }

        private void Game_onPowerStop()
        {
            tm.Enabled = false;
            onAutoTilainChangeEvent?.Invoke(tm.Enabled);
        }

        public void StartAutoTiLian()
        {
            tm.Enabled = true;
            onAutoTilainChangeEvent?.Invoke(tm.Enabled);
        }

        public void StopAutoLian()
        {
            tm.Enabled = false;
            onAutoTilainChangeEvent?.Invoke(tm.Enabled);
        }

    }
}
