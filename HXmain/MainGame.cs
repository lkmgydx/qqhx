using HFrameWork.SystemDll;
using HFrameWork.SystemInput;
using HXmain.HXAction;
using HXmain.HXInfo;
using HXmain.HXInfo.Map;
using HXmain.Properties;
using HXmain.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tool;
using WSTools.WSLog;

namespace HXmain
{
    public class MainGame : IDisposable
    {
        private static WSTools.WSHook.KeyboardHook key = new WSTools.WSHook.KeyboardHook();

        /// <summary>
        /// 技能1
        /// </summary>
        private static readonly Rectangle JiNeng = new Rectangle(205, 782, 12, 12);

        private static String JiNeng1Str = null;

        private bool _isUseSkill = false;
        /// <summary>
        /// 是否在使用技能
        /// </summary>
        public bool isUseSkill
        {
            get { return _isUseSkill; }
            private set
            {
                _isUseSkill = value;
            }
        }

        public delegate void onSelectChange(bool isSelect);
        public event onSelectChange onSelectChangeEvent;

        public bool RightClick { get; set; } = false;

        public LockUtil lockUtil { get; } = new LockUtil();
        /// <summary>
        /// 是否有选中(人或怪)
        /// </summary>
        public bool hasSelectSomeOne
        {
            get; set;
        }

        private string _SelectName = "";
        /// <summary>
        /// 当前选择名称
        /// </summary>
        public string SelectName
        {
            get { return _SelectName; }

            private set
            {
                if (value == _SelectName)
                {
                    return;
                }
                _SelectName = value;
                onSelectChangeEvent?.Invoke(hasSelectSomeOne);
            }
        }

        bool isRealKey = false;
        /// <summary>
        /// 刷新技能
        /// </summary>
        public void RefreshUseSkill()
        {
            if (isRealKey)
            {
                return;
            }
            bool onkey = AutoOneKey;
            sleep(3000);
            string afff = UUIDImageRec(JiNeng);
            if (afff == JiNeng1Str)
            {
                isRealKey = true;
            }
            JiNeng1Str = afff;
            AutoOneKey = onkey;
        }

        HasSomeToHit hasSomeToHit;

        SelectInfo selectInfo;
        CloseGameWins closeWin;
        public WuPingLanInfo WuPingLanInfo { get; private set; }
        public RunGamePath runGamePath { get; private set; }
        public AUTOTiLianWuPing aUTOTiLianWuPing { get; private set; }
        /// <summary>
        /// 站立检测
        /// </summary>
        public StandCheck StandCheck { get; private set; }

        public 界面显示 界面显示;
        /// <summary>
        /// 人物移动
        /// </summary>
        public MovePerson movePersion { get; private set; }

        /// <summary>
        /// 游戏状态信息
        /// </summary>
        public GameState GameState { get; private set; }
        /// <summary>
        /// 游戏动作
        /// </summary>
        public GameAction GameAction { get; private set; }

        /// <summary>
        /// 收到强制停止运行命令
        /// </summary>
        public event Event.Events.VoidEventHand onPowerStop;

        /// <summary>
        /// 是否有怪可以攻击
        /// </summary>
        public bool HasSomeToHit
        {
            get
            {
                return hasSomeToHit.HasHit();
            }
        }

        /// <summary>
        /// 强制停止正在运行的任务 
        /// </summary>
        public void PowerStop()
        {
            aUTOTiLianWuPing.StopAutoLian();//停止自动提练
            canGo = false;
            onPowerStop?.Invoke();
        }

        /// <summary>
        /// 文件目录
        /// </summary>
        public static string FILE_ROOT = "d:\\qqhximg\\";
        /// <summary>
        /// 验证码检查
        /// </summary>
        public bool YZMcheck { get; set; }

        public delegate void SignleEventHandler();
        /// <summary>
        /// 当有验证码时
        /// </summary>
        public event SignleEventHandler onYZMEvent;

        public event EventHandler onlog;

        public delegate void LocationChangeEvent(Point before, Point now);
        /// <summary>
        /// 位置改变事件
        /// </summary>
        public event LocationChangeEvent onLocationChange;

