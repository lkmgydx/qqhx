using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyScreen.Draw.Baseinfo
{
    class HDrag
    { 
        /// <summary>
        /// 实际矩形区域
        /// </summary>
        public Rectangle DragRectangle { get; set; }

        private Point _start { get; set; }
        public Point Start { get { return _start; } set { _start = value; refresh(); } }

        private Point _end { get; set; }
        public Point End { get { return _end; } set { _end = value; refresh(); } }

        public HDrag() : this(Point.Empty, Point.Empty)
        {
        }

        public HDrag(Point start, Point end)
        {
            _start = start;
            _end = end;
            refresh();
        }

        private void refresh()
        {
            DragRectangle = Utils.getRec(_start, _end);
        }
    }
}
