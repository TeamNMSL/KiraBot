using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace KiraDX.Bot.Mod_System
{
    class setMain
    {
        public static void mainbot(GroupMsg g,string bottype) {
            if (g.botid==G.BotList.Miffy)
            {
                return;
            }
            if (bottype=="Soffy")
            {
               
                if (g.botid==G.BotList.Laffy)
                {
                    File.WriteAllText($"{G.path.Apppath}{G.path.MainBotData}{g.fromGroup}.kira", "Soffy");
                    Users.Info.GetGroupConfig(g.fromGroup);
                    Users.Info.GroupInfo[g.fromGroup].mainbot = "Soffy";
                    KiraPlugin.SendGroupMessage(g.s,g.fromGroup,"已切换响应bot为Soffy，走了");
                }
                else
                {
                    File.WriteAllText($"{G.path.Apppath}{G.path.MainBotData}{g.fromGroup}.kira", "Soffy");
                    Users.Info.GetGroupConfig(g.fromGroup);
                    Users.Info.GroupInfo[g.fromGroup].mainbot = "Soffy";
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "已切换响应bot为Soffy，我来力");
                }

                return;
            }
            else if (bottype == "Laffy")
            {
                if (g.botid == G.BotList.Soffy)
                {
                    File.WriteAllText($"{G.path.Apppath}{G.path.MainBotData}{g.fromGroup}.kira", "Laffy");
                    Users.Info.GetGroupConfig(g.fromGroup);
                    Users.Info.GroupInfo[g.fromGroup].mainbot = "Laffy";
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "已切换响应bot为Laffy，走了");
                }
                else
                {
                    File.WriteAllText($"{G.path.Apppath}{G.path.MainBotData}{g.fromGroup}.kira", "Laffy");
                    Users.Info.GetGroupConfig(g.fromGroup);
                    Users.Info.GroupInfo[g.fromGroup].mainbot = "Laffy";
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "已切换响应bot为Laffy，我来力");
                }
            }
        }
    }
}
