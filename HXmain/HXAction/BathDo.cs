using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;
using WSTools.WSLog;

namespace HXmain.HXAction
{
    class BathDo
    {
        protected MainGame game = null;
        protected bool exit = false;
        protected List<Bitmap> searchBmp = new List<Bitmap>();
        /// <summary>
        /// 进程锁
        /// </summary>
        protected object lockGameOp = new object();
        /// <summary>
        /// 事件退出
        /// </summary>
        public event BooleanEvent onExit;
        /// <summary>
        /// 默认为30秒超时，退出
        /// </summary>
        const int MaxWaitTime = 1000 * 30;
        /// <summary>
        /// 程序最多等待时间
        /// </summary>
        protected int ProcessMaxWaitTime = MaxWaitTime;

        public BathDo(MainGame game, int maxWaitTim = MaxWaitTime)
        {
            this.game = game;
            ProcessMaxWaitTime = maxWaitTim;
        }
        /// <summary>
        /// 退出图片信息
        /// </summary>
        public Bitmap ExitBmp { get; set; }
        /// <summary>
        /// 退出时是否点击
        /// </summary>
        public bool ExitClik { get; set; } = true;

        public int SucClick = 0;

        public void AddSearchBmp(Bitmap bmp)
        {
            searchBmp.Add(bmp);
        }

        /// <summary>
        /// 处理图片
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        protected virtual Bitmap filterImgae(Bitmap bmp, Bitmap gameIMG)
        {
            return bmp;
        }

        /// <summary>
        /// 开始执行操作
        /// </summary>
        public virtual bool Start()
        {
            int nowTime = Environment.TickCount;
            while (true)
            {
                for (int i = 0; i < searchBmp.Count; i++)
                {
                    lock (lockGameOp)
                    {
                        Log.log("base检测：" + i);
                        if (exit || Environment.TickCount - nowTime > ProcessMaxWaitTime)
                        {
                            Console.WriteLine("exit 【false】");
                            onExit?.Invoke(false);
                            return false;
                        }
                        else
                        {
                            Console.WriteLine("t1:" + (Environment.TickCount - nowTime) + "____" + ProcessMaxWaitTime);
                        }
                        using (Bitmap bt = game.getImg())
                        {
                            if (ExitBmp != null)
                            {

                                var hsexit = ImageTool.findLikeImg(bt, ExitBmp);
                                Log.log("退出检测：" + hsexit.Success);
                                if (hsexit.Success)
                                {
                                    if (ExitClik)
                                    {
                                        Log.log("退出点击!");
                                        game.MouseClick(hsexit.CenterPoint);
                                    }
                                    Console.WriteLine("exit 【true】");
                                    onExit?.Invoke(true);
                                    return true;
                                }
                            }

                            Log.log("filterImgae!");
                            Bitmap bmp = filterImgae(searchBmp[i], bt);
                            var hs = ImageTool.findLikeImg(bmp, bt);
                            Log.log("查找？" + hs.Success);
                            if (hs.Success)
                            {
                                SucClick++;
                                nowTime = Environment.TickCount;
                                game.MouseClick(hs.CenterPoint);
                                Console.WriteLine("suc click ");
                                break;
                            }
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 强制停止
        /// </summary>
        public virtual void Stop()
        {
            exit = true;
        }
    }
}
