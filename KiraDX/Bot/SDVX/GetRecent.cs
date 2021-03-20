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
        static public string GetChartDiff(string json, string id)
        {
            JObject jo = (JObject)JsonConvert.DeserializeObject(json);
            JObject music = (JObject)jo["_related"];
            foreach (var item in music["charts"])
            {
                if (item["_id"].ToString() == id)
                {
                    return item["difficulty"].ToString()+" " +item["rating"].ToString();
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
            string rpl = $"UserName:{jo["_related"]["profiles"][0]["name"]}\nUserCode:{jo["_related"]["profiles"][0]["_id"]}\nSong:{songname}[{Diff}]\nScore:{Recent["score"].ToString()}({Recent["grade"].ToString()})\nLamp:{Recent["lamp"].ToString()}\nCritical:{Recent["critical"].ToString()}\nNear:{Recent["near"].ToString()}\nError:{Recent["error"].ToString()}\nMaxChain:{Recent["max_chain"].ToString()}";
            KiraPlugin.sendMessage(g, rpl);
         }
    }
}
