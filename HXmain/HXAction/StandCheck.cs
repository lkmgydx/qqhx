using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace HXmain.HXAction
{
    public class StandCheck
    {
        private MainGame Game;
        private Timer timer;
        /// <summary>
        /// 最大等待时长
        /// </summary>
        public int MaxWaitTime { get; set; }

        Point StartPostion;

        public delegate void PostionNotMoveEvent(Point postion);
        public event PostionNotMoveEvent PostionNotMoveEventHander;

        private long currentStartTime = 0;
        public StandCheck(MainGame game, int maxWaitTime = 2000)
        {
            Game = game;
            MaxWaitTime = maxWaitTime;
            game.onPowerStop += Game_onPowerStop;
        }

        private void Game_onPowerStop()
        {
            if (timer == null)
            {
                return;
            }
            timer.Enabled = false;
        }

        public void StartCheck()
        {
            if (timer == null)
            {
                timer = new Timer();
                timer.Interval = MaxWaitTime;
                timer.Elapsed += Timer_Elapsed;
            }
            currentStartTime = Environment.TickCount;
            StartPostion = Game.CurrentLocation;
            if (!timer.Enabled)
            {
                timer.Start();
            }
        }

        private volatile bool isWait = false;

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {

            if (isWait)
            {
                Console.WriteLine("位置不动检测跳过");
                return;
            }
            Console.WriteLine("位置不动检测");
            isWait = true;
            try
            {
                if (Game.AutoOneKey && Game.isUseSkill)//正在用技能，不检测位置不移动
                {
                    return;
                }

                Point pt = Game.CurrentLocation;
                if ((Environment.TickCount - currentStartTime > MaxWaitTime))
                {
                    if (pt.X == StartPostion.X && pt.Y == StartPostion.Y)
                    {
                        Console.WriteLine("------检测到人物未移动-------");
                        PostionNotMoveEventHander?.Invoke(pt);
                    }
                    else
                    {
                        Console.WriteLine(pt + "  ->  " + StartPostion);
                        StartPostion = pt;
                        "".ToString();
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                isWait = false;
            }

        }

        public void StopCheck()
        {
            if (timer != null)
            {
                timer.Stop();
            }
        }
    }
}
