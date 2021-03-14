using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace KiraDX.Bot.Others
{
    class Study
    {
        static public void Reply(GroupMsg g, Mirai_CSharp.Models.IGroupMessageEventArgs e) {
            try
            {
                string msg = "[]";
                foreach (var item in e.Chain)
                {
                    if (!item.ToString().Contains("[") && !item.ToString().Contains("]")&&item.ToString()!=" ")
                    {
                        

                        msg = item.ToString();
                            break;
                        
             
                    }
                }
                if (msg == "[]")
                {
                    return;
                }
                
                    msg = msg.TrimStart().TrimEnd();
                
                DataTable Table = DB.execute($"{G.path.Apppath}Study.db", $"SELECT * FROM Data where (Key='{msg}' and (IsPublic='true' or FromGroup='{g.fromGroup}')) ORDER BY RANDOM() limit 1");
                if (Table.Rows.Count==0)
                {
                    return;
                }
                string ans = Table.Rows[0]["Var"].ToString();
                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "[智障回复]"+ans);
               
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }
        }
        static public void Study_Main(GroupMsg g) {
            //INSERT INTO Data VALUES ("key","var","Group","IsPublic")
            try
            {
                if (Users.Info.GetUserConfig(g.fromAccount).IsStudy)
                {
                    return;
                }
                
                string[] dic = {"-","#","//","and","or","select","update","delete","drop","declare","insert","shell","(","）","|","+","'","&",";","；","$","%","\"","\\",">","<","CR","LF","*","/","Mirai_CSharp.Models" };
               
                foreach (var item in dic)
                {
                    if (g.msg.ToLower().Contains(item))
                    {
                        return;
                    }
                }

                if (g.msg.Contains("[") || g.msg.Contains("]"))
                {
                    return;
                }
                string fromGroup = g.fromGroup.ToString();
                bool IsPublic;
                if (BotFunc.FuncSwith(g, "浅度学习"))
                {
                    IsPublic = true;
                }
                else
                {
                    IsPublic = false;
                }
                if (g.msg==""|| g.msg == " ")
                {
                    return;
                }
                string Ques = null;
                string Ans = g.msg.TrimStart().TrimEnd();

                if (!G.Datas_Inst.MessageList.ContainsKey(g.fromGroup.ToString()))
                {
                    Ques = Ans;
                    G.Datas_Inst.MessageList.Add(g.fromGroup.ToString(), Ques);
                    return;
                }
                Console.WriteLine("---Study Start---");
                Ques = G.Datas_Inst.MessageList[g.fromGroup.ToString()];
                G.Datas_Inst.MessageList[g.fromGroup.ToString()] = Ans;
                DataTable Table;
                if (IsPublic)
                {
                    Table = DB.execute($"{G.path.Apppath}Study.db", $"SELECT * FROM Data where (Var='{Ans}' and Key='{Ques}' and IsPublic='true') ORDER BY RANDOM() limit 1");
                }
                else
                {
                    Table = DB.execute($"{G.path.Apppath}Study.db", $"SELECT * FROM Data where (Var='{Ans}' and Key='{Ques}' and IsPublic='false' and FromGroup='{g.fromGroup}') ORDER BY RANDOM() limit 1");
                }
                if (Table.Rows.Count != 0)
                {
                    return;
                }
                //Object lock_ = new object();
                lock (G.Datas_Inst.MessageList)
                {
                    if (IsPublic)
                    {
                        DB.execute($"{G.path.Apppath}Study.db", $"INSERT INTO Data VALUES ('{Ques}','{Ans}','{g.fromGroup}','true','{g.fromAccount}')");
                    }
                    else
                    {
                        DB.execute($"{G.path.Apppath}Study.db", $"INSERT INTO Data VALUES ('{Ques}','{Ans}','{g.fromGroup}','false','{g.fromAccount}')");
                    }
                }
                Console.WriteLine($"[STUDY]From:{g.fromGroup}({IsPublic.ToString()}) {Ques}->{Ans} ");
            }
            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }

        }
    }
}
