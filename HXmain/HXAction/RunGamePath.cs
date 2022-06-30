using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WSTools.WSLog;

namespace HXmain.HXAction
{
    public class RunGamePath
    {
        private MainGame game;
        Task run = null;
        private bool canGo = true;

        /// <summary>
        /// 自动技能
        /// </summary>
        public bool autoSkill { get; set; } = true;

        public event EventHandler onRoundEnd;

        public RunGamePath(MainGame game)
        {
            this.game = game;
            game.onPowerStop += Game_onPowerStop;
        }

        private void Game_onPowerStop()
        {
            canGo = false;
        }

        public void Run(Point[] pt, Action backFn)
        {
            var priRun = run;
            if (priRun != null)
            {
                try
                {
                    priRun.Dispose();
                }
                catch (Exception ex)
                {
                    Log.logError("前置进程结束失败!", ex);
                }
            }
            canGo = true;
            run = Task.Factory.StartNew(() =>
           {
               game.lockUtil.reSetLock();
               bool isFirst = true;
               Log.logForce("runPath进程开始 循环 [" + game.isXUNHUAN + "] ->坐标数:" + pt.Length);
               do
               {
                   Log.log("开始DRAW!!");
                   int prii = 0;
                   Point ptemp = game.CurrentLocation;
                   if (isFirst)
                   {
                       isFirst = false;
                       int len = 10000;
                       for (int i = 0; i < pt.Length; i++)//查找与当前位置最接近的点
                       {
                           int w = Math.Abs(pt[i].X - ptemp.X) + Math.Abs(pt[i].Y - ptemp.Y);
                           if (w < len)
                           {
                               prii = i;
                               len = w;
                           }
                       }
                       Log.log("===========START POINT ===============" + prii);
                   }

                   for (int i = prii; i < pt.Length - 1; i++)
                   {
                       if (!canGo)
                       {
                           Log.logForce("casego Stop - " + i);
                           onRoundEnd?.Invoke(game, null);
                           return;
                       }
                       runPoint(pt[i], pt[i + 1]);
                   }
                   runPoint(pt[pt.Length - 1], Point.Empty);
                   Log.log("运行完毕！");
                   Log.logForce("isXUNHUAN - " + game.isXUNHUAN);
                   try
                   {
                       onRoundEnd?.Invoke(game, null);
                   }
                   catch (Exception ex)
                   {
                       Log.logError("结束异常", ex);
                   }
                   Log.log("运行完毕2！");
               }
               while (game.isXUNHUAN);
               Log.logForce("isXUNHUAN - " + game.isXUNHUAN);
               if (backFn != null)
               {
                   Task.Factory.StartNew(backFn);
               }
           });
        }

        private void runPoint(Point pt, Point next)
        {
            if (!canGo)
            {
                return;
            }
            try
            {
               // game.StandCheck.StartCheck();
                game.movePersion.MovePoint(pt, next);
               // game.StandCheck.StopCheck();
            }
            catch (Exception)
            {

            }
            Log.log("===========SUC===============" + pt.X + ":" + pt.Y);
        }
    }
}
