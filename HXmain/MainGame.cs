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
using WSTools.WSThread;

namespace HXmain
{
    public class MainGame : IDisposable
    {

        public static Image rrbb = Resources.font_华夏帮帮;


        /// <summary>
        /// 当前角色名称
        /// </summary>
        private static readonly Rectangle NameRec = new Rectangle(120, 86, 100, 14);
        /// <summary>
        /// 选中对象整个区域
        /// </summary>
        private static readonly Rectangle SelectSlefRec = new Rectangle(305, 85, 95, 12);
        /// <summary>
        /// 选中对象血量
        /// </summary>
        private static readonly Rectangle SelectBlood = new Rectangle(300, 300, 100, 4);
        /// <summary>
        /// 是否有选择目标区域
        /// </summary>
        private static Rectangle _SELECT_RECTANGLE = new Rectangle(300, 87, 6, 5);

        /// <summary>
        /// 运行锁定
        /// </summary>
        public LockTopRun LockTopRun { get; private set; } = new LockTopRun();

        private string JiNeng1Str = null;

        private bool _isUseSkill = false;

        #region 事件

        /// <summary>
        /// 地图改变
        /// </summary>
        /// <param name="mapBefor"></param>
        /// <param name="mapAfter"></param>
        public delegate void onMapChangeEventHandler(MapBase mapBefor, MapBase mapAfter);
        /// <summary>
        /// 地图切换事件
        /// </summary>
        public event onMapChangeEventHandler onMapChangeEvent;

        #endregion

        /// <summary>
        /// 是否在使用技能
        /// </summary>
        public bool isUseSkill { get; private set; }

        /// <summary>
        /// 自动打坐
        /// </summary>
        public bool AutoSetDown { get; set; }


        public delegate void onSelectChange(bool isSelect);
        public event onSelectChange onSelectChangeEvent;

        public bool RightClick { get; set; } = false;

        public LockUtil lockUtil { get; } = new LockUtil();
        /// <summary>
        /// 是否有选中(人或怪)
        /// </summary>
        public bool hasSelectSomeOne
        {
            get; private set;
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
                if (_SelectName == value)
                {
                    return;
                }
                _SelectName = value;
                onSelectChangeEvent?.Invoke(hasSelectSomeOne);
            }
        }


        private string _Name;
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name
        {
            get
            {
                if (_Name == null)
                {
                    _Name = getOrcRange(NameRec);
                }
                return _Name;
            }
            private set
            {
                _Name = value;
            }
        }


        private HomeGetSome homeGetSome;
        public SelectInfo selectInfo { get; private set; }
        public CloseGameWins CloseWin { get; private set; }
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
        /// ESC停止任务
        /// </summary>
        public bool ESCSTOP { get; set; }
        /// <summary>
        /// 强制停止正在运行的任务 
        /// </summary>
        public void PowerStop()
        {
            ESCSTOP = true;
            try
            {
                aUTOTiLianWuPing.StopAutoLian();//停止自动提练
                canGo = false;
            }
            catch (Exception ex)
            {
                Log.logErrorForce("停止提练失败", ex);
            }
            try
            {
                Log.logForce("强制停止事件...");
                onPowerStop?.Invoke();
            }
            catch (Exception ex)
            {
                Log.logErrorForce("停止异常", ex);
            }
            WSTools.WSThread.WsThread.Run(() =>
            {
                Thread.Sleep(5000);
                ESCSTOP = true;
            });
        }

        /// <summary>
        /// 文件目录
        /// </summary>
        public static string FILE_ROOT = "d:\\qqhximg\\";
        /// <summary>
        /// 技能使用
        /// </summary>
        public event WSTools.WsEvent.WsBoolEvent onUseSkillEvent;
        /// <summary>
        /// 验证码检查
        /// </summary>
        public bool YZMcheck { get; set; }

