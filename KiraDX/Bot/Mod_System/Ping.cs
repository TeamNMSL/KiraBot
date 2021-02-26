using System;
using System.Collections.Generic;
using System.Text;

namespace KiraDX.Bot.Mod_System
{
    static public class Ping
    {
        internal static void PingBot(GroupMsg g)
        {
            try
            {

                string ext = "";
                bool IsMainBot = BotFunc.IsMainBot(g);
                if (IsMainBot)
                {
                    ext += "响应bot:True";
                    ext += $"\n数据目录:{G.path.Apppath}";
                    ext += $"\n启动后处理的消息总数:{Users.botMsgNum}";

                    TimeSpan ts = System.DateTime.Now.Subtract(G.Datas_Const.StartTime);
                    
                    ext += $"\n距启动已经过了{ts.Days}天{ts.Hours}小时{ts.Minutes}分{ts.Seconds}秒";
                }
                else
                {
                    ext += "响应bot:False";
                }

                ext += $"\n群数量:{KiraPlugin.GetGroupListAsync(g.s).Result.Count}";


                if (g.s.QQNumber == G.BotList.Miffy)
                {
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, $"[KiraDX]MiffyBot在线\n{ext}");
                }
                else if (g.s.QQNumber == G.BotList.Laffy)
                {
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, $"[KiraDX]LaffyBot在线\n{ext}");
                }
                else if (g.s.QQNumber == G.BotList.Soffy)
                {
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, $"[KiraDX]SoffyBot在线\n{ext}");
                }
                else
                {
                    return;
                }
            }
            catch (Exception e)
            {

                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, $"不会吧最基本的在线状态都能出问题的？？？？？？\n{e.Message}");
            }
        }
    }
}
