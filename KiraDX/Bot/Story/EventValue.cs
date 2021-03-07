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
    }

}
