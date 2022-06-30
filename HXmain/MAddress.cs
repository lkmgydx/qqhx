using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain
{
    class MAddress
    {
        public long CurrentSelect
        {
            get
            {
                return 0;//235B6508
                         //21F5C0B8
            }
        }
    }

    class Win7
    {
        public long PointX
        {
            get
            {
                return 0x00192F54;
                //Address = 0x00122F6C;
            }
        }

        public long PointY
        {
            get
            {
                return PointX - 4;
            }
        }
        public long CurrentSelect
        {
            get
            {
                return 0x21F5C0B8;
            }
        }
    }

    class Win10
    {
        public long PointX
        {
            get
            {
                return 0x00122F6C;
            }
        }
        public long PointY
        {
            get
            {
                return PointX - 4;
            }
        }
        public long CurrentSelect
        {
            get
            {
                return 0x235B6508;
            }
        }
    }
}