        public delegate void SignleEventHandler();
        /// <summary>
        /// 当有验证码时
        /// </summary>
        public event SignleEventHandler onYZMEvent;

        public delegate void LocationChangeEvent(Point before, Point now);
        /// <summary>
        /// 位置改变事件
        /// </summary>
        public event LocationChangeEvent onLocationChange;

        /// <summary>
        /// 安全区变化时触发
        /// </summary>
        public event WSTools.WsEvent.WsBoolEvent onSafeRegionChange;
        /// <summary>
        /// 位置获取定时器
        /// </summary>
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

        public Graphics getGriphs()
        {
            return Graphics.FromHwnd(MainHandler);
        }


        public bool AutoOneKey
        {
            get { return needSendOneKey; }
            set
            {
                needSendOneKey = value;
                t_AutoKey.Enabled = value;
                if (value)
                {
                    if (string.IsNullOrEmpty(Name))
                    {
                        Name = getOrcRange(NameRec);
                    }
                    ESCSTOP = false;
                    sw.Start();

                }
                else
                {
                }
            }
        }

        private volatile bool needSendOneKey = false;

        /// <summary>
        /// FB模式
        /// </summary>
        /// 
        public bool isFbMode { get; set; }

        public bool isFbMode4 { get; set; }

        public bool isFbMode1 { get; set; }

        public bool isFbMode2 { get; set; }

        public bool isFbMode3 { get; set; }


        private string _XL;

