#region using
using System;
using System.Collections.Generic;
using System.Text;
using Mirai_CSharp;
using Mirai_CSharp.Models;
using Mirai_CSharp.Extensions;
using System.IO;
using System.Linq;
using DSharpPlus;
using DSharpPlus.EventArgs;
using KiraDX.Frame;
#endregion


namespace KiraDX 
{
    public static partial class KiraPlugin
    {
        public static async System.Threading.Tasks.Task<bool> sendMessage(KiraDX.Bot.GroupMsg g, string msg, bool IsChain = false,long ToGroup=0,bool IsToFriend=false,long ToFriend=0)
        {
            try
            {
                MiraiHttpSession session = g.s;
                long GroupID;
                if (ToGroup == 0)
                {
                    GroupID = g.fromGroup;
                }
                else {

                    GroupID = ToGroup;
                }
                long FriendID;
                if (ToFriend==0)
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
    
    public class GroupMsg
    {
        public string msg;
        public long fromGroup;
        public long fromAccount;
        public long botid;
        public MiraiHttpSession s;
        public IGroupMessageEventArgs e;
        public string Type = "QQ_Group";
        public GroupMsg(string msg, long fromGroup, long fromAccount, long botid, MiraiHttpSession s, IGroupMessageEventArgs e)
        {
            this.msg = msg;
            this.fromGroup = fromGroup;
            this.fromAccount = fromAccount;
            this.botid = botid;
            this.s = s;
            this.e = e;
        }
    }
    static public partial class Bot 
    {
        #region 委托
        private delegate void HsoCommand(GroupMsg g);
        private delegate void Switches(GroupMsg g,IGroupMessageEventArgs e);
        #endregion
        public  static async void GroupMessage(string msg,long fromGroup,long fromAccount,long botid,Mirai_CSharp.MiraiHttpSession session, IGroupMessageEventArgs e ) {
            try
            {
                

                Users.botMsgNum += 1;

                #region 消息预处理
                GroupMsg g = new GroupMsg(msg, fromGroup, fromAccount, botid, session,e);
                Pretreatment.Pretreatment_MainBot(g);
                string[] cmd = msg.Split(' ');
                if (BotFunc.IsBanned(g.fromAccount))
                {
                    return;
                }
                #endregion

                if ((msg.format()=="/help"||msg.format() == "/k help")&&BotFunc.IsMainBot(g))
                {
                    KiraPlugin.SendGroupPic(g.s, fromGroup, $"{G.path.Apppath}{G.path.help}default.png");
                    KiraPlugin.SendGroupPic(g.s, fromGroup, $"{G.path.Apppath}{G.path.help}rule.png");
                    OnCommanded.onCommanded(g, "help");
                    return;
                    //KiraPlugin.SendGroupPic(session, fromGroup, $"{G.path.Apppath}{G.path.help}group.png");
                }
                #region admin
                if (BotFunc.isAdmin(g))
                {
                    if (msg.StartsWith("/k 全局 "))
                    {
                        KiraDX.Bot.Mirai.ADMINCOMMANDS.sendAll(g,e);
                        return;
                    }
                }
                #endregion


                #region System
                switch (msg.format())
                {
                    case @"/k":
                        KiraDX.Bot.Mod_System.Ping.PingBot(g);
                        OnCommanded.onCommanded(g,"k");
                        return;
                    default:
                        break;
                }
                if (msg.format() == "/k mainbot=soffy")
                {
                    KiraDX.Bot.Mod_System.setMain.mainbot(g,"Soffy");
                    OnCommanded.onCommanded(g, "ChangeBot");
                    return;
                }
                else if (msg.format() == "/k mainbot=laffy")
                {
                    KiraDX.Bot.Mod_System.setMain.mainbot(g, "Laffy");
                    OnCommanded.onCommanded(g, "ChangeBot");
                    return;
                }
                
                
                if (BotFunc.IsMainBot(g))
                {
                    if (msg == "/k pass")
                    {
                        KiraPlugin.sendMessage(g, $"bot给您使用不是义务，如果您是抱着用bot是义务的心态来加bot，那么请回，如果被我们察觉到类似的想法，有概率被挂，密码是{g.fromAccount*3}");
                        OnCommanded.onCommanded(g, "GetPassword");
                        return;
                    }

                    if (cmd.Length >= 3&&msg.format().StartsWith("/k mod"))
                    {
                        Dictionary<string, Switches> System = new Dictionary<string, Switches>() {
                    {"enable",(g,e)=>{KiraDX.Bot.Mod_System.Switches.SwitchOn(g,e);  } },
                    {"disable",(g,e)=>{KiraDX.Bot.Mod_System.Switches.SwitchOff(g,e); } },
                    {"show",(g,e)=>{KiraDX.Bot.Mod_System.Switches.SwitchShow(g,e);} }
                };
                        if (System.ContainsKey(cmd[2]) && cmd[1].format() == "mod")
                        {
                            System[cmd[2]].Invoke(g, e);
                            OnCommanded.onCommanded(g, "mod");
                        }
                        
                    }
                }

                
                #endregion

                if (BotFunc.FuncSwith(g,"bot"))
                {
                    #region 色图来
                    if (botid == G.BotList.Miffy&& BotFunc.FuncSwith(g, "图片"))
                    {
                        Dictionary<string, HsoCommand> HsoCommand = new Dictionary<string, HsoCommand>() {
                    {"*hso",(g)=>{ KiraDX.Bot.Picture.Hso.Hso.HsoEX(g,"colorpic");} },
                    {"*cu",(g)=>{ KiraDX.Bot.Picture.Hso.Hso.HsoEX(g,"cu");} },
                    {"*pic",(g)=>{ KiraDX.Bot.Picture.Hso.Hso.HsoEX(g,"pic");} }};
                        if (HsoCommand.ContainsKey(cmd[0]))
                        {
                            HsoCommand[cmd[0]].Invoke(g);
                            OnCommanded.onCommanded(g, "Hso");
                        }
                        switch (msg)
                        {
                            case @"涩图":
                            case @"色图来":
                            case @"[mirai:image:{B407F708-A2C6-A506-3420-98DF7CAC4A57}.jpg]":
                            case @"[mirai:image:{523F541F-30A6-84B4-71EA-B31695310299}.jpg]":

                                KiraDX.Bot.Picture.Hso.Hso.GetHso(g, "colorpic");
                                KiraPlugin.SendGroupMessage(g.s,g.fromGroup,"为了减少bot被ban的概率，我们在此呼吁，少看涩图，一分钟不超过五张，手离几把越近，健康离你越远。");
                                OnCommanded.onCommanded(g, "Hso");
                                return;
                            case @"铜图来":
                                KiraDX.Bot.Picture.Hso.Hso.GetHso(g, "cu");
                                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "为了减少bot被ban的概率，我们在此呼吁，少看涩图，一分钟不超过五张，手离几把越近，健康离你越远。");
                                OnCommanded.onCommanded(g, "Hso");
                                return;
                            case @"那你发":
                                KiraDX.Bot.Picture.Hso.Hso.YouFa(g, "colorpic");
                                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "为了减少bot被ban的概率，我们在此呼吁，少看涩图，一分钟不超过五张，手离几把越近，健康离你越远。");
                                OnCommanded.onCommanded(g, "Hso");
                                return;
                            case @"/cu":
                                KiraDX.Bot.Picture.Hso.Hso.GetHso(g, "cu");
                                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "为了减少bot被ban的概率，我们在此呼吁，少看涩图，一分钟不超过五张，手离几把越近，健康离你越远。");
                                OnCommanded.onCommanded(g, "Hso");
                                return;
                            case @"/pic":
                                KiraDX.Bot.Picture.Hso.Hso.GetHso(g, "pic");
                                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "为了减少bot被ban的概率，我们在此呼吁，少看涩图，一分钟不超过五张，手离几把越近，健康离你越远。");
                                OnCommanded.onCommanded(g, "Hso");
                                return;
                            case @"/hso":
                                KiraDX.Bot.Picture.Hso.Hso.GetHso(g, "colorpic");
                                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "为了减少bot被ban的概率，我们在此呼吁，少看涩图，一分钟不超过五张，手离几把越近，健康离你越远。");
                                OnCommanded.onCommanded(g, "Hso");
                                return;
                            default:
                                break;
                        }
                        if ((msg.Contains("来") || msg.Contains("给我")) && (msg.Contains("张") || msg.Contains("份")) && Functions.GetNumberInString(msg) != 0 && botid == G.BotList.Miffy)
                        {
                            KiraDX.Bot.Picture.Hso.Hso.GetHso(g, Functions.GetNumberInString(msg), "colorpic");
                            KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "为了减少bot被ban的概率，我们在此呼吁，少看涩图，一分钟不超过五张，手离几把越近，健康离你越远。");
                            OnCommanded.onCommanded(g, "Hso");
                            return;
                        }
                        if ((msg.Contains("这也能叫涩图") || msg.Contains("这也能叫色图")) || (msg.Contains("不够涩") || msg.Contains("不够色")) && botid == G.BotList.Miffy)
                        {
                            KiraDX.Bot.Picture.Hso.Hso.NotSexyEnough(g);
                            KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "为了减少bot被ban的概率，我们在此呼吁，少看涩图，一分钟不超过五张，手离几把越近，健康离你越远。");
                            OnCommanded.onCommanded(g, "Hso");
                            return;
                        }
                    }
                    #endregion
                    
                    
                    if (BotFunc.IsMainBot(g)) 
                    {
                        #region arcaea
                        string[] dic = new []{ "查分", "/as", "/a score", "/arc score", "/a info", "/arc info" };
                            foreach (var item in dic)
                            {
                                if (msg.format().StartsWith(item))
                                {

                                    if (BotFunc.FuncSwith(g, "arc"))
                                    {
                                        KiraDX.Bot.arcaea.arcaea.SongBest(g);
                                        OnCommanded.onCommanded(g, "arc");
                                        return;
                                    }
                                    else
                                    {
                                        KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群Arc模块处于关闭状态，请使用/k mod enable arc打开本群Arc模块后再查分");
                                        
                                    return;
                                    }
                                }
                            }
                            dic = new []{ "查分", "/r", "/a", "/arc", "/最近", "/arς", "/αrc", @"/@rc", "/ARForest", "/ārc" };
                            foreach (var item in dic)
                            {
                                if (msg.format() == item)
                                {

                                    if (BotFunc.FuncSwith(g, "arc"))
                                    {
                                        KiraDX.Bot.arcaea.arcaea.Arc(g);
                                    OnCommanded.onCommanded(g, "arc");
                                    return;
                                    }
                                    else
                                    {
                                        KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群Arc模块处于关闭状态，请使用/k mod enable arc打开本群Arc模块后再查分");
                                        return;
                                    }
                                }
                            }
                        
                        dic = new[] { "/b30", "/arc b30", "/a3", "/a b30", "b30", "查b30","/arc b114514" };
                        foreach (var item in dic)
                        {
                            if (msg.format() == item)
                            {

                                if (BotFunc.FuncSwith(g, "arc"))
                                {
                                    KiraDX.Bot.arcaea.arcaea.b30(g);
                                    OnCommanded.onCommanded(g, "arc");
                                    return;
                                }
                                else
                                {
                                    KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群Arc模块处于关闭状态，请使用/k mod enable arc打开本群Arc模块后再查分");
                                    return;
                                }
                            }
                        }

                        dic = new[] { "绑定", "/ab", "/a bind", "/arc bind" ,"/arc绑定"};
                        foreach (var item in dic)
                        {
                            if (msg.format().StartsWith(item))
                            {

                                if (BotFunc.FuncSwith(g, "arc"))
                                {
                                    KiraDX.Bot.arcaea.arcaea.Bind(g);
                                    OnCommanded.onCommanded(g, "arc");
                                    return;
                                }
                                else
                                {
                                    KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群Arc模块处于关闭状态，请使用/k mod enable arc打开本群Arc模块后再绑定");
                                    return;
                                }
                            }
                        }
                        dic = new[] { "/a rand", "/arc rand", "随机选曲","抽歌"};
                        foreach (var item in dic)
                        {
                            if (msg.format().StartsWith(item)) {

                                if (BotFunc.FuncSwith(g, "arc"))
                                {
                                    KiraDX.Bot.arcaea.arcaea.RandArc(g);
                                    OnCommanded.onCommanded(g, "arc");
                                    return;
                                }
                                else
                                {
                                    KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群Arc模块处于关闭状态，请使用/k mod enable arc打开本群Arc模块后再随机");
                                    return;
                                }
                            }
                        }
                        #endregion
                        #region 图片
                        switch (msg.format())
                        {
                            case "/k 龙":
                            case "龙图来":
                                KiraDX.Bot.Picture.Pic.GetPic.getPic(g, "龙图",false);
                                OnCommanded.onCommanded(g, "龙图");
                                return;
                            case "/k 彩":
                            case "/一键彩彩":
                                KiraDX.Bot.Picture.Pic.GetPic.getPic(g, "丸山彩", false);
                                OnCommanded.onCommanded(g, "丸山彩");
                                return;
                            case "/k 鹦鹉":
                            case "/k parrot":
                                KiraDX.Bot.Picture.Pic.GetPic.getPic(g, "鹦鹉", false);
                                OnCommanded.onCommanded(g, "鹦鹉图");//鸟图
                                return;
                        }
                        #endregion
                        #region 迫害
                        switch (msg.format())
                        {
                            case "/金句":
                            case "来点金句":
                            case "金句":
                                KiraDX.Bot.Sentences.GoldSentences.GetSent(g, "GoldenSentences");
                                OnCommanded.onCommanded(g, "金句");
                                return;
                            case "/Bot":
                            case "/bot":
                                KiraDX.Bot.Sentences.GoldSentences.GetSent(g, "About");
                                OnCommanded.onCommanded(g, "金句");
                                return;
                            case "/柴爹":
                            case "/火柴王":
                                KiraDX.Bot.Sentences.GoldSentences.GetSent(g, "Chaidie");
                                OnCommanded.onCommanded(g, "金句");
                                return;
                            case "/胡离":
                                KiraDX.Bot.Sentences.GoldSentences.GetSent(g, "Huli");
                                OnCommanded.onCommanded(g, "金句");
                                return;
                            case "/dk":
                            case "/DK":
                            case "/DK龙":
                            case "来点dk龙":
                                KiraDX.Bot.Sentences.GoldSentences.GetSent(g, "Long");
                                OnCommanded.onCommanded(g, "金句");
                                return;
                            case "/小鬼":
                                KiraDX.Bot.Sentences.GoldSentences.GetSent(g, "Xiaogui");
                                OnCommanded.onCommanded(g, "金句");
                                return;
                            case "/dev":
                                KiraDX.Bot.Sentences.GoldSentences.GetSent(g, "dev");
                                OnCommanded.onCommanded(g, "金句");
                                return;
                            case "/暮里":
                                KiraDX.Bot.Sentences.GoldSentences.getPic(g, "暮里");
                                OnCommanded.onCommanded(g, "金句");
                                return;
                            case "/海滨":
                            case "/王校长":
                            case "/校长":
                            case "/WJX":
                            case "/王巨星":
                            case "/1818":
                                KiraDX.Bot.Sentences.GoldSentences.getPic(g, "海滨");
                                OnCommanded.onCommanded(g, "金句");
                                return;
                            case "/cc":
                                KiraDX.Bot.Sentences.GoldSentences.getPic(g, "ccccc");
                                OnCommanded.onCommanded(g, "金句");
                                return;
                        }
                        #endregion
                        #region 漂流瓶
                        if (msg.format() == "/k pick")
                        {
                            KiraDX.Bot.bottle.Bottle.PickBottle(g);
                            OnCommanded.onCommanded(g, "漂流瓶");
                            return;
                        }

                        if (msg.format() == "/k pick e")
                        {
                            KiraDX.Bot.bottle.Bottle.PickBottle_E(g);
                            OnCommanded.onCommanded(g, "漂流瓶");
                            return;
                        }

                        if (msg.ToLower().TrimStart().StartsWith ("/k throw ")|| msg.StartsWith("/k throw-u "))
                        {
                            KiraDX.Bot.bottle.Bottle.SendBottle(g);
                            OnCommanded.onCommanded(g, "漂流瓶");
                            return;
                        }
                        #endregion
                        #region Pa
                        if (msg.ToLower().StartsWith("/k pa"))
                        {
                            KiraDX.Bot.Extended.pacgnjny.pacgn(g);
                            OnCommanded.onCommanded(g, "pacgn");
                            return;
                        }

                        if (msg.format().StartsWith("/k story "))
                        {
                            KiraDX.Bot.Story.Story.StoryGet(g);
                        }
                        #endregion
                        #region 工具
                        if (msg.format().StartsWith("ycm") || msg.format() == "有车吗")
                        {
                            if (BotFunc.FuncSwith(g, "工具"))
                            {
                                KiraDX.Bot.bangdori.IsCar(g);
                                OnCommanded.onCommanded(g, "ycm");
                                return;
                            }
                            else
                            {
                                KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群工具处于关闭状态，请使用/k mod enable 工具打开本群工具模块后再操作");
                                return;
                            }
                            
                        }

                        if (msg.ToLower().TrimStart().StartsWith("/k trans "))
                        {
                            if (BotFunc.FuncSwith(g,"工具"))
                            {
                                KiraDX.Bot.Others.Trans.Translate(g);
                                OnCommanded.onCommanded(g, "translate");
                                return;
                            }
                            else
                            {
                                KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群工具处于关闭状态，请使用/k mod enable 工具打开本群工具模块后再操作");
                                return;
                            }
                        }

                        if (msg.ToLower().TrimStart().StartsWith("/k len "))
                        {

                            if (BotFunc.FuncSwith(g, "工具"))
                            {
                                string text = g.msg.Replace("/k len ", "");
                                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, $"字符串{text}的长度为{Functions.StrLength(text)},字符个数为{text.Length}", false);
                                
                                return;
                            }
                            else
                            {
                                KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群工具处于关闭状态，请使用/k mod enable 工具打开本群工具模块后再操作");
                                return;
                            }
                        }
                        
