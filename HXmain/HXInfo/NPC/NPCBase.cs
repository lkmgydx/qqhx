using HXmain.HXAction;
using HXmain.HXInfo.CacheData;
using HXmain.HXInfo.Map;
using HXmain.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;
using WSTools.WSLog;

namespace HXmain.HXInfo.NPC
{
    /// <summary>
    /// NPC信息
    /// </summary>
    public class NPCBase
    {
        /// <summary>
        /// NPC所在地图
        /// </summary>
        public MapBase Map { get; private set; }
        /// <summary>
        /// NPC名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// NPC所在地图位置
        /// </summary>
        public Point Position { get; private set; }
        /// <summary>
        /// NPC所匹配的图片信息
        /// </summary>
        public NpcData MachPic { get; private set; }

        public NPCBase(MapBase map, string name, Point point)
        {
            this.Map = map;
            this.Name = name;
            this.Position = point;
            this.MachPic = NpcData.getNpcInfo(map.Name, name);
        }

        protected virtual void initNpc() { }

        public void MoveTo(MainGame game)
        {
            if (game.CurrentMap != Map)
            {
                return;
            }
            Log.LogToConsole = true;
            Log.LogEnable = true;
            game.movePersion.GoToPoint(Position.X - 2, Position.Y - 2);
        }

        public void MoveTo(MainGame game, string name)
        {
            if (game.CurrentMap != this.Map)
            {
                return;
            }
            game.isXUNHUAN = false;
            var path = PathPointUtil.getResourcePoint("天圣原路径.txt");
            //天圣原路径 驿站车夫3
            Point realPoint = this.Position;
            Map天圣原 天圣原Map = MapBase.Maps.天圣原;
            if (天圣原Map.NPC_驿站车夫 == this || 天圣原Map.NPC_驿站车夫2 == this || 天圣原Map.NPC_驿站车夫3 == this)
            {
                Point nowPosition = game.CurrentLocation;
                if (nowPosition.X > 180)
                {
                    realPoint = 天圣原Map.NPC_驿站车夫2.Position;
                    if (nowPosition.Y > 339)
                    {
                        realPoint = 天圣原Map.NPC_驿站车夫.Position;
                    }
                }
                else
                {
                    if (nowPosition.Y < 333)
                    {
                        path = PathPointUtil.getResourcePoint("天圣原路径 驿站车夫3.txt");
                        realPoint = 天圣原Map.NPC_驿站车夫3.Position;
                    }
                    else
                    {
                        realPoint = 天圣原Map.NPC_驿站车夫.Position;
                    }
                }
            }
            game.MovePoint(realPoint, path, new Action(() =>
            {
                redo:
                System.Threading.Thread.Sleep(1000);
                bool isSuc = BaseAction.waitDialogFontSingle(game, this.MachPic);
                if (!isSuc)
                {
                    return;
                }
                if (!name.StartsWith("move"))
                {
                    name = "move_" + name;
                }
                Bitmap bmp = (Bitmap)Resources.ResourceManager.GetObject(name, Resources.Culture);
                if (bmp == null)
                {
                    return;
                }
                var hs = BaseAction.waitDialogFontImg(game, bmp, true);
                if (hs.Success)
                {
                    System.Threading.Thread.Sleep(1000);
                    if (game.CurrentLocation.X > 200)
                    {
                        game.SendKey(System.Windows.Forms.Keys.D0);
                        System.Threading.Thread.Sleep(1000);
                    }
                    天圣原Map.NPC_驿站车夫.MoveTo(game, "祭台");
                    return;
                }
                else
                {
                    goto redo;
                }
            }));
        }

        public void bySome(MainGame game)
        {
            if (game.CurrentMap != this.Map)
            {
                return;
            }
            game.isXUNHUAN = false;
            game.MovePoint(new Point(this.Position.X, this.Position.Y), PathPointUtil.getResourcePoint("天圣原路径.txt"), new Action(() =>
            {
                bool isSuc = BaseAction.waitDialogFontSingle(game, this.MachPic);
                if (!isSuc)
                {
                    return;
                }
                var hs = BaseAction.waitDialogFontImg(game, Resources.font_看看有什么好东西);
                if (hs.Success)
                    game.MouseClick(hs.CenterPoint);
                else
                {
                    "".ToString();
                }
            }));
        }

       

        public void Ask(MainGame game)
        {
            if (game.CurrentMap != this.Map)
            {
                return;
            }
            game.isXUNHUAN = false;
            game.MovePoint(new Point(this.Position.X, this.Position.Y), PathPointUtil.getResourcePoint("天圣原路径.txt"), new Action(() =>
            {
                //System.Threading.Thread.Sleep(5000);
                bool isSuc = BaseAction.waitDialogFontSingle(game, this.MachPic);
                if (!isSuc)
                {
                    return;
                }
                var hs = BaseAction.waitDialogFontImg(game, Resources.font_看看有什么好东西);
                // WSTools.WSWinFn.ShowInfo("成功");
                if (hs.Success)
                    game.MouseClick(hs.CenterPoint);
                else
                {
                    "".ToString();
                }
            }));
        }
    }
}
