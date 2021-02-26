using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace KiraDX
{
    public static partial class KiraPlugin
    {
        public static async System.Threading.Tasks.Task<object> SendPicAsync(string filename, KiraDX.Bot.DisMsg g)
        {
            return await new DiscordMessageBuilder().WithFile(filename).SendAsync(g.e.Channel);
        }
        public static async System.Threading.Tasks.Task<object> SendMsg(string msg, KiraDX.Bot.DisMsg g)
        {
            return await g.e.Message.RespondAsync(msg);
        }
    }
}
