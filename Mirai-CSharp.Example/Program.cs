using Mirai_CSharp.Models;
using System;
using System.Threading.Tasks;
using System.Threading;
using DSharpPlus;

namespace Mirai_CSharp.Example
{
    public static class Program // 前提: nuget Mirai-CSharp, 版本需要 >= 1.0.1.0
    {
        public static async Task Main()
        {
           
                KiraDX.G.initialize();
                ExamplePlugin plugin = new ExamplePlugin();



                MiraiHttpSessionOptions Miffy_options = new MiraiHttpSessionOptions("192.168.0.171", 39253, "MizunaNakooo");
                await using MiraiHttpSession Miffy = new MiraiHttpSession();
                Miffy.AddPlugin(plugin);
            
                Miffy.ConnectAsync(Miffy_options, KiraDX.G.BotList.Miffy);

            MiraiHttpSessionOptions Laffy_options = new MiraiHttpSessionOptions("192.168.0.171", 39252, "MizunaNakooo");
                await using MiraiHttpSession Laffy = new MiraiHttpSession();
                Laffy.AddPlugin(plugin);
                Laffy.ConnectAsync(Laffy_options, KiraDX.G.BotList.Laffy);


                MiraiHttpSessionOptions Soffy_options = new MiraiHttpSessionOptions("192.168.0.171", 39251, "MizunaNakooo");
                await using MiraiHttpSession Soffy = new MiraiHttpSession();
                Soffy.AddPlugin(plugin);
                Soffy.ConnectAsync(Soffy_options, KiraDX.G.BotList.Soffy);

            Task.Run(async () =>
            {
                var discord = new DiscordClient(new DiscordConfiguration()
                {
                    Token = KiraDX.G.DiscordBotToken,
                    TokenType = TokenType.Bot
                });

                discord.MessageCreated += async (s, e) =>
                {

                    Console.WriteLine("[Discord]收到服務器" + e.Channel.Id.ToString() + "用戶:" + ((long)(e.Message.Author.Id)).ToString() + "的消息:" + e.Message.Content);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(KiraDX.Bot.Discord.DiscordMsg), new KiraDX.Bot.DisMsg(s, e, e.Message.Content, (long)e.Message.Author.Id, (long)e.Channel.Id));

                    if (e.Message.Content.ToLower().StartsWith("ping"))
                        await e.Message.RespondAsync("pong!");
                };


                await discord.ConnectAsync();
                await Task.Delay(-1);
            });
            

            while (true)
            {
                if ( await Console.In.ReadLineAsync() == "exit")
                {
                    Environment.Exit(0);
                }
                else if(await  Console.In.ReadLineAsync() == "reload")
                {
                    KiraDX.Users.Reload();
                }
            }
        }
        
    }
}
