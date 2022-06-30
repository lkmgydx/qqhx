using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool
{
    /// <summary>
    /// 搜索图像列表
    /// </summary>
    public class HSearchPoints
    {
        private bool _isSuccess;
        List<Point> _leftUpPoints = new List<Point>();
        List<Point> _centerPoints = new List<Point>();
        private int times;

        public HSearchPoints(bool success, List<Point> points, List<Point> centerPoints, int times)
        {
            this._isSuccess = success;
            if (points != null)
            {
                _leftUpPoints.AddRange(points);
            }
            if (centerPoints != null)
            {
                _centerPoints.AddRange(centerPoints);
            }
            this.times = times;
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuc
        {
            get
            {
                return _isSuccess;
            }
        }

        /// <summary>
        /// 左上角开始匹配的点
        /// </summary>
        public Point[] Points
        {
            get
            {
                return _leftUpPoints.ToArray();
            }
        }
        /// <summary>
        /// 居中所有点
        /// </summary>
        public Point[] PointsCenter
        {
            get
            {
                return _centerPoints.ToArray();
            }
        }
        /// <summary>
        /// 总计耗时 
        /// </summary>
        public int Time
        {
            get
            {
                return times;
            }
        }
    }
}
