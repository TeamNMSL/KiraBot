using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;

namespace KiraDX.Bot
{

    
    public class DisMsg
    {
        public DiscordClient s;
        public MessageCreateEventArgs e;
        public string msg;
        public long fromAccount;
        public long fromGroup;
        public long BotId = 1145141919810;
        public string Type = "Discord";

        public DisMsg(DiscordClient s, MessageCreateEventArgs e, string msg, long fromAccount, long fromGroup)
        {
            this.s = s;
            this.e = e;
            this.msg = msg;
            this.fromAccount = fromAccount;
            this.fromGroup = fromGroup;
            BotId = 1145141919810;
        }
    }
    public class Discord
    {
        public static void DiscordMsg(Object oj) {
            
            
            DisMsg g = oj as DisMsg;
            if (g.e.Author.IsBot)
            {
                return;
            }
            string msg = g.msg;
            if (msg=="/k help")
            {
                KiraPlugin.SendMsg("[KiraDiscord]\nBind your Arcaea account:\n/arc bind <FriendCode>\nCheck your recent score:\n /arc\nCheck your best score in the song:\n /arc info <songid> [Difficulty]\n Check your best 30:\n /arc b30", g);
                return;
            }

            switch (msg)
            {
                case "色圖來":
                    KiraDX.Bot.Picture.Hso.Hso.GetHso(g,"colorpic");
                    return;
                case "色图来":
                    KiraDX.Bot.Picture.Hso.Hso.GetHso(g, "colorpic");
                    return;
                case "/hso":
                    KiraDX.Bot.Picture.Hso.Hso.GetHso(g, "colorpic");
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

            dic = new[] { "/b30", "/arc b30", "/a3", "/a b30", "b30", "查b30", "/arc b114514","/r10" };
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

        }
    }
}
