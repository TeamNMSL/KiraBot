using System;
using System.Collections.Generic;
using System.Text;

namespace KiraDX.Bot.Story
{
    class Story
    {
        static public void StoryGet(GroupMsg g) {
            try
            {
                if (EventValue.IsLock(g.fromAccount, g.msg.format().Replace("/c story ", "")))
                {
                    KiraPlugin.sendMessage(g, $"[mirai:image:File:{G.path.Stories}{g.msg.format().Replace("/c story ", "")}.png]",true);
                }
                else
                {
                    KiraPlugin.sendMessage(g, "故事不存在或未解锁");
                }

            }
            catch (Exception e)
            {

                KiraPlugin.sendMessage(g, e.Message);
            }
            
        }
    }
}
