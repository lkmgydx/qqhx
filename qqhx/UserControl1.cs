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
    public partial class UserControl1 : UserControl
    {
        WSTools.WSThread.WsSyn syn;
        public UserControl1()
        {

            InitializeComponent();
            syn = new WSTools.WSThread.WsSyn(this);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        public bool qEnable
        {
            get
            {
                return checkBoxX1.Checked;
            }
            set
            {
                syn.Run(() =>
                {
                    checkBoxX1.Checked = value;
                });
            }
        }

        public Image qImg
        {
            get
            {
                return pictureBox1.Image;
            }
            set
            {
                pictureBox1.Image = value;
            }
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {

        }

        public enum ActionModel
        {
            Img, Posiotion, RecImg
        }

        public ActionModel Model { get; private set; }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Model = (ActionModel)comboBox1.SelectedIndex;
            }
            catch (Exception)
            {
                 
            }
        }
    }
}
