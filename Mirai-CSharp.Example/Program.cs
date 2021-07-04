using Mirai_CSharp.Models;
using System;
using System.Threading.Tasks;
using System.Threading;
using Team123it.QBC.Client;

namespace Mirai_CSharp.Example
{
    public static class Program // 前提: nuget Mirai-CSharp, 版本需要 >= 1.0.1.0
    {
        public static async Task Main()
        {
           
                KiraDX.G.initialize();
                ExamplePlugin plugin = new ExamplePlugin();



                MiraiHttpSessionOptions Calista_options = new MiraiHttpSessionOptions("192.168.0.171", 39253, "MizunaNakooo");
                await using MiraiHttpSession Calista = new MiraiHttpSession();
                Calista.AddPlugin(plugin);
            
                Calista.ConnectAsync(Calista_options, KiraDX.G.BotList.Calista);

            MiraiHttpSessionOptions Nadia_options = new MiraiHttpSessionOptions("192.168.0.171", 39252, "MizunaNakooo");
                await using MiraiHttpSession Nadia = new MiraiHttpSession();
                Nadia.AddPlugin(plugin);
                Nadia.ConnectAsync(Nadia_options, KiraDX.G.BotList.Nadia);


                MiraiHttpSessionOptions Alice_options = new MiraiHttpSessionOptions("192.168.0.171", 39251, "MizunaNakooo");
                await using MiraiHttpSession Alice = new MiraiHttpSession();
                Alice.AddPlugin(plugin);
                Alice.ConnectAsync(Alice_options, KiraDX.G.BotList.Alice);


          
        
            
            Task.Run(async () => {
                Bot QBCBot = new Bot(new BotInfo() //初始化Bot实例
                {
                    Token =KiraDX.G.QBCToken //Bot的Token
                });
                try
                {
                    QBCBot.CorruptedDataReceived += (sender, e) => { 
                    
                    };
                    QBCBot.BotBroadcastReceived += (sender, e) => {  
                    
                    };
                    QBCBot.RepoCalled += (sender, e) => { 
                   
                    };
                    QBCBot.BotMessageReceived += (sender, e) => //绑定接收到其它Bot的消息事件的方法
                    {
                        if (e.Type == Team123it.QBC.Client.MessageType.Bot) //如果是其它Bot主动发送的消息而不是系统消息或其它Bot回复的消息
                        {
                            
                            if (e.Message == "ping")
                            {
                                e.ReplyMessage("Pong!"); //回复消息
                            }
                            
                        }
                    };
                    await QBCBot.ConnectAsync(); //连接QBC服务器
                    await Task.Delay(-1); //无限期等待
                }
                catch (QBCException ex) //如果出现了QBC系统异常
                {
                    /*
                    QBCBot.Disconnect(); //断开连接
                    Console.WriteLine($"Oops! An error occurred:{ex.GetType().ToString()}:{ex.Message}({ex.Code})");
                    Console.WriteLine("Press any key to close program.");
                    Console.ReadKey();
                    await Task.CompletedTask;*/
                }


            });
            Task.Run(async() => {
                string[] a = { "" };
                KiraWeb.Program.Main(a);
            });
            Task.Run(async () => {
                string[] a = { "" };
                KiraWebPage.Program.Main(a);
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
