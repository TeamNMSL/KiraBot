using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace KiraDX.Bot.bottle
{
    class Bottle
    {
        public static void SendBottle(GroupMsg g) {

            try
            {
                if (BotFunc.FuncSwith(g, "漂流瓶"))
                {
                    if (Users.ban.BanBottleList.Contains(g.fromAccount.ToString()))
                    {
                        KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "你在漂流瓶封禁列表内，无法使用漂流瓶功能");
                        return;
                    }
                    if (g.msg.Contains("[mirai:flashimage"))
                    {
                        KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "不要把闪照往漂流瓶丢，谢谢茄子");
                        return;
                    }
                    if ((g.msg.Contains("[mirai:image")&&!BotFunc.isWhite(g)))
                    {
                        KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "不要把图往漂流瓶丢，谢谢茄子");
                        return;
                    }
                    bool HideGroup = true;
                    if (g.msg.StartsWith("/c throw-u "))
                    {
                        HideGroup = false;
                    }
                    g.msg = g.msg.Replace("/c throw-u ", "");
                    g.msg = g.msg.Replace("/c throw ", "");
                    if (g.msg == "")
                    {
                        KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "不要丢空瓶子啊kora");
                        return;
                    }
                    string[] banwords = Users.ban.BanBottleWord.Split(';');
                    foreach (var item in banwords)
                    {
                        if (g.msg.Contains(item))
                        {
                            KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "检测到违禁词");
                            return;
                        }
                    }
                    string Ispublic;
                    if (HideGroup)
                    {
                        Ispublic = "false";
                    }
                    else
                    {
                        Ispublic = "true";
                    }
                    SendBottle(g.msg, g.fromAccount.ToString(), g.fromGroup.ToString(), Ispublic, "true");
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "瓶子漂走力");
                    
                    return;
                    /*string savepath = $"{ G.path.Apppath}{G.path.Bottle}";
                    DirectoryInfo dir = new System.IO.DirectoryInfo(savepath);
                    int fileNum = dir.GetFiles().Length;
                    int fileid = fileNum + 1;
                    while (File.Exists($"{savepath}{fileid}.kirabottle"))
                    {
                        fileid += 1;
                    }
                    string fromgroup;
                    if (HideGroup)
                    {
                        fromgroup = "[已隐藏]";
                    }
                    else
                    {
                        fromgroup = g.fromGroup.ToString();
                    }
                    string content = $"[漂流瓶]\n来自群:{fromgroup}\n来自用户:{g.fromAccount}\n瓶子id:{fileid}\n内容:\n{g.msg}";
                    File.WriteAllText($"{savepath}{fileid}.kirabottle", content);
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "瓶子漂走力");
                    return;*/
                }
                else 
                {
                    if (!BotFunc.FuncSwith(g, "模块提示"))
                    {
                        return;
                    }
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "该群漂流瓶模块暂未打开，请使用/c mod enable 漂流瓶 再使用漂流瓶功能");

                }
            }
            catch (Exception e)
            {
                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, e.Message);
                return;
            }
        }
        public static void PickBottle_E(GroupMsg g) {
            try
            {
                if (BotFunc.FuncSwith(g, "漂流瓶"))
                {
                    if (Users.ban.BanBottleList.Contains(g.fromAccount.ToString()))
                    {
                        KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "你在漂流瓶封禁列表内，无法使用漂流瓶功能");
                        return;
                    }
                    string ctt = "[漂流瓶]\n";
                    DataTable table=DB.execute($"{G.path.Apppath}Bottle.db", $"SELECT * FROM Bottle where (CanBePick='true' and IsElite='true') ORDER BY RANDOM() limit 1");
                    ctt += "來自群:";
                    if (table.Rows[0]["IsPublic"].ToString()=="false")
                    {
                        ctt += "[已隱藏]\n";
                    }
                    else
                    {
                        ctt += table.Rows[0]["fromGroup"].ToString() + "\n";
                    }
                    ctt += "來自用戶:" + table.Rows[0]["fromUser"].ToString() + "\n";
                    ctt+="瓶子id:"+ table.Rows[0]["id"].ToString() + "\n";
                    ctt += "内容:\n" + table.Rows[0]["Content"].ToString();
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, ctt,true);
                    return;
                }
                else
                {
                    if (!BotFunc.FuncSwith(g, "模块提示"))
                    {
                        return;
                    }
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "该群漂流瓶模块暂未打开，请使用/c mod enable 漂流瓶 再使用漂流瓶功能");

                }
            }
            catch (Exception e)
            {

                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, e.Message);
                return;
            }
        }

        public static void PickBottle(GroupMsg g)
        {
            try
            {
                if (BotFunc.FuncSwith(g, "漂流瓶"))
                {
                    if (Users.ban.BanBottleList.Contains(g.fromAccount.ToString()))
                    {
                        KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "你在漂流瓶封禁列表内，无法使用漂流瓶功能");
                        return;
                    }
                    string ctt = "[漂流瓶]\n";
                    DataTable table = DB.execute($"{G.path.Apppath}Bottle.db", $"SELECT * FROM Bottle where CanBePick='true' ORDER BY RANDOM() limit 1");
                    ctt += "來自群:";
                    if (table.Rows[0]["IsPublic"].ToString() == "false")
                    {
                        ctt += "[已隱藏]\n";
                    }
                    else
                    {
                        ctt += table.Rows[0]["fromGroup"].ToString() + "\n";
                    }
                    ctt += "來自用戶:" + table.Rows[0]["fromUser"].ToString() + "\n";
                    ctt += "瓶子id:" + table.Rows[0]["id"].ToString() + "\n";
                    ctt += "内容:\n" + table.Rows[0]["Content"].ToString();
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, ctt, true);
                    return;
                }
                else
                {
                    if (!BotFunc.FuncSwith(g, "模块提示"))
                    {
                        return;
                    }
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "该群漂流瓶模块暂未打开，请使用/c mod enable 漂流瓶 再使用漂流瓶功能");

                }
            }
            catch (Exception e)
            {

                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, e.Message);
                return;
            }
        }


        static void SendBottle(string content,string fromaccount,string fromgroup,string IsPublic,string CanBePick) {
            SQLiteDB Db = new SQLiteDB($"{G.path.Apppath}Bottle.db");
            Db.setcmd("INSERT INTO Bottle VALUES((SELECT count(*) FROM Bottle) + 1, @content, @user, @group, @IsPublic, 'true','')");
            Db.addParameters("content",content);
            Db.addParameters("user",fromaccount);
            Db.addParameters("group",fromgroup);
            Db.addParameters("IsPublic", IsPublic);
            Db.execute();
            //DB.execute($"{G.path.Apppath}Bottle.db", $"INSERT INTO Bottle VALUES((SELECT count(*) FROM Bottle) + 1, '{content}', '{fromaccount}', '{fromgroup}', '{IsPublic}', 'true')");

        }
    }
}
