using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KiraDX.Bot.Sentences
{
    class GoldSentences
    {
        public static void getPic(GroupMsg vs, string type) {

            if (BotFunc.FuncSwith(vs, "迫害"))
            {
                string Path = Functions.Random_File(G.path.Apppath + G.path.SentencesData + type + "\\");
                KiraPlugin.SendGroupPic(vs.s, vs.fromGroup, Path);
            }
            else
            {
                if (!BotFunc.FuncSwith(vs, "模块提示"))
                {
                    return;
                }
                KiraPlugin.SendGroupMessage(vs.s, vs.fromGroup, "本群迫害模块处于关闭状态，请使用/k mod enable 迫害 打开本群迫害模块后再获取金句");
            }
        }
        public static void GetSent(GroupMsg g,string type) {

            if (BotFunc.FuncSwith(g, "迫害"))
            {
                string txts = File.ReadAllText($"{G.path.Apppath }{G.path.SentencesData}{type}.kira.txt");
                int nums=Functions.GetKiraLines(txts);
                int rand = Functions.GetRandomNumber(1,nums);
                KiraPlugin.SendGroupMessage(g.s,g.fromGroup, Functions.TextGainCenter($"<kiraLine{rand}>", $"</kiraLine{rand}>", txts));

            }
            else
            {
                if (!BotFunc.FuncSwith(g, "模块提示"))
                {
                    return;
                }
                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "本群迫害模块处于关闭状态，请使用/k mod enable 迫害 打开本群迫害模块后再获取金句");
            }

        }
    }
}
