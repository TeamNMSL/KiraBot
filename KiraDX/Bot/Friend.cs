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
                    char[] chars = msg.ToCharArray();
                    bool CodeStart = false;
                    string Text = "";
                    string Code = "";
                    MessageBuilder messageBuilder = new MessageBuilder();
                    foreach (var c in chars)
                    {
                        if (c == '[')
                        {
                            if (CodeStart)
                            {
                                /*True,换句话说如果在这里了，应该是像 114514[[114514] 这样的文本
                                 *                                      ▲
                                 * 
                                 * 
                                */

                                //将前面的内容作为普通文本记录
                                Text += Code;
                                Code = "";
                                Code += c.ToString();
                            }
                            else
                            {
                                /*
                                 *False,这个位置应该像是 114514[1919]这样的文本
                                 *                          ▲
                                 *
                                 */
                                CodeStart = true;
                                //开始记录code内容
                                Code += c.ToString();
                            }
                        }
                        else if (c == ']')
                        {
                            if (CodeStart)
                            {
                                /*True,换句话说如果在这里了，应该是像 114514[[114514] 这样的文本
                                 *                                             ▲
                                 * 
                                */
                                CodeStart = false;
                                //记录code并转义
                                //记录
                                Code += c.ToString();
                                messageBuilder.AddPlainMessage(Text);

                                if (Code.Contains("mirai:face:"))
                                {
                                    messageBuilder.AddFaceMessage(int.Parse(Functions.TextGainCenter("[mirai:face:", "]", Code)));
                                }
                                else if (Code.Contains("mirai:at:"))
                                {
                                    messageBuilder.AddAtMessage(long.Parse(Functions.TextGainCenter("[mirai:at:", "]", Code)));
                                }
                                else if (Code.Contains("mirai:image:"))
                                {
                                    if (Code.Contains("mirai:image:File:"))
                                    {
                                        ImageMessage pic = await session.UploadPictureAsync(PictureTarget.Group, Functions.TextGainCenter("[mirai:image:File:", "]", Code));
                                        IMessageBase[] chain = new IMessageBase[] { pic };
                                        messageBuilder.Add(chain[0]);
                                    }
                                    else
                                    {
                                        messageBuilder.AddImageMessage(imageId: Functions.TextGainCenter("mirai:image:", "]", Code));
                                    }

                                }
                                else if (Code.Contains("mirai:flashimage:"))
                                {
                                    if (Code.Contains("mirai:flashimage:File:"))
                                    {

                                        messageBuilder.AddFlashImageMessage(imageId: session.UploadPictureAsync(PictureTarget.Group, Functions.TextGainCenter("[mirai:flashimage:File:", "]", Code)).Result.ImageId);
                                    }
                                    else
                                    {
                                        messageBuilder.AddFlashImageMessage(imageId: Functions.TextGainCenter("mirai:flashimage:", "]", Code));
                                    }


                                }
                                else
                                {
                                    messageBuilder.AddPlainMessage(Code);
                                }
                                Code = "";
                                Text = "";

                            }
                            else
                            {
                                /*
                                 *False,这个位置应该像是 114514[1919]]这样的文本
                                 *                                 ▲
                                 */
                                //当作普通文本计入
                                Text += c.ToString();
                            }

                        }
                        else
                        {
                            if (CodeStart)
                            {
                                //code内容
                                Code += c.ToString();
                            }
                            else
                            {
                                //普通文本
                                Text += c.ToString();
                            }

                        }

                    }
                    if (CodeStart)
                    {
                        Text = Code;
                    }
                    messageBuilder.AddPlainMessage(Text);
                    final = messageBuilder;

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
                    OnCommanded.onCommanded(g, "help");
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
                    OnCommanded.onCommanded(g, "arc");
                    return;

                }
            }
            dic = new[] { "查分", "/r", "/a", "/arc", "/最近", "/arς", "/αrc", @"/@rc", "/ARForest", "/ārc" };
            foreach (var item in dic)
            {
                if (msg == item)
                {


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
            return;
        }
    }
}
