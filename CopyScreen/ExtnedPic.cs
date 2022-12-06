using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CopyScreen
{
    public partial class ExtnedPic : PictureBox
    {
        public ExtnedPic()
        {
            InitializeComponent();
            this.Cursor = Cursors.Hand;
            MouseEnter += ExtnedPic_MouseEnter;
            MouseLeave += ExtnedPic_MouseLeave;
            SizeMode = PictureBoxSizeMode.StretchImage;
        }


        public event EventHandler onActChange;
        private bool _act;
        /// <summary>
        /// 是否为激动状态
        /// </summary>
        public bool Act
        {
            get { return _act; }
            set
            {
                if (value)
                {
                    base.Image = _overImg;
                }
                else
                {
                    base.Image = _Img;
                }
                _act = value;
                onActChange?.Invoke(this, null);
            }
        }

        private void ExtnedPic_MouseLeave(object sender, EventArgs e)
        {
            Act = false;
        }

        private void ExtnedPic_MouseEnter(object sender, EventArgs e)
        {
            Act = true;
        }

        private Image _overImg;

        [Browsable(true), Description("鼠标移上去时显示的图片")]
        public Image OverImg { get { return _overImg; } set { _overImg = value; } }

        private Image _Img;
        public new Image Image { get { return base.Image; } set { _Img = value; base.Image = value; } }
    }
}
