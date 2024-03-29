﻿using System;
using System.Collections.Generic;
using System.Text;

namespace KiraDX.Bot
{
    public static class Pretreatment
    {
        public static async void Pretreatment_MainBot(GroupMsg g) {
           List<Mirai_CSharp.Models.IGroupInfo>Groups=await KiraPlugin.GetGroupListAsync(g.s);
            string gl = "";
            foreach (var item in Groups)
            {
                gl += $"{item.Id};";
            }
            if (g.botid==G.BotList.Alice)
            {
                Users.BotInfo.Groups[0] = gl;
                return;
            }
            else if (g.botid == G.BotList.Nadia)
            {
                Users.BotInfo.Groups[1] = gl;
                return;
            }
            else if (g.botid == G.BotList.Calista)
            {
                Users.BotInfo.Groups[2] = gl;
                return;
            }
            else
            {
                return;
            }
        } 
    }
}
