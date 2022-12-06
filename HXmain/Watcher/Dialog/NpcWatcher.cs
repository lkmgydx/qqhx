using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace HXmain.Watcher.Dialog
{
    abstract class NpcWatcher : ImageWatcher
    {
        public override HSearchPoint Watch(Bitmap bmp)
        {
            return base.Watch(bmp);
        }

        public bool WaitDo(MainGame game,int time = 300)
        {
            return waitSucFn(() =>
             {
                 using (var ff = game.getImg())
                 {
                     return Watch(ff).Success;
                 }
             }, 30000);
        }
    }
}
