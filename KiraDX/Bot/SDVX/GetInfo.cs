using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace KiraDX.Bot.SDVX
{
    class GetInfo
    {
        static public string GetBest(string playerID) {

            var score = new HttpClient();
            score.DefaultRequestHeaders.Add("Authorization", G.Sdvx.Authorization);
            string a = Encoding.UTF8.GetString(score.GetByteArrayAsync($"{G.Sdvx.ApiPath}{G.Sdvx.InfoCheck}{playerID}").Result);
            return a;
        }
        static public string GetUser(string playerID) {

            var score = new HttpClient();
            score.DefaultRequestHeaders.Add("Authorization", G.Sdvx.Authorization);
            string a = Encoding.UTF8.GetString(score.GetByteArrayAsync($"{G.Sdvx.ApiPath}{G.Sdvx.usercheck}{playerID}").Result);
            return a;
        }
    }
}
