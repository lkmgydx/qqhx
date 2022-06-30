using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ConsoleApplication2
{
    class SearchSplitImg
    {
        private List<PointGroup> li = new List<PointGroup>();
        private PointGroup mainGroup = new PointGroup();
        private Point StartPoint;
        private Bitmap mainPic;
        public SearchSplitImg(Point start, Bitmap bmp)
        {
            mainGroup.addPoint(start);
            StartPoint = start;
            mainPic = bmp;
            li.Add(mainGroup);
        }

        public List<PointGroup> getRoups()
        {
            return getGroups(StartPoint);
        }

        public List<PointGroup> getGroups(Point pt) {
            Point[] pts = PointGroup.getRound(StartPoint);
            int count = 0;
            foreach (var item in pts)
            {
                if (mainPic.GetPixel(item.X, item.Y).Name == "ffffff")
                {
                    count++;
                }
            }
            return li;
        }

        public bool Next(Point pt)
        {
            for (int i = 0; i < li.Count; i++)
            {

            }
            return true;
        }

        public bool isSplit(Point pt)
        {

            return false;
        }

    }
}
