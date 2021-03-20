using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace KiraDX.Bot.SDVX
{
    class UserBind
    {

        private static void Save(long userId,string SdvxID) {
            File.WriteAllText($"{G.Sdvx.CodePath}{userId}.ini", SdvxID);
            Users.Info.GetUserConfig(userId);
            if (Users.Info.UserInfo.ContainsKey(userId))
            {
                Users.Info.UserInfo[userId].SdvxCode = SdvxID;
            }
        }
        public static void  Bind_UserName(GroupMsg g) {
            string[] cmd = (g.msg.Split(" "));
            string json=GetInfo.GetUser(cmd[cmd.Length-1].ToUpper());
            JObject jo = (JObject)JsonConvert.DeserializeObject(json);
           
            
            List<JObject> UserList = new List<JObject>();
            foreach (var item in jo["_items"])
            {
                UserList.Add((JObject)item);
            }
            if (UserList.Count==1)
            {
                Save(g.fromAccount,UserList[0]["_id"].ToString());
                KiraPlugin.sendMessage(g, $"已绑定至用户{UserList[0]["name"].ToString()}({UserList[0]["_id"].ToString()})");
            }
            else if (UserList.Count==0)
            {
                KiraPlugin.sendMessage(g, "未找到该用户，请尝试使用Id而不是用户名绑定");
            }
            else
            {
                KiraPlugin.sendMessage(g, "用户过多，请到A网获取你的Id并使用Id绑定");
            }
            return;
        }

        public static void Bind_UserID(GroupMsg g)
        {
            string[] cmd = (g.msg.Split(" "));
            try
            {
                string json = GetInfo.GetBest(cmd[cmd.Length - 1]);
                JObject jo = (JObject)JsonConvert.DeserializeObject(json);
                Save(g.fromAccount,jo["_related"]["profiles"][0]["_id"].ToString());
                KiraPlugin.sendMessage(g, $"已绑定至用户{jo["_related"]["profiles"][0]["name"].ToString()}({jo["_related"]["profiles"][0]["_id"].ToString()})");
            }
            catch (Exception)
            {
                KiraPlugin.sendMessage(g, "出现了异常，可能是Id错误");
                return;
            }
            
            

            return;
        }
    }
}