                        if (msg.ToLower().TrimStart().StartsWith("/k write "))
                        {
                            

                            if (BotFunc.FuncSwith(g, "工具"))
                            {
                                KiraDX.Bot.Others.Writer.GetNovel(g);
                                OnCommanded.onCommanded(g, "Writer");
                                return;
                            }
                            else
                            {
                                KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群工具处于关闭状态，请使用/k mod enable 工具打开本群工具模块后再操作");
                                return;
                            }
                        }

                        if (msg.ToLower().TrimStart().StartsWith("/k 说 ") && BotFunc.isWhite(g))
                        {

                            if (BotFunc.FuncSwith(g, "工具"))
                            {
                                string text = g.msg.Replace("/k 说 ", "");
                                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, text, true);
                                OnCommanded.onCommanded(g, "Speak");
                                return;
                            }
                            else
                            {
                                KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群工具处于关闭状态，请使用/k mod enable 工具打开本群工具模块后再操作");
                                return;
                            }
                            

                        }

                        if (msg.format()=="/k get eventvalue")
                        {
                            if (BotFunc.FuncSwith(g, "工具"))
                            {
                                KiraPlugin.sendMessage(g, Story.EventValue.EventValue_Get(g.fromAccount).ToString());
                            }
                            else
                            {
                                KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群工具处于关闭状态，请使用/k mod enable 工具打开本群工具模块后再操作");
                                return;
                            }
                        }

                        if (msg.ToLower().TrimStart().StartsWith("/k code ") && BotFunc.IsMainBot(g))
                        {

                            if (BotFunc.FuncSwith(g, "工具"))
                            {
                                string text = g.msg.Replace("/k code ", "");
                                
                                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, text);
                            }
                            else
                            {
                                KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群工具处于关闭状态，请使用/k mod enable 工具打开本群工具模块后再操作");
                                return;
                            }
                           
                        }

                        #endregion

                        

                        if (BotFunc.IsMainBot(g))
                        {
                            if (msg.ToLower() == "/k dismiss group")
                            {
                                KiraDX.Bot.Mod_System.Dismiss.Dismiss_Group(g, e);
                                return;
                            }
                            if (msg.ToLower() == "/k dismiss me")
                            {
                                KiraDX.Bot.Mod_System.Dismiss.Dismiss_User(g);
                                return;
                            }
                        }
                        
                    }


                }

                /*
                if (BotFunc.FuncSwith(g, "*防风控模块")&&botid==G.BotList.Miffy)
                {
                    KiraDX.Bot.Mirai.Repeater.repeat(g, e,true);
                    return;
                }
                */
                if (BotFunc.FuncSwith(g,"bot"))
                {
                    if (Functions.GetRandomNumber(0,1000)==8)
                    {
                        KiraPlugin.SendGroupMessage(g.s,g.fromGroup,"qwq");
                    }

                    if (msg.Contains($"[mirai:at:{botid}]") && BotFunc.FuncSwith(g, "自动回复"))
                    {
                        KiraDX.Bot.Others.Study.Reply(g, e);
                        return;
                    }

                    if (BotFunc.IsIllegal(g.msg))
                    {
                        return;
                    }
                    if (BotFunc.FuncSwith(g, "复读"))
                    {
                        KiraDX.Bot.Mirai.Repeater.repeat(g, e);
                    }

                    if (BotFunc.FuncSwith_System("浅度学习") && BotFunc.IsMainBot(g))
                    {
                        KiraDX.Bot.Others.Study.Study_Main(g);
                    }
                }

            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
