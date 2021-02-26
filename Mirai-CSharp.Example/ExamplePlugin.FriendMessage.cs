using Mirai_CSharp.Extensions;
using Mirai_CSharp.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable CA1822 // Mark members as static // 示例方法禁用Information
#pragma warning disable IDE0059 // 不需要赋值 // 禁用+1
namespace Mirai_CSharp.Example
{
    public partial class ExamplePlugin
    {
        public async Task<bool> FriendMessage(MiraiHttpSession session, IFriendMessageEventArgs e) // 法1: 使用 IMessageBase[]
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(KiraDX.KiraPlugin.Event_FriendMessage), new KiraDX.KiraPlugin.FriendVar(session, e));
            return true;// 不阻断消息传递。如需阻断请返回true
        }

 
    }
}
