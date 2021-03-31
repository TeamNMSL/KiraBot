using System;
using System.Collections.Generic;
using Mirai_CSharp;
using Mirai_CSharp.Models;
using System.Threading;
using Mirai_CSharp.Extensions;
using System.Linq;

namespace KiraDX
{


   public  static partial class KiraPlugin {

        public static async System.Threading.Tasks.Task<List<IGroupInfo>> GetGroupListAsync(MiraiHttpSession s) {
            IGroupInfo[] info=await s.GetGroupListAsync();
            List<IGroupInfo> ts=new List<IGroupInfo>();
            foreach (var item in info)
            {
                ts.Add(item);
            }
            return ts;
        }
        
        
        public class GroupInviteVar
        {
            public MiraiHttpSession session;
            public IBotInvitedJoinGroupEventArgs e;

            public GroupInviteVar(MiraiHttpSession session, IBotInvitedJoinGroupEventArgs e)
            {
                this.session = session;
                this.e = e;
            }
        }
        public class GroupVar
        {
            public MiraiHttpSession session;
            public IGroupMessageEventArgs e;

            public GroupVar(MiraiHttpSession session, IGroupMessageEventArgs e)
            {
                this.session = session;
                this.e = e;
            }
        }
        public static async void SendGroupVoice(MiraiHttpSession session, long GroupID, string path)
        {

            try
            {
                VoiceMessage msg = await session.UploadVoiceAsync(UploadTarget.Group, path);
                IMessageBuilder builder = new MessageBuilder();
                builder.Add(msg);
                await session.SendGroupMessageAsync(GroupID, builder);
                
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.White;

            }

        }
        public static void Event_GroupMessage(Object oj) {
            try
            {
                GroupVar groupvar = oj as GroupVar;
                MiraiHttpSession session=groupvar.session;
                IGroupMessageEventArgs e=groupvar.e;
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
                
               Console.WriteLine($"Bot{session.QQNumber.ToString()}收到群{e.Sender.Group.Name}({e.Sender.Group.Id.ToString()})成员{e.Sender.Name}({e.Sender.Id.ToString()})的消息:{msg}");
               Bot.Bot.GroupMessage(msg, e.Sender.Group.Id,e.Sender.Id, (long)session.QQNumber,session,e);
            }
            catch (Exception)
            {

                
            }



        }
        public class BotKickOutVar{
            public MiraiHttpSession session;
            public IBotKickedOutEventArgs e;

            public BotKickOutVar(MiraiHttpSession session, IBotKickedOutEventArgs e)
            {
                this.session = session;
                this.e = e;
            }
        }
        public static void Event_KickBot(Object oj) {

            try
            {
                BotKickOutVar groupvar = oj as BotKickOutVar;
                MiraiHttpSession session = groupvar.session;
                IBotKickedOutEventArgs e = groupvar.e;
                Bot.Bot.GroupKick( session,e);
            }
            catch (Exception)
            {


            }

        }
        public static void Event_GroupInviteMessage(Object oj)
        {
            try
            {
                GroupInviteVar groupvar = oj as GroupInviteVar;
                MiraiHttpSession session = groupvar.session;
                IBotInvitedJoinGroupEventArgs e = groupvar.e;
                Bot.Bot.GroupInvite(e, session);
            }
            catch (Exception)
            {
               

            }



        }

        [Obsolete("已弃用")]
        public static async void SendGroupMessage(MiraiHttpSession session,long GroupID,string msg,bool ToChain=false) {

            try
            {

                if (ToChain)
                {
                    IMessageBuilder builder = KiraDX.Frame.Mirai.GetChainAsync(msg,session).Result;
                    await session.SendGroupMessageAsync(GroupID, builder);
                }
                else
                {
                    if (G.EventCfg.fool)
                    {
                        msg = msg.rvs();
                    }
                    IMessageBuilder builder = new MessageBuilder();
                    builder.AddPlainMessage(msg);
                    await session.SendGroupMessageAsync(GroupID, builder);
                }
                
            }
            catch (Exception)
            {

                
            }
          
        }
        public static async void SendGroupPic(MiraiHttpSession session, long GroupID, string path)
        {
            try
            {

                ImageMessage msg = await session.UploadPictureAsync(PictureTarget.Group, path);
                IMessageBase[] chain = new IMessageBase[] { msg };
                //await session.SendGroupMessageAsync(GroupID, chain);

                await session.SendGroupMessageAsync(GroupID, chain);

            }
            catch (Exception)
            {

                
            }
        }

    }
}
