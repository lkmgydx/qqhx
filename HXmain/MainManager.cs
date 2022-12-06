using HFrameWork.SystemDll;
using HFrameWork.SystemInput;
using HXmain.HXAction;
using HXmain.HXInfo;
using HXmain.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tool;
using WSTools.WSLog;
using WSTools.WSThread;

namespace HXmain
{

    public class MainManager : IDisposable
    {

        private List<MainGame> list_games = new List<MainGame>();
        ScreenCapture sc = new ScreenCapture();//X
        Timer tm = new Timer();
        private bool enableYZM = false;

        public event EventHandler onlog;


        private bool _cehckDie = false;
        /// <summary>
        /// 角色死亡检测
        /// </summary>
        public bool AutoLive
        {
            get { return _cehckDie; }
            set
            {
                _cehckDie = value;
                for (int i = 0; i < games.Count; i++)
                {
                    games[i].dieCheck = value;
                    games[i].DieAutoLive = value;
                }
            }
        }

        /// <summary>
        /// 自动组队
        /// </summary>
        private bool _autoZudui = false;
        /// <summary>
        /// 自动组队，默认false
        /// </summary>
        public bool AutoZudui
        {
            get { return _autoZudui; }
            set
            {
                for (int i = 0; i < games.Count; i++)
                {
                    games[i].AutoZuDui = value;
                }
                _autoZudui = value;
            }
        }
        public int BaseAddress { get; set; } = 0;

        /// <summary>
        /// 主游戏进程
        /// </summary>
        public MainGame MainGame { get; private set; }

        /// <summary>
        /// 返回指定游戏进程管理器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public MainGame this[int index] { get { return games[index]; } }
        /// <summary>
        /// 游戏总数
        /// </summary>
        public int Count { get { return this.games.Count; } }
        /// <summary>
        /// 自动提练是否启用
        /// </summary>
        public bool AutoTilian { get; set; }