        private System.Timers.Timer locationFreshTimer = new System.Timers.Timer();


        public delegate void onPersonDieHandler(HSearchPoint hs, MainGame m);
        /// <summary>
        /// 当人物阵亡事件
        /// </summary>
        public event onPersonDieHandler onDie;

        /// <summary>
        /// 是否自动组队
        /// </summary>
        public bool AutoZuDui { get; set; } = false;



        AuToGet autoget;
        /// <summary>
        /// 自动拾取
        /// </summary>
        public bool AutoGet
        {
            get { return autoget.Enable; }
            set
            {
                autoget.Enable = value;
            }
        }

        internal void sleep(int v)
        {
            Mouse.sleep(v);
        }

        public void drawMouse()
        {
            Mouse.DrawMousePosition();
        }


        public bool AutoOneKey
        {
            get { return t_autoOneKey.Enabled; }
            set
            {
                t_autoOneKey.Enabled = value;
            }
        }

        private bool needSendOneKey = true;

        private void T_autoOneKey_Tick(object sender, EventArgs e)
        {
            if (!needSendOneKey)
            {
                return;
            }
            if (JiNeng1Str == null)
            {
                JiNeng1Str = "_";
                JiNeng1Str = UUIDImageRec(JiNeng);
            }
            else
            {
                isUseSkill = JiNeng1Str != UUIDImageRec(JiNeng);
            }
            try
            {
                //User32.SendMessage(MainHandler, 0x0100, 192, 1);

                SendKey(Keys.Oemtilde);
                KeyBoard.sleep(100);
                SendKey(Keys.D1);
                KeyBoard.sleep(100);
                SendKey(Keys.D2);

                //SendKey(Keys.Space);
            }
            catch (Exception ex)
            {
                Log.logError("send key Error!", ex);
            }

        }

        System.Windows.Forms.Timer t_autoOneKey;


        System.Windows.Forms.Timer t_Main;


        /// <summary>
        /// 主进程HANDLER
        /// </summary>
        public IntPtr MainHandler { get; private set; }
        /// <summary>
        /// 主进程
        /// </summary>
        public Process MainProcess { get; private set; }

        public int Width
        {
            get
            {
                if (sc != null)
                {
                    return sc.ImageSize.Width;
                }
                return 0;
            }
        }

        public int Height
        {
            get
            {
                if (sc != null)
                {
                    return sc.ImageSize.Height;
                }
                return 0;
            }
        }

        public int X
        {
            get
            {
                if (sc != null)
                {
                    return sc.HLeft;
                }
                return 0;
            }
        }

        public int Y
        {
            get
            {
                if (sc != null)
                {
                    return sc.HTop;
                }
                return 0;
            }
        }

        ScreenCapture sc;
        /// <summary>
        /// 内存地址坐标
        /// </summary>
        public int Point_Address { get; private set; }

        public void TiLianSome()
        {
            Task.Run(() =>
            {
                TiLian.TiLianSome(this);
            });
        }

        /// <summary>
        /// 地图名称
        /// </summary>
        public string MapNameMemery { get { return getString(0x0D331568); } }

        /// <summary>
        /// 地图名称
        /// </summary>
        public string MapName { get { return CurrentMap.Name; } }

        /// <summary>
        /// 地图base64字符串
        /// </summary>
        public string MapNameBase64
        {
            get
            {
                return ImageTool.getImgBase64(sc.getImg(MapBase.MapRectangle));
            }
        }

        /// <summary>
        /// 当前地图
        /// </summary>
        public MapBase CurrentMap
        {
            get;
            private set;
        }

        /// <summary>
        /// 当前坐标
        /// </summary>
        public Point CurrentLocation
        {
            get; private set;
        }

        public Point getCurrentLocation()
        {
            return new Point(MemoeryOpt.ReadMemoryValue(Point_Address, MainProcess.Id), MemoeryOpt.ReadMemoryValue(Point_Address - 4, MainProcess.Id));
        }


