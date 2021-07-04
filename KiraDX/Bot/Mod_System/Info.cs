using System;
using System.Collections.Generic;
using System.Text;

namespace KiraDX.Bot.Mod_System
{
   public  class Info
    {
        public static void GetAllInfo(GroupMsg g) {
            if (!BotFunc.IsMainBot(g))
            {
                return;
            }
            (bool, Mirai_CSharp.MiraiHttpSession) so = (false,null);
            (bool, Mirai_CSharp.MiraiHttpSession) la = (false, null);
            (bool, Mirai_CSharp.MiraiHttpSession) mi = (false, null);
            string rtinf = "[BotUsersInfo]\n";
            if (Users.BotSession.Alice==null)
            {
                rtinf += "Session.Alice is null\n";
            }
            else
            {
                rtinf += "Session.Alice has been assigned a value\n";
                so = (true, Users.BotSession.Alice);
            }
            if (Users.BotSession.Nadia == null)
            {
                rtinf += "Session.Nadia is null\n";

            }
            else
            {
                rtinf += "Session.Nadia has been assigned a value\n";
                la = (true, Users.BotSession.Nadia);
            }
            if (Users.BotSession.Calista == null)
            {
                rtinf += "Session.Calista is null\n";
            }
            else
            {
                rtinf += "Session.Calista has been assigned a value\n";
                mi = (true, Users.BotSession.Calista);
            }
            
            Dictionary<long, Mirai_CSharp.Models.IGroupInfo> gl = new Dictionary<long, Mirai_CSharp.Models.IGroupInfo>();
            Dictionary<long, Mirai_CSharp.Models.IGroupMemberInfo> ml = new Dictionary<long, Mirai_CSharp.Models.IGroupMemberInfo>();
            Dictionary<long, Mirai_CSharp.Models.IFriendInfo> fl = new Dictionary<long, Mirai_CSharp.Models.IFriendInfo>();
            if (so.Item1)
            {
                List<Mirai_CSharp.Models.IGroupInfo> sogl=KiraPlugin.GetGroupListAsync(so.Item2).Result;
                foreach (var item in sogl)
                {
                    if (!gl.ContainsKey(item.Id))
                    {
                        gl.Add(item.Id, item);
                        foreach (var i in KiraPlugin.GetMemberListAsync(so.Item2,item.Id).Result)
                        {
                            if (!ml.ContainsKey(i.Id))
                            {
                                ml.Add(i.Id, i);
                            }
                        }
                    }
                }
                foreach (var item in KiraPlugin.GetFriendListAsync(so.Item2).Result)
                {
                    if (!fl.ContainsKey(item.Id))
                    {
                        fl.Add(item.Id, item);
                    }
                }
            }
            if (la.Item1)
            {
                List<Mirai_CSharp.Models.IGroupInfo> lagl = KiraPlugin.GetGroupListAsync(la.Item2).Result;
                foreach (var item in lagl)
                {
                    if (!gl.ContainsKey(item.Id))
                    {
                        gl.Add(item.Id, item);
                        foreach (var i in KiraPlugin.GetMemberListAsync(la.Item2, item.Id).Result)
                        {
                            if (!ml.ContainsKey(i.Id))
                            {
                                ml.Add(i.Id, i);
                            }
                        }
                    }
                }
                foreach (var item in KiraPlugin.GetFriendListAsync(la.Item2).Result)
                {
                    if (!fl.ContainsKey(item.Id))
                    {
                        fl.Add(item.Id, item);
                    }
                }
            }
            if (mi.Item1)
            {
                List<Mirai_CSharp.Models.IGroupInfo> migl = KiraPlugin.GetGroupListAsync(mi.Item2).Result;
                foreach (var item in migl)
                {
                    if (!gl.ContainsKey(item.Id))
                    {
                        gl.Add(item.Id, item);
                        foreach (var i in KiraPlugin.GetMemberListAsync(mi.Item2, item.Id).Result)
                        {
                            if (!ml.ContainsKey(i.Id))
                            {
                                ml.Add(i.Id, i);
                            }
                        }
                    }
                }
                foreach (var item in KiraPlugin.GetFriendListAsync(mi.Item2).Result)
                {
                    if (!fl.ContainsKey(item.Id))
                    {
                        fl.Add(item.Id, item);
                    }
                }

            }
            rtinf += $"GroupTotal:{gl.Count}\n";
            rtinf += $"GroupMemberTotal:{ml.Count}\n";
            rtinf += $"FriendTotal:{fl.Count}\n";
            Dictionary<long, int> ac = new Dictionary<long, int>();
            foreach (var item in ml)
            {
                if (!ac.ContainsKey(item.Key))
                {
                    ac.Add(item.Key, 0);
                }
            }
            foreach (var item in fl)
            {
                if (!ac.ContainsKey(item.Key))
                {
                    ac.Add(item.Key, 0);
                }
            }
            rtinf += $"UsersTotal:{ac.Count}";
            KiraPlugin.sendMessage(g, rtinf);
        }
    }
}
