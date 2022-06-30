using HFrameWork.SystemInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WSTools.WSLog;
using HXmain.HXInfo.Map;

namespace HXmain.HXAction
{
    public class MovePerson
    {
        private MainGame game;

        /// <summary>
        /// 强制停止移动
        /// </summary>
        public bool PowerStop { get; set; }

        private Point WantToPoint = Point.Empty;

        private Point WantToPointNext = Point.Empty;

        private Rectangle MoveRange = Rectangle.Empty;
        /// <summary>
        /// 范围检测
        /// </summary>
        public bool CheckInRange { get; set; } = false;

        /// <summary>
        /// 是否曾经经过此位置
        /// </summary>
        private bool isSucInPoint = false;

        private int maxMoveStep { get; set; } = 3;

        /// <summary>
        /// X坐标最大偏移
        /// </summary>
        public int MaxXFlex { get; set; } = 2;
        /// <summary>
        /// Y坐标最大偏移
        /// </summary>
        public int MaxYFlex { get; set; } = 2;

        /// <summary>
        /// 是否在移动中...
        /// </summary>
        public bool isMoving { get; private set; }
        /// <summary>
        /// 锁定坐标
        /// </summary>
        public Point LockPoint { get; set; }

        public MovePerson(MainGame game)
        {
            this.game = game;
            game.onLocationChange += MovePerson_onLocationChange;
            game.onPowerStop += Game_onPowerStop;
        }



        public void StartLockPoint()
        {

        }


        private void Game_onPowerStop()
        {
            PowerStop = true;
        }

        private void MovePerson_onLocationChange(Point before, Point now)
        {
            if (isMoving && CheckIsSuc())
            {
                isSucInPoint = true;
            }
        }

        private bool rangeCheck = false;

        public void MovePoint(int x, int y, bool rangeCheck = false)
        {
            this.rangeCheck = rangeCheck;
            MovePoint(new Point(x, y), Point.Empty);
        }

        public void MovePoint(Point pt, Point[] path, Action action)
        {
            if (path.Length == 0)
            {
                MovePoint(pt, Point.Empty);
                if (action != null)
                {
                    action();//
                }
                return;
            }
            Point nearPoint = new Point(1000, 1000);

            int i = 0;

            int minLength = 1000;
            int pLeng = 1000;
            Point currentLocation = game.CurrentLocation;


            int startLocation = 0;//靠近人物最近的位置
            int lastLocation = 0;//靠近目标最近的位置

            for (; i < path.Length; i++)
            {
                Point pp = path[i];
                int len = pp.X + pp.Y;
                int cP = Math.Abs(pp.X - currentLocation.X) + Math.Abs(pp.Y - currentLocation.Y);
                if (cP < pLeng)
                {
                    startLocation = i;
                    pLeng = cP;
                }
                cP = Math.Abs(pp.X - pt.X) + Math.Abs(pp.Y - pt.Y);
                if (cP < minLength)
                {
                    lastLocation = i;
                    minLength = cP;
                }
            }
            Point[] ps = new Point[Math.Abs(startLocation - lastLocation)];
            if (startLocation < lastLocation)
            {
                Array.Copy(path, startLocation, ps, 0, ps.Length);
            }
            else
            {
                path = path.Reverse().ToArray();
                Array.Copy(path, lastLocation, ps, 0, ps.Length);
                ps = ps.Reverse().ToArray();
            }

            if (ps.Length > 1)
            {
                List<Point> ptt = new List<Point>(ps);//
                ptt.Add(pt);
                game.runPath(ptt.ToArray(), action);
            }
            else
            {
                MovePoint(pt, Point.Empty);
                if (action != null)
                {
                    action();//
                }
                return;
            }
        }

        public bool GoPoPoint(Point p)
        {
            PowerStop = false;
            if (p == Point.Empty)
            {
                return false;
            }
            isMoving = true;
            Log.log("GoTo移动坐标:" + p);
            WantToPoint = p;
            while (true)
            {
                if (PowerStop)
                {
                    return false;
                }
                Mouse.sleep(2000);
                if (PowerStop)
                {
                    return false;
                }
                if (moveAction())
                {
                    return true;
                }
            }
        }

