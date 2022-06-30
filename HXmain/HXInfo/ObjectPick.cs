using HXmain.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Tool;
using WSTools.WSLog;

namespace HXmain.HXInfo
{
    public class ObjectPick
    {
        private static List<Bitmap> listBmp = new List<Bitmap>();
        static ObjectPick()
        {
            Assembly ass = Assembly.GetAssembly(typeof(ObjectPick));

            ResourceSet rs = Properties.Resources.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true);

            foreach (DictionaryEntry dic in rs)
            {
                string name = dic.Key.ToString();
                if (name.IndexOf("some_") != -1)
                {
                    try
                    {
                        listBmp.Add((Bitmap)Resources.ResourceManager.GetObject(name));
                    }
                    catch (Exception ex)
                    {
                        Log.logErrorForce("加载可拾取图片异常!", ex);
                    }
                }
            }
        }
        //{X=272,Y=207,Width=544,Height=415}
        //检测区间
        private static Rectangle rec = new Rectangle(272, 207, 544, 415);
        public static bool hasObjectWaitPick(MainGame gm)
        {
            using (Bitmap bmp = gm.getImg(rec))
            {
                return hasObjectWaitPick(bmp);
            }
        }

        private static bool hasObjectWaitPick(Bitmap bitmap)
        {
            HSearchPoint hs = ImageTool.findImageFromCenter(listBmp.ToArray(), bitmap);
            return hs.Success;
        }



        public static void startSQ(MainGame gm)
        {
            using (var bmp = gm.getImg(rec))
            {
                HSearchPoints hs = ImageTool.findImgesAll(listBmp.ToArray(), bmp);
                if (hs.IsSuc)
                {
                    List<Point> pts = new List<Point>();
                    for (int i = 0; i < hs.PointsCenter.Length; i++)
                    {
                        Point pt = new Point(hs.PointsCenter[i].X, hs.PointsCenter[i].Y);
                        Point current = gm.CurrentLocation;
                        Point fp = new Point((pt.X - rec.Width / 2) / (rec.Width / 10), (pt.Y - rec.Height / 2) / (rec.Height / 12));
                        gm.MovePoint(new Point(current.X + fp.X, current.Y + fp.Y));
                    }
                }
            }
        }

    }
}
