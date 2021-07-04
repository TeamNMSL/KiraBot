using Mirai_CSharp;
using Mirai_CSharp.Extensions;
using Mirai_CSharp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using KiraDX.Frame;


namespace KiraDX
{
    public static partial class KiraPlugin
    {
        public static async System.Threading.Tasks.Task<bool> sendMessage(KiraDX.Bot.FriendVars g, string msg, bool IsChain = false, long ToGroup = 0, bool IsToFriend = true, long ToFriend = 0)
        {
            try
            {
                MiraiHttpSession session = g.s;
                long GroupID;
                if (ToGroup == 0)
                {
                    GroupID = 0;
                    if (IsToFriend==false)
                    {
                        
                        return false;
                    }
                }
                else
                {

                    GroupID = ToGroup;
                }
                long FriendID;
                if (ToFriend == 0)
                {
                    FriendID = g.fromAccount;
                }
                else
                {
                    FriendID = ToFriend;
                }
                MessageBuilder final = new MessageBuilder();
                if (IsChain)
                {
                    final = KiraDX.Frame.Mirai.GetChainAsync(msg, g.s).Result;
                    //await session.SendGroupMessageAsync(GroupID, messageBuilder);

                }
                else
                {

                    MessageBuilder builder = new MessageBuilder();
                    builder.AddPlainMessage(msg);
                    final = builder;
                    //await session.SendGroupMessageAsync(GroupID, builder);
                }

                if (IsToFriend)
                {
                    session.SendFriendMessageAsync(FriendID, final);
                }
                else
                {
                    session.SendGroupMessageAsync(GroupID, final);
                }

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }




    }

}


namespace KiraDX.Bot
{
    public class FriendVars
    {
        public string msg;
        public long fromAccount;
        public long botid;
        public Mirai_CSharp.MiraiHttpSession s;
        public IFriendMessageEventArgs e;
        public string Type = "QQ_Friend";
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

            if (f.session.QQNumber != G.BotList.Calista)
            {
                await f.session.HandleNewFriendApplyAsync(f.e, FriendApplyAction.Allow);
                System.Threading.Thread.Sleep(5000);

                KiraDX.Bot.Mod_System.help.GetHelp(new FriendVars("", f.e.FromQQ, (long)f.session.QQNumber, f.session, null));

            }
            return;
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
            bool IsAgree;
            if (message.Contains((f.e.FromQQ* 3).ToString()))
            {
                IsAgree = true;
            }
            else
            {
                IsAgree = false;
            }

            

            /*
            if (Functions.StrLength(message)<2)
            {
                IsAgree = false;
            }
            string[] dic = {
               
                "，，，",
                "，，",
                "谔谔",
                "arc",
                ",,",
                ",,,",
                "...",
                "..",
                "。。",
                "。。。","额",
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
            }*/
            if (f.session.QQNumber!=G.BotList.Calista&&IsAgree)
            {
                await f.session.HandleNewFriendApplyAsync(f.e, FriendApplyAction.Allow);
                System.Threading.Thread.Sleep(5000);
                
                KiraDX.Bot.Mod_System.help.GetHelp(new FriendVars("",f.e.FromQQ,(long)f.session.QQNumber,f.session,null));
               
            }
            else
            {
                Users.ban.BanFriend_Tmp += f.e.FromQQ.ToString  ()+"\n";
                await f.session.HandleNewFriendApplyAsync(f.e, FriendApplyAction.Deny, "略略略");
            }
        }

       


