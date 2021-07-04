using System;
using System.Collections.Generic;
using System.Text;
using KiraDX;
namespace KiraDX.Bot
{
    class CmdedVars {
       public string type;
       public  string msg;
       public long fromUser;
       public  long fromGroup = 0;
        public string from = "";
        public CmdedVars(string from,string type, string msg, long fromUser, long fromGroup=0)
        {
            this.type = type;
            this.msg = msg;
            this.fromUser = fromUser;
            this.fromGroup = fromGroup;
            this.from = from;
        }
    }
}


namespace KiraDX.Frame
{
    class OnCommanded
    {
        public static void onCommanded(KiraDX.Bot.GroupMsg g,string type)
            => exe(new KiraDX.Bot.CmdedVars("QQ_Group",type,g.msg,g.fromAccount,g.fromGroup));
        
        public static void onCommanded(KiraDX.Bot.FriendVars g, string type)
            => exe(new Bot.CmdedVars("QQ_Private",type, g.msg, g.fromAccount));

        private static void exe(KiraDX.Bot.CmdedVars e) {
            try
            {
                KiraDX.Bot.Story.EventValue.EventValue_Add(e);
                KiraDX.Bot.Story.EventValue.EventValue_Test(e);
            }
            catch (Exception)
            {

                
            }
        }
    }
}
