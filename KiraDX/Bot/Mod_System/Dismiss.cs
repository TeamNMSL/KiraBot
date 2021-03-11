using Mirai_CSharp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KiraDX.Bot.Mod_System
{
    class Dismiss
    {
        public static void Dismiss_User(GroupMsg g) {
            string ctt = File.ReadAllText($"{G.path.Apppath}ban.kira");
            string inf;
            if (g.msg.ToLower()=="1145141919")
            {
                inf = "非admin自助退群";
            }
            else
            {
                inf = "自助ban";

            }
            ctt += $"{g.fromAccount} {inf}\n";
            File.WriteAllText($"{G.path.Apppath}ban.kira", ctt);

            KiraPlugin.SendGroupMessage(g.s, g.fromGroup, $"已将恁加入黑名单,如果是误操作请联系开发者");
            Users.ban.Reload();
            return;
        }

        public static void Dismiss_Group(GroupMsg g, IGroupMessageEventArgs e)
        {
            try
            {
                if (e.Sender.Permission == GroupPermission.Administrator || e.Sender.Permission == GroupPermission.Owner || g.fromAccount == 1848200159 || g.fromAccount == G.BotList.Laffy || g.fromAccount == G.BotList.Miffy || g.fromAccount == G.BotList.Soffy)
                {
                    string ctt = File.ReadAllText($"{G.path.Apppath}dismiss.TXT");
                    ctt += $"Group:{g.fromGroup} User:{g.fromAccount} {DateTime.Now.ToString("f")} 已处理\n";
                    File.WriteAllText($"{G.path.Apppath}dismiss.TXT", ctt);

                    ctt=File.ReadAllText($"{G.path.Apppath}bangroup.TXT");
                    ctt += $"{g.fromGroup} 自助退群\n";
                    File.WriteAllText($"{G.path.Apppath}bangroup.TXT", ctt);

                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, $"bot将退出当前群聊，操作人：{g.fromAccount},已记录至数据库");
                    System.Threading.Thread.Sleep(5000);
                    g.s.LeaveGroupAsync(g.fromGroup);
                }
                else
                {
                    string ctt = File.ReadAllText($"{G.path.Apppath}dismiss.TXT");
                    ctt += $"Group:{g.fromGroup} User:{g.fromAccount} {DateTime.Now.ToString("f")} 未处理\n";
                    File.WriteAllText($"{G.path.Apppath}dismiss.TXT", ctt);
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, $"{g.fromAccount}试图将bot移出群聊,然而他并没有权限，已记录至数据库并将命令转义为自助拉黑");
                    Users.ban.Reload();
                    g.msg = "1145141919";
                    Dismiss_User(g);
                }


                return;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
