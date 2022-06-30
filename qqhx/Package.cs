using HXmain.HXAction;
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
    public partial class Package : Form
    {
        public Package()
        {
            InitializeComponent();
        }

        public HXmain.MainGame game { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            if (game == null) return; 
            var point = TiLian.show提练Win(game);
            WuPingLanInfo wi = new WuPingLanInfo(game);
            wi.WuPings();
        }
    }
}
