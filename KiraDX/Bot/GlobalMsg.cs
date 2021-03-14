using Mirai_CSharp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace KiraDX.Bot
{
    /// <summary>
    /// 内部全部使用的是mirai的接口而不是自己写的接口，维护注意
    /// </summary>
    class AllGroup
    {

        public static void ChannelList(GroupMsg g) {
            string[] functions = Users.cfgs.Channel.Split(",");
            string rt="[频道列表]\n";
            foreach (var item in functions)
            {
                if (ChannelIsOpen(g,item))
                {
                    rt += $"{item} on\n";
                }
                else
                {
                    rt += $"{item} off\n";
                }
            }
            rt += "您可以使用/k channel on/off ChannelName来打开或关闭某个频道";
            KiraPlugin.sendMessage(g, rt);
        }

        public static void ChannelEdit(GroupMsg g) {
            try
            {
                if (g.e.Sender.Permission!=GroupPermission.Administrator && g.e.Sender.Permission != GroupPermission.Owner)
                {
                    KiraPlugin.sendMessage(g, "你冇权限啦!w");
                    return;
                }
                string type = "";
                // /k channel on xxxx
                string[] vs = g.msg.Split(" ", count: 4);
                if (vs[2]=="on")
                {
                    type = "on";
                }
                else if (vs[2]=="off")
                {
                    type = "off";
                }
                else
                {
                    KiraPlugin.sendMessage(g, $"未知参数 {vs[2]}");
                    return;
                }
                if (!Users.cfgs.Channel.Contains(vs[3]))
                {
                    KiraPlugin.sendMessage(g, $"未知频道 {vs[3]}");
                    return;
                }

                if (!File.Exists($"{G.path.Channel}{g.fromGroup}.kira"))
                {
                    File.Create($"{G.path.Channel}{g.fromGroup}.kira");
                }
                string cfg = File.ReadAllText($"{G.path.Channel}{g.fromGroup}.kira");
                if (type=="on")
                {
                    cfg.Replace(vs[3], "");
                }
                else
                {
                    cfg += "\n" + vs[3]+"\n";
                }

                File.WriteAllText( $"{G.path.Channel}{g.fromGroup}.kira",cfg);
                if (type=="on")
                {
                    KiraPlugin.sendMessage(g, $"已打开频道 {vs[3]}");
                    return;
                }
                else
                {
                    KiraPlugin.sendMessage(g, $"已关闭频道 {vs[3]}");
                    return;
                }

            }
            catch (Exception e)
            {
             //   throw;
                KiraPlugin.sendMessage(g, e.Message);
            }
        }


        public static bool ChannelIsOpen(GroupMsg g,string c) {
            if (File.Exists($"{G.path.Channel}{g.fromGroup}.kira"))
            {
                if (File.ReadAllText($"{G.path.Channel}{g.fromGroup}.kira").Contains(c))
                {
                    return false;
                }
            }
            return true;
        
        }
        public static async void sendAll(GroupMsg g, IGroupMessageEventArgs e) {
            if (g.botid==G.BotList.Miffy)
            {
                return;
            }
            // /k qj c info
            string[] info = g.msg.Split(" ", count: 4);
            if (info.Length!=4)
            {
                KiraPlugin.sendMessage(g, "参数错误");
                return;
            }
            if (!Users.cfgs.Channel.Contains(info[2]))
            {
                KiraPlugin.sendMessage(g, $"不存在的频道 {info[2]}");
                return;
            }

            List<IGroupInfo> gl = KiraPlugin.GetGroupListAsync(g.s).Result;
            foreach (var item in gl)
            {
                GroupMsg gm = new GroupMsg(g.msg, item.Id, g.fromAccount, g.botid, g.s,g.e);
                if (ChannelIsOpen(gm,info[2]))
                {
                    if (BotFunc.IsMainBot(gm))
                    {
                        KiraPlugin.sendMessage(gm, $"[{info[2]}] \n{info[3]}\n提示:你可以通过/k channel off/on {info[2]}来打开或禁用这个频道\n随机数[{Functions.GetRandomNumber(1,114514)}]", true);
                    }
                    
                }
            }
           

            KiraPlugin.sendMessage(g, "广播完毕");
           
        } 
    }
}
