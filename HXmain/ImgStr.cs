using HXmain.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain
{
    public class ImgStrCls
    {
        public string this[string index]
        {
            get { return "123"; }

            private set
            {
            }
        }

        public Bitmap bmpCF {
          get { return Resources.Npc_cf; }
        }
    }
}
