using KiraDX;
using System;
using System.Data;

namespace KiraDX.Bot.Story
{
    class EventValue {
        public static void EventValue_Add(CmdedVars e) {
            try
            {
                DB.execute($"{G.path.Apppath}commandUse.db", $"INSERT into record VALUES('{e.fromUser}','{e.fromGroup}','{e.type}','{e.from}','{(Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds}')");
            }
            catch (Exception)
            {

                
            }
            
        }

        public static int EventValue_Get(long fromUser) {
            try
            {
                DataTable dt = DB.execute($"{G.path.Apppath}commandUse.db", $"SELECT * from record where user='{fromUser}'");
                int rowCount = dt.Rows.Count;
                return rowCount;
            }
            catch (Exception)
            {
                return -1;
            }
            
        }
        public static void StoryLock(long user,string story) {
            DataTable dt = DB.execute($"{G.path.Apppath}StoryLock.db", $"SELECT * from record where ( User='{user}' and StoryName='{story}' )");
            int rc = dt.Rows.Count;
            if (rc<1)
            {
                DB.execute($"{G.path.Apppath}StoryLock.db", $"INSERT into record VALUES('{user}','{story}')");
            }
        }

        public static bool IsLock(long user, string story)
        {
            DataTable dt = DB.execute($"{G.path.Apppath}StoryLock.db", $"SELECT * from record where ( User='{user}' and StoryName='{story}' )");
            int rc = dt.Rows.Count;
            if (rc < 1)
            {
                return false;
            }
            return true;
        }


        public static void EventValue_Test(CmdedVars e)
        {
            int v = EventValue_Get(e.fromUser);
            //throw new NotImplementedException();
            if (v>10)
            {
                StoryLock(e.fromUser,"前言");
            }
        }
    }

}
