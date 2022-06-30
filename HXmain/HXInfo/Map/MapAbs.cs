using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain.HXInfo.Map
{
    abstract class MapAbs : MapBase
    {
        static MapAbs()
        {
            //initNpc();
        }

        public  abstract MapAbs getInstance();

        protected abstract void initNpc();
    }
}
