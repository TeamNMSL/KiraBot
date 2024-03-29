﻿#region using
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
using System.Net.Http;
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
                    if (true)
                    {
                        final = KiraDX.Frame.Mirai.GetChainAsync(msg, g.s).Result;
                    }
                    

                }
                else
                {
                    if (G.EventCfg.fool&&Functions.GetRandomNumber(0,2)==1)
                    {
                        msg = msg.rvs();
                    }
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
        public  static async void GroupMessage(string msg,long fromGroup,long fromAccount,long botid,Mirai_CSharp.MiraiHttpSession session, IGroupMessageEventArgs e) {
            try
            {

                
                Users.botMsgNum += 1;

                #region 消息预处理
                GroupMsg g = new GroupMsg(msg, fromGroup, fromAccount, botid, session,e);
                Pretreatment.Pretreatment_MainBot(g);
                string[] cmd = msg.Split(' ');
                if (Users.Info.GetUserConfig(fromAccount).IsBanned)
                {
                    return;
                }
                if (botid==G.BotList.Nadia)
                {
                    Users.BotSession.Nadia = session;
                }
                else if (botid==G.BotList.Alice)
                {
                    Users.BotSession.Alice = session;
                }
                else
                {
                    Users.BotSession.Calista = session;
                }

                if (!BotFunc.IsWhiteGroup(g.fromGroup.ToString()))
                {
                    Mod_System.UnWhiteExit.TD(g);
                }
                #endregion

                if (msg=="_NameEdit")
                {
                   
                }

                if (msg.format().Contains("/c bat"))
                {
                    string[] coms;
                    if (msg.Contains("\r\n"))
                    {
                    coms     = msg.Split("\r\n");
                    }
                    else if (msg.Contains("\n\r"))
                    {
                        coms = msg.Split("\n\r");
                    }
                    else if (msg.Contains("\n"))
                    {
                        coms = msg.Split('\n');
                    }
                    else if (msg.Contains("\r"))
                    {
                        coms = msg.Split('\r');
                    }
                    else
                    {
                        coms = msg.Split('\n');
                    }
                    foreach (var item in coms)
                    {
                        if (!item.format().StartsWith("/c bat"))
                        {
                            
                            IGroupMessageEventArgs my_arg = new GroupMessageEventArgs(new IMessageBase[] {new PlainMessage(item) },e.Sender);
                            GroupMessage(item,fromGroup,fromAccount,botid,session,my_arg);
                            System.Threading.Thread.Sleep(1000);
                        }
                    }
                    return;
                }

                if ((msg.Contains("/c help")&&BotFunc.IsMainBot(g)))
                {
                    KiraDX.Bot.Mod_System.help.GetHelp(g);
                }
                #region admin
                if (Users.Info.GetUserConfig(g.fromAccount).IsAdmin)
                {
                    if (msg.StartsWith("/c 全局 "))
                    {
                        KiraDX.Bot.AllGroup.sendAll(g);
                        return;
                    }

                    if (msg.format()=="/c info allmember")
                    {
                        KiraDX.Bot.Mod_System.Info.GetAllInfo(g);
                        return;
                    }
                    if (msg.format() == "/c whitegroup setall"&&false)
                    {
                        string api = "http://botapi.mizunas.com/api/group/AddGroup?" + "MemberCount=" + "1000" + "&IsAgree=" + "true" + "&GroupType=" + "WithRhythmGamePersonalGroup";
                        foreach (var item in KiraPlugin.GetGroupListAsync(session).Result)
                        {
                            if (session.GetGroupMemberListAsync(item.Id).Result.Count()>=50)
                            {
                                Console.WriteLine(Encoding.UTF8.GetString(new HttpClient().GetByteArrayAsync(api + "&GroupNumber=" + item.Id).Result));
                                KiraPlugin.sendMessage(g,$"已设置群{session.GetGroupConfigAsync(item.Id).Result.Name}({item.Id})为白名单群");

                            }
                        }
                        return;
                    }
                }
                #endregion


                #region System
                switch (msg.format())
                {
                    case @"/c":
                        KiraDX.Bot.Mod_System.Ping.PingBot(g);
                        OnCommanded.onCommanded(g,"k");
                        return;
                    default:
                        break;
                }
                if (msg.format() == "/c mainbot=alice")
                {
                    KiraDX.Bot.Mod_System.setMain.mainbot(g,"Alice");
                    OnCommanded.onCommanded(g, "ChangeBot");
                    return;
                }
                else if (msg.format() == "/c mainbot=nadia")
                {
                    KiraDX.Bot.Mod_System.setMain.mainbot(g, "Nadia");
                    OnCommanded.onCommanded(g, "ChangeBot");
                    return;
                }
                
                
                if (BotFunc.IsMainBot(g))
                {
                    if (msg == "/c pass")
                    {
                        KiraPlugin.sendMessage(g, $"bot给您使用不是义务，如果您是抱着用bot是义务的心态来加bot，那么请回，如果被我们察觉到类似的想法，有概率被挂，密码是{g.fromAccount*3}\n不要试图传播密码给其他人，每个人的密码都不一样的，第一次没加上，直到下一次重启那就都加不上，不要因为你的好心而坑了你的朋友\n用之前记得看bot协议，我们不确定我们会做出什么事情",true);
                        OnCommanded.onCommanded(g, "GetPassword");
                        return;
                    }

                    if (cmd.Length >= 3&&msg.format().StartsWith("/c mod"))
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

                    if (msg.format().StartsWith("/c channel "))
                    {
                        if (msg.format()=="/c channel list")
                        {
                            KiraDX.Bot.AllGroup.ChannelList(g);
                            return;
                        }
                        else
                        {
                            KiraDX.Bot.AllGroup.ChannelEdit(g);
                            return;
                        }
                    }
                }

                
                #endregion

                if (BotFunc.FuncSwith(g,"bot"))
                {
                    #region 色图来
                    if (botid == G.BotList.Calista&& BotFunc.FuncSwith(g, "图片"))
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
                        if ((msg.Contains("来") || msg.Contains("给我")) && (msg.Contains("张") || msg.Contains("份")) && Functions.GetNumberInString(msg) != 0 && botid == G.BotList.Calista)
                        {
                            KiraDX.Bot.Picture.Hso.Hso.GetHso(g, Functions.GetNumberInString(msg), "colorpic");
                            KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "为了减少bot被ban的概率，我们在此呼吁，少看涩图，一分钟不超过五张，手离几把越近，健康离你越远。");
                            OnCommanded.onCommanded(g, "Hso");
                            return;
                        }
                        if ((msg.Contains("这也能叫涩图") || msg.Contains("这也能叫色图")) || (msg.Contains("不够涩") || msg.Contains("不够色")) && botid == G.BotList.Calista)
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

                        string[]  dic = new []{ "查分", "/r", "/a", "/arc", "/最近", "/arς", "/αrc", @"/@rc", "/ARForest", "/ārc" };
                            foreach (var item in dic)
                            {
                                if (msg.format() == item)
                                {

                                    if (BotFunc.FuncSwith(g, "arc"))
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
                                    else
                                    {
                                    if (!BotFunc.FuncSwith(g, "模块提示"))
                                    {
                                        return;
                                    }
                                    KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群Arc模块处于关闭状态，请使用/c mod enable arc打开本群Arc模块后再查分");
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
                                    if (G.EventCfg.IsArcBoom)
                                    {
                                        KiraPlugin.sendMessage(g, "查询Arcaea信息失败，因为616日常改加密算法\n你知道吗：Lowiro是一个背叛玩家的坏游戏制作厂商");
                                        return;
                                    }
                                    KiraDX.Bot.arcaea.arcaea.b30(g);
                                    OnCommanded.onCommanded(g, "arc");
                                    return;
                                }
                                else
                                {
                                    if (!BotFunc.FuncSwith(g, "模块提示"))
                                    {
                                        return;
                                    }
                                    KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群Arc模块处于关闭状态，请使用/c mod enable arc打开本群Arc模块后再查分");
                                    return;
                                }
                            }
                        }
                        dic = new[] { "查分", "/as", "/a score", "/arc score", "/a info", "/arc info" };
                        foreach (var item in dic)
                        {
                            if (msg.format().StartsWith(item))
                            {

                                if (BotFunc.FuncSwith(g, "arc"))
                                {
                                    if (G.EventCfg.IsArcBoom)
                                    {
                                        KiraPlugin.sendMessage(g, "查询Arcaea信息失败，因为616日常改加密算法\n你知道吗：Lowiro是一个背叛玩家的坏游戏制作厂商");
                                        return;
                                    }
                                    KiraDX.Bot.arcaea.arcaea.SongBest(g);
                                    OnCommanded.onCommanded(g, "arc");
                                    return;
                                }
                                else
                                {
                                    if (!BotFunc.FuncSwith(g, "模块提示"))
                                    {
                                        return;
                                    }
                                    KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群Arc模块处于关闭状态，请使用/c mod enable arc打开本群Arc模块后再查分");

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
                                    if (G.EventCfg.IsArcBoom)
                                    {
                                        KiraPlugin.sendMessage(g, "查询Arcaea信息失败，因为616日常改加密算法\n你知道吗：Lowiro是一个背叛玩家的坏游戏制作厂商");
                                        return;
                                    }
                                    KiraDX.Bot.arcaea.arcaea.Bind(g);
                                    OnCommanded.onCommanded(g, "arc");
                                    return;
                                }
                                else
                                {
                                    if (!BotFunc.FuncSwith(g, "模块提示"))
                                    {
                                        return;
                                    }
                                    KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群Arc模块处于关闭状态，请使用/c mod enable arc打开本群Arc模块后再绑定");
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
                                    if (!BotFunc.FuncSwith(g,"模块提示"))
                                    {
                                        return;
                                    }
                                    KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群Arc模块处于关闭状态，请使用/c mod enable arc打开本群Arc模块后再随机");
                                    return;
                                }
                            }
                        }
                        #endregion
                        #region 图片
                        switch (msg.format())
                        {
                            case "/c 龙":
                            case "龙图来":
                                KiraDX.Bot.Picture.Pic.GetPic.getPic(g, "龙图",false);
                                OnCommanded.onCommanded(g, "龙图");
                                return;
                            case "/c 彩":
                            case "/一键彩彩":
                                KiraDX.Bot.Picture.Pic.GetPic.getPic(g, "丸山彩", false);
                                OnCommanded.onCommanded(g, "丸山彩");
                                return;
                            case "/c 鹦鹉":
                            case "/c parrot":
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
                            case "/hy-o":
                            case "/寒意-o":
                                KiraDX.Bot.Sentences.GoldSentences.GetSent(g, "hanyi");
                                OnCommanded.onCommanded(g, "金句");
                                return;
                            case "/hy":
                            case "/寒意":
                                KiraDX.Bot.Sentences.GoldSentences.GetSent(g, "hanyi_org");
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
                            case "/hs":
                            case "/钢琴人":
                                KiraDX.Bot.Sentences.GoldSentences.getPic(g, "hs");
                                OnCommanded.onCommanded(g, "金句");
                                return;
                        }
                        #endregion
                        #region 漂流瓶
                        if (msg.format() == "/c pick")
                        {
                            KiraDX.Bot.bottle.Bottle.PickBottle(g);
                            OnCommanded.onCommanded(g, "漂流瓶");
                            return;
                        }

                        if (msg.format() == "/c pick e")
                        {
                            KiraDX.Bot.bottle.Bottle.PickBottle_E(g);
                            OnCommanded.onCommanded(g, "漂流瓶");
                            return;
                        }

                        if (msg.ToLower().TrimStart().StartsWith ("/c throw ")|| msg.StartsWith("/c throw-u "))
                        {
                            KiraDX.Bot.bottle.Bottle.SendBottle(g);
                            OnCommanded.onCommanded(g, "漂流瓶");
                            return;
                        }
                        #endregion
                        #region Pa
                        if (msg.ToLower().StartsWith("/c pa"))
                        {
                            KiraDX.Bot.Extended.pacgnjny.pacgn(g);
                            OnCommanded.onCommanded(g, "pacgn");
                            return;
                        }

                        if (msg.format().StartsWith("/c story "))
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

                                if (!BotFunc.FuncSwith(g, "模块提示"))
                                {
                                    return;
                                }
                                KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群工具处于关闭状态，请使用/c mod enable 工具打开本群工具模块后再操作");
                                return;
                            }
                            
                        }

                        if (msg.ToLower().TrimStart().StartsWith("/c trans "))
                        {
                            if (BotFunc.FuncSwith(g,"工具"))
                            {
                                KiraDX.Bot.Others.Trans.Translate(g);
                                OnCommanded.onCommanded(g, "translate");
                                return;
                            }
                            else
                            {
                                if (!BotFunc.FuncSwith(g, "模块提示"))
                                {
                                    return;
                                }
                                KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群工具处于关闭状态，请使用/c mod enable 工具打开本群工具模块后再操作");
                                return;
                            }
                        }
                        if (msg.ToLower().TrimStart().StartsWith("/c picinfo"))
                        {

                            if (BotFunc.FuncSwith(g, "工具"))
                            {
                                
                                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, Mirai.PicInfo.GetInfo(g,e), false);

                                return;
                            }
                            else
                            {
                                if (!BotFunc.FuncSwith(g, "模块提示"))
                                {
                                    return;
                                }
                                KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群工具处于关闭状态，请使用/c mod enable 工具打开本群工具模块后再操作");
                                return;
                            }
                        }
                        if (msg.ToLower().TrimStart().StartsWith("/c len "))
                        {

                            if (BotFunc.FuncSwith(g, "工具"))
                            {
                                string text = g.msg.Replace("/c len ", "");
                                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, $"字符串{text}的长度为{Functions.StrLength(text)},字符个数为{text.Length}", false);
                                
                                return;
                            }
                            else
                            {
                                if (!BotFunc.FuncSwith(g, "模块提示"))
                                {
                                    return;
                                }
                                KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群工具处于关闭状态，请使用/c mod enable 工具打开本群工具模块后再操作");
                                return;
                            }
                        }
                        
                        if (msg.ToLower().TrimStart().StartsWith("/c write "))
                        {
                            

                            if (BotFunc.FuncSwith(g, "工具"))
                            {
                                KiraDX.Bot.Others.Writer.GetNovel(g);
                                OnCommanded.onCommanded(g, "Writer");
                                return;
                            }
                            else
                            {
                                if (!BotFunc.FuncSwith(g, "模块提示"))
                                {
                                    return;
                                }
                                KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群工具处于关闭状态，请使用/c mod enable 工具打开本群工具模块后再操作");
                                return;
                            }
                        }

                        if (msg.ToLower().TrimStart().StartsWith("/c 说 ") && BotFunc.isWhite(g))
                        {

                            if (BotFunc.FuncSwith(g, "工具"))
                            {
                                string text = g.msg.Replace("/c 说 ", "");
                                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, text, true);
                                OnCommanded.onCommanded(g, "Speak");
                                return;
                            }
                            else
                            {
                                if (!BotFunc.FuncSwith(g, "模块提示"))
                                {
                                    return;
                                }
                                KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群工具处于关闭状态，请使用/c mod enable 工具打开本群工具模块后再操作");
                                return;
                            }
                            

                        }

                        if (msg.format()=="/c get eventvalue")
                        {
                            if (BotFunc.FuncSwith(g, "工具"))
                            {
                                KiraPlugin.sendMessage(g, Story.EventValue.EventValue_Get(g.fromAccount).ToString());
                            }
                            else
                            {
                                if (!BotFunc.FuncSwith(g, "模块提示"))
                                {
                                    return;
                                }
                                KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群工具处于关闭状态，请使用/c mod enable 工具打开本群工具模块后再操作");
                                return;
                            }
                        }

                        if (msg.ToLower().TrimStart().StartsWith("/c code ") && BotFunc.IsMainBot(g))
                        {

                            if (BotFunc.FuncSwith(g, "工具"))
                            {
                                string text = g.msg.Replace("/c code ", "");
                                
                                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, text);
                            }
                            else
                            {
                                if (!BotFunc.FuncSwith(g, "模块提示"))
                                {
                                    return;
                                }
                                KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群工具处于关闭状态，请使用/c mod enable 工具打开本群工具模块后再操作");
                                return;
                            }
                           
                        }

                        #endregion
                        if (BotFunc.IsMainBot(g))
                        {
                            if (msg.format().StartsWith("/sv bind id "))
                            {
                                if (!BotFunc.FuncSwith(g, "sdvx"))
                                {
                                    if (!BotFunc.FuncSwith(g, "模块提示"))
                                    {
                                        return;
                                    }
                                    KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群sdvx模块处于关闭状态，请使用/c mod enable sdvx 打开本群sdvx模块后再操作");
                                    
                                    return;
                                }
                               
                                SDVX.UserBind.Bind_UserID(g);
                                OnCommanded.onCommanded(g, "sdvx");
                                return;
                            }
                            if (msg.format().StartsWith("/sv bind "))
                            {
                                if (!BotFunc.FuncSwith(g, "sdvx"))
                                {
                                    if (!BotFunc.FuncSwith(g, "模块提示"))
                                    {
                                        return;
                                    }
                                    KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群sdvx模块处于关闭状态，请使用/c mod enable sdvx 打开本群sdvx模块后再操作");
                                    
                                    return;
                                }
                                SDVX.UserBind.Bind_UserName(g);
                                OnCommanded.onCommanded(g, "sdvx");
                                return;
                            }
                            if (msg.format()=="/sv")
                            {
                                if (!BotFunc.FuncSwith(g, "sdvx"))
                                {
                                    if (!BotFunc.FuncSwith(g, "模块提示"))
                                    {
                                        return;
                                    }
                                    KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群sdvx模块处于关闭状态，请使用/c mod enable sdvx 打开本群sdvx模块后再操作");
                                    return;
                                }
                                SDVX.GetRecent.Recent(g);
                                OnCommanded.onCommanded(g, "sdvx");
                                return;
                            }
                            if (msg.format().StartsWith("/sv info "))
                            {
                                if (!BotFunc.FuncSwith(g, "sdvx"))
                                {
                                    if (!BotFunc.FuncSwith(g, "模块提示"))
                                    {
                                        return;
                                    }
                                    KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群sdvx模块处于关闭状态，请使用/c mod enable sdvx 打开本群sdvx模块后再操作");
                                    return;
                                }
                                SDVX.GetRecent.info(g);
                                OnCommanded.onCommanded(g, "sdvx");
                                return;
                            }
                        }

                        if (BotFunc.IsMainBot(g))
                        {
                            if (g.msg.format().StartsWith("/nb"))
                            {
                                if (BotFunc.FuncSwith(g,"软糖联动"))
                                {
                                    otherbot.lxbot.NB.GetNB(g);
                                }
                                else
                                {
                                    KiraPlugin.SendGroupMessage(g.s, fromGroup, "本群软糖联动模块处于关闭状态，请使用/c mod enable sdvx 打开本群sdvx模块后再操作");
                                    return;
                                }
                            }
                        }

                        if (BotFunc.IsMainBot(g))
                        {
                            if (g.msg.format().StartsWith("/c todev ")|| g.msg.format().StartsWith("/c todev-l "))
                            {
                                try
                                {
                                    string sts = "public";
                                    if (g.msg.format().StartsWith("/c todev-l "))
                                    {
                                        sts = "private";
                                    }
                                    KiraPlugin.sendMessage(g, $"[{sts}]\n[Group {g.fromGroup}]\n[Account {g.fromAccount}]\n{g.msg.Split(" ", count: 3)[2]}",true,IsToFriend:true,ToFriend:1930300830);
                                    KiraPlugin.sendMessage(g, "已传达");
                                    return;
                                }
                                catch (Exception ex)
                                {

                                    KiraPlugin.sendMessage(g, ex.Message);
                                    return;
                                }
                            }

                            if (msg.ToLower() == "/c dismiss group")
                            {
                                KiraDX.Bot.Mod_System.Dismiss.Dismiss_Group(g, e);
                                return;
                            }
                            if (msg.ToLower() == "/c dismiss me")
                            {
                                KiraDX.Bot.Mod_System.Dismiss.Dismiss_User(g);
                                return;
                            }
                        }
                        
                    }


                }

                /*
                if (BotFunc.FuncSwith(g, "*防风控模块")&&botid==G.BotList.Calista)
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