        private void T_Main_Tick(object sender, EventArgs e)
        {
            t_Main.Enabled = false;
            if (MainProcess.HasExited)
            {
                Environment.Exit(0);
            }
            try
            {
                realMainDo();
            }
            catch (Exception ex)
            {
                Log.logError("定时主程序异常!", ex);
            }
            finally
            {
                t_Main.Enabled = true;
            }
        }

        private long lastClick = 0;

        private void realMainDo()
        {
            using (Bitmap Bitmap = sc.getImg(MapBase.MapRectangle))
            {
                this.CurrentMap = MapBase.getCurrentMap(Bitmap);
            }

            if (RightClick && Environment.TickCount - lastClick > 1000 * 7)
            {
                Mouse.cacheLocation();
                Active();
                MouseMove(400 + 134, 300 + 108);
                SendKey(Keys.R);
                Mouse.rightclick();
                Mouse.reventLocation();
                lastClick = Environment.TickCount;
            }

            hasSelectSomeOne = selectInfo.hasSelect();
            SelectName = selectInfo.SelectName;
            if (AutoZuDui && GameState.hasZuDuiinfo())
            {
                SendKey(Keys.F);
                SendKey(Keys.Space);
            }
            if (YZMcheck && GameState.hasYZM())
            {
                EmailTool.SendToQQ("有验证码需要处理!", "验证信息");
                onYZMEvent?.Invoke();
            }
            using (Bitmap bmp = sc.HImage)
            {
                hasDie = false;
                if (dieCheck)
                {
                    HSearchPoint hs = HXDieAutoLive.hasDie(bmp);
                    if (hs.Success)
                    {
                        hasDie = true;
                        if (DieAutoLive)
                        {
                            Mouse.cacheLocation();
                            pointClick(hs.CenterPoint);
                            Mouse.reventLocation();
                        }
                        onDie?.Invoke(hs, this);
                    }
                }
            }
        }


        public void Active()
        {
            User32.ShowWindow(MainHandler, 1);
            User32.SetForegroundWindow(MainHandler);
        }

        public void DrawPointToPoint(Point start, Point end)
        {
            Mouse.cacheLocation();
            MouseMove(start);
            Mouse.leftdown();
            MouseMove(end);
            Mouse.leftup();
            Mouse.reventLocation();
            sleep(100);
        }
        public Bitmap getImg(Rectangle rec, bool move = true)
        {
            if (move)
            {
                //Mouse.cacheLocation();
                //MouseMove(1022, 785);
                try
                {
                    return sc.getImg(rec);
                }
                catch { }
                finally
                {
                    // Mouse.reventLocation();
                }
                return new Bitmap(1, 1);
            }
            else
            {
                try
                {
                    return sc.getImg(rec);
                }
                catch { }
                return new Bitmap(1, 1);
            }
        }

        /// <summary>
        /// 获取游戏指定区域UUID
        /// </summary>
        /// <param name="rec"></param>
        /// <returns></returns>
        public string UUIDImageRec(Rectangle rec)
        {
            using (Bitmap bmp = getImg(rec, false))
            {
                return ImageTool.UUIDImg(bmp);
            }
        }

        public Bitmap getImg()
        {
            //Mouse.cacheLocation();
            Mouse.HideMouse();
            //MouseMove(1022 + 45, 785 + 25);
            //Mouse.sleep(3000);
            try
            {
                return sc.getImg(new Rectangle(Point.Empty, sc.ImageSize));
            }
            catch
            {
            }
            finally
            {
                //Mouse.reventLocation();
                Mouse.ShowMouse();
            }
            return new Bitmap(1, 1);
        }

        /// <summary>
        /// 人物阵亡检测
        /// </summary>
        public bool dieCheck { get; set; }
        /// <summary>
        /// 人物是否已经阵亡
        /// </summary>
        public bool hasDie { get; private set; }
        /// <summary>
        /// 阵亡后自动复活
        /// </summary>
        public bool DieAutoLive { get; set; }

        public string savePic_MapStr()
        {
            using (Bitmap pic = sc.getImg(MapBase.MapRectangle))
            {
                string id = ImageTool.UUIDImg(pic);
                string path = FILE_ROOT + "dt_" + Environment.TickCount + "_" + id + ".bmp";
                pic.Save(path, System.Drawing.Imaging.ImageFormat.Bmp);
                return path;
            }
        }

