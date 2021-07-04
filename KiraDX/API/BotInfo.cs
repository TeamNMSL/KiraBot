using System;
using System.Collections.Generic;
using System.Text;

namespace KiraDX.API
{
    static public class BotInfo
    {
       public static string[] ValuedBot() {
            
            (bool, Mirai_CSharp.MiraiHttpSession) so = (false, null);
            (bool, Mirai_CSharp.MiraiHttpSession) la = (false, null);
            (bool, Mirai_CSharp.MiraiHttpSession) mi = (false, null);
            List<string> blst = new List<string>();
            if (Users.BotSession.Alice != null)
            {
                blst.Add("Alice");
            }

            if (Users.BotSession.Nadia != null)
            {
                blst.Add("Nadia");

            }

            if (Users.BotSession.Calista != null)
            {
                blst.Add("Calista");

            }
            if (blst.Count==0)
            {
                return new string[] {};
            }
            return blst.ToArray();
        }
    }
}
