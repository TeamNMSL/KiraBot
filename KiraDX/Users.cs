using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using KiraDX.Bot;
using System.Collections;
using System.Linq;

namespace KiraDX
{
    public class Users
    {
        /*
                public class GroupConfig {

                   public  GroupConfig(string fromGroup, string botid)
                    {
                        GroupInlt(fromGroup,botid);

                    }

                  public  static  void GroupInlt(string fromGroup,string botid) {

                        if (botid==G.BotList.Miffy.ToString())
                        {
                            return;
                        }

                        if (Directory.Exists($"{G.path.Apppath}{G.path.GroupData}") == false)
                        {
                            Directory.CreateDirectory($"{G.path.Apppath}{G.path.GroupData}");
                        }
                        if (!System.IO.File.Exists($"{G.path.Apppath}{G.path.GroupData}{fromGroup}.dx"))
                        {
                            string cfg = $"{botid}\n";
                            File.WriteAllText($"{G.path.Apppath}{G.path.GroupData}{fromGroup}.dx",cfg);
                        }
                        string[] cfgs = File.ReadAllLines($"{G.path.Apppath}{G.path.GroupData}{fromGroup}.dx");
                        try
                        {
                            Mainbot = int.Parse(cfgs[0]).ToString();
                            GroupId = fromGroup.ToString();
                            if (Users.White.WhiteGroup.Contains(GroupId))
                            {
                                Configs.Info.IsWhite = true;
                            }
                            else
                            {
                                Configs.Info.IsWhite = false;
                            }
                            if (cfgs.ToString().Contains("bot"))
                            {
                                Configs.Switches.BotSwitch = false;
                            }
                        }
                        catch (Exception)
                        {

                            return;
                        }

                    }
                    public static string Mainbot="";//响应的bot
                    public  static string GroupId = "";//群号
                    public  class Configs {
                        public  class Info {
                            public static bool IsWhite;//是否在白名单
                        }
                        public  class Switches {
                            public static bool BotSwitch=true;//Bot总开关
                        }

                    }
                }
                public static List<GroupConfig> GroupList=new List<GroupConfig>();
                public static void GroupLoad()
                {
                    //var distinctItems = items.GroupBy(x => x.Id).Select(y => y.First());
                    string dirPath = $"{G.path.Apppath}{G.path.GroupData}";
                    try
                    {
                        List<GroupConfig> temp = new List<GroupConfig>();
                        temp.Clear();
                        //ArrayList list = new ArrayList();
                        List<string> dirs = new List<string>(Directory.GetDirectories(dirPath, "*", System.IO.SearchOption.AllDirectories));
                        foreach (var dir in dirs)
                        {
                            temp.Add(new GroupConfig(dir.Replace(".dx", "").Replace($"{dirPath}", ""), G.BotList.Soffy.ToString()));
                            //Console.WriteLine("{0}", dir);
                            //list.Add(dir.Replace(".dx","").Replace($"{dirPath}",""));
                            /* if (GroupList.All(b => temp.Any(a => a.equal(b))))
                             {
                                 GroupList.Add(new GroupConfig(dir.Replace(".dx", "").Replace($"{dirPath}", ""), G.BotList.Soffy.ToString()));
                             }
                        }

                        return;
                    }
                    catch (Exception e)
                    {
                        return;
                    }
                }
               */

        
        public static class BotInfo {
            public static string[] Groups = { "", "", "" };
        }

        public static class UsersHso {
            public static string HsoWhite = File.ReadAllText($"{G.path.Apppath}hso-white.kira");

            internal static void Reload()
            {
                HsoWhite = File.ReadAllText($"{G.path.Apppath}hso-white.kira");
                Console.WriteLine("涩图权限列表重载完毕");
            }
        }
        public static class White { 
        
        public static string adminlst= File.ReadAllText($"{G.path.Apppath}Admin.kira");
        public static string WhiteUser = File.ReadAllText($"{G.path.Apppath}WhiteUser.kira");
            internal static void Reload()
            {
                adminlst = File.ReadAllText($"{G.path.Apppath}Admin.kira");
                WhiteUser = File.ReadAllText($"{G.path.Apppath}WhiteUser.kira");
                Console.WriteLine("权限列表重载完毕");
            }

        }
        public static class ban {
            public static string banList = File.ReadAllText($"{G.path.Apppath}ban.kira");
            public static string BanSpecialList = File.ReadAllText($"{G.path.Apppath}banSP.kira");
            public static string BanBottleList = File.ReadAllText($"{G.path.Apppath}bottleban.TXT");
            public static string BanBottleWord= File.ReadAllText($"{G.path.Apppath}banWord.TXT");
            public static string BanGroup = File.ReadAllText($"{G.path.Apppath}bangroup.TXT");
            public static string BanFriend_Tmp = "";
            internal static void Reload()
            {
                BanFriend_Tmp = "";
                banList = File.ReadAllText($"{G.path.Apppath}ban.kira");
                BanSpecialList = File.ReadAllText($"{G.path.Apppath}banSP.kira");
                BanBottleList = File.ReadAllText($"{G.path.Apppath}bottleban.TXT");
                BanBottleWord = File.ReadAllText($"{G.path.Apppath}banWord.TXT");
                BanGroup = File.ReadAllText($"{G.path.Apppath}bangroup.TXT");
                Console.WriteLine("封禁列表重载完毕");
            }
        }
        public static class cfgs {
            public static string FunctionList = File.ReadAllText($"{G.path.Apppath}FunctionList.Kira");
            public static string NonStudy= File.ReadAllText($"{G.path.Apppath}NonStudy.Kira");
            public static void reload() {
                
                FunctionList = File.ReadAllText($"{G.path.Apppath}FunctionList.Kira");
                NonStudy = File.ReadAllText($"{G.path.Apppath}NonStudy.Kira");
                Console.WriteLine("配置列表重载完毕");
            }
        }
        public  static void Reload()
        {
            UsersHso.Reload();
            ban.Reload();
            cfgs.reload();
            White.Reload();
            
        }
     public static long botMsgNum=0;
    }
}
