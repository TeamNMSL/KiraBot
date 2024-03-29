﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KiraDX
{
    public class ConfigUser
    {
        public long ID;
        public bool IsBanned;
        public string ArcID = "-1";
        public string SdvxCode = "-1";
        public bool IsStudy;
        public bool IsWhite;
        public bool IsAdmin;
        public bool IsPassed;
        public ConfigUser(long Id)
        {
            this.ID = Id;
            if (BotFunc.IsBanned(ID))
            {
                IsBanned = true;
            }
            else
            {
                IsBanned = false;
            }
            if (File.Exists($"{G.path.Apppath }{G.path.ArcUser}{ID}.ini"))
            {
                ArcID = File.ReadAllText($"{G.path.Apppath }{G.path.ArcUser}{ID}.ini");
            }
            else
            {
                ArcID = "-1";
            }
            if (File.Exists($"{G.Sdvx.CodePath}{ID}.ini"))
            {
                SdvxCode = File.ReadAllText($"{G.Sdvx.CodePath}{ID}.ini");
            }
            else
            {
                SdvxCode = "-1";
            }

            if (BotFunc.isWhite(ID))
            {
                IsWhite = true;
            }
            else
            {
                IsWhite = false;
            }
            if ( Users.cfgs.NonStudy.Contains(ID.ToString()))
            {
                IsStudy = false;
            }
            else
            {
                IsStudy = true;
            }
            if (BotFunc.isAdmin(ID))
            {
                IsAdmin = true;
            }
            else
            {
                IsAdmin = false;
            }
            var sb = new SQLiteDB(G.path.DB_KiraPass);
            sb.addParameters("id", ID.ToString());
            sb.setcmd("SELECT * FROM PassList where id=@id");
            if (sb.execute().Rows.Count<=0)
            {
                IsPassed = false;
            }
            else
            {
                IsPassed = true;
            }
        }
    }

    public class ConfigGroup {
        public long ID;
        public string mainbot="none";
        public string switches = "";
        public ConfigGroup(long Id)
        {
            try
            {

                this.ID = Id;
                if (!File.Exists($"{G.path.Apppath}{G.path.MainBotData}{ID}.kira"))
                {
                    mainbot = "none";
                }
                else
                {

                    string Mainbot = File.ReadAllText($"{G.path.Apppath}{G.path.MainBotData}{Id}.kira");
                    //Console.WriteLine(Mainbot);
                    if (Mainbot.Contains("Alice") || Mainbot.Contains("YUI")||Mainbot.Contains("Soffy"))
                    {
                        mainbot = "Alice";
                    }
                    else if (Mainbot.Contains("Nadia") || Mainbot.Contains("AOI")||Mainbot.Contains("Laffy"))
                    {
                        mainbot = "Nadia";
                    }
                    else
                    {
                        mainbot = "none";
                    }
                }
                if (!File.Exists($"{G.path.Apppath}{G.path.SwitchData}{Id}.kira"))
                {
                    switches = "";
                }
                else
                {
                    switches = File.ReadAllText($"{G.path.Apppath}{G.path.SwitchData}{Id}.kira");
                }
            }
            catch (Exception)
            {
                
                
            }
        }
    }
}