        public bool setMainProcess(Process p)
        {
            for (int i = 0; i < this.games.Count; i++)
            {
                if (games[i].MainProcess.Id == p.Id)
                {
                    MainGame = games[i];
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 验证码，30秒检查一次
        /// </summary>
        public bool EnableYZM
        {
            get
            {
                return enableYZM;
            }
            set
            {
                for (int i = 0; i < games.Count; i++)
                {
                    games[i].YZMcheck = value;
                }
                enableYZM = value;
            }
        }

        private List<MainGame> games = new List<MainGame>();
        

        WSTools.WSHook.KeyboardHook key = new WSTools.WSHook.KeyboardHook();

        public MainManager()
        {
            key.Start();
            key.KeyUpEvent += Key_KeyUpEvent;
        }


        private void Key_KeyUpEvent(object sender, KeyEventArgs e)
        {
            if (MainGame == null)
            {
                return;
            }
            if (e.KeyCode == Keys.Escape)
            {
                if (MainGame != null)
                {
                    MainGame.PowerStop();
                }
            }

            if (!e.Control)
            {
                return;
            }
            if (e.KeyCode == Keys.Escape)//CTRL+ESC 退出
            {
                if (MainGame != null)
                {
                    MainGame.Dispose();
                }
                Application.Exit();
            }
            else if (e.KeyCode == Keys.K)
            {
                Log.logForce("CTRL+K");
                if (MainGame.isRun)
                {
                    Log.logForce("-----STOP");
                    MainGame.PowerStop();
                }
                else
                {
                    Log.logForce("+++++START");
                    MainGame.AutoRun(e.Shift);
                }
            }
            else if (e.KeyCode == Keys.T)
            {
                WsThread.Run(() =>
                {
                    new WuPingLanInfo(MainGame).TilianAllForce();
                    MainGame.drawMouse();
                });
            }
            else if (e.KeyCode == Keys.P)
            {
                MainGame.TiLianSome();
            }
            else if (e.KeyCode == Keys.O)
            {
                TiLian.show技能Win(MainGame);
            }
            else if (e.KeyCode == Keys.D1)
            {
                三海经.三第经日常(MainGame);
            }
            else if (e.KeyCode == Keys.N)
            {
                if (MainGame == null)
                {
                    return;
                }
                using (Bitmap bmp = MainGame.getImg())
                {
                    Directory.CreateDirectory("img");
                    bmp.Save("img/" + DateTime.Now.ToString("hh_mm_ss-") + Environment.TickCount + ".bmp");
                }
            }
            else if (e.KeyCode == Keys.Y)
            {
                if (YZM.hasYZM(MainGame))
                {
                    using (Bitmap bmp = YZM.getYZMBmp(MainGame))
                    {
                        Directory.CreateDirectory("img_YZM");
                        bmp.Save("img_YZM/" + DateTime.Now.ToString("hh_mm_ss-") + Environment.TickCount + ".bmp");
                    }
                }
            }
        }

        public void addGageProcee(Process p)
        {
            if (p == null)
            {
                return;
            }

            foreach (MainGame pr in games)
            {
                if (pr.MainHandler == p.MainWindowHandle)
                {
                    return;
                }
            }
            MainGame mg = new MainGame(p);
            games.Add(mg);
            setMainProcess(p);
        }

        public void clearGageProcee()
        {
            games.Clear();
        }
        public void delGageProcee(Process p)
        {
            if (p == null)
            {
                return;
            }

            foreach (MainGame pr in games)
            {
                if (pr.MainProcess == p)
                {
                    games.Remove(pr);
                    break;
                }
            }
            games.Add(new MainGame(p));
        }


        public void SetDown()
        {
            for (int i = 0; i < games.Count; i++)
            {
                try
                {
                    games[i].SetDown();
                }
                catch (Exception ex)
                {
                    Log.logErrorForce("打坐异常", ex);
                }

            }
            GC.Collect();
        }

        public void startCuiDiao()
        {
            HSearchPoint hs = ImageTool.findImageFromScreen(Resources.垂钓_钓鱼, 80);
            if (hs.Success)
            {
                Point pt = hs.CenterPoint;
                Point pyd = Point.Empty;
                do
                {
                    hs = ImageTool.findImageFromScreen(Resources.垂钓_鱼点, 80);
                    KeyBoard.sleep(1000);
                    pyd = hs.CenterPoint;
                }
                while (!hs.Success);
                int x = 1;
                do
                {
                    Mouse.move(pt);
                    KeyBoard.sleep(100);
                    Mouse.leftclick();
                    Point ff = hs.CenterPoint;
                    x += 5;
                    Mouse.move(pyd.X + x, pyd.Y);
                    Mouse.leftclick();
                    KeyBoard.sleep(500);
                    HSearchPoint ht = ImageTool.findImageFromScreen(Resources.垂钓, 80);
                    if (!ht.Success)
                    {

                        startCuiDiao();
                        return;
                    }
                    hs = ImageTool.findImageFromScreen(Resources.垂钓_钓鱼, 80);
                    pt = hs.CenterPoint;
                }
                while (hs.Success);
            }
            else
            {
                MainGame.SendKey(Keys.D0);
                KeyBoard.sleep(300);
                startCuiDiao();
            }
        }

        /// <summary>
        /// 流程查找
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool searchFloorStep(Bitmap bmp, string name)
        {
            HSearchPoint hs = ImageTool.findLikeImg(bmp, sc.HImage, 90);
            if (hs.Success)
            {
                Mouse.cacheLocation();
                flexMove(hs.CenterPoint);
                Mouse.leftclick();
                Mouse.move(0, 0);
                // 寄售Search(name);
                Mouse.reventLocation();
                return true;
            }
            return false;
        }

        private void searchFloor(List<Bitmap> list, string name)
        {
            HSearchPoint hst = ImageTool.findLikeImg(list[0], sc.HImage, 90);
            if (hst.Success)
            {
                寄售Search(name);
                return;
            }
            for (int i = 1; i < list.Count; i++)
            {
                KeyBoard.sleep(1000);
                if (searchFloorStep(list[i], name))
                {
                    searchFloor(list, name);
                    return;
                }
            }
        }

        public void ss()
        {
            HSearchPoint hs = ImageTool.findLikeImg(Resources.寄售_材料_宝石2, sc.HImage, 90);
            if (hs.Success)
            {
                flexMove(hs.CenterPoint);
                Mouse.leftclick();
            }
        }

        private void flexMove(Point p)
        {
            Mouse.move(sc.HLeft + p.X, sc.HTop + p.Y);
        }

        private void flexMove(int x, int y)
        {
            Mouse.move(sc.HLeft + x, sc.HTop + y);
        }

        public void follow()
        {
            HSearchPoint hs = ImageTool.findLikeImg(Resources.friend, sc.HImage, 80);
            onlog?.Invoke("flollow [" + hs.Success + "]", null);
            if (hs.Success)
            {
                flexMove(hs.CenterPoint);
                Mouse.leftclick();
            }
        }

        public void 寄售search元如(string name)
        {
            searchFloor(new List<Bitmap>() {
                Resources.寄售_材料_宝石_元如2,
                Resources.寄售_材料_宝石_元如1,
                Resources.寄售_材料_宝石2,
                Resources.寄售_材料_宝石1,
                Resources.寄售_材料2,
                Resources.寄售_材料
            }, name);
        }

        public void 寄售search无相(string name)
        {
            searchFloor(new List<Bitmap>() {
                Resources.寄售_材料_宝石_无相1,
                Resources.寄售_材料_宝石_无相2,
                Resources.寄售_材料_宝石2,
                Resources.寄售_材料_宝石1,
                Resources.寄售_材料2,
                Resources.寄售_材料
            }, name);
        }

        public void 寄售search天行(string name)
        {
            searchFloor(new List<Bitmap>() {
                Resources.寄售_材料_宝石_天行2,
                Resources.寄售_材料_宝石_天行1,
                Resources.寄售_材料_宝石2,
                Resources.寄售_材料_宝石1,
                Resources.寄售_材料2,
                Resources.寄售_材料
            }, name);
        }

        public void 寄售search望月(string name)
        {
            searchFloor(new List<Bitmap>() {
                Resources.寄售_材料_宝石_望月2,
                Resources.寄售_材料_宝石_望月1,
                Resources.寄售_材料_宝石2,
                Resources.寄售_材料_宝石1,
                Resources.寄售_材料2,
                Resources.寄售_材料
            }, name);
        }


        public void 寄售search北斗(string name)
        {
            //searchFloor(new List<Bitmap>() {
            //    Resources.寄售_材料_宝石_北斗2,
            //    Resources.寄售_材料_宝石_北斗,
            //    Resources.寄售_材料_宝石2,
            //    Resources.寄售_材料_宝石1,
            //    Resources.寄售_材料2,
            //    Resources.寄售_材料
            //}, name);
            return;
            //展开在宝石界面
            //HSearchPoint hs = ImageTool.findLikeImg(Properties.Resources.寄售_材料_宝石_北斗2, sc.HImage, 80);
            //if (hs.Success)
            //{
            //    寄售Search(name);
            //    return;
            //}
            //hs = ImageTool.findLikeImg(Properties.Resources.寄售_材料_宝石_北斗, sc.HImage, 90);
            //if (hs.Success)
            //{
            //    Mouse.cacheLocation();
            //    flexMove(hs.CenterPoint);
            //    Mouse.leftclick();
            //    Mouse.move(0, 0);
            //    寄售Search(name);
            //    Mouse.reventLocation();
            //    return;
            //}

            //hs = ImageTool.findLikeImg(Properties.Resources.寄售_材料_宝石2, sc.HImage, 90);
            //if (hs.Success)
            //{
            //    Mouse.cacheLocation();
            //    flexMove(hs.CenterPoint);
            //    Mouse.leftclick();
            //    Mouse.move(0, 0);
            //    寄售search北斗(name);
            //    Mouse.reventLocation();
            //    return;
            //}
            //hs = ImageTool.findLikeImg(Properties.Resources.寄售_材料_宝石1, sc.HImage, 90);
            //if (hs.Success)
            //{
            //    Mouse.cacheLocation();
            //    flexMove(hs.CenterPoint);
            //    Mouse.leftclick();
            //    Mouse.move(0, 0);
            //    寄售search北斗(name);
            //    Mouse.reventLocation();
            //    return;
            //}
            //hs = ImageTool.findLikeImg(Properties.Resources.寄售_材料2, sc.HImage, 90);
            //if (hs.Success)
            //{
            //    Mouse.cacheLocation();
            //    flexMove(hs.CenterPoint);
            //    Mouse.leftclick();
            //    Mouse.move(0, 0);
            //    寄售search北斗(name);
            //    Mouse.reventLocation();
            //    return;
            //}
            //hs = ImageTool.findLikeImg(Properties.Resources.寄售_材料, sc.HImage, 90);
            //if (hs.Success)
            //{
            //    Mouse.cacheLocation();
            //    flexMove(hs.CenterPoint);
            //    Mouse.leftclick();
            //    Mouse.move(0, 0);
            //    寄售search北斗(name);
            //    Mouse.reventLocation();
            //    return;
            //}
        }


        public void 寄售SearchSingle(string text)
        {
            HSearchPoint hs = ImageTool.findLikeImg(HXmain.Properties.Resources.寄售, sc.HImage, 80);
            if (hs.Success)
            {
                Mouse.cacheLocation();

                flexMove(hs.CenterPoint);

                Mouse.moveR(300, 300);
                return;
                //Mouse.move(sc.HLeft + 470, sc.HTop + 620);
                //Mouse.leftclick();

                //KeyBoard.down_upCtrl(Keys.A);
                //KeyBoard.down_up(Keys.Delete);

                //IDataObject obj = Clipboard.GetDataObject();
                //Clipboard.SetData(DataFormats.Text, text);
                //KeyBoard.sleep(300);
                //KeyBoard.down_upCtrl(Keys.V);

                //KeyBoard.sleep(100);

                //Clipboard.SetDataObject(obj);

                //Mouse.move(sc.HLeft + 590, sc.HTop + 610);
                //Mouse.leftclick();

                //Mouse.reventLocation();
            }
        }

        public bool IsFromScreen { get; set; } = true;


        public void 寄售Search(string text)
        {

            Bitmap from = this.MainGame.getImg();

            HSearchPoint hs = ImageTool.findLikeImg(Resources.寄售, from, 80);
            if (hs.Success)
            {
                Mouse.cacheLocation();
                MainGame.MouseMove(hs.CenterPoint);
                Mouse.moveR(300, 430);
                Mouse.leftclick();
                Mouse.sleep(300);

                Mouse.leftdown();
                Mouse.moveR(-200, 0);
                Mouse.leftup();
                KeyBoard.down_up(Keys.Delete);

                Clipboard.SetData(DataFormats.Text, text);
                KeyBoard.sleep(300);
                KeyBoard.down_upCtrl(Keys.V);

                KeyBoard.sleep(100);

                Mouse.moveR(280, 10);

                Mouse.leftclick();

                hs = ImageTool.findLikeImg(Resources.寄售_单价, from, 90);
                if (hs.Success)
                {
                    MainGame.MouseMove(hs.CenterPoint);
                    KeyBoard.sleep(200);
                }
                else
                {
                    Mouse.moveR(2, -370);
                    KeyBoard.sleep(500);
                }
                Mouse.leftclick();
                KeyBoard.sleep(500);
                Mouse.leftclick();
                Mouse.reventLocation();
            }

        }

        public void Dispose()
        {
            for (int i = 0; i < this.games.Count; i++)
            {
                try
                {
                    games[i].Dispose();
                }
                catch (Exception ex)
                {
                    Log.logError("释放异常!", ex);
                }
            }
            key.Stop();
        }

        public void SendKey(Keys d3)
        {
            try
            {
                for (int i = 0; i < games.Count; i++)
                {
                    games[i].SendKey(d3);
                }
            }
            catch (Exception ex)
            {
                Log.logError("sendKey erro", ex);
            }
        }

        public void removeGameProcess(MainGame p)
        {
            games.Remove(p);
            p.Dispose();
        }
    }
}
