using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using KiraDX.Bot;
namespace KiraDX
{
    public static class BotFunc
    {
        /// <summary>
        /// 是否为黑名单用户
        /// </summary>
        /// <param name="User"></param>
        /// <returns>返回bool 被ban的话返回true</returns>
        public static bool IsBanned(long User) {
            if (Users.ban.banList.Contains(User.ToString()))
            {
                return true;
            }
            else { 
                return false; 
            }
        
        }

        /// <summary>
        /// 获取这个bot是否需要在这个群回复，在if内用
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public static bool singleBot(GroupMsg g) {
            if (Users.BotInfo.Groups[0].Contains(g.fromGroup.ToString())&& Users.BotInfo.Groups[1].Contains(g.fromGroup.ToString()))
            {
                return IsMainBot(g);
            }
            else
            {
                return true;
            }
        
        }
        /// <summary>
        /// 判断是不是你群mainbot，事实上，设个private也无伤大雅
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public static bool IsMainBot(GroupMsg g) {
            try
            {
                if (g.botid == G.BotList.Miffy)
                {
                    return false;
                }
                //判断是否在bot的grouplst内，当两个都存在的时候，进行下一步判断，仅存在一个不进行判断
                bool A = (Users.BotInfo.Groups[0].Contains(g.fromGroup.ToString()));
                bool B = (Users.BotInfo.Groups[1].Contains(g.fromGroup.ToString()));
                if (A&&B)//同时存在
                {
                    //暂时不理，后面处理
                }
                else if (A&&!B)//A存在，B不存在，B为false，取反使B为True
                {
                    return true;
                }
                else if (!A && B)
                {
                    return true;
                }

                if (Users.Info.GetGroupConfig(g.fromGroup).mainbot=="none")
                {
                    string str = "";
                    if (g.botid == G.BotList.Laffy)
                    {
                        str = "Laffy";
                    }
                    else if (g.botid == G.BotList.Soffy)
                    {
                        str = "Soffy";
                    }
                    else
                    {
                        return false;
                    }
                    File.WriteAllText($"{G.path.Apppath}{G.path.MainBotData}{g.fromGroup}.kira", str);
                    Users.Info.GroupInfo[g.fromGroup].mainbot = str;
                }
                long bid = 0;
                string Mainbot =Users.Info.GetGroupConfig(g.fromGroup).mainbot;
                //Console.WriteLine(Mainbot);
                if (Mainbot.Contains("Soffy") || Mainbot.Contains("YUI"))
                {
                    bid = G.BotList.Soffy;
                }
                else if (Mainbot.Contains("Laffy") || Mainbot.Contains("AOI"))
                {
                    bid = G.BotList.Laffy;
                }
                else
                {
                    return false;
                }
                if (g.botid == bid)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// 判断是否为bot白名单
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public static bool isWhite(GroupMsg g)
        {
            if (isAdmin(g))
            {
                return true;
            }
            if (Users.White.WhiteUser.Contains(g.fromAccount.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool isWhite(long g)
        {
            if (isAdmin(g))
            {
                return true;
            }
            if (Users.White.WhiteUser.Contains(g.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 我想知道这个开关是否打开？
        /// </summary>
        /// <param name="g"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool FuncSwith(GroupMsg g,string name) {
            try
            {
                string cfg=Users.Info.GetGroupConfig(g.fromGroup).switches;
                if (cfg.Contains(name))
                {
                    return false;
                }
                cfg = File.ReadAllText($"{G.path.Apppath}{G.path.SwitchData}0.kira");
                if (cfg.Contains(name))
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {

                return true;
            }
            
        }


        public static bool FuncSwith_System(string name)
        {
            try
            {
                string cfg = Users.cfgs.SystemFunc;
                if (cfg.Contains(name))
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {

                return true;
            }

        }
        /// <summary>
        /// 判断是否为bot管理员
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public static bool isAdmin(GroupMsg g) {
            
            if (Users.White.adminlst.Contains(g.fromAccount.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }


        }
        public static bool isAdmin(long g)
        {

            if (Users.White.adminlst.Contains(g.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }


        }
        /// <summary>
        /// 是否为屏蔽词
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>是就true。不是返回false</returns>
        public static bool IsIllegal(string msg)
        {
            string[] dic = {"独立","香港","台湾","hk","tw","cn","国","幼","童" };
            foreach (var item in dic)
            {
                if (msg.ToLower().Contains(item))
                {
                    return true;
                }
            }
            return false;
        }

       
    }
}
