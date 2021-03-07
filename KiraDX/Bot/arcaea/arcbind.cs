using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KiraDX.Bot.arcaea
{
    public static partial class arcaea
    {
        public static void Bind(GroupMsg g) {
            string msg = g.msg;
            string[] cmd = { "绑定", "/ab", "/a bind", "/arc bind","/arc绑定" };
            foreach (var item in cmd)
            {
                if (msg.StartsWith(item))
                {
                    msg = msg.Replace(item, "");
                    break;
                }
            }

            string fcd = Functions.GetNumberInString(msg).ToString();
            
            
                while (fcd.Length<9)
                {
                    fcd = "0" + fcd;
                }

            JObject Info = GetRecent(fcd);
            if (Info["status"].ToString() != "0")
            {
                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "绑定失败，好友码正确吗");
                return;
            }
            else
            {
                File.WriteAllText($"{G.path.Apppath}{G.path.ArcUser}{g.fromAccount}.ini",fcd);
                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "绑定成功qwq");
                return;
            }
        }
        public static void Bind(DisMsg g)
        {
            string msg = g.msg;
            string[] cmd = { "绑定", "/ab", "/a bind", "/arc bind", "/arc绑定" };
            foreach (var item in cmd)
            {
                if (msg.StartsWith(item))
                {
                    msg = msg.Replace(item, "");
                    break;
                }
            }

            string fcd = Functions.GetNumberInString(msg).ToString();


            while (fcd.Length < 9)
            {
                fcd = "0" + fcd;
            }

            JObject Info = GetRecent(fcd);
            if (Info["status"].ToString() != "0")
            {
                KiraPlugin.SendMsg("Wrong friend code", g);
                
                return;
            }
            else
            {
                File.WriteAllText($"{G.path.Apppath}{G.path.ArcUser}{g.fromAccount}.ini", fcd);
                KiraPlugin.SendMsg("Bind successfully", g);
                return;
            }
        }
        public static void Bind(FriendVars g)
        {
            string msg = g.msg;
            string[] cmd = { "绑定", "/ab", "/a bind", "/arc bind", "/arc绑定" };
            foreach (var item in cmd)
            {
                if (msg.StartsWith(item))
                {
                    msg = msg.Replace(item, "");
                    break;
                }
            }

            string fcd = Functions.GetNumberInString(msg).ToString();


            while (fcd.Length < 9)
            {
                fcd = "0" + fcd;
            }

            JObject Info = GetRecent(fcd);
            if (Info["status"].ToString() != "0")
            {
                KiraPlugin.SendFriendMessage(g.s, g.fromAccount, "绑定失败，好友码正确吗");
                return;
            }
            else
            {
                File.WriteAllText($"{G.path.Apppath}{G.path.ArcUser}{g.fromAccount}.ini", fcd);
                KiraPlugin.SendFriendMessage(g.s, g.fromAccount, "绑定成功qwq");
                return;
            }
        }
    }
}
