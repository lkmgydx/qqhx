using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain.HXAction
{
    class ActionFactory<T> where T : BaseAction
    {
        public void start(T t)
        {
            if (t.ActionType == ActionType.LeftClick)
            {

            }
        }
    }
}
