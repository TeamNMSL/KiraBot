using System;
using System.Collections.Generic;
using System.Text;
using KiraDX.Bot;
using System.IO;
namespace KiraDX.otherbot.lxbot
{
    public static class NB
    {
        public static void GetNB(GroupMsg g) {
           string[] nb= File.ReadAllLines(G.path.NBpath);
            string target = nb[Functions.GetRandomNumber(0,nb.Length-1)];
            string name = $"[mirai:at:{g.fromAccount}]";
            if (g.msg.Split(" ").Length>=2)
            {
                name = g.msg.Split(" ", 2)[1];
            }
            target = target.Replace("%name%",name);
            KiraPlugin.sendMessage(g, target, true);
        }
    }
}
