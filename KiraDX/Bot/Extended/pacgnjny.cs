using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace KiraDX.Bot.Extended
{
    class pacgnjny
    {
        public static void pacgn(GroupMsg g) {
            try
            {
                if (g.msg.ToLower()=="/c pa")
                {
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "你指令有问题，但是我也不知道出了啥问题就算我知道出了啥问题我也不知道该如何解决这个问题因为我也不知道这个功能是干啥的（悲）");
                    return;
                }
                var data = g.msg.Trim().ToLower().Split(new[] { ' ' }, count: 3)[2];
                var task = GetAnswerAsync(data);
                
                task.Wait();
                string r = task.Result;
                if (r== "获取失败,要不再试下?\r\n")
                {
                    r = "哈↑哈↓ 这东西又出bug了。我不知道是不是你指令的问题，反正关于爬模块的东西我啥都不知道，毕竟不是我写的（悲";
                }
                KiraPlugin.SendGroupMessage(g.s, g.fromGroup,r);
                

            }
            catch (Exception e)
            {

                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, e.Message);
            }
        }

        public static async Task<string> GetAnswerAsync(string data)
        {
            return await Task.Run(new Func<string>(() =>
            {
                var proc = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = false,
                        FileName = $"{G.path.Apppath}{G.path.ext}{G.path.pacgn}",
                        Arguments = data
                    }
                };
                proc.Start();
                string r;
                using (var sr = proc.StandardOutput)
                {
                    proc.WaitForExit();
                    r = sr.ReadToEnd();
                    sr.Close();
                }
                return r;
            }));
        }
    }
}
