using Mirai_CSharp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KiraDX.Bot.Mirai
{
    /// <summary>
    /// 内部全部使用的是mirai的接口而不是自己写的接口，维护注意
    /// </summary>
    class ADMINCOMMANDS
    {
        public static async void sendAll(GroupMsg g, IGroupMessageEventArgs e) {
            if (g.botid==G.BotList.Miffy)
            {
                return;
            }

            IMessageBuilder builder = new MessageBuilder();
            IMessageBase msg;

            foreach (var item in e.Chain)
            {
                if (!item.ToString().Contains("[mirai:source"))
                {
                    msg = item;
                    if (item.ToString().Contains("/k 全局 "))
                    {
                        msg = new PlainMessage(item.ToString().Replace("/k 全局", ""));
                        
                    }
                    builder.Add(msg);
                }
            }
           GroupMsg gs;
            List<IGroupInfo> glst =await KiraPlugin.GetGroupListAsync(g.s);
            foreach (var item in glst)
            {
                gs = new GroupMsg(g.msg, item.Id, g.fromAccount, g.botid, g.s,g.e);

                if (BotFunc.FuncSwith(gs,"广播")&& BotFunc.FuncSwith(gs, "bot"))
                {
                    if (BotFunc.IsMainBot(gs))
                    {
                        g.s.SendGroupMessageAsync(item.Id, builder);
                    }
                }
            }
            KiraPlugin.SendGroupMessage(g.s,g.fromGroup,"广播完毕");
            return;
            
        } 
    }
}
