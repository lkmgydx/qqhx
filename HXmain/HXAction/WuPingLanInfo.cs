using HXmain.Properties;
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
    public class WuPingLanInfo
    {
        //240* 240
        //40*40,36+4,38+2
        //34 + 8 = 42
        //
        //0-36 [8] 44-80 [] 88- 
        //36*i+8
        // private Rectangle rec = new Rectangle(6, 43, 32, 32);
        private Rectangle rec = new Rectangle(6, 43, 32, 20);

        private WuPing[][] _info = new WuPing[6][];
        public WuPing[][] Info { get; }
        static WuPingLanInfo()
        {

        }

        private static bool isEmpty(Bitmap bmp)
        {
            return ImageTool.findLikeImg(Resources.wuping_empty, bmp, 90).Success;
        }

        MainGame game = null;
        public WuPingLanInfo(MainGame game)
        {
            this.game = game;
        }




        public List<WuPingData> TilianAllForce()
        {

            if (game.ESCSTOP)
            {
                return new List<WuPingData>();
            }

            List<WuPingData> tilians = new List<WuPingData>();
            try
            {
                tilians = getCanTiLianCount();
                Log.logForce("可提练数量:" + tilians.Count);
            }
            catch (Exception ex)
            {
                Log.logError("getCanTilian == null", ex);
            }
            if (tilians.Count == 0)
            {
                game.closeAllGameWin();
                return new List<WuPingData>(); ;
            }
            List<WuPingData> lirst = new List<WuPingData>();
            var hsv = TiLian.show提练Win(this.game);
            if (!hsv.Success)
            {
                return lirst;
            }
            var hs = WuPing.openWuPing(game);

            if (hs.Success)
            {
                Point leftTop = new Point(hs.Point.X + rec.X, hs.Point.Y + rec.Y);
                using (Bitmap btemp = game.getImg())
                {
                    for (int j = 0; j < 6; j++)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            var rr = new Rectangle(leftTop.X + (i * 41), leftTop.Y + (j * 41), rec.Width, rec.Height);
                            using (Bitmap bt = btemp.Clone(rr, btemp.PixelFormat))
                            {
                                if (game.ESCSTOP)
                                {
                                    return lirst;
                                }
                                WuPingData wp = WuPingData.getWuping(bt);
                                int x = rr.Left + 10;
                                int y = rr.Top + 10;
                                bool add = false;
                                if (wp != null && wp.isThrow)//丢掉可丢物品
                                {
                                    doThrow(x, y);
                                    lirst.Add(null);
                                    continue;
                                }
                                if (wp == null)
                                {
                                    add = doTilian(x, y);
                                }
                                else if (!wp.isEmpty && wp.CanTiLian)
                                {
                                    add = doTilian(x, y);
                                    if (!add)
                                    {
                                        doThrow(x, y);
                                    }
                                }
                                if (add)
                                    lirst.Add(null);
                                else
                                {
                                    lirst.Add(wp);
                                }
                            }
                        }
                    }
                }
            }
            game.sleep(100);
            game.MouseRigthClick();//取消选中
            game.closeAllGameWin();
            return lirst;
        }

        public List<WuPingData> getCanTiLianCount()
        {
            List<WuPingData> liRst = new List<WuPingData>();
            List<WuPingData> liAll = this.WuPings();
            foreach (var it in liAll)
            {
                try
                {
                    if (it.CanTiLian)
                    {
                        liRst.Add(it);
                    }
                    else
                    {
                        Log.log(it.ImgID + "_" + it.Name + "_EMPTY?" + it.isEmpty + " isZB-" + it.isZuangBei);
                    }
                }
                catch (Exception ex)
                {
                    Log.logError("eff", ex);
                }

            }
            return liRst;
        }

        private bool doTilian(int x, int y)
        {
            game.sleep(100);
            game.MouseRigthClick(new Point(50, 50));//取消选中
            game.sleep(100);
            game.MouseClick(new Point(x, y));
            return TiLian.TiLianSome(game);
        }

        private void doThrow(int x, int y)
        {
            game.MouseRigthClick(new Point(50, 50));//取消选中
            game.sleep(100);
            game.MouseClick(new Point(x, y));
            game.sleep(100);
            game.MouseClick(new Point(150, 130));
            game.sleep(500);
            game.SendKey(System.Windows.Forms.Keys.Space);
            game.sleep(100);
            game.SendKey(System.Windows.Forms.Keys.Space);
            game.sleep(100);
        }

        public List<WuPingData> TilianAll()
        {
            List<WuPingData> lirst = new List<WuPingData>();
            var hsv = TiLian.show提练Win(this.game);
            if (!hsv.Success)
            {
                return lirst;
            }
            var hs = WuPing.openWuPing(this.game);
            if (hs.Success)
            {
                Point leftTop = new Point(hs.Point.X + rec.X, hs.Point.Y + rec.Y);
                using (Bitmap btemp = game.getImg())
                {
                    for (int j = 0; j < 6; j++)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            var rr = new Rectangle(leftTop.X + (i * 41), leftTop.Y + (j * 41), rec.Width, rec.Height);
                            using (Bitmap bt = btemp.Clone(rr, btemp.PixelFormat))
                            {
                                WuPingData wp = WuPingData.getWuping(bt);
                                if (wp != null && wp.CanTiLian)
                                {
                                    game.MouseClick(new Point(rr.Left + 10, rr.Top + 10));
                                    bool suc = TiLian.TiLianSome(game);
                                    if (suc)
                                        lirst.Add(wp);
                                    else
                                    {
                                        lirst.Add(null);
                                    }
                                }
                                else
                                {
                                    lirst.Add(null);
                                }
                            }
                        }
                    }
                }
            }
            return lirst;
        }

        public List<WuPingData> WuPings()
        {
            List<WuPingData> lirst = new List<WuPingData>();
            var hs = WuPing.openWuPing(this.game);
            if (hs.Success)
            {
                Point leftTop = new Point(hs.Point.X + rec.X, hs.Point.Y + rec.Y);
                using (Bitmap btemp = game.getImg())
                {
                    for (int j = 0; j < 6; j++)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            var rr = new Rectangle(leftTop.X + (i * 41), leftTop.Y + (j * 41), rec.Width, rec.Height);
                            using (Bitmap bt = btemp.Clone(rr, btemp.PixelFormat))
                            {
                                var data = WuPingData.getWuping(bt);
                                if (data != null)
                                {
                                    lirst.Add(data);
                                }
                            }
                        }
                    }
                }
            }
            return lirst;
        }

        public List<WuPingData> getEmpty()
        {
            List<WuPingData> liRst = new List<WuPingData>();
            List<WuPingData> liAll = this.WuPings();
            foreach (var it in liAll)
            {
                if (it.isEmpty)
                {
                    liRst.Add(it);
                }
            }
            return liRst;
        }

        private Bitmap copyImge(Bitmap bmp, int x, int y)
        {
            return null;
            // return bmp.Clone(new Rectangle(x, y, 36, 36),System.Drawing.Imaging.PixelFormat.for);
        }

    }
}