        public string getString(int address)
        {
            string rst = MemoeryOpt.ReadMemoryValue(address, 64, MainProcess.Id);
            return rst;
        }

        public string getString(int address, int length)
        {
            return getString(address, length);
        }

        public void AutoRun(bool isRevernt = false)
        {
            Task.Run(() =>
            {
                AutoRunGame.Run(this, isRevernt);
            });
        }

        private static List<MainGame> mainGames = new List<MainGame>();

        public void closeAllGameWin()
        {
            closeWin.closeAll();
        }

        public MainGame(Process process)
        {
            if (process.ProcessName.ToUpper() != "QQHXGAME")
            {
                //  throw new Exception("请输入QQ华夏进程");
            }
            MainProcess = process;
            MainHandler = process.MainWindowHandle;
            sc = new ScreenCapture(MainHandler);
            StandCheck = new StandCheck(this);
            StandCheck.PostionNotMoveEventHander += StandCheck_PostionNotMoveEventHander;

            hasSomeToHit = new HasSomeToHit(this);
            runGamePath = new RunGamePath(this);
            WuPingLanInfo = new WuPingLanInfo(this);
            界面显示 = new 界面显示(this);
            selectInfo = new SelectInfo(this);
            aUTOTiLianWuPing = new AUTOTiLianWuPing(this);
            closeWin = new CloseGameWins(this);

            var os = Environment.OSVersion;
            if (os.Version.ToString() == "6.2.9200.0")
            {
                Point_Address = 0x00192F54;
                Point_Address = 0x00192F4C;
            }
            CurrentLocation = getCurrentLocation();

            if (CurrentLocation.X == 0)
            {
                Point_Address = 0x00122F6C;
                Point_Address = 0x00122F64;
                CurrentLocation = getCurrentLocation();
                if (CurrentLocation.X == 0)
                {
                    Point_Address = 0x00182F6C;
                    Point_Address = 0x00182F64;
                    CurrentLocation = getCurrentLocation();
                }
            }


            t_autoOneKey = new System.Windows.Forms.Timer();
            t_autoOneKey.Interval = 1000;
            t_autoOneKey.Tick += T_autoOneKey_Tick;


            t_Main = new System.Windows.Forms.Timer();
            t_Main.Tick += T_Main_Tick;
            t_Main.Start();//主流程检测，每2秒执行一次检测任务

            locationFreshTimer.Interval = 200;
            locationFreshTimer.Elapsed += locationFreshTimer_Elapsed;
            locationFreshTimer.Start();

            GameState = new GameState(this);
            GameAction = new GameAction(this);
            movePersion = new MovePerson(this);
            autoget = new AuToGet(this);
            AutoGet = false;//开启自动拾取

            mainGames.Add(this);//所有游戏进程列表

            CurrentMap = MapBase.STATIC_BASE_MAP;

            //string rst=  MemoeryOpt.ReadMemoryValueStr(0x9F223A8, MainProcess.Id, 555);
            // MemoeryOpt.ReadMemoryValueStr(0x09F223A8, MainProcess.Id);
        }

