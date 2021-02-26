using System;
using System.Collections.Generic;
using Mirai_CSharp;
using Mirai_CSharp.Models;
using System.Threading;
using Mirai_CSharp.Extensions;

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


        public static async void SendGroupMessage(MiraiHttpSession session,long GroupID,string msg,bool ToChain=false) {

            try
            {
                if (ToChain)
                {
                    char[] chars=msg.ToCharArray();
                    bool CodeStart=false;
                    string Text = "";
                    string Code = "";
                    MessageBuilder messageBuilder = new MessageBuilder();
                    foreach (var c in chars)
                    {
                        if (c == '[')
                        {
                            if (CodeStart)
                            {
                                /*True,换句话说如果在这里了，应该是像 114514[[114514] 这样的文本
                                 *                                      ▲
                                 * 
                                */

                                //将前面的内容作为普通文本记录
                                Text += Code;
                                Code = "";
                                Code += c.ToString();
                            }
                            else
                            {
                                /*
                                 *False,这个位置应该像是 114514[1919]这样的文本
                                 *                          ▲
                                 *
                                 */
                                CodeStart = true;
                                //开始记录code内容
                                Code += c.ToString();
                            }
                        } 
                        else if ( c==']') 
                        {
                            if (CodeStart)
                            {
                                /*True,换句话说如果在这里了，应该是像 114514[[114514] 这样的文本
                                 *                                             ▲
                                 * 
                                */
                                CodeStart = false;
                                //记录code并转义
                                //记录
                                Code += c.ToString();
                                messageBuilder.AddPlainMessage(Text);
                                
                                if (Code.Contains("mirai:face:"))
                                {
                                    messageBuilder.AddFaceMessage(int.Parse(Functions.TextGainCenter("[mirai:face:", "]", Code)));
                                }
                                else if (Code.Contains("mirai:at:"))
                                {
                                    messageBuilder.AddAtMessage(long.Parse(Functions.TextGainCenter("[mirai:at:", "]", Code)));
                                }
                                else if (Code.Contains("mirai:image:"))
                                {
                                    if (Code.Contains("mirai:image:File:"))
                                    {
                                        ImageMessage pic = await session.UploadPictureAsync(PictureTarget.Group, Functions.TextGainCenter("[mirai:image:File:", "]", Code));
                                        IMessageBase[] chain = new IMessageBase[] { pic };
                                        messageBuilder.Add(chain[0]);
                                    }
                                    else
                                    {
                                        messageBuilder.AddImageMessage(imageId:Functions.TextGainCenter("mirai:image:", "]", Code));
                                    }

                                }
                                else if (Code.Contains("mirai:flashimage:"))
                                {
                                    if (Code.Contains("mirai:flashimage:File:"))
                                    {
                                        
                                        messageBuilder.AddFlashImageMessage(imageId: session.UploadPictureAsync(PictureTarget.Group, Functions.TextGainCenter("[mirai:flashimage:File:", "]", Code)).Result.ImageId);
                                    }
                                    else
                                    {
                                        messageBuilder.AddFlashImageMessage(imageId: Functions.TextGainCenter("mirai:flashimage:", "]", Code));
                                    }
                                   
                                    
                                }
                                else
                                {
                                    messageBuilder.AddPlainMessage(Code);
                                }
                                Code = "";
                                Text = "";

                            }
                            else
                            {
                                /*
                                 *False,这个位置应该像是 114514[1919]]这样的文本
                                 *                                 ▲
                                 */
                                //当作普通文本计入
                                Text += c.ToString();
                            }

                        }
                        else
                        {
                            if (CodeStart)
                            {
                                //code内容
                                Code += c.ToString();
                            }
                            else
                            {
                                //普通文本
                                Text += c.ToString();
                            }

                        }
                        
                    }
                    if (CodeStart)
                    {
                        Text = Code;
                    }
                    messageBuilder.AddPlainMessage(Text);
                    await session.SendGroupMessageAsync(GroupID, messageBuilder);
                }
                else
                {
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
