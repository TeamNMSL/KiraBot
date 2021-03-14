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
                
                if (Info["status"].ToString() != "0")
                {
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "出现了我不知道是什么已知问题的已知问题");
                    return;
                }
               
                    
                    string username = Info["content"]["name"].ToString();
                    string songid = Info["content"]["recent_score"]["song_id"].ToString();
                    string score = Info["content"]["recent_score"]["score"].ToString();
                    string diff = Info["content"]["recent_score"]["difficulty"].ToString();
                    string ptt= Info["content"]["rating"].ToString();
                    string pure= Info["content"]["recent_score"]["perfect_count"].ToString();
                    string Bigpure = Info["content"]["recent_score"]["shiny_perfect_count"].ToString();
                    string far= Info["content"]["recent_score"]["near_count"].ToString();
                    string lost= Info["content"]["recent_score"]["miss_count"].ToString();
                    string sptt= Info["content"]["recent_score"]["rating"].ToString();
                    string cleartype = Info["content"]["recent_score"]["clear_type"].ToString();
                    string playtime= Info["content"]["recent_score"]["time_played"].ToString();
                    ArcPic(g,username,songid, score,diff,friendcode,ptt,pure,Bigpure,far,lost,sptt,playtime,cleartype);
                
                
                
            }
            catch (Exception e)
            {
                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, e.Message);
                return;
            }

        }
        public static void Arc(DisMsg g)
        {
            try
            {
                string msg = g.msg;
                if (Users.Info.GetUserConfig(g.fromAccount).ArcID == "-1")
                {
                    KiraPlugin.SendMsg("Please bind your arcaea account before check your score", g);
                    return;
                }

                //string friendcode = File.ReadAllText($"{G.path.Apppath}{G.path.ArcUser}{g.fromAccount}.ini");
                string friendcode = Users.Info.GetUserConfig(g.fromAccount).ArcID;
                /*if (!File.Exists($"{G.path.Apppath}{G.path.ArcUser}{g.fromAccount}.ini"))
                {
                    KiraPlugin.SendMsg("Please bind your arcaea account before check your score", g);
                    
                    return;
                }
                string friendcode = File.ReadAllText($"{G.path.Apppath}{G.path.ArcUser}{g.fromAccount}.ini");*/
                JObject Info = GetRecent(friendcode);

                if (Info["status"].ToString() != "0")
                {
                    KiraPlugin.SendMsg("An unknown error has occurred", g);
                    return;
                }


                string username = Info["content"]["name"].ToString();
                string songid = Info["content"]["recent_score"]["song_id"].ToString();
                string score = Info["content"]["recent_score"]["score"].ToString();
                string diff = Info["content"]["recent_score"]["difficulty"].ToString();
                string ptt = Info["content"]["rating"].ToString();
                string pure = Info["content"]["recent_score"]["perfect_count"].ToString();
                string Bigpure = Info["content"]["recent_score"]["shiny_perfect_count"].ToString();
                string far = Info["content"]["recent_score"]["near_count"].ToString();
                string lost = Info["content"]["recent_score"]["miss_count"].ToString();
                string sptt = Info["content"]["recent_score"]["rating"].ToString();
                string cleartype = Info["content"]["recent_score"]["clear_type"].ToString();
                string playtime = Info["content"]["recent_score"]["time_played"].ToString();
                ArcPic(g, username, songid, score, diff, friendcode, ptt, pure, Bigpure, far, lost, sptt, playtime, cleartype);



            }
            catch (Exception e)
            {
                KiraPlugin.SendMsg(e.Message, g);
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
                string songid = Info["content"]["recent_score"]["song_id"].ToString();
                string score = Info["content"]["recent_score"]["score"].ToString();
                string diff = Info["content"]["recent_score"]["difficulty"].ToString();
                string ptt = Info["content"]["rating"].ToString();
                string pure = Info["content"]["recent_score"]["perfect_count"].ToString();
                string Bigpure = Info["content"]["recent_score"]["shiny_perfect_count"].ToString();
                string far = Info["content"]["recent_score"]["near_count"].ToString();
                string lost = Info["content"]["recent_score"]["miss_count"].ToString();
                string sptt = Info["content"]["recent_score"]["rating"].ToString();
                string cleartype = Info["content"]["recent_score"]["clear_type"].ToString();
                string playtime = Info["content"]["recent_score"]["time_played"].ToString();
                ArcPic(g, username, songid, score, diff, friendcode, ptt, pure, Bigpure, far, lost, sptt, playtime, cleartype);



            }
            catch (Exception e)
            {
                KiraPlugin.SendFriendMessage(g.s, g.fromAccount, e.Message);
                return;
            }

        }
    }
}
