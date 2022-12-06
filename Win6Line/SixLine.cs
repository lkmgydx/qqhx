using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Win6Line
{
    public partial class SixLine : UserControl
    {
        public SixLine()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 六边形边框大小
        /// </summary>
        public int BWith { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            int h2 = BWith / 2;
            int h4 = h2 / 2;
            GraphicsPath gp = new GraphicsPath();
            Point[] pts = new Point[] {
                new Point(0, h4),
                 new Point(h2, 0),
                 new Point(BWith,h2),
                 new Point(BWith,BWith+h2),

                 new Point(h2,BWith+h2),
                 new Point()
            };
            gp.AddPolygon(pts);
            Region = new Region(gp);
            // this.Region = new Region(new System.Drawing.Drawing2D.RegionData())
        }
    }
}
