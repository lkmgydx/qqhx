using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSTools.WSFile;

namespace qqhx
{

    /// <summary>
    /// 坐标缓存配置
    /// </summary>
    /// 
    [WsIniRemark("窗口启动坐标")]
    class StratFormLocation : WsIniAbs
    {
        /// <summary>
        /// X坐标起始位置
        /// </summary>
        ///  
        [WsIniRemark("X坐标起始位置")]
        public string X { get; set; }

        [WsIniRemark("Y坐标起始位置")]
        public string Y { get; set; }
        [WsIniRemark("执行程序路径")]
        public string RunPath { get; set; }
        [WsIniRemark("QQ号码列表，多个以逗号,分隔")]
        public string QQNums { get; set; }
    }

    /// <summary>
    /// 位置缓存
    /// </summary>
    class LocationCache
    {
        static StratFormLocation sl = new StratFormLocation();


        static LocationCache()
        {
            X = getX(sl.X);
            Y = getX(sl.Y);
        }

        private static int getX(String key)
        {
            try
            {
                return Int32.Parse(key);
            }
            catch
            {
                return -1;
            }
        }

        private static int X = 0;
        private static int Y = 0;
        public static Point getPoint()
        {
            return new Point(X, Y);
        }

        public static void setPoint(Point p)
        {
            X = p.X;
            Y = p.Y;
            sl.X = X.ToString();
            sl.Y = Y.ToString();
            sl.save();
        }

        private static void save()
        {
            sl.save();
        }
    }
}
