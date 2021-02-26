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
                
               Console.WriteLine($"Bot{session.QQNumber.ToString()}收到好友{e.Sender.Name}({e.Sender.Id.ToString()})的消息:{msg}");
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
        
        public static async void SendFriendMessage(MiraiHttpSession session,long FriendID,string msg,bool ToChain=false) {

            try
            {
                if (ToChain)
                {
                    char[] chars = msg.ToCharArray();
                    bool CodeStart = false;
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
                        else if (c == ']')
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
                                        messageBuilder.AddImageMessage(imageId: Functions.TextGainCenter("mirai:image:", "]", Code));
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
                    await session.SendFriendMessageAsync(FriendID, messageBuilder);
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
