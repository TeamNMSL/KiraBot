using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using KiraDX.Bot;
using System.Collections;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Xml;

namespace KiraDX
{
    public class Users
    {
       
        public static Dictionary<string, int> VegetableList = new Dictionary<string, int>() {
            {"029143294",573 },
            {"000000000",616 }
        
        };
        public static class BotSession {
           public  static Mirai_CSharp.MiraiHttpSession Nadia=null;
           public  static Mirai_CSharp.MiraiHttpSession Alice = null;
           public  static Mirai_CSharp.MiraiHttpSession Calista = null;
        }
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
            public static string Channel = File.ReadAllText($"{G.path.Apppath}ChannelList.Kira");
            public static string SystemFunc = File.ReadAllText($"{G.path.Apppath}SystemFunc.kira");
            //public static JObject SDVXMDB=GetMDB();
            public static Dictionary<string,int> SDVXMDB=new Dictionary<string, int>();
            private static int init1 = GetMDB();
            public static void reload() {
                
                FunctionList = File.ReadAllText($"{G.path.Apppath}FunctionList.Kira");
                NonStudy = File.ReadAllText($"{G.path.Apppath}NonStudy.Kira");
                Channel = File.ReadAllText($"{G.path.Apppath}ChannelList.Kira");
                SystemFunc= File.ReadAllText($"{G.path.Apppath}SystemFunc.kira");
                SDVXMDB = new Dictionary<string, int>();
                GetMDB();
                Console.WriteLine("配置列表重载完毕");
            }
            private static int GetMDB() {
                System.Threading.Tasks.Task.Run(async()=> {
                    Console.WriteLine("SoundVoltex MDB loading");
                    string xml2json(string xml)
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(xml);
                        return JsonConvert.SerializeXmlNode(doc);
                    }
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    Encoding shiftjis = Encoding.GetEncoding(932);
                    string t = File.ReadAllText($"{G.path.Apppath }music_db.xml", shiftjis);
                    t = xml2json(t);
                    JObject slst = (JObject)JsonConvert.DeserializeObject(t);
                    foreach (var s in slst["mdb"]["music"])
                    {
                        SDVXMDB.Add(s["info"]["title_name"].ToString(), int.Parse(s["@id"].ToString()));
                    }
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("SoundVoltex MDB load success");
                    Console.ForegroundColor = ConsoleColor.White;

                });
                return 0;
            }
        }
        public  static void Reload()
        {
            UsersHso.Reload();
            ban.Reload();
            cfgs.reload();
            White.Reload();
            Info.Reload();
            
        }
        public static class Info
        {
            public static Dictionary<long, ConfigUser> UserInfo = new Dictionary<long, ConfigUser>() { };
            public static Dictionary<long, ConfigGroup> GroupInfo = new Dictionary<long, ConfigGroup>() { };
            public static ConfigGroup GetGroupConfig(long id)
            {
                try
                {
                    if (!GroupInfo.ContainsKey(id))
                    {
                        GroupInfo.Add(id, new ConfigGroup(id));
                    }
                    return GroupInfo[id];
                }
                catch (Exception)
                {
                    return null;
                }

            }
            public static ConfigUser GetUserConfig(long id)
            {
                try
                {
                    if (!UserInfo.ContainsKey(id))
                    {
                        UserInfo.Add(id, new ConfigUser(id));
                    }
                    return UserInfo[id];
                }
                catch (Exception)
                {
                    return null;
                }

            }
            public static void Reload()
            {
                /*  foreach (var item in UserInfo)
                  {
                      UserInfo[item.Key] = new ConfigUser(item.Key);
                  }
                  foreach (var item in GroupInfo)
                  {
                      GroupInfo[item.Key] = new ConfigGroup(item.Key);
                  }*/
                UserInfo.Clear();
                GroupInfo.Clear();
                Console.WriteLine("用户数据重载完毕");
            }
        }
        public static long botMsgNum=0;
        public static Dictionary<string, int> WhiteGroupList = new Dictionary<string, int>(); 
    }
}
