using Mirai_CSharp;
using Mirai_CSharp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace KiraDX.Bot
{
    
    static public partial class Bot 
    {
        public static void GroupKick(MiraiHttpSession session, IBotKickedOutEventArgs e) {
           string ctt = File.ReadAllText($"{G.path.Apppath}bangroup.TXT");
            ctt += $"{e.Group.Id} 飞机票\n";
            File.WriteAllText($"{G.path.Apppath}bangroup.TXT", ctt);
        }
       public  static void GroupInvite(IBotInvitedJoinGroupEventArgs e, Mirai_CSharp.MiraiHttpSession session ) {
            if (Users.White.WhiteUser.Contains(e.FromQQ.ToString()))
            {
                session.HandleBotInvitedJoinGroupAsync(e, GroupApplyActions.Allow);
            }
            if (Users.ban.BanGroup.Contains(e.FromGroup.ToString()))
            {
                session.HandleBotInvitedJoinGroupAsync(e, GroupApplyActions.Deny);
                KiraPlugin.SendFriendMessage(session,e.FromQQ,"该群在黑名单内，若执意拉群请联系bot管理员");
                return;
            }
            if (Users.ban.banList.Contains(e.FromQQ.ToString()))
            {
                session.HandleBotInvitedJoinGroupAsync(e, GroupApplyActions.Deny);
                KiraPlugin.SendFriendMessage(session, e.FromQQ, "您在黑名单内，别拉了");
                return;
            }
        }
    }
}