        System.Windows.Forms.Timer t_Main;
        System.Timers.Timer t_AutoKey;

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
            WsThread.Run(() =>
            {
                LockTopRun.Run(() =>
                {
                    TiLian.TiLianSome(this);
                });
            });
        }

        public void TopRun(Action t, bool isSyn = false)
        {
            if (isSyn)
            {
                LockTopRun.Run(t);
            }
            else
            {
                WsThread.Run(() =>
                {
                    LockTopRun.Run(t);
                });
            }
        }

        public void SubRun(Action t, bool isSyn = false)
        {
            if (isSyn)
            {
                LockTopRun.Run(t);
            }
            else
            {
                WsThread.Run(() =>
                {
                    LockTopRun.Run(t);
                });
            }
        }

        /// <summary>
        /// 地图名称
        /// </summary>
        public string MapName { get { return CurrentMap.Name; } }

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
                return;
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
                if (Bitmap.Width != MapBase.MapRectangle.Width)
                {
                    Log.logWarnForce("???异常 " + Bitmap.Width + "____" + MapBase.MapRectangle.Width);
                    //获取图片异常
                    return;
                }
                var now_map = MapBase.getCurrentMap(Bitmap);

                if (CurrentMap != now_map)
                {
                    var before_map = CurrentMap;
                    CurrentMap = now_map;
                    onMapChangeEvent?.Invoke(before_map, now_map);
                }
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
            if (hasSelectSomeOne)
            {
                string xl = UUIDImageRec(SelectBlood);
                if (_XL != xl)
                {
                    Log.log("当前血量:" + xl);
                }
                _XL = xl;
            }

            bool us = GameState.hasUseSkill();
            if (us != isUseSkill)
            {
                isUseSkill = us;
                onUseSkillEvent.Invoke(us);
            }

            SelectName = selectInfo.SelectName;

            if (AutoZuDui && GameState.hasZuDuiinfo())
            {
                SendKey(Keys.F);
                sleep(100);
                SendKey(Keys.Space);
                sleep(100);
                SendKey(Keys.Space);
            }
            if (YZMcheck && GameState.hasYZM())
            {
                EmailTool.SendToQQ("[" + MapName + "]有验证码需要处理!", "验证信息");
                onYZMEvent?.Invoke();
            }
            if (AutoSetDown && !PeronSetDown.isSetdown(this))
            {
                PeronSetDown.SetDown(this);
            }

            if (dieCheck)
            {
                hasDie = false;
                using (Bitmap bmp = sc.HImage)
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
        public Bitmap getImg(Rectangle rec)
        {
            try
            {
                return sc.getImg(rec);
            }
            catch { }
            return new Bitmap(1, 1);
        }

        /// <summary>
        /// 获取游戏指定区域UUID
        /// </summary>
        /// <param name="rec"></param>
        /// <returns></returns>
        public string UUIDImageRec(Rectangle rec)
        {
            using (Bitmap bmp = getImg(rec))
            {
                return ImageTool.UUIDImg(bmp);
            }
        }

        public string UUIDImageRec(Bitmap img)
        {
            return ImageTool.UUIDImg(img);
        }

        public Bitmap getImg()
        {
            //Mouse.cacheLocation();
            //Mouse.HideMouse();
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
                //Mouse.ShowMouse();
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
            WsThread.Run(() =>
            {
                AutoRunGame.Run(this, isRevernt);
            });
        }

        private static List<MainGame> mainGames = new List<MainGame>();

        public void closeAllGameWin()
        {
            CloseWin.closeAll();
        }

        private static readonly Rectangle rec_Tip = new Rectangle(380, 580, 280, 90);
        public string TipInfo()
        {
            return getOrcRange(rec_Tip, Color.Yellow);
        }

        /// <summary>
        /// 等待拾取
        /// </summary>
        public void WaitGet()
        {
            homeGetSome.WaitGet();
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
            sc.onError += Sc_onError;
            StandCheck = new StandCheck(this);
            StandCheck.PostionNotMoveEventHander += StandCheck_PostionNotMoveEventHander;


            runGamePath = new RunGamePath(this);
            WuPingLanInfo = new WuPingLanInfo(this);
            界面显示 = new 界面显示(this);
            selectInfo = new SelectInfo(this);
            aUTOTiLianWuPing = new AUTOTiLianWuPing(this);
            CloseWin = new CloseGameWins(this);
            homeGetSome = new HomeGetSome(this);

            var os = Environment.OSVersion;
            if (os.Version.ToString() == "6.2.9200.0")
            {
                Point_Address = 0x00192F54;
                Point_Address = 0x00192F4C;
                CurrentLocation = getCurrentLocation();
            }
            else
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

            t_Main = new System.Windows.Forms.Timer();
            t_Main.Tick += T_Main_Tick;
            t_Main.Start();//主流程检测，每2秒执行一次检测任务

            t_AutoKey = new System.Timers.Timer();
            t_AutoKey.Elapsed += T_AutoKey_Tick;
            t_AutoKey.Interval = 400;

            locationFreshTimer.Interval = 400;
            locationFreshTimer.Elapsed += locationFreshTimer_Elapsed;
            locationFreshTimer.Start();

            GameState = new GameState(this);
            GameAction = new GameAction(this);
            movePersion = new MovePerson(this);
            autoget = new AuToGet(this);
            AutoGet = false;//开启自动拾取

            mainGames.Add(this);//所有游戏进程列表

            CurrentMap = MapBase.STATIC_BASE_MAP;

            sw_location.Start();
        }

        /// <summary>
        /// 攻击列表，对象的名称
        /// </summary>
        public string ActName { get; set; }

        Stopwatch sw = new Stopwatch();

        private void T_AutoKey_Tick(object sender, EventArgs e)
        {
            if (!needSendOneKey)
            {
                Log.log("skip...");
                return;
            }
            SendKey(Keys.Space);
            sleep(300);

            //如果选择自己，退出
            if (selectInfo.isSelectSelf)
            {
                selectInfo.unSelect();
                SendKey(Keys.Oemtilde);
                sleep(500);
                sw.Restart();
                return;
            }

            bool hasSelect = hasSelectSome();
            var secName = SelectName;
            var actName = ActName;

            if (!hasSelect || sw.ElapsedMilliseconds > 5000)
            {
                SendKey(Keys.Oemtilde);
                sleep(500);
                sw.Restart();
                return;
            }
            if (string.IsNullOrWhiteSpace(secName))//没有攻击对象，退出
            {
                return;
            }
            bool isNull = string.IsNullOrWhiteSpace(actName);//攻击过滤
            if (!isNull)
            {
                if (secName.IndexOf(actName) == -1)
                {
                    SendKey(Keys.Oemtilde);
                    sleep(500);
                    sw.Restart();
                    return;
                }
            }
            sendSub();
        }

        private void sendSub()
        {
            SendKey(Keys.D1);
            KeyBoard.sleep(200);
            SendKey(Keys.D2);
        }


        private void Sc_onError()
        {
            Log.logErrorForce("!!!pid change....");
            Process p = Process.GetProcessById(MainProcess.Id);
            MainHandler = p.MainWindowHandle;
            sc.Handle = MainHandler;
            Log.logErrorForce("!!!pid changeed....");
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

        /// <summary>
        /// 是否为安全区
        /// </summary>
        public bool IsSafeRegion { get; private set; }

        private object lockLoationChangeObj = new object();


        Stopwatch sw_location = new Stopwatch();
        private void locationFreshTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (lockLoationChangeObj)
            {
                try
                {
                    Point beforePoint = CurrentLocation;
                    Point afterPoint = getCurrentLocation();
                    if (afterPoint.X <= 0 || afterPoint.Y <= 0 || afterPoint.X > 600 || afterPoint.Y > 600)
                    {
                        return;
                    }
                    if (afterPoint.Y == 10)
                    {
                        return;
                    }
                    CurrentLocation = afterPoint;
                    var isLocationChanged = !afterPoint.Equals(beforePoint);

                    var isSafeBefor = IsSafeRegion;
                    var isSafeAfter = IsSafeRegion = CurrentMap.isSafeRegion(this);

                    var isSafeChange = (isSafeBefor != isSafeAfter);


                    if (onLocationChange != null && (sw_location.ElapsedMilliseconds>5000|| isLocationChanged))
                    {
                        sw_location.Restart();
                        //Log.logForce("[" + IsSafeRegion + "] - location before  " + beforePoint + " -> " + afterPoint);
                        try
                        {
                            onLocationChange(beforePoint, afterPoint);
                        }
                        catch (Exception ex)
                        {
                            Log.logErrorForce("onLocationChange异常..", ex);
                        }
                    }

                    if (isSafeChange && onSafeRegionChange != null)
                    {
                        //Log.logForce("invoke safe..." + IsSafeRegion);
                        onSafeRegionChange(IsSafeRegion);
                    }
                }
                catch (Exception ex)
                {
                    Log.logErrorForce("位置处理异常!", ex);
                }
            }
        }

        public string getCurrentSelectName()
        {
            return selectInfo.SelectName;
        }

        public string getOrcRange(Rectangle rec)
        {
            using (var img = getImg(rec))
            {
                return ImageCacheTool.getOrcText(img);
            }
        }

        /// <summary>
        /// 抽取指定颜色信息识别
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="filterColor"></param>
        /// <returns></returns>
        public string getOrcRange(Rectangle rec, Color filterColor)
        {
            using (var img = getImg(rec))
            {
                using (var t = ImageTool.filterBmp(img, filterColor))
                {
                    return ImageCacheTool.getOrcText(t);
                }
            }
        }
         

        /// <summary>
        /// 循环执行指定路径，默认为true
        /// </summary>
        public bool isXUNHUAN { get; set; } = true;

        /// <summary>
        /// 是否正在运行
        /// </summary>
        public bool isRun { get { return canGo; } }

        private bool canGo = false;



        private static readonly Point CancelSelectPoint = new Point(1053, 41);

        private object lockSelect = new object();
        public void CancelSelect()
        {
            lock (lockSelect)
            {
                Mouse.cacheLocation();
                MouseMove(CancelSelectPoint);
                Mouse.rightclick();
                Mouse.reventLocation();
            }
        }

        public bool SlectNextCanHit()
        {
            lock (lockSelect)
            {
                SendKey(Keys.Oemtilde);
                return hasSelectSome();
            }
        }

        public bool hasSelectSome()
        {
            return selectInfo.hasSelect();
        }


        public void runPath(Point[] pt, Action fn = null)
        {
            runGamePath.Run(pt, fn);
        }

        /// <summary>
        /// 获取资源缓存路径
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Point[] getResoucePath(string name)
        {
            return PathPointUtil.getResourcePoint(name);
        }

        public void runPath(string arName, bool xh = false, bool revert = false, Action fn = null)
        {
            runGamePath.Run(PathPointUtil.getResourcePoint(arName + ".txt"), fn);
        }

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
            //Mouse.cacheLocation();
            //User32.SendMessage(MainHandler, 0x201, 1, (y << 16) | x);
            //User32.SendMessage(MainHandler, 0x202, 1, (y << 16) | x);
            //Mouse.reventLocation();
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

        public void GotoBoss(Action action)
        {
            CurrentMap.GotoBoss(this, action);
        }

        public void MouseMove(int x, int y)
        {
            int fx = sc.HLeft;
            int fy = sc.HTop;
            Mouse.move(fx + x, fy + y);
        }
        /// <summary>
        /// 移动后点击,位置会自动复位
        /// </summary>
        /// <param name="p"></param>
        public void MouseClick(Point p)
        {
            MouseClick(p.X, p.Y);
        }

        /// <summary>
        /// 移动后点击,位置会自动复位
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MouseClick(int x, int y)
        {
            Mouse.cacheLocation();
            MouseMove(x, y);
            MouseClick();
            Mouse.reventLocation();
        }

        public void MouseDbClick(Point p)
        {
            MouseDbClick(p.X, p.Y);
        }

        public void MouseDbClick(int x, int y)
        {
            Point p = Mouse.Location();
            MouseClick_Real(p.X, p.Y);
            MouseClick_Real(p.X, p.Y);
        }

        public void MouseClick()
        {
            Point p = Mouse.Location();
            MouseClick_Real(p.X, p.Y);
        }

        private void MouseClick_Real(int x, int y)
        {
            Mouse.leftclick();
            sleep(100);
            //User32.SendMessage(MainHandler, 0x201, 1, (y << 16) | x);
            //sleep(10);
            //User32.SendMessage(MainHandler, 0x202, 1, (y << 16) | x);
        }

        private void MouseRightClick_Real(int x, int y)
        {
            //User32.SendMessage(MainHandler, 0x204, 1, (y << 16) | x);
            //User32.SendMessage(MainHandler, 0x205, 1, (y << 16) | x);
        }

        public void MouseRigthClick()
        {
            Mouse.rightclick();
            sleep(100);
            //MouseRightClick_Real(afterPoint.X, afterPoint.Y);
        }

        public void MouseRigthClick(Point p)
        {
            Mouse.cacheLocation();
            MouseMove(p);
            MouseRigthClick();
            Mouse.reventLocation();
        }

        public void MouseRigthDBClick(int x, int y)
        {
            Mouse.cacheLocation();
            MouseMove(x, y);
            //Mouse.rightDbclick();
            MouseRigthClick();
            MouseRigthClick();
            Thread.Sleep(200);
            MouseRigthClick();
            MouseRigthClick();
            //Mouse.rightDbclick();
            Thread.Sleep(200);
            Mouse.reventLocation();
        }

        public void MouseRigthClick(int x, int y)
        {
            Mouse.cacheLocation();
            MouseMove(x, y);
            Mouse.rightclick();
            Thread.Sleep(200);
            Mouse.reventLocation();
            //Mouse.rightclick();
            // System.Windows.Forms.SendKeys.Send()
        }

        public void SendKey(Keys key)
        {
            //System.Windows.Forms.SendKeys.Send(key.ToString());
            //System.Windows.Forms.SendKeys.Send
            //User32.SetForegroundWindow(MainHandler);

            //User32.SetForegroundWindow(MainHandler);
            //KeyBoard.down_up(key);

            User32.PostMessage(MainHandler, 0x0100, (int)key, 0x20001);
            Thread.Sleep(1);
            User32.PostMessage(MainHandler, 0x0101, (int)key, 0x20001);

            //SEND 卡
            //User32.SendMessage(MainHandler, 0x0100, (int)key, 0x20001);
            ////User32.PostMessage(MainHandler, 0x0102, (int)key, 0x20001);
            //Thread.Sleep(1);
            //User32.SendMessage(MainHandler, 0x0101, (int)key, 0xC0020001);
            Thread.Sleep(1);
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
            PowerStop();
        }

        /// <summary>
        /// 传入的bmp将自动释放
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public HSearchPoint clickImg(Bitmap bmp, int x = 0, int y = 0, bool waitSuc = false, int waitTime = 3000, int f = 0)
        {
            return clickImg(bmp, x, y, waitSuc, waitTime);
        }

        /// <summary>
        /// 点击等待图片
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="waitTime">3秒</param>
        /// <returns></returns>
        public HSearchPoint clickWaitImg(Bitmap bmp, int x = 0, int y = 0, int waitTime = 3000)
        {
            long start = Environment.TickCount;
            HSearchPoint hs = null;
            using (bmp)
            {
                do
                {
                    hs = findImg(bmp);
                    if (hs.Success)
                    {
                        Point p = hs.Point;
                        p.Offset(x, y);
                        MouseClick(p);
                        break;
                    }
                } while (Environment.TickCount - waitTime > 0);
            }
            return hs;
        }

        /// <summary>
        /// 传入的bmp将自动释放
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public HSearchPoint clickImg(Bitmap bmp, Point fl, bool waitSuc = false, int waitTime = 3000)
        {
            return clickImg(bmp, fl.X, fl.Y, waitSuc, waitTime);
        }

        /// <summary>
        /// 传入的bmp将自动释放
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public HSearchPoint clickImg(Bitmap bmp, int x, int y, bool waitSuc, int waitTime)
        {
            if (waitSuc)
            {
                long start = Environment.TickCount;
                HSearchPoint hs = null;
                using (bmp)
                {
                    do
                    {
                        hs = findImg(bmp);
                        if (hs.Success)
                        {
                            Point p = hs.Point;
                            p.Offset(x, y);
                            MouseClick(p);
                            break;
                        }
                    } while (Environment.TickCount - waitTime > 0);
                }
                return hs;
            }
            else
            {
                using (bmp)
                {
                    HSearchPoint hs = findImg(bmp);
                    if (hs.Success)
                    {
                        Point p = hs.Point;
                        p.Offset(x, y);
                        MouseClick(p);
                    }
                    return hs;
                }
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

        public List<HSearchPoint> findImgAll(List<Bitmap> bmp)
        {
            List<HSearchPoint> rst = new List<HSearchPoint>();
            using (Bitmap bit = getImg())
            {
                for (int i = 0; i < bmp.Count; i++)
                {
                    rst.AddRange(ImageTool.findEqImgAll(bmp[i], bit));
                }
            }
            return rst;
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

        /// <summary>
        /// 是否在BOSS范围内
        /// </summary>
        /// <returns></returns>
        public bool isInBoosRange()
        {
            return CurrentMap.isInBossRange(this);
        }

        private static void log(string msg)
        {
            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss") + " - " + msg);
        }

        public void AUTOTiLianWuPing()
        {
            aUTOTiLianWuPing.StartAutoTiLian();
        }

        public void slep(int v)
        {
            Mouse.sleep(v);
        }
    }
}