        public static async void FriendMessage(string msg,  long fromAccount, long botid, Mirai_CSharp.MiraiHttpSession session, IFriendMessageEventArgs e)
        {
            try
            {
                FriendVars g = new FriendVars(msg, fromAccount, botid, session, e);
                if (BotFunc.IsBanned(fromAccount))
                {
                    return;
                }
                if (!Users.Info.GetUserConfig(g.fromAccount).IsPassed)
                {
                    KiraPlugin.sendMessage(g, @"因为Bot最近遭受的申必行为，因此现在开启了私聊白名单模式，请到这里去申请白名单（看完内容填完qq点申请秒过的），同时这里也有一些私聊使用的注意事项，复制到浏览器访问 http://blogs.mizunas.com/Bot/PasswordGet/");
                    return;
                }

                switch (msg)
                {
                    case "帮助":
                    case "help":
                    case "/c help":
                    case "/help":
                        KiraDX.Bot.Mod_System.help.GetHelp(g);
                        return;
                    default:
                        break;
                }
                if ((msg.Contains("/c help")))
                {
                    KiraDX.Bot.Mod_System.help.GetHelp(g);
                }
                string[] dic = new[] { "查分", "/as", "/a score", "/arc score", "/a info", "/arc info" };
                foreach (var item in dic)
                {
                    if (msg.StartsWith(item))
                    {


                        KiraDX.Bot.arcaea.arcaea.SongBest(g);
                        OnCommanded.onCommanded(g, "arc");
                        return;

                    }
                }
                dic = new[] { "查分", "/r", "/a", "/arc", "/最近", "/arς", "/αrc", @"/@rc", "/ARForest", "/ārc" };
                foreach (var item in dic)
                {
                    if (msg == item)
                    {

                        if (G.EventCfg.IsArcBoom)
                        {
                            KiraPlugin.sendMessage(g, "查询Arcaea信息失败，因为616日常改加密算法\n你知道吗：Lowiro是一个背叛玩家的坏游戏制作厂商");
                            return;
                        }
                        KiraDX.Bot.arcaea.arcaea.Arc(g);
                        OnCommanded.onCommanded(g, "arc");
                        return;

                    }
                }

                dic = new[] { "/b30", "/arc b30", "/a3", "/a b30", "b30", "查b30", "/arc b114514" };
                foreach (var item in dic)
                {
                    if (msg == item)
                    {
                        if (G.EventCfg.IsArcBoom)
                        {
                            KiraPlugin.sendMessage(g, "查询Arcaea信息失败，因为616日常改加密算法\n你知道吗：Lowiro是一个背叛玩家的坏游戏制作厂商");
                            return;
                        }
                        KiraDX.Bot.arcaea.arcaea.b30(g);
                        OnCommanded.onCommanded(g, "arc");
                        return;
                    }
                }

                dic = new[] { "绑定", "/ab", "/a bind", "/arc bind", "/arc绑定" };
                foreach (var item in dic)
                {
                    if (msg.StartsWith(item))
                    {
                        if (G.EventCfg.IsArcBoom)
                        {
                            KiraPlugin.sendMessage(g, "查询Arcaea信息失败，因为616日常改加密算法\n你知道吗：Lowiro是一个背叛玩家的坏游戏制作厂商");
                            return;
                        }
                        KiraDX.Bot.arcaea.arcaea.Bind(g);

                        OnCommanded.onCommanded(g, "arc");
                        return;

                    }
                }
                dic = new[] { "/a rand", "/arc rand", "随机选曲", "抽歌" };
                foreach (var item in dic)
                {
                    if (msg.StartsWith(item))
                    {
                        KiraDX.Bot.arcaea.arcaea.RandArc(g);
                        OnCommanded.onCommanded(g, "arc");
                        return;

                    }
                }

                if (g.msg.format().StartsWith("/c todev ") || g.msg.format().StartsWith("/c todev-l "))
                {
                    try
                    {
                        string sts = "public";
                        if (g.msg.format().StartsWith("/c todev-l "))
                        {
                            sts = "private";
                        }
                        KiraPlugin.sendMessage(g, $"[{sts}]\n[私讯]\n[Account {g.fromAccount}]\n{g.msg.Split(" ", count: 3)[2]}", true, IsToFriend: true, ToFriend: 1930300830);
                        KiraPlugin.sendMessage(g, "已传达");
                        return;
                    }
                    catch (Exception ex)
                    {

                        KiraPlugin.sendMessage(g, ex.Message);
                        return;
                    }
                }

                return;
            }
            catch (Exception ex)
            {
                KiraPlugin.SendFriendMessage(session, fromAccount, ex.ToString());
                Console.WriteLine(ex.ToString());
                return;
            }
        }
    }
}
