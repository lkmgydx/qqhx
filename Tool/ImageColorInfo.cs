using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool
{
    public class ImageColorInfo
    {
        public Bitmap Bitmap { get; private set; }

        private List<Point> _org;
        public Point[] Points
        {
            get
            {
                return _org.ToArray();
            }
        }

        private List<Point> _offset;
        public Point[] OffSetPoints
        {
            get
            {
                if (_offset == null)
                {
                    var t = _org.ToArray();
                    ResetList(t);
                    _offset = new List<Point>(t);
                }
                return _offset.ToArray();
            }
        }

        public Color PaintColor { get; }

        public ImageColorInfo(Bitmap bmp, List<Point> points, Color color)
        {
            Bitmap = bmp;
            _org = new List<Point>(points);
            PaintColor = color;
        }

        private static void ResetList(Point[] p)
        {
            if (p == null || p.Length <= 1)
            {
                return;
            }
            int x = -p[0].X;
            int y = -p[0].Y;

            for (int i = 0; i < p.Length; i++)
            {
                var pt = p[i];
                pt.Offset(x, y);
                p[i] = pt;
            }
        }
    }
}
