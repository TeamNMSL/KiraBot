using System;
using System.Collections.Generic;
using Mirai_CSharp;
using Mirai_CSharp.Models;
using System.Threading;
using Mirai_CSharp.Extensions;

namespace KiraDX
{


   public  static partial class KiraPlugin {
        public class FriendVar
        {
            public MiraiHttpSession session;
            public IFriendMessageEventArgs e;

            public FriendVar(MiraiHttpSession session, IFriendMessageEventArgs e)
            {
                this.session = session;
                this.e = e;
            }
        }
        public static async System.Threading.Tasks.Task<List<IFriendInfo>> GetFriendListAsync(MiraiHttpSession s)
        {
            IFriendInfo[] info = await s.GetFriendListAsync();

            List<IFriendInfo> ts = new List<IFriendInfo>();
            foreach (var item in info)
            {
                ts.Add(item);
            }
            return ts;
        }
        public static void Event_FriendMessage(Object oj) {
            try
            {
                FriendVar groupvar = oj as FriendVar;
                MiraiHttpSession session=groupvar.session;
                IFriendMessageEventArgs e=groupvar.e;
                string msg="";
                int i = 0;
                foreach (var item in e.Chain)
                {
                    if (i==0)
                    {
                        i+=1;
                    }
                    else
                    {
                        i += 1;
                        msg += item.ToString();
                    }
                }
                if (G.EventCfg.showMessage)
                {
                    Console.WriteLine($"Bot{session.QQNumber.ToString()}收到好友{e.Sender.Name}({e.Sender.Id.ToString()})的消息:{msg}");
                }
              
               Bot.Bot.FriendMessage(msg,e.Sender.Id, (long)session.QQNumber,session,e);
            }
            catch (Exception)
            {

                
            }



        }
        public static async void SendFriendPic(MiraiHttpSession session, long FriendID, string path)
        {
            try
            {
                ImageMessage msg = await session.UploadPictureAsync(PictureTarget.Friend, path);
                IMessageBase[] chain = new IMessageBase[] { msg };
                await session.SendFriendMessageAsync(FriendID, chain);
            }
            catch (Exception)
            {

                
            }

        }
        
        [Obsolete("已弃用")]
        public static async void SendFriendMessage(MiraiHttpSession session,long FriendID,string msg,bool ToChain=false) {

            try
            {
                if (ToChain)
                {
                    IMessageBuilder builder = KiraDX.Frame.Mirai.GetChainAsync(msg, session).Result;

                    await session.SendFriendMessageAsync(FriendID, builder);
                }
                else
                {
                    IMessageBuilder builder = new MessageBuilder();
                    builder.AddPlainMessage(msg);
                    await session.SendFriendMessageAsync(FriendID, builder);
                }

            }
            catch (Exception)
            {


            }

        }
        

    }
}
