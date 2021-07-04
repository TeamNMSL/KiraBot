using System;
using System.Collections.Generic;
using System.Text;

namespace KiraDX.Bot.Mod_System
{
    static class UnWhiteExit
    {
       static public void TD(GroupMsg g) {
            if (g.fromAccount==G.BotList.Nadia||g.fromAccount==G.BotList.Alice||g.fromAccount==G.BotList.Calista)
            {
                return;
            }
            KiraPlugin.sendMessage(g, "该群未走规范的拉群流程，若需要正常使用bot，请参阅这个页面，请复制到浏览器查看http://blogs.mizunas.com/Bot/");
            System.Threading.Thread.Sleep(10000);
            g.s.LeaveGroupAsync(g.fromGroup);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"群{g.fromGroup}不是白名单,已退出");
            Console.ResetColor();

        }
    }
}