        internal void GoToPoint(int v1, int v2)
        {
            GoPoPoint(new Point(v1, v2));
        }

        public void MovePoint(Point p, Point next)
        {
            PowerStop = false;
            if (p == Point.Empty)
            {
                return;
            }
            isMoving = true;
            WantToPoint = p;
            WantToPointNext = next;
            if (!CheckInRange || WantToPointNext == Point.Empty)
            {
                MoveRange = Rectangle.Empty;
            }
            else
            {
                int left = Math.Min(WantToPoint.X, WantToPointNext.X);
                int top = Math.Min(WantToPoint.Y, WantToPointNext.Y);
                int right = Math.Max(WantToPoint.X, WantToPointNext.X);
                int bottom = Math.Max(WantToPoint.Y, WantToPointNext.Y);
                MoveRange = new Rectangle(left, top, right - left, bottom - top);
            }
            Log.log("移动坐标:" + p);
            MovePoint();
            isMoving = false;
        }

        private object moveObj = new object();
        private void MovePoint()
        {
            lock (moveObj)
            {
                while (true)
                {
                    Mouse.sleep(10);
                    if (PowerStop)
                    {
                        return;
                    }
                    // Console.Write("[MovePoint]等等获取锁....");
                    Log.log("获取锁");
                    game.lockUtil.getLock();
                    Log.log("获取完成");
                    int time = Environment.TickCount;
                    while (game.AutoOneKey && game.isUseSkill)
                    {
                        if (PowerStop)
                        {
                            return;
                        }
                        if (Environment.TickCount - time > 60000)
                        {//超一分钟
                            game.RefreshUseSkill();
                        }
                        game.sleep(100);
                    }
                    try
                    {
                        //int waitTime = 2000;
                        //int nowTime = Environment.TickCount;
                        //while (game.hasSelectSome() && (Environment.TickCount - nowTime) <= waitTime)
                        //{
                        //    game.sleep(1000);
                        //}
                        Log.log("移动moveAction");
                        if (moveAction())
                        {
                            break;
                        }
                    }
                    catch
                    {
                    }
                    finally
                    {
                        Log.log("准备释放ReleaseLock");
                        game.lockUtil.ReleaseLock();
                        Log.log("释放完成ReleaseLock");
                        // Console.WriteLine("MovePoint - ReleaseLock");
                    }
                }
            }
        }

        internal void RadomMoveAllMap(MapBase mapBase)
        {

        }

        public void RadomMove()
        {
            Console.WriteLine("随机移动..");
            Mouse.cacheLocation();
            Point p = new Point(new Random(Guid.NewGuid().GetHashCode()).Next(150, 600), new Random(Guid.NewGuid().GetHashCode()).Next(250, 550));
            int maxTime = 50;
            while (true || maxTime-- >= 0)
            {
                if (p.Y < 545)
                {
                    if (!MapPoints.CanUp(game))
                    {
                        continue;
                    }
                }
                else
                {
                    if (!MapPoints.CanBottom(game))
                        continue;
                }

                if (p.X < 411)
                {
                    if (!MapPoints.CanLeft(game))
                    {
                        continue;
                    }
                }
                else
                {
                    if (!MapPoints.CanRigth(game))
                    {
                        continue;
                    }
                }
                break;
            }
            game.MouseMove(p);
            game.sleep(200);
            Mouse.leftclick();
            game.sleep(200);
            Mouse.reventLocation();
        }


        private bool CheckIsSuc()
        {
            Point ptNow = game.CurrentLocation;
            Point pt = WantToPoint;

            if (MoveRange != Rectangle.Empty && MoveRange.Contains(ptNow))
            {
                return true;
            }

            bool xSuc = Math.Abs(pt.X - ptNow.X) < MaxXFlex;
            bool ySuc = Math.Abs(pt.Y - ptNow.Y) < MaxYFlex;
            if (xSuc && ySuc)
            {
                return true;
            }
            return false;
        }

