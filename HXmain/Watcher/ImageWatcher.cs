using HXmain.HXAction;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;
using WSTools.WSLog;
using static HXmain.HXAction.BaseAction;

namespace HXmain.Watcher
{
    /// <summary>
    /// 图片观察类
    /// </summary>
    public class ImageWatcher : BaseWatcher
    {
        public ImageWatcher() { }

        public delegate void WatcherRunPonit<T>(T iw, HSearchPoint hs) where T : class, IWatcher, new();
        public delegate void WatcherRun<T>(T iw) where T : class, new();

        public static T getWatcher<T>(WatcherRun<T> run) where T : ImageWatcher, IWatcher, new()
        {
            T wz = null;
            wz = getWatcher<T>((s, f) =>
            {
                run.Invoke(wz);
            });
            return wz;
        }
        public static T getWatcher<T>(WatcherRunPonit<T> run) where T : ImageWatcher, IWatcher, new()
        {
            T wz = null;
            WatchSucEventHandler wh = (s) => { };
            wz = getWatcher<T>((s) =>
            {
                run?.Invoke(wz, s);
            }, null);
            return wz;
        }

        /// <summary>
        /// NPC自动对话Watcher
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="game"></param>
        /// <returns></returns>
        public static T getNpcWatcher<T>(MainGame game) where T : ImageWatcher, IWatcher, new()
        {
            var t = getWatcher<T>((s, e) =>
             {
                 Log.logErrorForce(s.ToString() + "___NPC Right CLICK __" + e.CenterPoint);
                 var tp = e.CenterPoint;
                 tp.Offset(0, 70);
                  
                 game.MouseRigthClick(tp);
             });
           
            return t;
        }

        //public static T getActionWatcher<T>(MainGame game) where T : class, IWatcher, new()
        //{
        //    return getWatcher<T>((s, e) =>
        //    {
        //        if (typeof(T).IsSubclassOf(typeof(ActionWatcher)))
        //        {
        //            ActionWatcher ac = s as ActionWatcher;

        //            Point p = e.Point;
        //            if (ac.isCenter)
        //            {
        //                p = e.CenterPoint;
        //            }
        //            if (ac.FlotPoint != Point.Empty)
        //            {
        //                p.Offset(ac.FlotPoint);
        //            }
        //            switch (ac.ActionType)
        //            {
        //                case ActionType.LeftClick:
        //                    {
        //                        game.MouseClick();
        //                        break;
        //                    }
        //                case ActionType.LeftDbClick:
        //                    {
        //                        game.MouseDbClick(p);
        //                        break;
        //                    }
        //                case ActionType.RightClick:
        //                    {
        //                        game.MouseRigthClick(p);
        //                        break;
        //                    }
        //                case ActionType.RigthDbClick:
        //                    {
        //                        game.MouseRigthDBClick(p);
        //                        break;
        //                    }
        //                default:
        //                    break;
        //            }
        //        }

        //    });


        //}

        public static T getWatcher<T>(WatchSucEventHandler suc = null, WatchSucEventHandler notfind = null) where T : class, IWatcher, new()
        {
            if (typeof(T).IsSubclassOf(typeof(ImageWatcher)))
            {
                T iw = new T();
                if (suc != null || notfind != null)
                {
                    (iw as ImageWatcher).onWatchChange += (e, s) =>
                    {
                        if (e)
                        {
                            suc?.Invoke(s);
                        }
                        else
                        {
                            notfind?.Invoke(s);
                        }
                    };
                }
                return iw;
            }
            throw new Exception("not watcher class");
        }

        public bool WatcherClick<T>(MainGame game) where T : class, IWatcher, new()
        {
            bool suc = false;
            getWatcher<T>((s) =>
            {
                Log.logErrorForce(ToString() + "___ CLICK __" + s.CenterPoint);
                var p = s.CenterPoint;
                game.MouseClick(p);
                suc = true;
            }).Watch(WatcherGameBmp);
            return suc;
        }

        public bool WatcherClick<T>(MainGame game, Point offset) where T : class, IWatcher, new()
        {
            bool suc = false;
            getWatcher<T>((s) =>
            {
                Log.logErrorForce(ToString() + "___Offset CLICK __" + s.CenterPoint);
                var p = s.CenterPoint;
                p.Offset(offset);
                game.MouseClick(p);
                suc = true;
            }).Watch(WatcherGameBmp);
            return suc;
        }

        public bool WaitDo(MainGame game, isSucFn fn = null, int time = 3000)
        {
            if (fn == null)
            {
                return waitSucFn(() =>
                {
                    if (game.ESCSTOP)
                    {
                        return false;
                    }
                    using (var ff = game.getImg())
                    {
                        return Watch(ff).Success;
                    }
                }, time);
            }
            else
            {
                return waitSucFn(fn, time);
            }

        }

        /// <summary>
        /// 等待wacher执行成功,否则退出
        /// </summary>
        /// <param name="game"></param>
        /// <param name="wacher"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool WaitDo(MainGame game, ImageWatcher wacher, int time = 3000)
        {
            return BaseAction.waitSucFn(() =>
            {
                return wacher.Watch(game.getImg()).Success;
            }, time);
        }

        /// <summary>
        /// 等待多个wacher执行成功,否则退出
        /// </summary>
        /// <param name="game"></param>
        /// <param name="wacher"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool WaitDo(MainGame game, List<ImageWatcher> wacher, int time = 3000)
        {
            return BaseAction.waitSucFn(() =>
            {
                using (var g = game.getImg())
                {
                    for (int i = 0; i < wacher.Count; i++)
                    {
                        if (wacher[i].Watch(g).Success)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }, time);
        }

        public bool waitSucFn(isSucFn fn, int waitTime = 0)
        {
            return BaseAction.waitSucFn(fn, waitTime);
        }

        /// <summary>
        /// 等待观察出现的图片列表，优先单张
        /// </summary>
        protected virtual List<Bitmap> WatcherBmpList { get; }

        /// <summary>
        /// 等待观察出现的图片
        /// </summary>
        protected virtual Bitmap WatcherBmp { get; }
        /// <summary>
        /// 观察的整个图片
        /// </summary>
        public Bitmap WatcherGameBmp { get; protected set; }
 

        public override event WatchEventHandler onWatchChange;

     

        /// <summary>
        /// 成功观测到
        /// </summary>
        public event WatchSucEventHandler onWatchSucess;

        /// <summary>
        /// 匹配成功后，缓存下来的位置信息
        /// </summary>
        protected HSearchPoint WinSearchPoint = HSearchPoint.Empty;
        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="bmp"></param>
        public override HSearchPoint Watch(Bitmap bmp)
        {
            Log.logForce("[开始处理]:" + ToString());
            WatcherGameBmp = bmp;
            var hs = HSearchPoint.Empty;
            if (WatcherBmp != null)
            {
                hs = ImageTool.findEqImg(bmp, WatcherBmp);
            }
            if (!hs.Success && WatcherBmpList != null)
            {
                foreach (var item in WatcherBmpList)
                {
                    hs = ImageTool.findEqImg(bmp, item);
                    if (hs.Success)
                    {
                        break;
                    }
                }
            }

            WinSearchPoint = hs;
            try
            {
                onWatchChange?.Invoke(hs.Success, hs);
            }
            catch (Exception ex)
            {
                Log.logError("ER", ex);
            }

            if (hs.Success)
            {
                onWatchSucess?.Invoke(hs);
            }
            Log.logForce("[结束处理]:【" + hs.Success + "】" + ToString());
            return hs;
        }
         
    }
}
