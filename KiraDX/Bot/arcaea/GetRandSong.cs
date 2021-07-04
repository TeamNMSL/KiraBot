using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

 namespace KiraDX.Bot.arcaea
    {
        public static partial class arcaea
        {
            public static void RandArc(GroupMsg g) {
            try
            {
                bool IsExt = true;
                int rt = 1;
                string t = "0";
                string rpl = "";
                if (g.msg.Contains("取") && g.msg.Contains("次"))
                {
                    t = Functions.TextGainCenter("取", "次", g.msg);
                    rt = int.Parse(t);
                    g.msg = g.msg.Replace("取" + t + "次", "");
                }
                string[] apd = { "倒立收割", "HardClear", "单手收割", "念力游玩" };
                string ext;
                int diff = 0;
                if (g.msg.Contains("+"))
                {
                    diff = 1;
                }
                if (g.msg.Contains("-"))
                {
                    if (g.msg.Contains("-j"))
                    {
                        IsExt = false;
                    }
                    else
                    {
                        KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "查无此歌");
                        return;
                    }

                }
                diff += Functions.GetNumberInString(g.msg) * 2;

                for (int i = 0; i < rt; i++)
                {
                    if (IsExt)
                    {
                        ext = apd[Functions.GetRandomNumber(0, apd.Length - 1)];
                        rpl += RandSong($"{ext},请\n", diff);
                    }
                    else
                    {
                        rpl += RandSong(diff) + "\n";
                    }

                }
                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, rpl);
            }
            catch (Exception e)
            {
                KiraPlugin.sendMessage(g, e.Message);

            }
        }
       
        public static void RandArc(FriendVars g)
        {
            try
            {
                bool IsExt = true;
                int rt = 1;
                string t = "0";
                string rpl = "";
                if (g.msg.Contains("取") && g.msg.Contains("次"))
                {
                    t = Functions.TextGainCenter("取", "次", g.msg);
                    rt = int.Parse(t);
                    g.msg = g.msg.Replace("取" + t + "次", "");
                }
                string[] apd = { "倒立收割", "HardClear", "单手收割", "念力游玩" };
                string ext;
                int diff = 0;
                if (g.msg.Contains("+"))
                {
                    diff = 1;
                }
                if (g.msg.Contains("-"))
                {
                    if (g.msg.Contains("-j"))
                    {
                        IsExt = false;
                    }
                    else
                    {
                        KiraPlugin.SendFriendMessage(g.s, g.fromAccount, "查无此歌");
                        return;
                    }

                }
                diff += Functions.GetNumberInString(g.msg) * 2;

                for (int i = 0; i < rt; i++)
                {
                    if (IsExt)
                    {
                        ext = apd[Functions.GetRandomNumber(0, apd.Length - 1)];
                        rpl += RandSong($"{ext},请\n", diff);
                    }
                    else
                    {
                        rpl += RandSong(diff) + "\n";
                    }

                }
                KiraPlugin.SendFriendMessage(g.s, g.fromAccount, rpl);
            }
            catch (Exception e)
            {
                KiraPlugin.sendMessage(g, e.Message);
                
            }
            
        }
        public static string RandSong(int diff) {
            try
            {
                DataTable Table;
                if (diff == 0)
                {
                    Table = DB.execute($"{G.path.Apppath}arcsong.db", $"SELECT * FROM songs ORDER BY RANDOM() limit 1");
                    return ($"随机结果\n{Table.Rows[0]["name_en"]}");
                }
                Table = DB.execute($"{G.path.Apppath}arcsong.db", $"SELECT * FROM songs where (difficultly_ftr='{diff}' or difficultly_prs='{diff}' or difficultly_pst='{diff}' or difficultly_byn='{diff}') ORDER BY RANDOM() limit 1");
                string rtinfo = $"随机结果\n{Table.Rows[0]["name_en"]}";
                if (Table.Rows[0]["difficultly_ftr"].ToString() == diff.ToString())
                {
                    rtinfo += $"[FTR{float.Parse(Table.Rows[0]["rating_ftr"].ToString()) / 10}]";
                }
                if (Table.Rows[0]["difficultly_prs"].ToString() == diff.ToString())
                {
                    rtinfo += $"[PRS{float.Parse(Table.Rows[0]["rating_prs"].ToString()) / 10}]";
                }
                if (Table.Rows[0]["difficultly_pst"].ToString() == diff.ToString())
                {
                    rtinfo += $"[PST{float.Parse(Table.Rows[0]["rating_pst"].ToString()) / 10}]";
                }
                if (Table.Rows[0]["difficultly_byn"].ToString() == diff.ToString())
                {
                    rtinfo += $"[BYD{float.Parse(Table.Rows[0]["rating_byn"].ToString()) / 10}]";
                }
                return rtinfo;
            }
            catch (Exception e)
            {

                return "查无此歌" + e.Message;
            }
        }//SELECT * FROM songs where (difficultly_ftr='16' or difficultly_prs='16' or difficultly_pst='16' or difficultly_byn='16') ORDER BY RANDOM() limit 1
        public static string RandSong(string ext,int diff)
        {
            try
            {
                DataTable Table;
                if (diff==0)
                {
                    Table = DB.execute($"{G.path.Apppath}arcsong.db", $"SELECT * FROM songs ORDER BY RANDOM() limit 1");
                    return ($"随机结果\n{Table.Rows[0]["name_en"]}\n{ext}");
                }
                Table = DB.execute($"{G.path.Apppath}arcsong.db", $"SELECT * FROM songs where (difficultly_ftr='{diff}' or difficultly_prs='{diff}' or difficultly_pst='{diff}' or difficultly_byn='{diff}') ORDER BY RANDOM() limit 1");
                string rtinfo= $"随机结果\n{Table.Rows[0]["name_en"]}";
                if (Table.Rows[0]["difficultly_ftr"].ToString()==diff.ToString())
                {
                    rtinfo += $"[FTR{float.Parse(Table.Rows[0]["rating_ftr"].ToString())/10}]";
                }
                if (Table.Rows[0]["difficultly_prs"].ToString() == diff.ToString())
                {
                    rtinfo += $"[PRS{float.Parse(Table.Rows[0]["rating_prs"].ToString())/10}]";
                }
                if (Table.Rows[0]["difficultly_pst"].ToString() == diff.ToString())
                {
                    rtinfo += $"[PST{float.Parse(Table.Rows[0]["rating_pst"].ToString())/10}]";
                }
                if (Table.Rows[0]["difficultly_byn"].ToString() == diff.ToString())
                {
                    rtinfo += $"[BYD{float.Parse(Table.Rows[0]["rating_byn"].ToString())/10}]";
                }
                rtinfo += $"\n{ext}";
                return rtinfo;
            }
            catch (Exception e)
            {

                return "查无此歌"+e.Message;
            }
        }//SELECT * FROM songs where difficultly_ftr='19' ORDER BY RANDOM() limit 1
    }
    }