        private bool isGoLocation()
        {
            if (isSucInPoint)
            {
                isSucInPoint = false;
                return true;
            }
            return false;
        }

        private bool moveAction()
        {
            if (isInRange() || isGoLocation() || CheckIsSuc())
            {
                return true;
            }
            Point pt = game.CurrentLocation;

            int x = WantToPoint.X;
            int y = WantToPoint.Y;

            int fx = 0;
            int fy = 0;
            bool needEnd = true;
            if (Math.Abs(pt.Y - y) > maxMoveStep)
            {
                fy = maxMoveStep;
                needEnd = true;
            }
            else
            {
                fy = Math.Abs(pt.Y - y);
            }
            if (Math.Abs(pt.X - x) > maxMoveStep)
            {
                fx = maxMoveStep;
                needEnd = true;
            }
            else
            {
                fx = Math.Abs(pt.X - x);
            }
            needEnd = true;

            if (pt.X == x)
            {
                if (pt.Y > y) //上移
                {
                    Log.log("上移");
                    if (!canTop)
                    {
                        radomMove();
                    }
                    canTop = run(TopStep, fy + 2, needEnd);
                }
                else
                {
                    Log.log("下移");
                    if (!canBottom)
                    {
                        radomMove();
                    }
                    canBottom = run(BottomStep, x, false);// BottomStep(fy + 1, needEnd);
                }
            }
            else if (pt.Y == y)
            {
                if (pt.X > x) //左移
                {
                    Log.log("左移");
                    if (!canLeft)
                    {
                        radomMove();
                    }
                    LeftStep(fx, needEnd);
                }
                else
                {
                    Log.log("右移");
                    if (!canRigth)
                    {
                        radomMove();
                    }
                    canRigth = run(RightStep, fx, needEnd);
                }
            }
            else if (pt.X > x)
            {
                if (pt.Y > y)
                {
                    Log.log("左上移");
                    if (!Left_Top)
                    {
                        radomMove();
                        Left_Top = true;
                    }
                    Left_Top = run(Left_TopStep, fx, fy, needEnd);
                }
                else
                {
                    Log.log("左下移");
                    if (!Left_Down)
                    {
                        radomMove();
                        Left_Down = true;
                    }
                    Left_Down = run(Left_DownStep, fx, fy, needEnd);
                }
            }
            else
            {
                if (pt.Y > y)
                {
                    Log.log("右上");
                    if (!Rigth_Top)
                    {
                        radomMove();
                        Rigth_Top = true;
                    }
                    Rigth_Top = run(Rigth_TopStep, fx, fy, needEnd);
                }
                else
                {
                    Log.log("右下移");//右下强制最多2位
                    if (!Rigth_Down)
                    {
                        radomMove();
                        Rigth_Down = true;
                    }
                    Rigth_Down = run(Rigth_DownStep, fx, fy, needEnd);
                }
            }
            return false;
        }

        private void radomMove()
        {
            List<StepDelegate> li = new List<StepDelegate>();
            li.Add(TopStep);
            li.Add(LeftStep);
            li.Add(RightStep);
            li.Add(BottomStep);

            Dictionary<StepDelegate, bool> fns = new Dictionary<StepDelegate, bool>();
            fns.Add(TopStep, canTop);
            fns.Add(LeftStep, canLeft);
            fns.Add(RightStep, canRigth);
            fns.Add(BottomStep, canBottom);

            while (true)
            {
                int radom = new Random().Next(1, 4);
                var sd = fns[li[radom]];
                if (sd)
                {
                    if (run(li[radom], radom, false))
                    {
                        canTop = true;
                        canLeft = true;
                        canRigth = true;
                        canBottom = true;
                        break;
                    }
                }
            }

        }

        private bool isInRange()
        {
            Point c = game.CurrentLocation;
            return Math.Abs(c.X - WantToPoint.X) < 3 && Math.Abs(c.Y - WantToPoint.Y) < 3;
        }


