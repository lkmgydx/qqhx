using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace qqhx
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int size = getNum(txt_size, 12);
            int ra = getNum(txt_ra, 0);
            
            var g = this.panel1.CreateGraphics();
            panel1.Refresh();
            g.RotateTransform(ra);
            g.DrawString(this.textBox1.Text, new Font("宋体", size), Brushes.Red, new Point(getNum(txt_x,0), getNum(txt_y, 0)));
          
        }

        private int getNum(TextBox txt, int defaultNum) {
            int rst = defaultNum;
            try
            {
                rst = Convert.ToInt32(txt.Text);
            }
            catch 
            {
            }
            return rst;
        }
    }
}
