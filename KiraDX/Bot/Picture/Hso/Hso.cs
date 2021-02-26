using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KiraDX.Bot.Picture.Hso
{
    public static class Hso
    {
      
        public static void GetHso(GroupMsg vs,string type) {

            try
            {
                string hsoPath = Functions.Random_File(Functions.Random_Folders(G.path.Apppath+G.path.Pic+type));
                KiraPlugin.SendGroupPic(vs.s, vs.fromGroup, hsoPath);
            }
            catch (Exception e)
            {

                KiraPlugin.SendGroupMessage(vs.s, vs.fromGroup, e.Message);
            }
            
        }
        public static void GetHso(DisMsg vs, string type)
        {

            try
            {
                string hsoPath = Functions.Random_File(Functions.Random_Folders(G.path.Apppath + G.path.Pic + type));
                
                KiraPlugin.SendPicAsync(hsoPath, vs);
            }
            catch (Exception e)
            {
                KiraPlugin.SendMsg(e.Message, vs);
               
            }

        }

        public static void YouFa(GroupMsg vs,string type)
        {

            try
            {
                KiraPlugin.SendGroupMessage(vs.s, vs.fromGroup, "发就发");
                string hsoPath = Functions.Random_File(Functions.Random_Folders(G.path.Apppath + G.path.Pic+type));
                KiraPlugin.SendGroupPic(vs.s, vs.fromGroup, hsoPath);
            }
            catch (Exception e)
            {

                KiraPlugin.SendGroupMessage(vs.s, vs.fromGroup, e.Message);
            }

        }
        public static void HsoEX(GroupMsg vs,string type)
        {

            try
            {
                if (Users.UsersHso.HsoWhite.Contains(vs.fromAccount.ToString()))
                {
                    int times = Functions.GetNumberInString(vs.msg);
                    for (int i = 0; i < times; i++)
                    {
                        string hsoPath = Functions.Random_File(Functions.Random_Folders(G.path.Apppath + G.path.Pic+type));
                        KiraPlugin.SendGroupPic(vs.s, vs.fromGroup, hsoPath);
                        System.Threading.Thread.Sleep(500);
                    }
                }
                else
                {
                    KiraPlugin.SendGroupMessage(vs.s, vs.fromGroup, "你冇权限啦！");
                }

               

            }
            catch (Exception e)
            {

                KiraPlugin.SendGroupMessage(vs.s, vs.fromGroup, e.Message);
            }

        }

        public static void GetHso(GroupMsg vs,int times,string type)
        {

            try
            {
                if (times>5||times<=0)
                {
                    Console.WriteLine($"[涩图]：用户输入的涩图数量为{times}");
                    KiraPlugin.SendGroupMessage(vs.s, vs.fromGroup, "只能1~5");
                    return;
                }
                for (int i = 0; i < times; i++)
                {
                    string hsoPath = Functions.Random_File(Functions.Random_Folders(G.path.Apppath + G.path.Pic+type));
                    KiraPlugin.SendGroupPic(vs.s, vs.fromGroup, hsoPath);
                    System.Threading.Thread.Sleep(500);
                }
                
            }
            catch (Exception e)
            {

                KiraPlugin.SendGroupMessage(vs.s, vs.fromGroup, e.Message);
            }

        }

        public static void NotSexyEnough(GroupMsg vs)
        {
            KiraPlugin.SendGroupMessage(vs.s, vs.fromGroup, "那你发");
        }
    }
}
