using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace KiraDX.Bot.Mod_System
{
    class setMain
    {
        public static void mainbot(GroupMsg g,string bottype) {
            if (g.botid==G.BotList.Calista)
            {
                return;
            }
            if (bottype=="Alice")
            {
               
                if (g.botid==G.BotList.Nadia)
                {
                    File.WriteAllText($"{G.path.Apppath}{G.path.MainBotData}{g.fromGroup}.kira", "Alice");
                    Users.Info.GetGroupConfig(g.fromGroup);
                    Users.Info.GroupInfo[g.fromGroup].mainbot = "Alice";
                    KiraPlugin.SendGroupMessage(g.s,g.fromGroup,"已切换响应bot为Alice，走了");
                }
                else
                {
                    File.WriteAllText($"{G.path.Apppath}{G.path.MainBotData}{g.fromGroup}.kira", "Alice");
                    Users.Info.GetGroupConfig(g.fromGroup);
                    Users.Info.GroupInfo[g.fromGroup].mainbot = "Alice";
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "已切换响应bot为Alice，我来力");
                }

                return;
            }
            else if (bottype == "Nadia")
            {
                if (g.botid == G.BotList.Alice)
                {
                    File.WriteAllText($"{G.path.Apppath}{G.path.MainBotData}{g.fromGroup}.kira", "Nadia");
                    Users.Info.GetGroupConfig(g.fromGroup);
                    Users.Info.GroupInfo[g.fromGroup].mainbot = "Nadia";
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "已切换响应bot为Nadia，走了");
                }
                else
                {
                    File.WriteAllText($"{G.path.Apppath}{G.path.MainBotData}{g.fromGroup}.kira", "Nadia");
                    Users.Info.GetGroupConfig(g.fromGroup);
                    Users.Info.GroupInfo[g.fromGroup].mainbot = "Nadia";
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "已切换响应bot为Nadia，我来力");
                }
            }
        }
    }
}
