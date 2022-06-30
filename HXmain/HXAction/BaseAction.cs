using HFrameWork.SystemInput;
using HXmain.HXInfo.CacheData;
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
    /// <summary>
    /// 动作类型
    /// </summary>
    public enum ActionType
    {
        LeftClick, RightClick, LeftDbClick, RigthDbClick, RigthDbClickNPC, Other
    }

    /// <summary>
    /// 基本动作
    /// </summary>
    public class BaseAction
    {
        public static BaseAction getAction(MainGame game, Bitmap bmp)
        {
            BaseAction ba = new BaseAction();
            ba.AfterWaitTime = 0;
            ba.Bmp = bmp;
            ba.MatchOp = 90;
            ba.game = game;
            return ba;
        }

        public static BaseAction get对话Action(MainGame game, Bitmap bmp, string name = "baseAction", bool beforSucSkipThis = false, int runWaitTime = 3000, bool beforFalsSkipThis = true)
        {
            BaseAction ba = new BaseAction();
            ba.BeforActionTime = 100;//休息100
            ba.AfterWaitTime = 10;
            ba.Bmp = bmp;
            ba.isCenter = true;
            ba.MatchOp = 100;
            ba.game = game;
            ba.beforSucSkipThis = beforSucSkipThis;
            ba.ActionName = name;
            ba.RunWaitTime = runWaitTime;
            ba.beforFalsSkipThis = beforFalsSkipThis;
            return ba;
        }

        public static BaseAction getCenterAction(MainGame game, Bitmap bmp, string name = "baseAction", bool beforSucSkipThis = false, int runWaitTime = 3000, bool beforFalsSkipThis = false)
        {
            BaseAction ba = new BaseAction();
            ba.AfterWaitTime = 0;
            ba.Bmp = bmp;
            ba.isCenter = true;
            ba.MatchOp = 90;
            ba.game = game;
            ba.beforSucSkipThis = beforSucSkipThis;
            ba.ActionName = name;
            ba.RunWaitTime = runWaitTime;
            ba.beforFalsSkipThis = beforFalsSkipThis;
            return ba;
        }

        public static BaseAction getNpcAction(MainGame game, Bitmap bmp)
        {
            BaseAction ba = new BaseAction();
            ba.AfterWaitTime = 100;
            ba.Bmp = bmp;
            ba.isCenter = true;
            ba.MatchOp = 90;
            ba.FlotPoint = new Point(10, 50);
            ba.ActionType = ActionType.RigthDbClickNPC;
            ba.BeforActionTime = 100;
            ba.ReverMousePostion = true;
            ba.game = game;
            return ba;
        }
        /// <summary>
        /// 截图范围
        /// </summary>
        public Rectangle SearChRectangle { get; set; }
        /// <summary>
        /// 是否为中间
        /// </summary>
        public bool isCenter { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Delegate HXAction { get; set; }
        /// <summary>
        /// 默认为鼠标左击
        /// </summary>
        public ActionType ActionType { get; set; }
        /// <summary>
        /// 同样或类似图片
        /// </summary>
        public List<Bitmap> SameImg { get; set; } = new List<Bitmap>();
        /// <summary>
        /// 执行等待时间(默认等待3秒)
        /// </summary>
        public int RunWaitTime { get; set; } = 3000;
        /// <summary>
        /// 在行动前等待时间
        /// </summary>
        public int BeforActionTime { get; set; }
        /// <summary>
        /// 匹配度，默认90%
        /// </summary>
        public int MatchOp { get; set; } = 90;

        public int notFindWaitTime { get; set; } = 3000;
        /// <summary>
        /// 要寻找的图片
        /// </summary>
        public Bitmap Bmp { get; set; }

        /// <summary>
        /// 游戏主进程
        /// </summary>
        public MainGame game { get; set; }

        public Bitmap getScreenBmp()
        {
            if (SearChRectangle != Rectangle.Empty)
            {
                return game.getImg(SearChRectangle);
            }
            else
            {
                return game.getImg();
            }
        }

        /// <summary>
        /// 执行等待时间
        /// </summary>
        public int AfterWaitTime { get; set; }

        /// <summary>
        /// 最大尝试次数
        /// </summary>
        public int MaxErorrTestTime { get; set; }

        public BaseAction NotSucAction { get; set; }
        /// <summary>
        /// 偏移位置
        /// </summary>
        public Point FlotPoint { get; set; }

        /// <summary>
        /// 鼠标移回原始位置,默认为true
        /// </summary>
        public bool ReverMousePostion { get; set; } = true;
        /// <summary>
        /// 成功时将退出此批量处理
        /// </summary>
        public bool SucStop { get; set; }
        /// <summary>
        /// 如果成功是否跳过后面所有
        /// </summary>
        public bool SucSkipNextAll { get; set; }
        /// <summary>
        /// 上一条成功，则此条一执行
        /// </summary>
        public bool beforSucSkipThis { get; set; }
        /// <summary>
        /// 上一条失败，则跳过此执行
        /// </summary>
        public bool beforFalsSkipThis { get; set; }
        /// <summary>
        /// 上一条是否成功执行
        /// </summary>
        public bool beforIsSuc { get; set; }
        /// <summary>
        /// 执行命令名称
        /// </summary>
        public string ActionName { get; set; }

        public static void DoActions(List<BaseAction> actions)
        {
            bool beforSuc = true;
            for (int i = 0; i < actions.Count; i++)
            {
                BaseAction act = actions[i];
                actions[i].beforIsSuc = beforSuc;
                Clog.log(string.Format("即将执行任务:[{0}] - {1}", i, act.ActionName));
                RunInfo info = null;
                if (beforSuc)//上一条执行成功的情况
                {
                    if (act.beforSucSkipThis)
                    {
                        Clog.log("前置任务成功，跳过此任务");
                        info = new RunInfo();
                        info.IsSuc = true;
                    }
                    else
                    {
                        info = DoBaseAction(actions[i]);
                    }
                }
                else //上一条执行失败的情况
                {
                    if (act.beforFalsSkipThis)
                    {
                        Clog.log("前置任务失败，跳过此任务");
                        info = new RunInfo();
                        info.IsSuc = false;
                    }
                    else
                    {
                        info = DoBaseAction(actions[i]);
                    }
                }
                Clog.log("前一条执行状态:" + info.ToString());
                if (info.IsExit)
                {
                    Log.logForce("执行完成，退出");
                    return;
                }
                beforSuc = info.IsSuc;
                if (!beforSuc && actions[i].MaxErorrTestTime != 0)
                {
                    for (int j = 0; j < actions[i].MaxErorrTestTime; j++)
                    {
                        info = DoBaseAction(actions[i]);
                        if (info.IsExit)
                        {
                            Log.logForce("执行完成，退出");
                            return;
                        }
                        beforSuc = info.IsSuc;
                        j--;
                        if (beforSuc)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public static RunInfo DoBaseAction(BaseAction obj)
        {
            RunInfo ri = new RunInfo();
            try
            {
                bool suc = RealRun(obj);
                if (suc)
                {
                    if (obj.SucSkipNextAll || obj.SucStop)
                    {
                        ri.IsExit = true;
                    }
                }
                ri.IsSuc = suc;
            }
            catch (Exception ex)
            {
                Log.logErrorForce("运行动作异常!", ex);
            }
            return ri;
        }

        private static bool RealRun(BaseAction obj)
        {
            Clog.log("执行任务 - " + obj.ActionName);
            if (obj.BeforActionTime != 0)
            {
                KeyBoard.sleep(obj.BeforActionTime);
            }
            obj.game.Active();
            HSearchPoint hs = waitImg(obj);
         
          
            if (hs.Success)
            {
                Clog.log("解析成功");
                obj.game.Active();
                if (obj.HXAction != null)
                {
                    obj.HXAction.Method.Invoke(null, null);
                }
                else
                {
                    Point pt = hs.Point;
                    if (obj.isCenter)
                    {
                        pt = hs.CenterPoint;
                    }
                    if (obj.FlotPoint != Point.Empty)
                    {
                        pt = new Point(pt.X + obj.FlotPoint.X, pt.Y + obj.FlotPoint.Y);
                    }
                    if (obj.BeforActionTime != 0)
                    {
                        KeyBoard.sleep(obj.BeforActionTime);
                    }
                    if (obj.ReverMousePostion)
                    {
                        Mouse.cacheLocation();
                    }
                    if (obj.game != null)
                    {
                        Log.log("move ->" + pt);
                        obj.game.MouseMove(new Point(pt.X - 1, pt.Y + 1));
                        obj.game.MouseMove(pt);
                        KeyBoard.sleep(100);
                    }
                    else
                    {
                        Mouse.move(new Point(pt.X - 1, pt.Y + 1));
                        Mouse.move(pt);
                    }

                    if (obj.ActionType == ActionType.LeftClick)
                    {
                        Mouse.leftclick();
                    }
                    else if (obj.ActionType == ActionType.RightClick|| obj.ActionType == ActionType.RigthDbClickNPC)
                    {
                        Mouse.rightclick();
                        KeyBoard.sleep(500);
                    }
                    else if (obj.ActionType == ActionType.LeftDbClick)
                    {
                        Mouse.dbclick();
                        KeyBoard.sleep(500);
                    }
                    else if (obj.ActionType == ActionType.RigthDbClick)
                    {
                        Mouse.rightDbclick();
                        KeyBoard.sleep(500);
                    }
                    if (obj.ReverMousePostion)
                    {
                        Mouse.reventLocation();
                    }
                }
                if (obj.AfterWaitTime != 0)
                {
                    KeyBoard.sleep(obj.AfterWaitTime);
                }
                return true;
            }
            else
            {
                if (obj.NotSucAction != null)
                {
                    DoBaseAction(obj.NotSucAction);
                }
                Clog.log("解析失败");
                return false;
            }
        }

        public static void DoAll(List<BaseAction> actions)
        {
            Bitmap bmp = actions[0].game.getImg();
            for (int i = 0; i < actions.Count; i++)
            {
                HSearchPoint hs = ImageTool.findLikeImg(actions[i].Bmp, bmp, 90);
                if (hs.Success)
                {
                    DoBaseAction(actions[i]);
                }
            }
        }

        private static HSearchPoint waitImg(BaseAction action, string waitMsg = "等待中...", string sucMsg = "匹配成功!", bool isFind = true)
        {
            int max = Environment.TickCount + action.RunWaitTime;
            if (action.SameImg.Count == 0)
            {
                while (true)
                {
                    HSearchPoint suc = null;
                    using (var findimg = action.getScreenBmp())
                    {
                        suc = ImageTool.findEqImg(action.Bmp, findimg);
                    }

                    if (suc.Success == isFind)
                    {
                        Console.WriteLine(sucMsg);
                        return suc;
                    }
                    else
                    {
                        Console.WriteLine(waitMsg);
                    }
                    Mouse.sleep(100);
                    if (Environment.TickCount > max)
                    {
                        return new HSearchPoint(false, Point.Empty, Size.Empty, action.RunWaitTime);
                    }
                }
            }
            else
            {
                while (true)
                {
                    using (Bitmap big = action.getScreenBmp())
                    {
                        var suc = ImageTool.findEqImg(action.Bmp, big);
                        if (suc.Success == isFind)
                        {
                            Console.WriteLine(sucMsg);
                            return suc;
                        }
                        for (int i = 0; i < action.SameImg.Count; i++)
                        {
                            suc = ImageTool.findEqImg(action.SameImg[i], big);
                            if (suc.Success)
                            {
                                return suc;
                            }
                        }
                        Mouse.sleep(100);
                        if (Environment.TickCount > max)
                        {
                            return new HSearchPoint(false, Point.Empty, Size.Empty, action.RunWaitTime);
                        }
                    }

                }
            }
        }

        //private static HSearchPoint waitImg(Bitmap [] img, BaseAction action, string waitMsg = "等待中...", string sucMsg = "匹配成功!", bool isFind = true)
        //{
        //    int max = Environment.TickCount + action.RunWaitTime;
        //    while (true)
        //    {
        //        var suc = ImageTool.findEqImg(img, action.getScreenBmp());
        //        if (suc.Success == isFind)
        //        {
        //            Console.WriteLine(sucMsg);
        //            return suc;
        //        }
        //        else
        //        {
        //            Console.WriteLine(waitMsg);
        //        }
        //        Mouse.sleep(100);
        //        if (Environment.TickCount > max)
        //        {
        //            return new HSearchPoint(false, Point.Empty, Size.Empty, action.RunWaitTime);
        //        }
        //    }
        //}


        public delegate bool isSucFn();



        public static bool waitDialogFontSingle(MainGame game, NpcData ndata, bool clickName = true)
        {
            for (int i = 0; i < 3; i++)
            {

                using (Bitmap bmpt = game.getImg())
                {
                    var hs = ImageTool.findSigleImg(ndata.NPCPoint, bmpt, Color.FromArgb(255, 255, 138, 0));
                    if (hs.Success)
                    {
                        if (clickName)
                            game.MouseRigthDBClick(new Point(hs.Point.X + 30, hs.Point.Y + 50));
                        return true;
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                }
            }
            return false;
        }

        public static HSearchPoint waitDialogFontImg(MainGame game, Bitmap bmp, bool clickCenter = false)
        {
            for (int i = 0; i < 3; i++)
            {

                using (Bitmap bmpt = game.getImg())
                {
                    var hs = ImageTool.findLikeImg(bmp, bmpt);
                    if (hs.Success)
                    {
                        if (clickCenter)
                        {
                            game.MouseClick(new Point(hs.CenterPoint.X, hs.CenterPoint.Y));
                        }
                        return hs;
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }
            return new HSearchPoint(false, Point.Empty, Size.Empty, 0);
        }

        public static bool waitSucFn(isSucFn fn, int waitTime)
        {
            System.Threading.AutoResetEvent ae = new System.Threading.AutoResetEvent(false);
            bool suc = false;
            string key = WSTools.WSThread.WsThread.PoolRoundRun(() =>
            {
                if (suc)
                {
                    return;
                }
                suc = fn();
                if (suc)
                {
                    ae.Set();
                }
            }, 50);
            if (waitTime == 0)
            {
                ae.WaitOne();
            }
            else
            {
                ae.WaitOne(waitTime);
            }
            WSTools.WSThread.WsThread.PoolRoundStop(key);
            return suc;
        }
    }
}
