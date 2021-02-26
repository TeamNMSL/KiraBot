using System;
using System.Collections.Generic;
using System.Text;

namespace KiraDX.Bot.Picture.Pic
{
    static class GetPic
    {
        /// <summary>
        /// 随机图片并发送
        /// </summary>
        /// <param name="vs"></param>
        /// <param name="type"></param>
        /// <param name="isFunc">是否鉴权，真就作为全局函数使用(不判断开关)</param>
        static public void getPic(GroupMsg vs,string type,bool isFunc) {

            try
            {
                string Path;
                if (isFunc)
                {
                    Path = Functions.Random_File(G.path.Apppath + G.path.Pic + type);
                    KiraPlugin.SendGroupPic(vs.s, vs.fromGroup, Path);
                    return;
                }

                if (!BotFunc.FuncSwith(vs, "图片"))
                {
                    KiraPlugin.SendGroupMessage(vs.s, vs.fromGroup, $"該群的图片模塊暫未打開，如有需要，請使用/k mod enable 图片 來打開");
                    return;
                }
                if (!BotFunc.FuncSwith(vs,type) )
                {
                    KiraPlugin.SendGroupMessage(vs.s, vs.fromGroup, $"該群的{type}模塊暫未打開，如有需要，請使用/k mod enable {type} 來打開");
                    return;
                }
                Path = Functions.Random_File(G.path.Apppath + G.path.Pic + type);
                KiraPlugin.SendGroupPic(vs.s, vs.fromGroup, Path);
            }
            catch (Exception e)
            {

                KiraPlugin.SendGroupMessage(vs.s, vs.fromGroup, e.Message);
            }

        }

    }
}
