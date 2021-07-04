using KiraDX.Frame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KiraDX.Bot.Mod_System
{
    static public class help
    {
        static public void  GetHelp(GroupMsg g) {
            if (File.Exists($"{ G.path.Apppath}{ G.path.help}{g.msg.Replace("/c help ", "")}.txt"))
            {
                KiraPlugin.sendMessage(g, File.ReadAllText($"{ G.path.Apppath}{ G.path.help}{g.msg.Replace("/c help ", "")}.txt"), true);
            }
            else
            {
                KiraPlugin.sendMessage(g, "欢迎使用ChocolateBot，这里是完整的什么都有的文档（复制到浏览器访问）http://blogs.mizunas.com/Bot/ \n在开始使用之前，请务必查阅协议");
                KiraPlugin.sendMessage(g, $"虽然您可以直接在qq内查看帮助，但是请务必去网页查看协议[mirai:image:File:{G.path.Apppath}{G.path.help}default.png]",IsChain:true);
                OnCommanded.onCommanded(g, "help");
                return;
            }

        }
        static public void GetHelp(FriendVars g)
        {
            if (File.Exists($"{ G.path.Apppath}{ G.path.help}{g.msg.Replace("/c help ", "")}.txt"))
            {
                KiraPlugin.sendMessage(g, File.ReadAllText($"{ G.path.Apppath}{ G.path.help}{g.msg.Replace("/c help ", "")}.txt"), true);
            }
            else
            {
                KiraPlugin.sendMessage(g, "欢迎使用ChocolateBot，这里是完整的什么都有的文档（复制到浏览器访问）http://blogs.mizunas.com/Bot/ \n在开始使用之前，请务必查阅协议");
                KiraPlugin.sendMessage(g, $"虽然您可以直接在qq内查看帮助，但是请务必去网页查看协议[mirai:image:File:{G.path.Apppath}{G.path.help}default.png]",IsChain: true);
                OnCommanded.onCommanded(g, "help");
                return;
            }

        }
    }
}
