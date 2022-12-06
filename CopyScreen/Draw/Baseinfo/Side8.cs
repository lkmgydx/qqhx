using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyScreen.Draw.Baseinfo
{
    /// <summary>
    /// 8个边框
    /// </summary>
    internal class Side8
    {
        public Rectangle lt { get; }
        public Rectangle rt { get; }
        public Rectangle lb { get; }
        public Rectangle rb { get; }


        public Rectangle t { get; }
        public Rectangle l { get; }

        public Rectangle r { get; }
        public Rectangle b { get; }


        public Rectangle top { get; }
        public Rectangle left { get; }
        public Rectangle right { get; }
        public Rectangle bottom { get; }

        public Rectangle mainRec { get; }

        /// <summary>
        /// 距离多少宽度
        /// </summary>
        private int MWith { get; }
        /// <summary>
        /// 距离多少高度
        /// </summary>
        private int MHeight { get; }

        /// <summary>
        /// 8个位置宽度
        /// </summary>
        public int Width { get; }
        /// <summary>
        /// 8个位置点的高度
        /// </summary>
        public int Height { get; }

        public Side8(Rectangle rec, int width = 16, int height = 16)
        {
            Width = width;
            Height = height;
            MWith = width / 2;
            MHeight = height / 2;
            lt = new Rectangle(rec.X - MWith, rec.Y - MHeight, width, height);
            rt = new Rectangle(rec.X + rec.Width - MWith, rec.Y - MHeight, width, height);
            lb = new Rectangle(rec.X - MWith, rec.Y + rec.Height - MHeight, width, height);
            rb = new Rectangle(rec.X + rec.Width - MWith, rec.Y + rec.Height - MHeight, width, height);

            t = new Rectangle(rec.X + rec.Width / 2 - MWith, rec.Y - MHeight, width, height);
            l = new Rectangle(rec.X - MWith, rec.Y + rec.Height / 2 - MHeight, width, height);
            r = new Rectangle(rec.X + rec.Width - MWith, rec.Y + rec.Height / 2 - MHeight, width, height);
            b = new Rectangle(rec.X + rec.Width / 2 - MWith, rec.Y + rec.Height - MHeight, width, height);

            //只有点才能移动
            //top = new Rectangle(rec.X + rec.Width / 2 - MWith, rec.Y - MHeight, width, height);
            top = new Rectangle(rec.X - MWith, rec.Y - MHeight, rec.Width + MWith, height);
            left = new Rectangle(rec.X - MWith, rec.Y - MHeight, width, rec.Height + height);

            right = new Rectangle(rec.X + rec.Width - MWith, rec.Y - MHeight, width, rec.Height + height);
            bottom = new Rectangle(rec.X - MWith, rec.Y + rec.Height - MHeight, rec.Width + width, height);

            mainRec = rec;
        }
    }
}
