using Mirai_CSharp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KiraDX.Bot.Mirai
{
    class PicInfo
    {
       

        public static string GetInfo(GroupMsg g, IGroupMessageEventArgs e)
        {
            string r = "PictureInfo:";
            foreach (var item in e.Chain)
            {
                if (item.Type== "Image")
                {
                    r += '\n'+((Mirai_CSharp.Models.ImageMessage)item).Url;
                }
            }
            return r;
        }
    }
}
