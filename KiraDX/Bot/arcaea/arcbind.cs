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
            try
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
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "绑定失败，好友码正确吗");
                    return;
                }
                else
                {
                    File.WriteAllText($"{G.path.Apppath}{G.path.ArcUser}{g.fromAccount}.ini", fcd);
                    Users.Info.GetUserConfig(g.fromAccount);
                    if (Users.Info.UserInfo.ContainsKey(g.fromAccount))
                    {
                        Users.Info.UserInfo[g.fromAccount].ArcID = fcd;
                    }
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "绑定成功qwq");
                    return;
                }
            }
            catch (Exception e)
            {

                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, e.Message);
            }
        }
       
        public static void Bind(FriendVars g)
        {
            try
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
                    Users.Info.GetUserConfig(g.fromAccount);
                    if (Users.Info.UserInfo.ContainsKey(g.fromAccount))
                    {
                        Users.Info.UserInfo[g.fromAccount].ArcID = fcd;
                    }
                    KiraPlugin.SendFriendMessage(g.s, g.fromAccount, "绑定成功qwq");
                    return;
                }
            }
            catch (Exception e)
            {
                KiraPlugin.SendFriendMessage(g.s, g.fromAccount, e.Message);
            }
        }
    }
}
