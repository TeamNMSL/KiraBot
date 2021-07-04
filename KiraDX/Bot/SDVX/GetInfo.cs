using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace KiraDX.Bot.SDVX
{
    class GetInfo
    {
        static public string GetSongid(string songname) {
            string id = "0";
            if (Users.cfgs.SDVXMDB.ContainsKey(songname))
            {
                id = Users.cfgs.SDVXMDB[songname].ToString();
            }
            if (id.Length == 1)
            {
                id = "000" + id;
            }
            else if (id.Length == 2)
            {
                id = "00" + id;
            }
            else if (id.Length == 3)
            {
                id = "0" + id;
            }
            return id;
        }
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
