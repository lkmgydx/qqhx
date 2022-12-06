using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HXmain.MainGame;

namespace HXmain.HXAction
{
    public class HomeGetSome
    {
        MainGame game;
        public HomeGetSome(MainGame game)
        {
            this.game = game;
        }

        public void WaitGet()
        {
            game.SendKey(System.Windows.Forms.Keys.Home);
            bool hasGo = false;
            bool isUseSkill = false;
            bool isSelect = false;
            while (true)
            {
                Console.WriteLine("start...");
                Task.WaitAll(Task.Run(() =>
                {
                    int k = 0;
                    LocationChangeEvent d = new LocationChangeEvent((x, f) =>
                    {
                        k++;
                        Console.WriteLine("go.....");
                    });
                    game.onLocationChange += d;
                    System.Threading.Thread.Sleep(5000);
                    game.onLocationChange -= d;
                    hasGo = k > 0;
                }), Task.Run(() =>
                {
                    //5秒内未使用技能 
                    isUseSkill = false;
                    int start = Environment.TickCount;
                    while (Environment.TickCount - start < 5000)
                    {
                        if (game.isUseSkill)
                        {
                            Console.WriteLine("not useSkilll.....");
                            isUseSkill = true;
                            return;
                        }
                        Console.WriteLine("useSkilll.....");
                        game.sleep(100);
                    }
                }), Task.Run(() =>
                {
                    isSelect = false;
                    int start = Environment.TickCount;
                    while (Environment.TickCount - start < 5000)
                    {
                        if (game.hasSelectSomeOne && !game.selectInfo.isSelectSelf)
                        {
                            Console.WriteLine("select.....");
                            isSelect = true;
                            return;
                        }
                        Console.WriteLine("select wait.....");
                        game.sleep(100);
                    }
                }));

                //await new Task(() => //5秒内未移动
                //{
                //    int k = 0;
                //    LocationChangeEvent d = new LocationChangeEvent((x, f) =>
                //    {
                //        k++;
                //        Console.WriteLine("go.....");
                //    });
                //    game.onLocationChange += d;
                //    System.Threading.Thread.Sleep(5000);
                //    game.onLocationChange -= d;
                //    hasGo = k > 0;

                //}).Start();


                //                , new Task(() => //5秒内未使用技能
                //                  {
                //                      isUseSkill = false;
                //                      int start = Environment.TickCount;
                //                      while (Environment.TickCount - start < 5000)
                //                      {
                //                          if (game.isUseSkill)
                //                          {
                //                              Console.WriteLine("not useSkilll.....");
                //                              isUseSkill = true;
                //                              return;
                //                          }
                //                          Console.WriteLine("useSkilll.....");
                //                          game.sleep(100);
                //                      }
                //                  })
                //                , new Task(() => //5秒内未选择任何可攻击对象
                //{
                //    isSelect = false;
                //    int start = Environment.TickCount;
                //    while (Environment.TickCount - start < 5000)
                //    {
                //        if (game.hasSelectSomeOne && !game.selectInfo.isSelectSelf)
                //        {
                //            Console.WriteLine("select.....");
                //            isSelect = true;
                //            return;
                //        }
                //        Console.WriteLine("select wait.....");
                //        game.sleep(100);
                //    }
                //})


                Console.WriteLine("next");
                if (!hasGo && !isUseSkill && !isSelect)
                {
                    Console.WriteLine("break");
                    break;
                }
            }
            game.SendKey(System.Windows.Forms.Keys.Home);
        }
    }
}
