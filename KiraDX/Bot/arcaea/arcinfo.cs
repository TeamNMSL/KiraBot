using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace KiraDX.Bot.arcaea
{
    public static partial class arcaea
    {
        private class DiffAndMsg {
            public string msg;
            public int diff;
            public DiffAndMsg(string msg, int diff)
            {
                this.msg = msg;
                this.diff = diff;
            }
        }
        private delegate DiffAndMsg DiffDelegate(string d);
        public static void SongBest(GroupMsg g) {
            try
            {
                string msg = g.msg;

                if (Users.Info.GetUserConfig(g.fromAccount).ArcID == "-1")
                {
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "你还没绑定辣！w");
                    return;
                }

                //string friendcode = File.ReadAllText($"{G.path.Apppath}{G.path.ArcUser}{g.fromAccount}.ini");
                string friendcode = Users.Info.GetUserConfig(g.fromAccount).ArcID;

                if (msg == "查分"||msg=="查")
                {
                    Arc(g);
                    return;
                }

                string[] cmd = { "查分", "/as", "/a score", "/arc score", "/a info", "/arc info","查" };
                foreach (var item in cmd)
                {
                    if (msg.StartsWith(item))
                    {
                        msg = msg.Replace(item, "");
                        break;
                    }
                }
                int diff = 2;
                string[] COMMAND_ARRAY = msg.Split(' ');
                Dictionary<string, DiffDelegate> diffs = new Dictionary<string, DiffDelegate>() {
                { "ftr",(msg)=>{return new DiffAndMsg(msg.Replace("ftr",""),2); } },
                { "future",(msg)=>{return new DiffAndMsg(msg.Replace("future",""),2); } },
                { "present",(msg)=>{return new DiffAndMsg(msg.Replace("present",""),1); } },
                { "prs",(msg)=>{return new DiffAndMsg(msg.Replace("prs",""),1); } },
                { "past",(msg)=>{return new DiffAndMsg(msg.Replace("past",""),0); } },
                { "pst",(msg)=>{return new DiffAndMsg(msg.Replace("pst",""),0); } },
                { "beyond",(msg)=>{return new DiffAndMsg(msg.Replace("beyond",""),3); } },
                { "byd",(msg)=>{return new DiffAndMsg(msg.Replace("byd",""),3); } },
                { "byn",(msg)=>{return new DiffAndMsg(msg.Replace("byn",""),3); } },
            };
                foreach (var item in COMMAND_ARRAY)
                {
                    if (diffs.ContainsKey(item.ToLower()))
                    {
                        diff = diffs[item.ToLower()].Invoke(item.ToLower()).diff;
                        msg = diffs[item.ToLower()].Invoke(msg.ToLower()).msg;

                        break;
                    }
                }
                //string friendcode = File.ReadAllText($"{G.path.Apppath}{G.path.ArcUser}{g.fromAccount}.ini");
                SongScore info = GetSongScore(friendcode, msg, diff.ToString());
                JObject UserInfo = info.User;
                JObject ScoreInfo = info.Score;
                /*
                  地位	描述
                 0	everything is OK
                -1	invalid usercode
                -2	invalid songname
                -3	invalid difficulty
                -4	invalid difficulty (map format failed)
                -5	this song is not recorded in the database
                -6	too many records
                -7	internal error
                -8	this song has no beyond level
                -9	allocate an arc account failed
                -10	clear friend list failed
                -11	add friend failed
                -12	internal error occurred
                -13	internal error occurred
                -14	not played yet
                -233	unknown error occurred
                */
                switch (ScoreInfo["status"].ToString())
                {
                    case "0":
                        break;

                    case "-2":
                        KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "这事什么屑歌啊（w(error code=-2");
                        return;
                    case "-5":
                        KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "这事什么屑歌啊（w(error code=-5");
                        return;
                    case "-6":
                        KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "别称过于模糊");
                        return;
                    case "-8":
                        KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "这歌🈚byd难度");
                        return;
                    case "-14":
                        KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "您没打（");
                        return;
                    default:
                        KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "出现了我不知道是什么已知问题的已知问题");
                        return;
                }
                if (UserInfo["status"].ToString() != "0")
                {
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "出现了我不知道是什么已知问题的已知问题");
                    return;
                }

                
                string username=UserInfo["content"]["name"].ToString();
                string songid=ScoreInfo["content"]["song_id"].ToString();
                string score = ScoreInfo["content"]["score"].ToString();
                string difficu = ScoreInfo["content"]["difficulty"].ToString();
                string ptt = UserInfo["content"]["rating"].ToString();
                string pure = ScoreInfo["content"]["perfect_count"].ToString();
                string Bigpure = ScoreInfo["content"]["shiny_perfect_count"].ToString();
                string far = ScoreInfo["content"]["near_count"].ToString();
                string lost = ScoreInfo["content"]["miss_count"].ToString();
                string sptt = ScoreInfo["content"]["rating"].ToString();
                string cleartype = ScoreInfo["content"]["clear_type"].ToString();
                string playtime = ScoreInfo["content"]["time_played"].ToString();
                ArcPic(g, username, songid, score, difficu, friendcode, ptt, pure, Bigpure, far, lost, sptt, playtime, cleartype);
               

            }
            catch (Exception e)
            {
                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, e.Message);
                return;
            }
            
        }
        public static void SongBest(FriendVars g)
        {
            try
            {
                string msg = g.msg;

                if (Users.Info.GetUserConfig(g.fromAccount).ArcID == "-1")
                {
                    KiraPlugin.SendFriendMessage(g.s, g.fromAccount, "你还没绑定辣！w");
                    return;
                }

                //string friendcode = File.ReadAllText($"{G.path.Apppath}{G.path.ArcUser}{g.fromAccount}.ini");
                string friendcode = Users.Info.GetUserConfig(g.fromAccount).ArcID;

                if (msg == "查分")
                {
                    Arc(g);
                    return;
                }

                string[] cmd = { "查分", "/as", "/a score", "/arc score", "/a info", "/arc info" };
                foreach (var item in cmd)
                {
                    if (msg.StartsWith(item))
                    {
                        msg = msg.Replace(item, "");
                        break;
                    }
                }
                int diff = 2;
                string[] COMMAND_ARRAY = msg.Split(' ');
                Dictionary<string, DiffDelegate> diffs = new Dictionary<string, DiffDelegate>() {
                { "ftr",(msg)=>{return new DiffAndMsg(msg.Replace("ftr",""),2); } },
                { "future",(msg)=>{return new DiffAndMsg(msg.Replace("future",""),2); } },
                { "present",(msg)=>{return new DiffAndMsg(msg.Replace("present",""),1); } },
                { "prs",(msg)=>{return new DiffAndMsg(msg.Replace("prs",""),1); } },
                { "past",(msg)=>{return new DiffAndMsg(msg.Replace("past",""),0); } },
                { "pst",(msg)=>{return new DiffAndMsg(msg.Replace("pst",""),0); } },
                { "beyond",(msg)=>{return new DiffAndMsg(msg.Replace("beyond",""),3); } },
                { "byd",(msg)=>{return new DiffAndMsg(msg.Replace("byd",""),3); } },
                { "byn",(msg)=>{return new DiffAndMsg(msg.Replace("byn",""),3); } },
            };
                foreach (var item in COMMAND_ARRAY)
                {
                    if (diffs.ContainsKey(item.ToLower()))
                    {
                        diff = diffs[item.ToLower()].Invoke(item.ToLower()).diff;
                        msg = diffs[item.ToLower()].Invoke(msg.ToLower()).msg;

                        break;
                    }
                }
               // string friendcode = File.ReadAllText($"{G.path.Apppath}{G.path.ArcUser}{g.fromAccount}.ini");
                SongScore info = GetSongScore(friendcode, msg, diff.ToString());
                JObject UserInfo = info.User;
                JObject ScoreInfo = info.Score;
                /*
                  地位	描述
                 0	everything is OK
                -1	invalid usercode
                -2	invalid songname
                -3	invalid difficulty
                -4	invalid difficulty (map format failed)
                -5	this song is not recorded in the database
                -6	too many records
                -7	internal error
                -8	this song has no beyond level
                -9	allocate an arc account failed
                -10	clear friend list failed
                -11	add friend failed
                -12	internal error occurred
                -13	internal error occurred
                -14	not played yet
                -233	unknown error occurred
                */
                switch (ScoreInfo["status"].ToString())
                {
                    case "0":
                        break;

                    case "-2":
                        KiraPlugin.SendFriendMessage(g.s, g.fromAccount, "这事什么屑歌啊（w(error code=-2");
                        return;
                    case "-5":
                        KiraPlugin.SendFriendMessage(g.s, g.fromAccount, "这事什么屑歌啊（w(error code=-5");
                        return;
                    case "-6":
                        KiraPlugin.SendFriendMessage(g.s, g.fromAccount, "别称过于模糊");
                        return;
                    case "-8":
                        KiraPlugin.SendFriendMessage(g.s, g.fromAccount, "这歌🈚byd难度");
                        return;
                    case "-14":
                        KiraPlugin.SendFriendMessage(g.s, g.fromAccount, "您没打（");
                        return;
                    default:
                        KiraPlugin.SendFriendMessage(g.s, g.fromAccount, "出现了我不知道是什么已知问题的已知问题");
                        return;
                }
                if (UserInfo["status"].ToString() != "0")
                {
                    KiraPlugin.SendFriendMessage(g.s, g.fromAccount, "出现了我不知道是什么已知问题的已知问题");
                    return;
                }


                string username = UserInfo["content"]["name"].ToString();
                string songid = ScoreInfo["content"]["song_id"].ToString();
                string score = ScoreInfo["content"]["score"].ToString();
                string difficu = ScoreInfo["content"]["difficulty"].ToString();
                string ptt = UserInfo["content"]["rating"].ToString();
                string pure = ScoreInfo["content"]["perfect_count"].ToString();
                string Bigpure = ScoreInfo["content"]["shiny_perfect_count"].ToString();
                string far = ScoreInfo["content"]["near_count"].ToString();
                string lost = ScoreInfo["content"]["miss_count"].ToString();
                string sptt = ScoreInfo["content"]["rating"].ToString();
                string cleartype = ScoreInfo["content"]["clear_type"].ToString();
                string playtime = ScoreInfo["content"]["time_played"].ToString();
                ArcPic(g, username, songid, score, difficu, friendcode, ptt, pure, Bigpure, far, lost, sptt, playtime, cleartype);


            }
            catch (Exception e)
            {
                KiraPlugin.SendFriendMessage(g.s, g.fromAccount, e.Message);
                return;
            }

        }
        public static void SongBest(DisMsg g)
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

                if (msg == "查分")
                {
                    Arc(g);
                    return;
                }

                string[] cmd = { "查分", "/as", "/a score", "/arc score", "/a info", "/arc info" };
                foreach (var item in cmd)
                {
                    if (msg.StartsWith(item))
                    {
                        msg = msg.Replace(item, "");
                        break;
                    }
                }
                int diff = 2;
                string[] COMMAND_ARRAY = msg.Split(' ');
                Dictionary<string, DiffDelegate> diffs = new Dictionary<string, DiffDelegate>() {
                { "ftr",(msg)=>{return new DiffAndMsg(msg.Replace("ftr",""),2); } },
                { "future",(msg)=>{return new DiffAndMsg(msg.Replace("future",""),2); } },
                { "present",(msg)=>{return new DiffAndMsg(msg.Replace("present",""),1); } },
                { "prs",(msg)=>{return new DiffAndMsg(msg.Replace("prs",""),1); } },
                { "past",(msg)=>{return new DiffAndMsg(msg.Replace("past",""),0); } },
                { "pst",(msg)=>{return new DiffAndMsg(msg.Replace("pst",""),0); } },
                { "beyond",(msg)=>{return new DiffAndMsg(msg.Replace("beyond",""),3); } },
                { "byd",(msg)=>{return new DiffAndMsg(msg.Replace("byd",""),3); } },
                { "byn",(msg)=>{return new DiffAndMsg(msg.Replace("byn",""),3); } },
            };
                foreach (var item in COMMAND_ARRAY)
                {
                    if (diffs.ContainsKey(item.ToLower()))
                    {
                        diff = diffs[item.ToLower()].Invoke(item.ToLower()).diff;
                        msg = diffs[item.ToLower()].Invoke(msg.ToLower()).msg;

                        break;
                    }
                }
               // string friendcode = File.ReadAllText($"{G.path.Apppath}{G.path.ArcUser}{g.fromAccount}.ini");
                SongScore info = GetSongScore(friendcode, msg, diff.ToString());
                JObject UserInfo = info.User;
                JObject ScoreInfo = info.Score;
                /*
                  地位	描述
                 0	everything is OK
                -1	invalid usercode
                -2	invalid songname
                -3	invalid difficulty
                -4	invalid difficulty (map format failed)
                -5	this song is not recorded in the database
                -6	too many records
                -7	internal error
                -8	this song has no beyond level
                -9	allocate an arc account failed
                -10	clear friend list failed
                -11	add friend failed
                -12	internal error occurred
                -13	internal error occurred
                -14	not played yet
                -233	unknown error occurred
                */
                switch (ScoreInfo["status"].ToString())
                {
                    case "0":
                        break;

                    case "-2":
                        KiraPlugin.SendMsg("Unknown song", g);
                       
                        return;
                    case "-5":
                        KiraPlugin.SendMsg("Unknown song", g);
                       
                        return;
                    case "-6":
                        KiraPlugin.SendMsg("Fuzzy query has multiple results", g);
                        
                        return;
                    case "-8":
                        KiraPlugin.SendMsg("No such difficulty", g);
                        
                        return;
                    case "-14":
                        KiraPlugin.SendMsg("You haven't played this chart", g);
                       
                        return;
                    default:
                        KiraPlugin.SendMsg("An unknown error has occurred", g);
                        
                        return;
                }
                if (UserInfo["status"].ToString() != "0")
                {
                    KiraPlugin.SendMsg("An unknown error has occurred", g);
                    return;
                }


                string username = UserInfo["content"]["name"].ToString();
                string songid = ScoreInfo["content"]["song_id"].ToString();
                string score = ScoreInfo["content"]["score"].ToString();
                string difficu = ScoreInfo["content"]["difficulty"].ToString();
                string ptt = UserInfo["content"]["rating"].ToString();
                string pure = ScoreInfo["content"]["perfect_count"].ToString();
                string Bigpure = ScoreInfo["content"]["shiny_perfect_count"].ToString();
                string far = ScoreInfo["content"]["near_count"].ToString();
                string lost = ScoreInfo["content"]["miss_count"].ToString();
                string sptt = ScoreInfo["content"]["rating"].ToString();
                string cleartype = ScoreInfo["content"]["clear_type"].ToString();
                string playtime = ScoreInfo["content"]["time_played"].ToString();
                ArcPic(g, username, songid, score, difficu, friendcode, ptt, pure, Bigpure, far, lost, sptt, playtime, cleartype);


            }
            catch (Exception e)
            {
                KiraPlugin.SendMsg(e.Message, g);
                return;
            }

        }
    }
}
