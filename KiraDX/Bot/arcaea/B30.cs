using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KiraDX.Bot.arcaea
{
    public static partial class arcaea
    {
        public static void b30(GroupMsg g) {
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
                b30info Info = GetArcBest30(friendcode);
                if (Info.e != "null")
                {
                    KiraPlugin.sendMessage(g, Info.e);
                }
                ArcB30(Info,g);

            }
            
            catch (Exception e)
            {
                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, e.Message);
                throw;
            }
        
        
        }
        public static void b30(FriendVars g)
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
                b30info Info = GetArcBest30(friendcode);
                if (Info.e!="null")
                {
                    KiraPlugin.sendMessage(g, Info.e);
                }
                ArcB30(Info, g);

            }

            catch (Exception e)
            {
                KiraPlugin.SendGroupMessage(g.s, g.fromAccount, e.Message);
                return;
            }


        }

    }
}
