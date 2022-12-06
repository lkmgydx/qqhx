using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HXmain
{
    public class PathPointUtil
    {
        public static Point[] getResourcePoint(string name)
        {
            List<Point> pt = new List<Point>();
            using (Stream fs = Assembly.GetExecutingAssembly().GetManifestResourceStream("HXmain.RunPath." + name))
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(fs))
                {
                    string[] str = sr.ReadToEnd().Split('\n');
                    for (int i = 0; i < str.Length; i++)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(str[i]))
                            {
                                continue;
                            }
                            string[] t = str[i].Replace("\r", "").Split(' ');
                            pt.Add(new Point(Int32.Parse(t[0]), Int32.Parse(t[1])));
                        }
                        catch (Exception ex)
                        {
                            Console.Write("转换异常!" + str[i], ex);
                        }
                    }
                }
            }
            return pt.ToArray();
        }


        /// <summary>
        /// 位置是否在范围内
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool isNear(Point[] pt, Point p)
        {

            for (int i = 0; i < pt.Length; i++)
            {
                if (Math.Abs(pt[i].X - p.X) < 10 && Math.Abs(pt[i].Y - p.Y) < 10)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
