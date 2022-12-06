using HFrameWork.SystemInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tool;

namespace qqhx
{
    /// <summary>
    /// 图片信息队列
    /// </summary>
    public class BitFindInfo
    {
        /// <summary>
        /// 图片信息
        /// </summary>
        public Bitmap bmp { get; set; }
        /// <summary>
        /// 等待信息
        /// </summary>
        public string Msg { get; set; } = "";
        /// <summary>
        /// 找到后等待时间
        /// </summary>
        public int WaitTime { get; set; } = 500;

        /// <summary>
        /// 偏移位置
        /// </summary>
        public Point Fp = Point.Empty;

        /// <summary>
        /// 是否双击
        /// </summary>
        public bool isDbClick { get; set; }
        /// <summary>
        /// 找到后退出
        /// </summary>
        public bool SucExit;
    }
    class ChannelWaitDo
    {

        private List<BitFindInfo> li = new List<BitFindInfo>();

        public void AddAction(BitFindInfo info)
        {
            li.Add(info);
        }
        public void AddImg(Bitmap bmp, string msg = "等等中...", int waitTime = 500, bool sucExit = false)
        {
            BitFindInfo bi = new BitFindInfo()
            {
                bmp = bmp,
                Msg = msg,
                WaitTime = waitTime,
                SucExit = sucExit
            };
            li.Add(bi);
        }

        public delegate void onSuc(HSearchPoint hs, BitFindInfo bf);
        public event onSuc onSucExitHandler;

        public event onSuc onSucHandler;

        public void AddImg(string path, string msg = "等待中...", int waitTime = 500, bool sucExit = false)
        {
            BitFindInfo bi = new BitFindInfo()
            {
                bmp = (Bitmap)Image.FromFile(path),
                Msg = msg,
                WaitTime = waitTime,
                SucExit = sucExit
            };
            li.Add(bi);
        }
        Thread th;
        public void Start()
        {
            PowerStop = false;
            th = new Thread(new ThreadStart(DoFind));
            th.Start();
        }


        private bool PowerStop = false;
        private void DoFind()
        {
            while (true)
            {
                if (PowerStop)
                {
                    return;
                }
                using (Bitmap bmp = ImageTool.CopyPriScreen())
                {
                    for (int i = 0; i < li.Count; i++)
                    {
                        HSearchPoint hs = ImageTool.findLikeImg(li[i].bmp, bmp);
                        if (hs.Success)
                        {
                            onSucHandler?.Invoke(hs, li[i]);
                            Thread.Sleep(li[i].WaitTime);
                            Mouse.cacheLocation();
                            Point movePint = hs.CenterPoint;
                            if (!li[i].Fp.IsEmpty)
                            {
                                movePint = new Point(movePint.X + li[i].Fp.X, movePint.Y + li[i].Fp.Y);
                            }
                            Mouse.move(movePint);
                            if (!li[i].isDbClick)
                            {
                                Mouse.leftclick();
                                Mouse.sleep(500);
                            }
                            else
                            {
                                Mouse.dbclick();
                                Mouse.sleep(100);
                            }
                            Mouse.reventLocation();
                            if (li[i].SucExit)
                            {
                                onSucExitHandler?.Invoke(hs, li[i]);
                                return;
                            }
                            break;
                        }
                    }

                }
            }
        }

        internal void Stop()
        {
            PowerStop = true;
        }
    }
}
