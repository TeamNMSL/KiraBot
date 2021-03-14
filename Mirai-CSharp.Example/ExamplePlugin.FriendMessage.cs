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
        public Task<bool> TempMessage(MiraiHttpSession session, ITempMessageEventArgs e)
        {
            return System.Threading.Tasks.Task<bool>.Run(()=> {
                session.SendTempMessageAsync(e.Sender.Id,e.Sender.Group.Id,new MessageBuilder().AddPlainMessage("不支持临时消息，请回群用罢"));
                return true;
            });
            //ThreadPool.QueueUserWorkItem(new WaitCallback(KiraDX.KiraPlugin.Event_FriendMessage), new KiraDX.KiraPlugin.FriendVar(session, null));
            // throw new System.NotImplementedException();
        }
        public async Task<bool> FriendMessage(MiraiHttpSession session, IFriendMessageEventArgs e) // 法1: 使用 IMessageBase[]
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(KiraDX.KiraPlugin.Event_FriendMessage), new KiraDX.KiraPlugin.FriendVar(session, e));
            return true;// 不阻断消息传递。如需阻断请返回true
        }

 
    }
}
