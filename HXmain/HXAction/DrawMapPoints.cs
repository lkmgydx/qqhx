using HFrameWork.SystemInput;
using HXmain.HXInfo.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain.HXAction
{
    public class DrawMapPoints
    {
        MapPoints mp;
        private bool stop = false;

        MainGame game;
        public DrawMapPoints(MainGame game)
        {
            MapBase map = game.CurrentMap;
            mp = new MapPoints(map.MaxWidth, map.MaxHeigth);
            game.onLocationChange += Game_onLocationChange;
            map.MapPoints = mp;
            game.onPowerStop += Game_onPowerStop;
            this.game = game;
        }

        private void Game_onPowerStop()
        {
            stop = true;
        }

        private void Game_onLocationChange(System.Drawing.Point before, System.Drawing.Point now)
        {
            mp[now.X, now.Y] = true;
        }

        public void Start()
        {
            while (true || !stop)
            {
                game.movePersion.RadomMove();
                Mouse.sleep(1000);
            }

        }
    }
}
