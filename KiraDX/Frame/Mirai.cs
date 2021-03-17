using Mirai_CSharp.Extensions;
using Mirai_CSharp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KiraDX.Frame
{
   public static class Mirai
    {
        public static async System.Threading.Tasks.Task<MessageBuilder> GetChainAsync(string msg,Mirai_CSharp.MiraiHttpSession session) {
            try
            {
                char[] chars = msg.ToCharArray();
                bool CodeStart = false;
                string Text = "";
                string Code = "";
                MessageBuilder final = new MessageBuilder();
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
                            else if (Code.Contains("mirai:at:"))
                            {
                                messageBuilder.AddAtMessage(long.Parse(Functions.TextGainCenter("[mirai:at:", "]", Code)));
                            }
                            else if (Code.Contains("mirai:image:"))
                            {
                                if (Code.Contains(@".\"))
                                {
                                    Code = Code.Replace(@".\", G.path.Apppath);
                                }
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
                final = messageBuilder;
                return final;
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}
