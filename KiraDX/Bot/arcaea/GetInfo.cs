using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace KiraDX.Bot.arcaea
{
    public static partial class arcaea
    {
        
        public class b30info {
           public JObject UserInfo;
           public JObject B30;

            public b30info(JObject userInfo, JObject b30)
            {
                UserInfo = userInfo;
                B30 = b30;
            }
        }
        public class SongScore {
            public JObject User;
            public JObject Score;
            public SongScore(JObject user, JObject score)
            {
                User = user;
                Score = score;
            }
        }
        public static SongScore GetSongScore(string usercode,string songname,string diff)
        {
            try
            {
                var score = new HttpClient();
                score.DefaultRequestHeaders.Add("User-Agent", G.APIs.ARCAPI.UserAgent);
                var uinfo = new HttpClient();
                uinfo.DefaultRequestHeaders.Add("User-Agent", G.APIs.ARCAPI.UserAgent);
                var Info_score = Encoding.UTF8.GetString(score.GetByteArrayAsync($"{G.APIs.ARCAPI.SongBest(usercode,songname,diff)}").Result);
                var Info_user = Encoding.UTF8.GetString(uinfo.GetByteArrayAsync($"{G.APIs.ARCAPI.site}{G.APIs.ARCAPI.userInfo}{usercode}").Result);
                JObject json_score = (JObject)JsonConvert.DeserializeObject(Info_score);
                JObject json_user = (JObject)JsonConvert.DeserializeObject(Info_user);
                return new SongScore(json_user, json_score);
            }
            catch (Exception)
            {

                return null;
            }
        }
        public static JObject GetSongInfo(string songname) {
            try
            {
                var recent = new HttpClient();
                recent.DefaultRequestHeaders.Add("User-Agent", G.APIs.ARCAPI.UserAgent);
                var R = Encoding.UTF8.GetString(recent.GetByteArrayAsync($"{G.APIs.ARCAPI.site}{G.APIs.ARCAPI.songInfo}{songname}").Result);
                return (JObject)JsonConvert.DeserializeObject(R);
            }
            catch (Exception)
            {

                return null;
            }
            
        }
        public static JObject GetRecent(string usercode) {
            try
            {
                var recent = new HttpClient();
                recent.DefaultRequestHeaders.Add("User-Agent", G.APIs.ARCAPI.UserAgent);
                var R = Encoding.UTF8.GetString(recent.GetByteArrayAsync($"{G.APIs.ARCAPI.site}{G.APIs.ARCAPI.Recently}{usercode}").Result);
                return (JObject)JsonConvert.DeserializeObject(R);
            }
            catch (Exception)
            {

                return null;
            }
        }


        public static b30info GetArcBest30(string usercode) {
            try
            {
                var b30 = new HttpClient();
                b30.DefaultRequestHeaders.Add("User-Agent", G.APIs.ARCAPI.UserAgent);
                var uinfo = new HttpClient();
                uinfo.DefaultRequestHeaders.Add("User-Agent", G.APIs.ARCAPI.UserAgent);
                var Info_b30 = Encoding.UTF8.GetString(b30.GetByteArrayAsync($"{G.APIs.ARCAPI.site}{G.APIs.ARCAPI.b30}{usercode}").Result);
                var Info_user = Encoding.UTF8.GetString(uinfo.GetByteArrayAsync($"{G.APIs.ARCAPI.site}{G.APIs.ARCAPI.userInfo}{usercode}").Result);
                JObject json_b30 = (JObject)JsonConvert.DeserializeObject(Info_b30);
                JObject json_user= (JObject)JsonConvert.DeserializeObject(Info_user);


                return new b30info(json_user,json_b30);
            }
            catch (Exception)
            {
                return null;
            }
        }  
    }
}

