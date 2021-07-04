using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KiraDX.Bot.SDVX
{
    class GetRecent
    {
        static public string GetSongName(string json, string id) {
            JObject jo = (JObject)JsonConvert.DeserializeObject(json);
            JObject music = (JObject)jo["_related"];
            foreach (var item in music["music"])
            {
                if (item["_id"].ToString()==id)
                {
                    return item["title"].ToString();
                }
            }
            return "?";
        }
        static public string GetChartDiff(string json, string id,bool justdiff=false)
        {
            JObject jo = (JObject)JsonConvert.DeserializeObject(json);
            JObject music = (JObject)jo["_related"];
            foreach (var item in music["charts"])
            {
                if (item["_id"].ToString() == id)
                {
                    if (justdiff)
                    {
                        return item["difficulty"].ToString();
                    }
                    else
                    {
                        return item["difficulty"].ToString() + " " + item["rating"].ToString();
                    }
                    
                }
            }
            return "?";
        }
        static public string GetRating(string json, string id)
        {
            JObject jo = (JObject)JsonConvert.DeserializeObject(json);
            JObject music = (JObject)jo["_related"];
            foreach (var item in music["charts"])
            {
                if (item["_id"].ToString() == id)
                {
                    
                        return item["rating"].ToString();
                    

                }
            }
            return "?";
        }
        static public string TimeRecord(string json)
        {
            JArray array = JArray.Parse(((JObject)JsonConvert.DeserializeObject(json))["_items"].ToString());

            JArray sorted = new JArray(array.OrderByDescending(obj => (string)obj["timestamp"]));
            JObject jo = new JObject(
           new JProperty("_items", sorted));


            return jo.ToString();
        }
        static public void info(GroupMsg g) {
            if (Users.Info.GetUserConfig(g.fromAccount).SdvxCode == "-1")
            {
                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "你还没绑定辣！w");
                return;
            }
            string json = GetInfo.GetBest(Users.Info.GetUserConfig(g.fromAccount).SdvxCode);
            JObject jo = (JObject)JsonConvert.DeserializeObject(json);
            JObject Scores = (JObject)JsonConvert.DeserializeObject(TimeRecord(json));
            JObject Recent;
// /sv info qwq adv
            //string[] cmds = g.msg.Split(" ", count:4);
            string udf="DFT";
            string usong = "ERRNULL";
            g.msg=g.msg.format().Replace("/sv info ","");
            if (g.msg.format().Contains("nov")|| g.msg.format().Contains("novice"))
            {
                g.msg = g.msg.format().Replace("nov", "").Replace("novice", "").format();
                udf = "NOV";
            }
            else if (g.msg.format().Contains("adv") || g.msg.format().Contains("advanced"))
            {
                g.msg = g.msg.format().Replace("adv", "").Replace("advanced", "").format();
                udf = "ADV";
            }
            else if (g.msg.format().Contains("exh") || g.msg.format().Contains("exhaust"))
            {
                g.msg = g.msg.format().Replace("exh", "").Replace("exhaust", "").format();
                udf = "EXH";
            }
            else if (g.msg.format().Contains("mxm") || g.msg.format().Contains("maximum"))
            {
                g.msg = g.msg.format().Replace("mxm", "").Replace("maximum", "").format();
                udf = "MXM";
            }
            else if (g.msg.format().Contains("inf") || g.msg.format().Contains("infinite"))
            {
                g.msg = g.msg.format().Replace("inf", "").Replace("infinite", "").format();
                udf = "INF";
            }
            else
            {
                udf = "DFT";
            }
           
            string songname="ERRNULL";
            string diff;
            foreach (var item in Scores["_items"])
            {
                songname = GetSongName(json, item["music_id"].ToString());
                diff = GetChartDiff(json, item["chart_id"].ToString(), true);
                if ((g.msg.format()==songname.format() && diff == udf) || (g.msg.format()==songname.format() && udf == "DFT"))
                {
                    Recent = (JObject)item;
                    string Diff = GetChartDiff(json, item["chart_id"].ToString());
                    //string rpl = $"UserName:{jo["_related"]["profiles"][0]["name"]}\nUserCode:{jo["_related"]["profiles"][0]["_id"]}\nSong:{songname}[{Diff}]\nScore:{Recent["score"].ToString()}({Recent["grade"].ToString()})\nLamp:{Recent["lamp"].ToString()}\nCritical:{Recent["critical"].ToString()}\nNear:{Recent["near"].ToString()}\nError:{Recent["error"].ToString()}\nMaxChain:{Recent["max_chain"].ToString()}";
                    Pic.SVPic(g, jo["_related"]["profiles"][0]["name"].ToString(), songname, Recent["score"].ToString(), GetChartDiff(json, item["chart_id"].ToString(), true), jo["_related"]["profiles"][0]["_id"].ToString(), "", Recent["critical"].ToString(), "", Recent["near"].ToString(), Recent["error"].ToString(), Recent["max_chain"].ToString(), "", "", GetRating(json, item["chart_id"].ToString()));
                    //KiraPlugin.sendMessage(g, rpl);
                    return;
                }

            }
            foreach (var item in Scores["_items"])
            {
                songname = GetSongName(json, item["music_id"].ToString());
                diff = GetChartDiff(json, item["chart_id"].ToString(),true);
                if ((g.msg.format().Contains(songname.format())&&diff==udf)|| (g.msg.format().Contains(songname.format()) && udf == "DFT"))
                {
                    Recent = (JObject)item;
                    string Diff = GetChartDiff(json, item["chart_id"].ToString());
                    //string rpl = $"UserName:{jo["_related"]["profiles"][0]["name"]}\nUserCode:{jo["_related"]["profiles"][0]["_id"]}\nSong:{songname}[{Diff}]\nScore:{Recent["score"].ToString()}({Recent["grade"].ToString()})\nLamp:{Recent["lamp"].ToString()}\nCritical:{Recent["critical"].ToString()}\nNear:{Recent["near"].ToString()}\nError:{Recent["error"].ToString()}\nMaxChain:{Recent["max_chain"].ToString()}";
                    Pic.SVPic(g, jo["_related"]["profiles"][0]["name"].ToString(), songname, Recent["score"].ToString(), GetChartDiff(json, item["chart_id"].ToString(),true), jo["_related"]["profiles"][0]["_id"].ToString(), "", Recent["critical"].ToString(), "", Recent["near"].ToString(), Recent["error"].ToString(), Recent["max_chain"].ToString(), "", "", GetRating(json, item["chart_id"].ToString()));
                    //KiraPlugin.sendMessage(g, rpl);
                    return;
                }

            }

            KiraPlugin.sendMessage(g, "我没查到");
        }

        static public void Recent(GroupMsg g) {
            if (Users.Info.GetUserConfig(g.fromAccount).SdvxCode == "-1")
            {
                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "你还没绑定辣！w");
                return;
            }
            string json = GetInfo.GetBest(Users.Info.GetUserConfig(g.fromAccount).SdvxCode);
            JObject jo = (JObject)JsonConvert.DeserializeObject(json);
            JObject Scores = (JObject)JsonConvert.DeserializeObject(TimeRecord(json));
            JObject Recent = (JObject)Scores["_items"][0];
            string songname = GetSongName(json, Recent["music_id"].ToString());
            string Diff = GetChartDiff(json, Recent["chart_id"].ToString());
            Pic.SVPic(g, jo["_related"]["profiles"][0]["name"].ToString(), songname, Recent["score"].ToString(), GetChartDiff(json, Recent["chart_id"].ToString(), true), jo["_related"]["profiles"][0]["_id"].ToString(), "", Recent["critical"].ToString(), "", Recent["near"].ToString(), Recent["error"].ToString(), Recent["max_chain"].ToString(), "", "", GetRating(json, Recent["chart_id"].ToString()));
           // string rpl = $"UserName:{jo["_related"]["profiles"][0]["name"]}\nUserCode:{jo["_related"]["profiles"][0]["_id"]}\nSong:{songname}[{Diff}]\nScore:{Recent["score"].ToString()}({Recent["grade"].ToString()})\nLamp:{Recent["lamp"].ToString()}\nCritical:{Recent["critical"].ToString()}\nNear:{Recent["near"].ToString()}\nError:{Recent["error"].ToString()}\nMaxChain:{Recent["max_chain"].ToString()}";
            //KiraPlugin.sendMessage(g, rpl);
         }
    }
}