        private void MainProcess_Exited(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void StandCheck_PostionNotMoveEventHander(Point postion)
        {
            Log.logError("RadomMove wait getLock....");
            lockUtil.getTopLock();
            Log.logError("V");
            Point pnow = CurrentLocation;
            Point pnex = pnow;
            while (pnow.X == pnex.X && pnow.Y == pnex.Y)
            {
                Log.logError("RadomMove moving....");
                movePersion.RadomMove();
                sleep(500);
                pnex = CurrentLocation;
            }
            lockUtil.ReleaseTopLock();
            Log.logError("RadomMove【ReleaseLock】");
        }

        private void locationFreshTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                Point before = CurrentLocation;
                Point pt = getCurrentLocation();
                if (pt.X <= 0 || pt.Y <= 0 || pt.X > 600 || pt.Y > 600)
                {
                    return;
                }
                if (onLocationChange != null)
                {
                    if (pt.X != CurrentLocation.X || pt.Y != CurrentLocation.Y)
                    {
                        // Console.WriteLine(before + "->" + pt);
                        CurrentLocation = pt;
                        Task.Run(() =>
                        {
                            onLocationChange(before, CurrentLocation);
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Log.logError("位置处理异常!", ex);
            }
        }

        public string getCurrentSelectName()
        {
            return selectInfo.SelectName;
            // return MemoeryOpt.ReadMemoryValue(Point_Address + 0x235B6508, 32, MainProcess.Id);
        }

        System.Threading.Thread run = null;

        /// <summary>
        /// 循环执行指定路径，默认为true
        /// </summary>
        public bool isXUNHUAN { get; set; } = true;

        /// <summary>
        /// 是否正在运行
        /// </summary>
        public bool isRun { get { return canGo; } }

        private bool canGo = false;


        /// <summary>
        /// 是否有选择目标区域
        /// </summary>
        private static Rectangle _SELECT_RECTANGLE = new Rectangle(300, 87, 6, 5);

        public bool hasSelectSome()
        {
            using (Bitmap bmp = sc.getImg(_SELECT_RECTANGLE))
            {
                {
                    for (int i = 0; i < bmp.Width; i++)
                    {
                        for (int j = 0; j < bmp.Height; j++)
                        {
                            Color c = bmp.GetPixel(i, j);
                            if (c.R != 0 || c.G != 0 || c.B != 0)
                            {
                                Log.log("-----------未选择攻击目标");
                                return false;
                            }
                        }
                    }
                }
                Log.log("++++++++++已选择攻击目标");
                return true;
            }
        }


        public event EventHandler onEndRun;


        public void runPath(Point[] pt, Action fn = null)
        {
            runGamePath.Run(pt, fn);
        }

        //private void runPoint(Point pt, Point next)
        //{
        //    if (!canGo)
        //    {
        //        return;
        //    }
        //    _runPoint = pt;
        //    CurrentMovePoint = pt;
        //    try
        //    {
        //        movePersion.MovePoint(pt, next);
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    Log.log("===========SUC===============" + pt.X + ":" + pt.Y);
        //}


        Point CurrentMovePoint = Point.Empty;


        public void MovePoint(Point p)
        {
            movePersion.MovePoint(p, Point.Empty);
        }

        public void MovePoint(Point p, Point[] list, Action action)
        {
            if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
            {
                new Thread(() =>
                {
                    movePersion.MovePoint(p, list, action);
                }).Start();
            }
            else
            {
                movePersion.MovePoint(p, list, action);
            }
        }

        /// <summary>
        /// 人物打坐
        /// </summary>
        public void SetDown()
        {
            PeronSetDown.SetDown(this);
        }
        /// <summary>
        /// 人物居中位置
        /// </summary>
        public Point CenterPersonPoint = new Point(545, 425);

        public void pointClick(Point p)
        {
            pointClick(p.X, p.Y);
        }

        public void pointClick(int x, int y, bool needEnd = true)
        {
            MouseMove(x, y);
            Mouse.leftclick();
        }

        public void pointRightClick(int x, int y)
        {
            MouseMove(x, y);
            Mouse.rightclick();
        }

        /// <summary>
        /// 相对窗口移动
        /// </summary>
        /// <param name="p"></param>
        public void MouseMove(Point p)
        {
            MouseMove(p.X, p.Y);
        }

        public void MouseMove(int x, int y)
        {
            int fx = sc.HLeft;
            int fy = sc.HTop;
            Mouse.move(fx + x, fy + y);
        }
        /// <summary>
        /// 移动后点击
        /// </summary>
        /// <param name="p"></param>
        public void MouseClick(Point p)
        {
            MouseClick(p.X, p.Y);
        }

        public void MouseClick(int x, int y)
        {
            Mouse.cacheLocation();
            MouseMove(x, y);
            Mouse.leftclick();
            Mouse.sleep(200);
            Mouse.reventLocation();
        }

        public void MouseClick()
        {
            Mouse.leftclick();
        }

        public void MouseRigthClick()
        {
            Mouse.rightclick();
        }

        public void MouseRigthClick(Point p)
        {
            MouseMove(p);
            Mouse.sleep(100);
            Mouse.rightclick();
        }

        public void MouseRigthDBClick(int x, int y)
        {
            Mouse.cacheLocation();
            MouseMove(x, y);
            Mouse.rightDbclick();
            Mouse.sleep(200);
            Mouse.reventLocation();
        }

        public void MouseRigthDBClick(Point p)
        {
            MouseRigthDBClick(p.X, p.Y);
        }

        public void MouseRigthClick(int x, int y)
        {
            MouseMove(x, y);
            Mouse.rightclick();
            // System.Windows.Forms.SendKeys.Send()
        }

        public void SendKey(Keys key)
        {
            //System.Windows.Forms.SendKeys.Send(key.ToString());
            //System.Windows.Forms.SendKeys.Send
            //User32.SetForegroundWindow(MainHandler);
            User32.SendMessage(MainHandler, 0x0100, (int)key, 1);
            Mouse.sleep(100);
            User32.SendMessage(MainHandler, 0x0000, (int)key, 1);
        }

        //public void SendKeyDown(Keys key)
        //{
        //    User32.SendMessage(MainHandler, 0x0100, (int)key, 1);
        //}

        //public void SendKeyUp(Keys key)
        //{
        //    User32.SendMessage(MainHandler, 0x0000, (int)key, 1);
        //}


        public void Dispose()
        {
            if (run != null)
            {
                try
                {
                    run.Abort();
                    if (onEndRun != null)
                        onEndRun.Invoke(null, null);
                }
                catch (Exception ex)
                {
                    Log.logError("close error", ex);
                }
            }
        }

        /// <summary>
        /// 传入的bmp将自动释放
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public HSearchPoint clickImg(Bitmap bmp)
        {
            return clickImg(bmp, Point.Empty);
        }

        /// <summary>
        /// 传入的bmp将自动释放
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public HSearchPoint clickImg(Bitmap bmp, Point fl)
        {
            return clickImg(bmp, fl.X, fl.Y, true);
        }


        /// <summary>
        /// 传入的bmp将自动释放
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public HSearchPoint clickImg(Bitmap bmp, Point fl, bool isCenter = true)
        {
            return clickImg(bmp, fl.X, fl.Y, isCenter);
        }

        /// <summary>
        /// 传入的bmp将自动释放
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public HSearchPoint clickImg(Bitmap bmp, int x = 0, int y = 0, bool isCenter = true)
        {
            using (bmp)
            {
                HSearchPoint hs = findImg(bmp);
                Point p = isCenter ? hs.CenterPoint : hs.Point;
                if (hs.Success)
                {
                    MouseClick(p.X + x, p.Y + y);
                }
                return hs;
            }
        }

        public HSearchPoint findImg(Bitmap bmp)
        {
            using (Bitmap bit = getImg())
            {
                return ImageTool.findLikeImg(bmp, bit);
            }
        }

        /// <summary>
        /// 查找所有相同图片
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public List<HSearchPoint> findImgAll(Bitmap bmp)
        {
            using (Bitmap bit = getImg())
            {
                return ImageTool.findEqImgAll(bmp, bit);
            }
        }

        public Point ToClientPoint(Point start)
        {
            int x = start.X - sc.HLeft;
            int y = start.Y - sc.HTop;
            if (x < 0)
            {
                x = 0;
            }
            if (y < 0)
            {
                y = 0;
            }
            if (x > sc.HRight)
            {
                x = sc.HRight;
            }
            if (y > sc.HBottom)
            {
                y = sc.HBottom;
            }
            return new Point(x, y);
        }

        public Point ToScreenPoint(Point start)
        {

            int x = start.X + sc.HLeft;
            log("x:" + x);
            int y = start.Y + sc.HTop;
            log("y:" + y);

            return new Point(x, y);
        }

        private static void log(string msg)
        {
            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss") + " - " + msg);
        }

        public void AUTOTiLianWuPing()
        {
            aUTOTiLianWuPing.StartAutoTiLian();
        }
    }
}
