using HXmain.HXInfo.CacheData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXmain.HXAction
{
    public class SelectInfo
    {
        /// <summary>
        /// 是否有选择范围
        /// </summary>
        private static readonly System.Drawing.Rectangle hasSelectRec = new System.Drawing.Rectangle(290, 86, 6, 9);
        private static string hasSelectHash = "750FEFE96A3B53F602E6FE0FD3007280";


        private static readonly System.Drawing.Rectangle SelectRec = new System.Drawing.Rectangle(297, 85, 106, 15);
        /// <summary>
        /// 选中对象整个区域
        /// </summary>
        private static readonly Rectangle SelectSlefRec = new Rectangle(305, 85, 95, 12);

        private MainGame game;

        static SelectInfo()
        {
            //SelectPersonData.addNpcInfo(new SelectPersonData()
            //{
            //    Key = "D7CECD58087736798728710D596AD3D1",
            //    Value = "炎灵兽"
            //});
            //SelectPersonData.addNpcInfo(new SelectPersonData()
            //{
            //    Key = "23402135F179907505D141144E8C099B",
            //    Value = "魅影"
            //});
            //SelectPersonData.addNpcInfo(new SelectPersonData()
            //{
            //    Key = "37F1FAF9F3F7D86300F9711687ADF6F0",
            //    Value = "煌叶偏将"
            //});
            //SelectPersonData.addNpcInfo(new SelectPersonData("65749870D335B1E17D2C5F5A42465B4C", "煌叶喽罗"));
            //SelectPersonData.addNpcInfo(new SelectPersonData("448B20ADE11688AEB930F61D40FAC065", "猎蜥"));
            //SelectPersonData.addNpcInfo(new SelectPersonData("31DBADB9CA0F74D37E022EAF8704E10D", "蛮淘"));
            //SelectPersonData.save();
        }

        public string SelectName
        {
            get
            {
                if (!hasSelect())
                {
                    return "";
                }
                return Tool.ImageCacheTool.getOrcText(game.getImg(SelectRec));
            }
        }

        /// <summary>
        /// 选择自身时的信息
        /// </summary>
        public string SelfKey { get; private set; }

        public SelectInfo(MainGame game)
        {
            this.game = game;
        }

        /// <summary>
        /// 是否选择了自已
        /// </summary>
        public bool isSelectSelf
        {
            get
            {
                if (SelfKey == null)
                {
                    game.MouseClick(75, 95);
                    SelfKey = game.UUIDImageRec(SelectSlefRec);
                    unSelect();
                    game.sleep(500);
                }
                return game.UUIDImageRec(SelectSlefRec) == SelfKey;
            }
        }

        public void unSelect()
        {
            //game.MouseClick(1040, 35);
            game.MouseRigthClick(1040, 50);
        }

        /// <summary>
        /// 是否有选中
        /// </summary>
        /// <returns></returns>
        public bool hasSelect()
        {
            return game.UUIDImageRec(hasSelectRec) == hasSelectHash;
        }
    }
}
