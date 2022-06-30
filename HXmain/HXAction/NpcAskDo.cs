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
    /// NCP对话
    /// </summary>
    class NpcAskDo : BathDo
    {
        protected Bitmap Npcbmp;
        protected string ActionName;

        protected Dictionary<Bitmap, int> waitTimeImages = new Dictionary<Bitmap, int>();
        protected Dictionary<Bitmap, Point> onceSucImages = new Dictionary<Bitmap, Point>();
        protected Dictionary<Bitmap, Func<bool>> onCallBack = new Dictionary<Bitmap, Func<bool>>();

        public NpcAskDo(MainGame game, Bitmap npcbmp, string actionName = "NPC对话操作...", int maxTime = 10000) : base(game, maxTime)
        {
            Npcbmp = npcbmp;
            ActionName = actionName;
        }

        public void addWaitDoImage(Bitmap bmp, int waitTime = 2000)
        {
            waitTimeImages.Add(bmp, waitTime);
        }

        public void addOnceImg(Bitmap bmp, int x, int y)
        {
            onceSucImages.Add(bmp, new Point(x, y));
        }

        protected override Bitmap filterImgae(Bitmap bmp, Bitmap gameImg)
        {
            foreach (var f in waitTimeImages)
            {
                var hs = ImageTool.findLikeImg(f.Key, gameImg);
                Log.log("等待图片查找:" + hs.Success);
                if (hs.Success)
                {
                    game.MouseClick(hs.CenterPoint);
                    game.sleep(f.Value);
                }
            }
            List<Bitmap> waitTodel = new List<Bitmap>();
            foreach (var f in onceSucImages)
            {
                var hs = game.findImg(f.Key);
                Log.log("退出图片查找:" + hs.Success);
                if (hs.Success)
                {
                    waitTodel.Add(f.Key);
                    game.MouseClick(hs.CenterPoint.X + f.Value.X, hs.CenterPoint.Y + f.Value.Y);
                }
            }
            for (int i = 0; i < waitTodel.Count; i++)
            {
                Log.log("移除成功图片!");
                onceSucImages.Remove(waitTodel[i]);
            }
            foreach (var f in onCallBack)
            {
                var hs = game.findImg(f.Key);
                Log.log("图片查找回调:" + hs.Success);
                if (hs.Success)
                {
                    game.MouseClick(hs.CenterPoint);
                    if (f.Value.Invoke())
                    {
                        return null;
                    }
                }
            }
            return base.filterImgae(bmp, gameImg);
        }

        internal void AddOnSucBmp(Bitmap bmp, Func<bool> p)
        {
            onCallBack.Add(bmp, p);
        }

        public override bool Start()
        {
            Log.LogEnable = true;
            Log.LogToConsole = true;
            bool waitToExit = true;
            bool isSunSuc = false;
            BaseAction ac = BaseAction.getNpcAction(game, Npcbmp);
            ac.FlotPoint = new Point(0, 70);
            ac.ActionType = ActionType.RigthDbClick;
            ac.ActionName = ActionName;

            WSTools.WSThread.WsThread.PoolRun(() =>
            {
                onExit += (bool suc) =>
                {
                    Console.WriteLine("exit  【" + ActionName + "】 - " + suc);
                    waitToExit = false;
                    isSunSuc = suc;
                };
                base.Start();
            });
            while (true)
            {
                bool isclickSuc = false;
                lock (lockGameOp)
                {
                    if (!waitToExit)
                    {
                        Log.log("主返回:" + isSunSuc);
                        return isSunSuc;
                    }
                    HSearchPoint hs = game.findImg(Npcbmp);
                    Log.log("查找NPC:" + hs.Success);
                    if (hs.Success)
                    {
                        Log.log("对话NPC");
                        game.MouseRigthDBClick(hs.CenterPoint.X, hs.CenterPoint.Y + 70);
                        isclickSuc = true;
                    }
                }
                if (isclickSuc)
                    game.sleep(2000);
            }
        }
    }
}
