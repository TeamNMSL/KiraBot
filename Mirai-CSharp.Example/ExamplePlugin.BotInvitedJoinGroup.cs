﻿using Mirai_CSharp.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Mirai_CSharp.Example
{
    public partial class ExamplePlugin
    {
        public async Task<bool> BotInvitedJoinGroup(MiraiHttpSession session, IBotInvitedJoinGroupEventArgs e)
        {
            //await session.HandleBotInvitedJoinGroupAsync(e, GroupApplyActions.Allow, "略略略"); // 在这个事件下, 只有 GroupApplyActions.Allow 和
            //                 GroupApplyActions.Deny 有效
            // 把整个事件信息直接作为第一个参数即可, 然后根据自己需要选择一个 GroupApplyActions 枚举去处理请求
            // 你也可以暂存 IGroupApplyEventArgs e, 之后再调用session处理
            ThreadPool.QueueUserWorkItem(new WaitCallback(KiraDX.KiraPlugin.Event_GroupInviteMessage), new KiraDX.KiraPlugin.GroupInviteVar(session, e));
            return true;// 不阻断消息传递。如需阻断请返回true
            
        }
    }
}
