using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ConsoleApplication2
{
    public class PointGroup
    {
        private List<Point> Points = new List<Point>();

        public bool ConcatPoint(Point pt)
        {
            return Points.Contains(pt);
        }

        public bool addPoint(Point p)
        {
            if (Points.Count == 0)
            {
                Points.Add(p);
                return true;
            }
            else
            {
                if (isRoundPoint(Points[0], p))
                {
                    Points.Insert(0, p);
                }
                else if (isRoundPoint(Points[Points.Count - 1], p))
                {
                    Points.Add(p);
                }
                return false;
            }
        }

        private bool isRoundPoint(Point start, Point round)
        {
            if (start == round)
            {
                return false;
            }
            else if (Math.Abs(start.X - round.X) > 1 || Math.Abs(start.Y - round.Y) > 1)
            {
                return false;
            }
            return getRound(start).Contains(round);
        }

        public static Point[] getRound(Point p)
        {
            List<Point> li = new List<Point>();
            li.Add(new Point(p.X - 1, p.Y - 1));
            li.Add(new Point(p.X, p.Y - 1));
            li.Add(new Point(p.X + 1, p.Y - 1));
            li.Add(new Point(p.X + 1, p.Y));
            li.Add(new Point(p.X + 1, p.Y + 1));
            li.Add(new Point(p.X, p.Y + 1));
            li.Add(new Point(p.X - 1, p.Y + 1));
            li.Add(new Point(p.X - 1, p.Y));
            return li.ToArray();
        }
    }
}