        private bool canLeft = true;
        /// <summary>
        /// 范围  1-5
        /// </summary>
        /// <param name="x"></param>
        public void LeftStep(int x, bool needEnd = false)
        {
            if (x <= 0)
            {
                x = 1;
            }
            if (x >= 5)
            {
                x = 5;
            }
            game.pointClick(530 - x * 70, 423, needEnd);
        }

        private bool canRigth = true;
        public void RightStep(int x, bool needEnd = false)
        {
            if (x <= 0)
            {
                x = 1;
            }
            if (x >= 5)
            {
                x = 5;
            }
            game.pointClick(530 + x * 70, 423, needEnd);
        }

        private Point CenterPersonPoint = new Point(545, 425);

        private bool canTop = true;

        public void TopStep(int x, bool needEnd = false)
        {
            if (x <= 0)
            {
                x = 1;
            }
            if (x >= 9)
            {
                x = 9;
            }
            x--;//从100开始
            game.pointClick(545, 423 - (100 + x * 30), needEnd);
        }

        private Point getPointUpDown(int x)
        {
            return new Point(CenterPersonPoint.X, CenterPersonPoint.Y + x * 30);
        }

        private Point getPointLeftRigth(int x)
        {
            return new Point(CenterPersonPoint.X + x * 60, CenterPersonPoint.Y);
        }

        private bool canBottom = true;

        private bool run(StepDelegate2 fn, int x, int y, bool needEnd)
        {
            Point before = game.CurrentLocation;
            fn.Invoke(x, y, needEnd);
            if (game.isUseSkill)
            {
                return true;
            }
            game.sleep(500);
            var after = game.CurrentLocation;
            return before.X != after.X || before.Y != after.Y;
        }

        private bool run(StepDelegate fn, int x, bool needEnd)
        {
            Point before = game.CurrentLocation;
            fn.Invoke(x, needEnd);
            if (game.isUseSkill)
            {
                return true;
            }
            game.sleep(500);
            var after = game.CurrentLocation;
            return before.X != after.X || before.Y != after.Y;
        }

        private delegate void StepDelegate(int x, bool needEnd = false);
        private delegate void StepDelegate2(int x, int y, bool needEnd = false);

        public void BottomStep(int x, bool needEnd = false)
        {
            if (x <= 0)
            {
                x = 1;
            }
            if (x >= 8)
            {
                x = 8;
            }
            game.pointClick(545, 423 + x * 30, needEnd);
        }

        private bool Left_Top = true;
        public void Left_TopStep(int x, int y, bool needEnd = false)
        {
            Point tx = getPointLeftRigth(-x);
            Point ty = getPointUpDown(-y);
            game.pointClick(tx.X, ty.Y, needEnd);
        }

        private bool Left_Down = true;
        public void Left_DownStep(int x, int y, bool needEnd = false)
        {
            Point tx = getPointLeftRigth(-x);
            Point ty = getPointUpDown(y);
            game.pointClick(tx.X, ty.Y, needEnd);
        }

        private bool Rigth_Top = true;
        public void Rigth_TopStep(int x, int y, bool needEnd = false)
        {
            Point tx = getPointLeftRigth(x);
            Point ty = getPointUpDown(-y);
            game.pointClick(tx.X, ty.Y, needEnd);
        }


        private bool Rigth_Down = true;

        public void Rigth_DownStep(int x, int y, bool needEnd = false)
        {
            if (x > 2)
            {
                x = 2;
            }
            Point tx = getPointLeftRigth(x);
            Point ty = getPointUpDown(y);
            game.pointClick(tx.X, ty.Y, needEnd);
        }

        private string getFormatePoint(Point pt)
        {
            return getFormatePoint(pt.X, pt.Y);
        }
        private string getFormatePoint(int x, int y)
        {
            return string.Format("[x:{0},y:{1}]", x, y);// [x:" + pt.ToString() + " -> 目标:[x:" + x + "],[y:" + y + "]");
        }

    }
}
