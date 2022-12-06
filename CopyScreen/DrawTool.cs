using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CopyScreen.ToolBar;

namespace CopyScreen
{
    public partial class DrawTool : UserControl
    {
       
        public DrawTool()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 操作类型
        /// </summary>
        public OpModle ToolOpModel { get; private set; }

        private void extnedPic1_Click(object sender, EventArgs e)
        {
            ToolOpModel = OpModle.圆;
        }

        private void extnedPic2_Click(object sender, EventArgs e)
        {
            ToolOpModel = OpModle.矩形;
        }

        private void extnedPic3_Click(object sender, EventArgs e)
        {
            ToolOpModel = OpModle.箭头 ;
        }

        private void extnedPic4_Click(object sender, EventArgs e)
        {
            ToolOpModel = OpModle.画笔;
        }
    }
}
