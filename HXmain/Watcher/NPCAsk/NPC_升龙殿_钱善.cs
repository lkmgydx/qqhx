using HXmain.Properties;
using HXmain.Watcher.Dialog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace HXmain.Watcher.NPCAsk
{
    class NPC_升龙殿_钱善 : ImageWatcher
    {

        class 出力任务 : ImageWatcher
        {
            public override HSearchPoint Watch(Bitmap bmp)
            {
                return base.Watch(bmp);
            }
        }

        public bool 我要出力(MainGame game)
        {
            return waitSucFn(() =>
           {
               var suc = false;
               NPC_升龙殿_钱善 t = getNpcWatcher<NPC_升龙殿_钱善>(game);
                
                
               var b = getWatcher<font_我知道了>((s, x) =>
               {
                   game.MouseClick(x.CenterPoint);
                   suc = true;
               });
               
               var f = getWatcher<Win钱善>((s) =>
               {
                   if (s.一键完成(game) || s.我要出力(game) || s.花5官银重置(game) || s.花5官银重置2(game) || s.花20官银重置2(game) || s.花20官银重置(game))
                   {
                   }
                   else
                   {
                       b.Watch(s.WatcherGameBmp);
                   }
               });

               using (var bmp = game.getImg())
               {
                   f.Watch(bmp);
                   t.Watch(bmp);
               }
               return suc;
           }, 160000);
        }
        protected override Bitmap WatcherBmp
        {
            get
            {
                return Resources.npc_钱善;
            }
        }

        class font_我知道了 : ImageWatcher
        {
            protected override Bitmap WatcherBmp
            {
                get
                {
                    return Resources.font_钱善_我知道了;
                }
            }
        }
    }
}
