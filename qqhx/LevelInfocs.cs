using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace qqhx
{
    public partial class LevelInfocs : UserControl
    {
        public LevelInfocs()
        {
            InitializeComponent();
        }

        internal void DisCheckAll()
        {
            c1.Checked = false;
            c2.Checked = false;
            c3.Checked = false;
            c4.Checked = false;
        }
    }
}
