using Microsoft.Data.Sqlite;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;


namespace KiraDX.Bot.arcaea
{
    public static partial class arcaea
    {
        public static void Arc(GroupMsg g) {
            try
            {
                string msg = g.msg;
                /*if (!File.Exists($"{G.path.Apppath}{G.path.ArcUser}{g.fromAccount}.ini"))
                {
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "你还没绑定辣！w");
                    return;
                }*/
                if (Users.Info.GetUserConfig(g.fromAccount).ArcID=="-1")
                {
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "你还没绑定辣！w");
                    return;
                }

                //string friendcode = File.ReadAllText($"{G.path.Apppath}{G.path.ArcUser}{g.fromAccount}.ini");
                string friendcode = Users.Info.GetUserConfig(g.fromAccount).ArcID;
                JObject Info = GetRecent(friendcode);
                if (G.EventCfg.debugMode)
                {
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, Info.ToString());
                }
                if (Info["status"].ToString() != "0")
                {
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "出现了我不知道是什么已知问题的已知问题");
                    return;
                }
               
                    
                    string username = Info["content"]["name"].ToString();
                    string songid = Info["content"]["recent_score"][0]["song_id"].ToString();
                    string score = Info["content"]["recent_score"][0]["score"].ToString();
                    string diff = Info["content"]["recent_score"][0]["difficulty"].ToString();
                    string ptt= Info["content"]["rating"].ToString();
                    string pure= Info["content"]["recent_score"][0]["perfect_count"].ToString();
                    string Bigpure = Info["content"]["recent_score"][0]["shiny_perfect_count"].ToString();
                    string far= Info["content"]["recent_score"][0]["near_count"].ToString();
                    string lost= Info["content"]["recent_score"][0]["miss_count"].ToString();
                    string sptt= Info["content"]["recent_score"][0]["rating"].ToString();
                    string cleartype = Info["content"]["recent_score"][0]["clear_type"].ToString();
                    string playtime= Info["content"]["recent_score"][0]["time_played"].ToString();
                    ArcPic(g,username,songid, score,diff,friendcode,ptt,pure,Bigpure,far,lost,sptt,playtime,cleartype);
                
                
                
            }
            catch (Exception e)
            {
                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, e.Message);
                return;
            }

        }
       
        
        public static void Arc(FriendVars g)
        {
            try
            {
                string msg = g.msg;
                /* if (!File.Exists($"{G.path.Apppath}{G.path.ArcUser}{g.fromAccount}.ini"))
                 {
                     KiraPlugin.SendFriendMessage(g.s, g.fromAccount, "你还没绑定辣！w");
                     return;
                 }
                 string friendcode = File.ReadAllText($"{G.path.Apppath}{G.path.ArcUser}{g.fromAccount}.ini");*/

                if (Users.Info.GetUserConfig(g.fromAccount).ArcID == "-1")
                {
                    KiraPlugin.SendFriendMessage(g.s, g.fromAccount, "你还没绑定辣！w");
                    return;
                }

                //string friendcode = File.ReadAllText($"{G.path.Apppath}{G.path.ArcUser}{g.fromAccount}.ini");
                string friendcode = Users.Info.GetUserConfig(g.fromAccount).ArcID;

                JObject Info = GetRecent(friendcode);

                if (Info["status"].ToString() != "0")
                {
                    KiraPlugin.SendFriendMessage(g.s, g.fromAccount, "出现了我不知道是什么已知问题的已知问题");
                    return;
                }



                string username = Info["content"]["name"].ToString();
                string songid = Info["content"]["recent_score"][0]["song_id"].ToString();
                string score = Info["content"]["recent_score"][0]["score"].ToString();
                string diff = Info["content"]["recent_score"][0]["difficulty"].ToString();
                string ptt = Info["content"]["rating"].ToString();
                string pure = Info["content"]["recent_score"][0]["perfect_count"].ToString();
                string Bigpure = Info["content"]["recent_score"][0]["shiny_perfect_count"].ToString();
                string far = Info["content"]["recent_score"][0]["near_count"].ToString();
                string lost = Info["content"]["recent_score"][0]["miss_count"].ToString();
                string sptt = Info["content"]["recent_score"][0]["rating"].ToString();
                string cleartype = Info["content"]["recent_score"][0]["clear_type"].ToString();
                string playtime = Info["content"]["recent_score"][0]["time_played"].ToString();
                ArcPic(g, username, songid, score, diff, friendcode, ptt, pure, Bigpure, far, lost, sptt, playtime, cleartype);



            }
            catch (Exception e)
            {
                KiraPlugin.SendFriendMessage(g.s, g.fromAccount, e.ToString());
                return;
            }

        }
    }
}
