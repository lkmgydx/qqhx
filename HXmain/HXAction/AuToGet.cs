using HFrameWork.SystemInput;
using HXmain.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tool;
using WSTools.WSLog;

namespace HXmain.HXAction
{
    public class AuToGet
    {
        private MainGame game;
        System.Timers.Timer timer = new System.Timers.Timer();

        private bool _enable = false;
        public bool Enable
        {
            get { return _enable; }
            set
            {
                if (value)
                {
                    Console.WriteLine("+++++【开启】自动拾取功能++++");
                }
                else
                {
                    Console.WriteLine("+++++【关闭】自动拾取功能++++");
                }
                timer.Enabled = value;
            }
        }
        public AuToGet(MainGame game)
        {
            this.game = game;
            timer.Interval = 5000;
            timer.Elapsed += Timer_Elapsed;
            game.onPowerStop += Game_onPowerStop;
        }

        private void Game_onPowerStop()
        {
            if (timer != null)
            {
                timer.Stop();
            }
        }

        bool check = false;

        List<Point> checkPoints = new List<Point>();
        List<Point> locationPoints = new List<Point>();

        int max = 9;
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (check)
            {
                return;
            }
            check = true;
            checkPoints = new List<Point>();
            if (max++ % 10 == 0)
            {
                Console.WriteLine("重置坐标点...");
                locationPoints = new List<Point>();
            }
            checkGet();
            check = false;
        }

        private bool hasInPoint(int x, int y)
        {
            foreach (var c in checkPoints)
            {
                if (c.X == x && c.Y == y)
                {
                    return true;
                }
            }
            return false;
        }

        private bool hasInLocation()
        {
            Point p = game.CurrentLocation;
            foreach (var c in locationPoints)
            {
                if (c.X == p.X && c.Y == p.Y)
                {
                    return true;
                }
            }
            return false;
        }

        public void checkGet()
        {
            // Log.log("开始自动拾取"); 
            try
            {
                game.lockUtil.getTopLock();
                reGet:
                if (!game.AutoGet) {
                    return;
                }
                Log.logError("自动拾取检测中...");
                int sizeX = 30, sizeY = 30;
                using (Bitmap bmp = Resources.btn_全部拾取)
                {
                    var hs = game.findImg(bmp);
                    if (hs.Success)
                    {
                        Console.WriteLine("=====>点击拾取");
                        game.MouseClick(hs.CenterPoint);
                    }
                }
                for (int i = 295; i < 900; i += sizeX) //竖线
                    for (int j = 90; j < 600; j += sizeY)
                    {
                        using (Bitmap bt = game.getImg(new Rectangle(i, j, sizeX, sizeY)))
                        {
                            if (ImageTool.hasSomeOne(bt) && !hasInPoint(i, j) && !hasInLocation())
                            {
                                checkPoints.Add(new Point(i, j));
                                Log.logWarn("即将自动拾取位置:" + i + j);
                                Mouse.cacheLocation();
                                game.MouseRigthClick(i, j);
                                game.sleep(200);
                                Mouse.leftclick();
                                game.sleep(200);
                                Mouse.reventLocation();
                                game.sleep(1000);
                                locationPoints.Add(game.CurrentLocation);
                                game.SendKey(Keys.Space);
                                goto reGet;
                            }
                        }
                    }
            }
            catch (Exception ex)
            {
                Log.logError("自动拾取异常!退出", ex);
                return;
            }
            finally
            {
                game.lockUtil.ReleaseTopLock();
                Log.logError("结束自动拾取中...");
            }
        }
    }
}
