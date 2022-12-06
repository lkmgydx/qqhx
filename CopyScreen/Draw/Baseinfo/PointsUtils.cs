using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyScreen.Draw.Baseinfo
{
    sealed class PointsUtils
    {
        public static Point[] getAll(Rectangle rec, int offeset = -1, int borderSize = 2)
        {
            List<Point> _liRegionPoints = new List<Point>();
            for (int i = rec.X + offeset; i < rec.Width + offeset + borderSize; i++)
            {
                for (int j = rec.Y + offeset; j < rec.Height + offeset + borderSize; j++)
                    _liRegionPoints.Add(new Point(i, j));
            }
            return _liRegionPoints.ToArray();
        }
    }
}
