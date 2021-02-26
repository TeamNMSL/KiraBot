using Mirai_CSharp.Extensions;
using Mirai_CSharp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KiraDX.Bot.Mirai
{
    class Repeater
    {
        public static async void repeat(GroupMsg g, IGroupMessageEventArgs e) {
            try
            {
                IMessageBuilder builder = new MessageBuilder();
                IMessageBase msg;
                if (Functions.GetRandomNumber(1, 100) == 9)
                {
                    foreach (var item in e.Chain)
                    {
                        if (!item.ToString().Contains("[mirai:source:"))
                        {
                            if (item.ToString().Contains("[mirai:quote:"))
                            {
                                return;
                            }
                            msg = item;
                            builder.Add(msg);
                        }
                    }
                    await g.s.SendGroupMessageAsync(e.Sender.Group.Id, builder);
                }
            }
            catch (Exception)
            {

                
            }
        
        }
        public static async void repeat(GroupMsg g, IGroupMessageEventArgs e,bool flag)
        {
            try
            {
                IMessageBuilder builder = new MessageBuilder();
                IMessageBase msg;
                if (Functions.GetRandomNumber(1, 5) == 3)
                {
                    foreach (var item in e.Chain)
                    {
                        if (!item.ToString().Contains("[mirai:source:"))
                        {
                            if (item.ToString().Contains("[mirai:quote:"))
                            {
                                return;
                            }
                            msg = item;
                            builder.Add(msg);
                        }
                    }
                    builder.AddPlainMessage( Functions.GetRandomNumber(1, 100000).ToString() );
                    await g.s.SendGroupMessageAsync(e.Sender.Group.Id, builder);
                }
            }
            catch (Exception)
            {


            }

        }
    }
}
