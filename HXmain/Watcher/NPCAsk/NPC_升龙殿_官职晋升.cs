using HXmain.Properties;
using HXmain.Watcher.Dialog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain.Watcher.NPCAsk
{
    class NPC_升龙殿_官职晋升 : ImageWatcher
    {
        public bool 官职任务(MainGame game)
        {
            return WaitDo(game, () =>
            {
                bool suc = false;
                var a1 = getWatcher<NPC_升龙殿_官职晋升>((s, e) =>
                {
                    var tp = e.CenterPoint;
                    tp.Offset(0, 70);
                    game.MouseRigthClick(tp);
                });

                var a2 = getWatcher<Win官职晋升>((v, x) =>
                {
                    v.Ask晋升官职(game);
                });

                var a3 = getWatcher<Font_我知道了>((s, e) =>
                {
                    game.MouseClick(e.CenterPoint);
                    suc = true;
                    game.closeAllGameWin();
                });

                using (Bitmap bmp = game.getImg())
                {
                    a1.Watch(bmp);
                    a3.Watch(bmp);
                    a2.Watch(bmp);
                }
                return suc;
            }, 30000);
        }

        public bool 领取俸禄(MainGame game)
        {


            return WaitDo(game, () =>
            {
                bool suc = false;
                var a1 = getNpcWatcher<NPC_升龙殿_官职晋升>(game);

                var a2 = getWatcher<Win官职晋升>((v, x) =>
                  {
                      suc = v.Ask领取俸禄(game);
                  });

                using (Bitmap bmp = game.getImg())
                {
                    a1.Watch(bmp);
                    a2.Watch(bmp);
                }
                return suc;
            }, 15000);
        }
        protected override Bitmap WatcherBmp
        {
            get
            {
                return Resources.NPC_官职晋升;
            }
        }

        class Font_我知道了 : ImageWatcher
        {
            protected override Bitmap WatcherBmp
            {
                get
                {
                    return Resources.font_我知道了;
                }
            }
        }
    }
}
