using Mirai_CSharp;
using Mirai_CSharp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KiraDX.Bot
{
    public class FriendVars
    {
        public string msg;
        public long fromAccount;
        public long botid;
        public Mirai_CSharp.MiraiHttpSession s;
        public IFriendMessageEventArgs e;

        public FriendVars(string msg, long fromAccount, long botid, MiraiHttpSession session, IFriendMessageEventArgs e)
        {
            this.msg = msg;
            this.fromAccount = fromAccount;
            this.botid = botid;
            this.s = session;
            this.e = e;
        }
    }
    static public partial class Bot
    {
        public class FriendAddVars {

            public MiraiHttpSession session;
            public INewFriendApplyEventArgs e;

            public FriendAddVars(MiraiHttpSession session, INewFriendApplyEventArgs e)
            {
                this.session = session;
                this.e = e;
            }
        }
        public static async void FriendAdd(Object oj) {
            //问题一:?过不过申请随缘
            //回答: 1
            
            FriendAddVars f = oj as FriendAddVars;
            if (BotFunc.IsBanned(f.e.FromQQ))
            {
                await f.session.HandleNewFriendApplyAsync(f.e, FriendApplyAction.Deny, "略略略");
                return;
            }
            if (Users.ban.BanFriend_Tmp.Contains(f.e.FromQQ.ToString()))
            {
                await f.session.HandleNewFriendApplyAsync(f.e, FriendApplyAction.Deny, "略略略");
                return;
            }
            string message = f.e.Message;
            message = message.Substring(message.IndexOf("回答:"), message.Length - message.IndexOf("回答:"));
            message = message.Replace("回答:","");
            message=message.TrimStart().TrimEnd();
            bool IsAgree = true;
            if (Functions.StrLength(message)<2)
            {
                IsAgree = false;
            }
            string[] dic = {
               
                "，，，",
                "，，",
                "谔谔",
                "arcaea查分",
                "arc查分",
                "arcaea查分bot",
                "arc查分bot",
                "arcbot",
                "arcaeabot",
                "/k help",
                ",,",
                ",,,",
                "...",
                "..",
                "。。",
                "。。。",
                "纯自动","过不过",
            "/"};
            Console.WriteLine(message);
            foreach (var item in dic)
            {
                if (message.Contains(item))
                {
                    IsAgree = false;
                }
            }
            dic =new string[] {
                "。",
                ",",
                "." ,
                "，"
            };
            foreach (var item in dic)
            {
                if (message.StartsWith(item))
                {
                    IsAgree = false;
                }
            }
            dic = new string[] {
                "1",
                "?" ,
                "？"
            };
            foreach (var item in dic)
            {
                if (message==item)
                {
                    IsAgree = false;
                }
            }
            if (f.session.QQNumber!=G.BotList.Miffy&&IsAgree)
            {
                await f.session.HandleNewFriendApplyAsync(f.e, FriendApplyAction.Allow);
                System.Threading.Thread.Sleep(5000);
                KiraPlugin.SendFriendMessage(f.session, f.e.FromQQ, "您好，欢迎使用KiraBotDX，因为一些用户的行为，本bot绝大部分指令仅支持群聊使用，请在使用前阅读协议qwq");
                KiraPlugin.SendFriendMessage(f.session, f.e.FromQQ, "下面是帮助文本，如果没有群能用的也欢迎加入bot的群1044241327");
                KiraPlugin.SendFriendPic(f.session, f.e.FromQQ, $"{G.path.Apppath}{G.path.help}default.png");
                KiraPlugin.SendFriendPic(f.session, f.e.FromQQ, $"{G.path.Apppath}{G.path.help}rule.png");
            }
            else
            {
                Users.ban.BanFriend_Tmp += f.e.FromQQ.ToString  ()+"\n";
                await f.session.HandleNewFriendApplyAsync(f.e, FriendApplyAction.Deny, "略略略");
            }
        }

       


        public static async void FriendMessage(string msg,  long fromAccount, long botid, Mirai_CSharp.MiraiHttpSession session, IFriendMessageEventArgs e)
        {
            FriendVars g = new FriendVars(msg, fromAccount, botid, session,e);
            if (BotFunc.IsBanned(fromAccount))
            {
                return;
            }
            switch (msg)
            {
                case "帮助":
                case "help":
                case "/k help":
                case "/help":
                    KiraPlugin.SendFriendMessage(g.s, g.fromAccount, "您好，欢迎使用KiraBotDX，因为一些用户的行为，本bot绝大部分指令仅支持群聊使用，请在使用前阅读协议qwq");
                    KiraPlugin.SendFriendMessage(g.s, g.fromAccount, "Bot的帮助是/k help");
                    KiraPlugin.SendFriendMessage(g.s, g.fromAccount, "下面是帮助文本，如果没有群能用的也欢迎加入bot的群1044241327");
                    KiraPlugin.SendFriendPic(g.s, g.fromAccount, $"{G.path.Apppath}{G.path.help}default.png");
                    KiraPlugin.SendFriendPic(g.s, g.fromAccount, $"{G.path.Apppath}{G.path.help}rule.png");
                    return;
                default:
                    break;
            }
            string[] dic = new[] { "查分", "/as", "/a score", "/arc score", "/a info", "/arc info" };
            foreach (var item in dic)
            {
                if (msg.StartsWith(item))
                {


                        KiraDX.Bot.arcaea.arcaea.SongBest(g);
                        return;

                }
            }
            dic = new[] { "查分", "/r", "/a", "/arc", "/最近", "/arς", "/αrc", @"/@rc", "/ARForest", "/ārc" };
            foreach (var item in dic)
            {
                if (msg == item)
                {


                        KiraDX.Bot.arcaea.arcaea.Arc(g);
                        return;

                }
            }

            dic = new[] { "/b30", "/arc b30", "/a3", "/a b30", "b30", "查b30", "/arc b114514" };
            foreach (var item in dic)
            {
                if (msg == item)
                {
                        KiraDX.Bot.arcaea.arcaea.b30(g);
                        return;
                }
            }

            dic = new[] { "绑定", "/ab", "/a bind", "/arc bind", "/arc绑定" };
            foreach (var item in dic)
            {
                if (msg.StartsWith(item))
                {

                        KiraDX.Bot.arcaea.arcaea.Bind(g);
                        return;
                   
                }
            }
            dic = new[] { "/a rand", "/arc rand", "随机选曲", "抽歌" };
            foreach (var item in dic)
            {
                if (msg.StartsWith(item))
                {
                        KiraDX.Bot.arcaea.arcaea.RandArc(g);
                        return;
                 
                }
            }
            return;
        }
    }
}
