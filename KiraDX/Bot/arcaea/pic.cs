using Microsoft.Data.Sqlite;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
namespace KiraDX.Bot.arcaea
{
    public static partial class arcaea
    {
       
        static void ArcPic(GroupMsg g, string username, string songname, string score, string diff, string friendcode, string userptt, string pure, string shinepure, string far, string lost, string songptt, string playtime, string type)
        {
            if (G.EventCfg.SellVegetableMode)
            {
                if (Users.VegetableList.ContainsKey(friendcode))
                {
                    userptt = Users.VegetableList[friendcode].ToString();
                }
            }
            string vocalPath=$"{G.path.Apppath}{G.path.Vocal}{G.path.Vocal_arc}";
            bool IsVocal=false;
            if (Functions.GetRandomNumber(1,100)==9)
            {
                IsVocal = true;
            }
            int width = 1280;
            int height = 960;
            
            if (userptt == "-1")
            {
                userptt = "1919810";
            }
            Bitmap Pic = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(Pic))
            {
                graphics.DrawImage(Image.FromStream(new FileStream($"{G.path.Apppath}Images\\ArcInfo\\Back.png", FileMode.Open, FileAccess.Read)), 0, 0, 1280, 960);
                //DataBase dataBase = new DataBase($"{G.path.Apppath}arcsong.db");
                //dataBase.open();    
                //DataTable Table = dataBase.Run($"SELECT * FROM songs Where sid='{songname}'");
                DataTable Table = DB.execute($"{G.path.Apppath}arcsong.db", $"SELECT * FROM songs Where sid='{songname}'");
                //dataBase.close();
                string songname_en = Table.Rows[0]["name_en"].ToString();
                string RealRating = "0";
                string filename;

                filename = $"{G.path.Apppath}Images\\ArcInfo\\";
                switch (diff)
                {
                    case "0":
                        RealRating = Table.Rows[0]["rating_pst"].ToString();

                        filename += "diff_0.png";
                        break;
                    case "1":
                        RealRating = Table.Rows[0]["rating_prs"].ToString();

                        filename += "diff_1.png";
                        break;
                    case "2":
                        RealRating = Table.Rows[0]["rating_ftr"].ToString();

                        filename += "diff_2.png";
                        break;
                    case "3":
                        RealRating = Table.Rows[0]["rating_byn"].ToString();

                        filename += "diff_3.png";
                        break;

                }
                RealRating = (float.Parse(RealRating) / 10).ToString();
                Rectangle rectangle = new Rectangle();

                rectangle.X = 790;
                rectangle.Y = 158;
                rectangle.Width = 110;
                rectangle.Height = 90;
                graphics.DrawImage(Image.FromStream(new FileStream(filename, FileMode.Open, FileAccess.Read)), rectangle);
                rectangle.X = 810;
                rectangle.Y = 180;
                Font Level = new Font("Slant", 25);
                graphics.DrawString(RealRating, Level, Brushes.White, rectangle);

                filename = $"{G.path.Apppath}Images\\ArcInfo\\";
                rectangle.Y = 280;
                rectangle.X = 780;
                rectangle.Width = 54;
                rectangle.Height = 90;
                foreach (var item in score)
                {
                    filename += $"{item}.png";
                    graphics.DrawImage(Image.FromStream(new FileStream(filename, FileMode.Open, FileAccess.Read)), rectangle);
                    rectangle.X += 54 + 0;
                    filename = $"{G.path.Apppath}Images\\ArcInfo\\";
                }//分数

                rectangle.X = 121;
                rectangle.Y = 156;
                rectangle.Width = 466;
                rectangle.Height = 466;
                if (diff == "3")
                {
                    if (!File.Exists($"{G.path.Apppath}arcsong\\{songname}\\3.jpg"))
                    {
                        if (!File.Exists($"{G.path.Apppath}arcsong\\dl_{songname}\\3.jpg"))
                        {
                            return;
                        }
                        else
                        {
                            filename = $"{G.path.Apppath}arcsong\\dl_{songname}\\3.jpg";
                        }
                    }
                    else
                    {
                        filename = $"{G.path.Apppath}arcsong\\{songname}\\3.jpg";
                    }
                }
                else
                {
                    if (!File.Exists($"{G.path.Apppath}arcsong\\{songname}\\base.jpg"))
                    {
                        if (!File.Exists($"{G.path.Apppath}arcsong\\dl_{songname}\\base.jpg"))
                        {
                            return;
                        }
                        else
                        {
                            filename = $"{G.path.Apppath}arcsong\\dl_{songname}\\base.jpg";
                        }

                    }
                    else
                    {
                        filename = $"{G.path.Apppath}arcsong\\{songname}\\base.jpg";
                    }
                }
                graphics.DrawImage(Image.FromStream(new FileStream(filename, FileMode.Open, FileAccess.Read)), rectangle);
                Font Title = new Font("Noto Sans", 30);
                rectangle.X = 118;
                rectangle.Y = 660;
                rectangle.Width = 490;
                rectangle.Height = 1270 - 407;
                graphics.DrawString(songname_en, Title, Brushes.Black, rectangle);
                rectangle.X = 812;
                rectangle.Y = 407;
                rectangle.Width = 114514;
                rectangle.Height = 59;
                Font UserName = new Font("Slant", 40);
                graphics.DrawString(username, UserName, Brushes.White, rectangle);
                rectangle.X = 947;
                rectangle.Y = 475;
                rectangle.Width = 178;
                rectangle.Height = 41;
                Font UserFriendCode = new Font("Slant", 20);
                graphics.DrawString(friendcode, UserFriendCode, Brushes.White, rectangle);
                rectangle.X = 1124;
                rectangle.Y = 520;
                rectangle.Width = 99;
                rectangle.Height = 24;
                userptt = (float.Parse(userptt) / 100).ToString();
                Font PttUser = new Font("Slant", 16);
                graphics.DrawString(userptt, PttUser, Brushes.Black, rectangle);
                Font Data = new Font("Noto Sans", 13);
                rectangle.X = 1012;
                rectangle.Y = 553;
                rectangle.Width = 214;
                rectangle.Height = 21;
                string puretxt = (int.Parse(pure)).ToString() + $"(+{shinepure })";
                graphics.DrawString(puretxt, Data, Brushes.White, rectangle);
                rectangle.Y = 594;
                graphics.DrawString(far, Data, Brushes.White, rectangle);
                rectangle.Y = 670;
                graphics.DrawString(lost, Data, Brushes.White, rectangle);
                rectangle.Y = 706;
                graphics.DrawString(songptt, Data, Brushes.White, rectangle);

                /// <summary>
                /// 将时间戳转换为日期类型，并格式化
                /// </summary>
                /// <param name="longDateTime"></param>
                /// <returns></returns>
                string LongDateTimeToDateTimeString(string longDateTime)
                {
                    //用来格式化long类型时间的,声明的变量
                    long unixDate;
                    DateTime start;
                    DateTime date;
                    //ENd

                    unixDate = long.Parse(longDateTime);
                    start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    date = start.AddMilliseconds(unixDate).ToLocalTime();

                    return date.ToString("yyyy-MM-dd HH:mm:ss");

                }
                string time = LongDateTimeToDateTimeString(playtime);
                rectangle.Y = 744;
                graphics.DrawString(time, Data, Brushes.White, rectangle);
                rectangle.Y = 779;
                long s = long.Parse(score);
                string grade = "E";
                if (s >= 10000000)
                {
                    grade = "EX+(P)";
                }
                else if (s >= 9900000)
                {
                    grade = "EX+";
                }
                else if (s >= 9800000)
                {
                    grade = "EX";
                }
                else if (s >= 9500000)
                {
                    grade = "AA";
                }
                else if (s >= 9200000)
                {
                    grade = "A";
                }
                else if (s >= 8900000)
                {
                    grade = "B";
                }
                else if (s >= 8600000)
                {
                    grade = "C";
                }
                else
                {
                    grade = "D";
                }
                graphics.DrawString(grade, Data, Brushes.White, rectangle);
                rectangle.X = 956;
                rectangle.Y = 631;
                rectangle.Width = 94;
                rectangle.Height = 25;
                graphics.DrawString((int.Parse(far) + int.Parse(lost) * 2).ToString(), Data, Brushes.White, rectangle);
                rectangle.X = 1125;
                graphics.DrawString((float.Parse(lost) + float.Parse(far) / 2).ToString(), Data, Brushes.White, rectangle);
                filename = $"{G.path.Apppath}Images\\ArcInfo\\";
                switch (type)
                {
                    case "0"://tl
                        filename += "tl.png";
                        rectangle.Width = 167;
                        rectangle.Height = 78;
                        break;
                    case "1"://nc
                    case "4"://ec
                    case "5"://hc
                        filename += "tc.png";
                        rectangle.Width = 237;
                        rectangle.Height = 78;
                        break;
                    case "2"://fr
                        filename += "fr.png";
                        rectangle.Width = 234;
                        rectangle.Height = 78;
                        break;
                    case "3"://pm
                        filename += "pm.png";
                        rectangle.Width = 234;
                        rectangle.Height = 60;
                        break;
                }
                rectangle.X = 996;
                rectangle.Y = 154;
                graphics.DrawImage(Image.FromStream(new FileStream(filename, FileMode.Open, FileAccess.Read)), rectangle);
                rectangle.X = 0;
                rectangle.Y = 0;
                rectangle.Width = 1280;
                rectangle.Height = 960;
                filename = $"{G.path.Apppath}Images\\ArcInfo\\";
                if ((lost == "1" & far == "0") || (lost == "0" & far == "1"))
                {
                    filename += "sex.png";
                    vocalPath += "sex.mp3";
                }
                else if (lost == "1" && far != "0")
                {
                    filename += "1lost.png";
                    vocalPath += "1miss.mp3";
                }
                else if (score == "0")
                {
                    filename += "score0.png";
                }
                else if (shinepure == pure&&far=="0"&&lost=="0")
                {
                    filename += "fpm.png";
                    vocalPath += "fpm.mp3";
                }
                else if (lost == "0" && far == "0" && pure != "0")
                {
                    filename += "pm_.png";
                    vocalPath += "pm.mp3";
                }
                else
                {
                    filename = "none";
                }
                if (filename != "none")
                {
                    graphics.DrawImage(Image.FromStream(new FileStream(filename, FileMode.Open, FileAccess.Read)), rectangle);
                }
            };


            ImageCodecInfo GetEncoderInfo(String mimeType)

            {
                int j;
                ImageCodecInfo[] encoders;
                encoders = ImageCodecInfo.GetImageEncoders();
                for (j = 0; j < encoders.Length; ++j)
                {
                    if (encoders[j].MimeType == mimeType)
                        return encoders[j];
                }
                return null;
            }
            ImageCodecInfo myImageCodecInfo;
            myImageCodecInfo = GetEncoderInfo("image/jpeg");
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;
            myEncoder = System.Drawing.Imaging.Encoder.Quality;
            myEncoderParameters = new EncoderParameters(1);
            myEncoderParameter = new EncoderParameter(myEncoder, 100L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            Pic.Save($"{G.path.Apppath}{G.path.ArcScore}{username}.jpg", myImageCodecInfo, myEncoderParameters);
            //KiraPlugin.SendGroupPic(g.s, g.fromGroup, $"{G.path.Apppath}{G.path.ArcScore}{username}.jpg");
            KiraPlugin.sendMessage(g, $"[mirai:image:File:{G.path.Apppath}{G.path.ArcScore}{username}.jpg]", true);
            if (BotFunc.FuncSwith(g,"语音")&&false)
            {
                if (vocalPath != $"{G.path.Apppath}{G.path.Vocal}{G.path.Vocal_arc}")
                {
                    KiraPlugin.SendGroupVoice(g.s, g.fromGroup, vocalPath);
                    return;
                }
                else if (IsVocal)
                {
                    vocalPath += "tql.mp3";
                    KiraPlugin.SendGroupVoice(g.s, g.fromGroup, vocalPath);
                    return;
                }
            }
        }
        static void ArcPic(FriendVars g, string username, string songname, string score, string diff, string friendcode, string userptt, string pure, string shinepure, string far, string lost, string songptt, string playtime, string type)
        {
            if (G.EventCfg.SellVegetableMode)
            {
                if (Users.VegetableList.ContainsKey(friendcode))
                {
                    userptt = Users.VegetableList[friendcode].ToString();
                }
            }
            string vocalPath = $"{G.path.Apppath}{G.path.Vocal}{G.path.Vocal_arc}";
            bool IsVocal = false;
            if (Functions.GetRandomNumber(1, 100) == 9)
            {
                IsVocal = true;
            }
            int width = 1280;
            int height = 960;

            if (userptt == "-1")
            {
                userptt = "1919810";
            }
            Bitmap Pic = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(Pic))
            {
                graphics.DrawImage(Image.FromStream(new FileStream($"{G.path.Apppath}Images\\ArcInfo\\Back.png", FileMode.Open, FileAccess.Read)), 0, 0, 1280, 960);
                //DataBase dataBase = new DataBase($"{G.path.Apppath}arcsong.db");
                //dataBase.open();    
                //DataTable Table = dataBase.Run($"SELECT * FROM songs Where sid='{songname}'");
                DataTable Table = DB.execute($"{G.path.Apppath}arcsong.db", $"SELECT * FROM songs Where sid='{songname}'");
                //dataBase.close();
                string songname_en = Table.Rows[0]["name_en"].ToString();
                string RealRating = "0";
                string filename;

                filename = $"{G.path.Apppath}Images\\ArcInfo\\";
                switch (diff)
                {
                    case "0":
                        RealRating = Table.Rows[0]["rating_pst"].ToString();

                        filename += "diff_0.png";
                        break;
                    case "1":
                        RealRating = Table.Rows[0]["rating_prs"].ToString();

                        filename += "diff_1.png";
                        break;
                    case "2":
                        RealRating = Table.Rows[0]["rating_ftr"].ToString();

                        filename += "diff_2.png";
                        break;
                    case "3":
                        RealRating = Table.Rows[0]["rating_byn"].ToString();

                        filename += "diff_3.png";
                        break;

                }
                RealRating = (float.Parse(RealRating) / 10).ToString();
                Rectangle rectangle = new Rectangle();

                rectangle.X = 790;
                rectangle.Y = 158;
                rectangle.Width = 110;
                rectangle.Height = 90;
                graphics.DrawImage(Image.FromStream(new FileStream(filename, FileMode.Open, FileAccess.Read)), rectangle);
                rectangle.X = 810;
                rectangle.Y = 180;
                Font Level = new Font("Slant", 25);
                graphics.DrawString(RealRating, Level, Brushes.White, rectangle);

                filename = $"{G.path.Apppath}Images\\ArcInfo\\";
                rectangle.Y = 280;
                rectangle.X = 780;
                rectangle.Width = 54;
                rectangle.Height = 90;
                foreach (var item in score)
                {
                    filename += $"{item}.png";
                    graphics.DrawImage(Image.FromStream(new FileStream(filename, FileMode.Open, FileAccess.Read)), rectangle);
                    rectangle.X += 54 + 0;
                    filename = $"{G.path.Apppath}Images\\ArcInfo\\";
                }//分数

                rectangle.X = 121;
                rectangle.Y = 156;
                rectangle.Width = 466;
                rectangle.Height = 466;
                if (diff == "3")
                {
                    if (!File.Exists($"{G.path.Apppath}arcsong\\{songname}\\3.jpg"))
                    {
                        if (!File.Exists($"{G.path.Apppath}arcsong\\dl_{songname}\\3.jpg"))
                        {
                            return;
                        }
                        else
                        {
                            filename = $"{G.path.Apppath}arcsong\\dl_{songname}\\3.jpg";
                        }
                    }
                    else
                    {
                        filename = $"{G.path.Apppath}arcsong\\{songname}\\3.jpg";
                    }
                }
                else
                {
                    if (!File.Exists($"{G.path.Apppath}arcsong\\{songname}\\base.jpg"))
                    {
                        if (!File.Exists($"{G.path.Apppath}arcsong\\dl_{songname}\\base.jpg"))
                        {
                            return;
                        }
                        else
                        {
                            filename = $"{G.path.Apppath}arcsong\\dl_{songname}\\base.jpg";
                        }

                    }
                    else
                    {
                        filename = $"{G.path.Apppath}arcsong\\{songname}\\base.jpg";
                    }
                }
                graphics.DrawImage(Image.FromStream(new FileStream(filename, FileMode.Open, FileAccess.Read)), rectangle);
                Font Title = new Font("Noto Sans", 30);
                rectangle.X = 118;
                rectangle.Y = 660;
                rectangle.Width = 490;
                rectangle.Height = 1270 - 407;
                graphics.DrawString(songname_en, Title, Brushes.Black, rectangle);
                rectangle.X = 812;
                rectangle.Y = 407;
                rectangle.Width = 114514;
                rectangle.Height = 59;
                Font UserName = new Font("Slant", 40);
                graphics.DrawString(username, UserName, Brushes.White, rectangle);
                rectangle.X = 947;
                rectangle.Y = 475;
                rectangle.Width = 178;
                rectangle.Height = 41;
                Font UserFriendCode = new Font("Slant", 20);
                graphics.DrawString(friendcode, UserFriendCode, Brushes.White, rectangle);
                rectangle.X = 1124;
                rectangle.Y = 520;
                rectangle.Width = 99;
                rectangle.Height = 24;
                userptt = (float.Parse(userptt) / 100).ToString();
                Font PttUser = new Font("Slant", 16);
                graphics.DrawString(userptt, PttUser, Brushes.Black, rectangle);
                Font Data = new Font("Noto Sans", 13);
                rectangle.X = 1012;
                rectangle.Y = 553;
                rectangle.Width = 214;
                rectangle.Height = 21;
                string puretxt = (int.Parse(pure)).ToString() + $"(+{shinepure })";
                graphics.DrawString(puretxt, Data, Brushes.White, rectangle);
                rectangle.Y = 594;
                graphics.DrawString(far, Data, Brushes.White, rectangle);
                rectangle.Y = 670;
                graphics.DrawString(lost, Data, Brushes.White, rectangle);
                rectangle.Y = 706;
                graphics.DrawString(songptt, Data, Brushes.White, rectangle);

                /// <summary>
                /// 将时间戳转换为日期类型，并格式化
                /// </summary>
                /// <param name="longDateTime"></param>
                /// <returns></returns>
                string LongDateTimeToDateTimeString(string longDateTime)
                {
                    //用来格式化long类型时间的,声明的变量
                    long unixDate;
                    DateTime start;
                    DateTime date;
                    //ENd

                    unixDate = long.Parse(longDateTime);
                    start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    date = start.AddMilliseconds(unixDate).ToLocalTime();

                    return date.ToString("yyyy-MM-dd HH:mm:ss");

                }
                string time = LongDateTimeToDateTimeString(playtime);
                rectangle.Y = 744;
                graphics.DrawString(time, Data, Brushes.White, rectangle);
                rectangle.Y = 779;
                long s = long.Parse(score);
                string grade = "E";
                if (s >= 10000000)
                {
                    grade = "EX+(P)";
                }
                else if (s >= 9900000)
                {
                    grade = "EX+";
                }
                else if (s >= 9800000)
                {
                    grade = "EX";
                }
                else if (s >= 9500000)
                {
                    grade = "AA";
                }
                else if (s >= 9200000)
                {
                    grade = "A";
                }
                else if (s >= 8900000)
                {
                    grade = "B";
                }
                else if (s >= 8600000)
                {
                    grade = "C";
                }
                else
                {
                    grade = "D";
                }
                graphics.DrawString(grade, Data, Brushes.White, rectangle);
                rectangle.X = 956;
                rectangle.Y = 631;
                rectangle.Width = 94;
                rectangle.Height = 25;
                graphics.DrawString((int.Parse(far) + int.Parse(lost) * 2).ToString(), Data, Brushes.White, rectangle);
                rectangle.X = 1125;
                graphics.DrawString((float.Parse(lost) + float.Parse(far) / 2).ToString(), Data, Brushes.White, rectangle);
                filename = $"{G.path.Apppath}Images\\ArcInfo\\";
                switch (type)
                {
                    case "0"://tl
                        filename += "tl.png";
                        rectangle.Width = 167;
                        rectangle.Height = 78;
                        break;
                    case "1"://nc
                    case "4"://ec
                    case "5"://hc
                        filename += "tc.png";
                        rectangle.Width = 237;
                        rectangle.Height = 78;
                        break;
                    case "2"://fr
                        filename += "fr.png";
                        rectangle.Width = 234;
                        rectangle.Height = 78;
                        break;
                    case "3"://pm
                        filename += "pm.png";
                        rectangle.Width = 234;
                        rectangle.Height = 60;
                        break;
                }
                rectangle.X = 996;
                rectangle.Y = 154;
                graphics.DrawImage(Image.FromStream(new FileStream(filename, FileMode.Open, FileAccess.Read)), rectangle);
                rectangle.X = 0;
                rectangle.Y = 0;
                rectangle.Width = 1280;
                rectangle.Height = 960;
                filename = $"{G.path.Apppath}Images\\ArcInfo\\";
                if ((lost == "1" & far == "0") || (lost == "0" & far == "1"))
                {
                    filename += "sex.png";
                    vocalPath += "sex.mp3";
                }
                else if (lost == "1" && far != "0")
                {
                    filename += "1lost.png";
                    vocalPath += "1miss.mp3";
                }
                else if (score == "0")
                {
                    filename += "score0.png";
                }
                else if (shinepure == pure && far == "0" && lost == "0")
                {
                    filename += "fpm.png";
                    vocalPath += "fpm.mp3";
                }
                else if (lost == "0" && far == "0" && pure != "0")
                {
                    filename += "pm_.png";
                    vocalPath += "pm.mp3";
                }
                else
                {
                    filename = "none";
                }
                if (filename != "none")
                {
                    graphics.DrawImage(Image.FromStream(new FileStream(filename, FileMode.Open, FileAccess.Read)), rectangle);
                }
            };


            ImageCodecInfo GetEncoderInfo(String mimeType)

            {
                int j;
                ImageCodecInfo[] encoders;
                encoders = ImageCodecInfo.GetImageEncoders();
                for (j = 0; j < encoders.Length; ++j)
                {
                    if (encoders[j].MimeType == mimeType)
                        return encoders[j];
                }
                return null;
            }
            ImageCodecInfo myImageCodecInfo;
            myImageCodecInfo = GetEncoderInfo("image/jpeg");
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;
            myEncoder = System.Drawing.Imaging.Encoder.Quality;
            myEncoderParameters = new EncoderParameters(1);
            myEncoderParameter = new EncoderParameter(myEncoder, 100L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            Pic.Save($"{G.path.Apppath}{G.path.ArcScore}{username}.jpg", myImageCodecInfo, myEncoderParameters);
          //  KiraPlugin.SendFriendPic(g.s, g.fromAccount, $"{G.path.Apppath}{G.path.ArcScore}{username}.jpg");
            KiraPlugin.sendMessage(g, $"[mirai:image:File:{G.path.Apppath}{G.path.ArcScore}{username}.jpg]", true);

        }
        static void ArcB30(b30info info, GroupMsg g)
        {
            try
            {
                
                string GetFilename_Songcover(string songname, string diff)
                {
                    string filename;
                    if (diff == "3")
                    {
                        if (!File.Exists($"{G.path.Apppath}arcsong\\{songname}\\3.jpg"))
                        {
                            if (!File.Exists($"{G.path.Apppath}arcsong\\dl_{songname}\\3.jpg"))
                            {
                                return null;
                            }
                            else
                            {
                                filename = $"{G.path.Apppath}arcsong\\dl_{songname}\\3.jpg";
                            }
                        }
                        else
                        {
                            filename = $"{G.path.Apppath}arcsong\\{songname}\\3.jpg";
                        }
                    }
                    else
                    {
                        if (!File.Exists($"{G.path.Apppath}arcsong\\{songname}\\base.jpg"))
                        {
                            if (!File.Exists($"{G.path.Apppath}arcsong\\dl_{songname}\\base.jpg"))
                            {
                                return null;
                            }
                            else
                            {
                                filename = $"{G.path.Apppath}arcsong\\dl_{songname}\\base.jpg";
                            }

                        }
                        else
                        {
                            filename = $"{G.path.Apppath}arcsong\\{songname}\\base.jpg";
                        }
                    }
                    return filename;
                }

                string getDiffFilename(string diff)
                {
                    string filename = $"{G.path.Apppath}Images\\ArcB30\\";
                    switch (diff)
                    {
                        case "0":
                            filename += "pst.png";
                            break;
                        case "1":
                            filename += "prs.png";
                            break;
                        case "2":
                            filename += "ftr.png";
                            break;
                        case "3":
                            filename += "byd.png";
                            break;
                        default:
                            filename += "ftr.png";
                            break;
                    }
                    return filename;
                }
                string getReal(string diff, string songname, DataTable table)
                {
                    switch (diff)
                    {
                        case "0":
                            return (float.Parse(table.Rows[0]["rating_pst"].ToString()) / 10).ToString();
                        case "1":
                            return (float.Parse(table.Rows[0]["rating_prs"].ToString()) / 10).ToString();
                        case "2":
                            return (float.Parse(table.Rows[0]["rating_ftr"].ToString()) / 10).ToString();
                        case "3":
                            return (float.Parse(table.Rows[0]["rating_byn"].ToString()) / 10).ToString();

                    }
                    return null;

                }

                DataTable table;
                string songname;
                string diff;
                JObject b3 = info.B30;
                JObject ur = info.UserInfo;
                string filename;
                string real;
                string songname_en;
                string username = ur["content"]["name"].ToString();
                string score;
                string ptt;
                string[] pttvs;
                string pttI;
                string pttF;
                //Rectangle cover = new Rectangle(99, 168, 764, 764);

                string rating;
                Bitmap Pic = new Bitmap(2160, 2900);
                using (Graphics graphics = Graphics.FromImage(Pic))
                {
                    graphics.DrawImage(Image.FromStream(new FileStream($"{G.path.Apppath}Images\\ArcB30\\back.png", FileMode.Open, FileAccess.Read)), 0, 0, 2160, 2900);
                    int place = 0;
                    int x = 1;
                    int y = 1;
                    Rectangle O = new Rectangle(52, 523, 410, 381);
                    Rectangle Base = O;
                    Rectangle Lev = new Rectangle();
                    Rectangle cover = new Rectangle();
                    Rectangle Sn = new Rectangle();
                    Rectangle Sc = new Rectangle();
                    Rectangle Pt = new Rectangle();
                    Rectangle PtS = new Rectangle();
                    Rectangle PL = new Rectangle();
                    Font Level = new Font("Slant", 23);
                    Font Songid = new Font("Noto Sans", 18);
                    Font Bptt = new Font("Slant", 35);
                    Font Sptt = new Font("Slant", 18);
                    Font PLF = new Font("Slant", 40);
                    foreach (var item in b3["content"]["best30_list"])
                    {
                        if (x == 6)
                        {
                            x = 1;
                            y += 1;
                        }
                        Base.X = O.X + (x - 1) * O.Width;
                        Base.Y = O.Y + (y - 1) * O.Height;
                        cover.X = Base.X + 99 - 60 - 7;
                        cover.Y = Base.Y + 170 - 111 - 2;
                        cover.Height = 261;
                        cover.Width = 261;
                        Lev.X = cover.X + cover.Width + 19;
                        Lev.Y = cover.Y + cover.Height - 42 - 30;
                        Lev.Width = 100;
                        Lev.Height = 50;
                        Sn.Y = cover.Y + 266;
                        Sn.X = cover.X;
                        Sn.Width = 290;
                        Sn.Height = 48;
                        Sc = Sn;
                        Sc.Y = Sn.Y - 38;

                        PL = cover;
                        PL.X = cover.X + cover.Width + 5;
                        PL.Y = cover.Y + 15;
                        PL.Width = 114514;
                        PL.Height = 152;
                        Pt = PL;
                        Pt.Y = PL.Y + 128 - 65;
                        PtS = Pt;
                        PtS.Y = Pt.Y + 40;
                        place += 1;
                        diff = item["difficulty"].ToString();
                        songname = item["song_id"].ToString();
                        score = item["score"].ToString();
                        ptt = item["rating"].ToString();
                        table = DB.execute($"{G.path.Apppath}arcsong.db", $"SELECT * FROM songs Where sid='{songname}'");
                        songname_en = table.Rows[0]["name_en"].ToString();
                        real = getReal(diff, songname, table);
                        filename = getDiffFilename(diff);
                        graphics.DrawImage(Image.FromStream(new FileStream(filename, FileMode.Open, FileAccess.Read)), Base);
                        graphics.DrawImage(Image.FromStream(new FileStream(GetFilename_Songcover(songname, diff), FileMode.Open, FileAccess.Read)), cover);
                        graphics.DrawImage(Image.FromStream(new FileStream($"{G.path.Apppath}Images\\ArcB30\\shade.png", FileMode.Open, FileAccess.Read)), cover);
                        graphics.DrawString(real, Level, Brushes.White, Lev);
                        graphics.DrawString(songname_en, Songid, Brushes.Black, Sn);
                        graphics.DrawString(score, Level, Brushes.White, Sc);

                        graphics.DrawString(place.ToString(), PLF, Brushes.Black, PL);

                        ptt = (float.Parse(ptt)).ToString("0.0000");
                        pttvs = ptt.Split('.');
                        pttI = pttvs[0];
                        pttF = pttvs[1];
                        graphics.DrawString(pttI + ".", Bptt, Brushes.Black, Pt);
                        graphics.DrawString("   " + pttF, Sptt, Brushes.Black, PtS);
                        x += 1;
                    }
                    string UserPtt = (float.Parse(ur["content"]["rating"].ToString()) / 100).ToString();
                    string friendcode;
                    friendcode = Users.Info.GetUserConfig(g.fromAccount).ArcID;
                    if (G.EventCfg.SellVegetableMode)
                    {
                        if (Users.VegetableList.ContainsKey(friendcode))
                        {
                            UserPtt = (float.Parse(Users.VegetableList[friendcode].ToString()) / 100).ToString();
                        }
                    }
                    if (UserPtt == "-0.01")
                    {
                        UserPtt = "19.19";
                    }
                    string B30Avg = (float.Parse(b3["content"]["best30_avg"].ToString())).ToString("0.0000");
                    string R10Avg = (float.Parse(b3["content"]["recent10_avg"].ToString())).ToString("0.0000");
                    Rectangle un = new Rectangle(1317, 140, 114514, 155);
                    Rectangle up = new Rectangle(1317, 268, 467, 155);
                    Rectangle ufc = new Rectangle(1570, 268, 114514, 155);
                    Rectangle avg = new Rectangle(1317, 360, 114514, 155);

                    Font Favg = new Font("Slant", 35);
                    Font UnF = new Font("Slant", 80);
                    Font UnFCF = new Font("Slant", 50);
                    Font Unpt = new Font("Slant", 50);
                    graphics.DrawString(username, UnF, Brushes.White, un);
                    
                    graphics.DrawString(friendcode, UnFCF, Brushes.White, ufc);
                    graphics.DrawString($"B30 Avg:{B30Avg}  R10 Avg:{R10Avg}", Favg, Brushes.White, avg);
                    graphics.DrawString(UserPtt, Unpt, Brushes.White, up);
                }

                ImageCodecInfo GetEncoderInfo(String mimeType)

                {
                    int j;
                    ImageCodecInfo[] encoders;
                    encoders = ImageCodecInfo.GetImageEncoders();
                    for (j = 0; j < encoders.Length; ++j)
                    {
                        if (encoders[j].MimeType == mimeType)
                            return encoders[j];
                    }
                    return null;
                }
                ImageCodecInfo myImageCodecInfo;
                myImageCodecInfo = GetEncoderInfo("image/jpeg");
                System.Drawing.Imaging.Encoder myEncoder;
                EncoderParameter myEncoderParameter;
                EncoderParameters myEncoderParameters;
                myEncoder = System.Drawing.Imaging.Encoder.Quality;
                myEncoderParameters = new EncoderParameters(1);
                myEncoderParameter = new EncoderParameter(myEncoder, 100L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                Pic.Save($"{G.path.Apppath}{G.path.ArcScore}{username}_b30.jpg", myImageCodecInfo, myEncoderParameters);

               // KiraPlugin.SendGroupPic(g.s, g.fromGroup, $"{G.path.Apppath}{G.path.ArcScore}{username}_b30.jpg");
                KiraPlugin.sendMessage(g, $"[mirai:image:File:{G.path.Apppath}{G.path.ArcScore}{username}_b30.jpg]", true);
                if (BotFunc.FuncSwith(g, "语音") && false)
                {
                    if (Functions.GetRandomNumber(1, 5) == 3)
                    {
                        KiraPlugin.SendGroupVoice(g.s, g.fromGroup, $"{G.path.Apppath}{G.path.Vocal}{G.path.Vocal_arc}先生你好屌.mp3");
                    }
                }

            }
            catch (Exception e)
            {

                return;
            }
        }
    
        static void ArcB30(b30info info, FriendVars g)
        {
            try
            {
                string GetFilename_Songcover(string songname, string diff)
                {
                    string filename;
                    if (diff == "3")
                    {
                        if (!File.Exists($"{G.path.Apppath}arcsong\\{songname}\\3.jpg"))
                        {
                            if (!File.Exists($"{G.path.Apppath}arcsong\\dl_{songname}\\3.jpg"))
                            {
                                return null;
                            }
                            else
                            {
                                filename = $"{G.path.Apppath}arcsong\\dl_{songname}\\3.jpg";
                            }
                        }
                        else
                        {
                            filename = $"{G.path.Apppath}arcsong\\{songname}\\3.jpg";
                        }
                    }
                    else
                    {
                        if (!File.Exists($"{G.path.Apppath}arcsong\\{songname}\\base.jpg"))
                        {
                            if (!File.Exists($"{G.path.Apppath}arcsong\\dl_{songname}\\base.jpg"))
                            {
                                return null;
                            }
                            else
                            {
                                filename = $"{G.path.Apppath}arcsong\\dl_{songname}\\base.jpg";
                            }

                        }
                        else
                        {
                            filename = $"{G.path.Apppath}arcsong\\{songname}\\base.jpg";
                        }
                    }
                    return filename;
                }

                string getDiffFilename(string diff)
                {
                    string filename = $"{G.path.Apppath}Images\\ArcB30\\";
                    switch (diff)
                    {
                        case "0":
                            filename += "pst.png";
                            break;
                        case "1":
                            filename += "prs.png";
                            break;
                        case "2":
                            filename += "ftr.png";
                            break;
                        case "3":
                            filename += "byd.png";
                            break;
                        default:
                            filename += "ftr.png";
                            break;
                    }
                    return filename;
                }
                string getReal(string diff, string songname, DataTable table)
                {
                    switch (diff)
                    {
                        case "0":
                            return (float.Parse(table.Rows[0]["rating_pst"].ToString()) / 10).ToString();
                        case "1":
                            return (float.Parse(table.Rows[0]["rating_prs"].ToString()) / 10).ToString();
                        case "2":
                            return (float.Parse(table.Rows[0]["rating_ftr"].ToString()) / 10).ToString();
                        case "3":
                            return (float.Parse(table.Rows[0]["rating_byn"].ToString()) / 10).ToString();

                    }
                    return null;

                }

                DataTable table;
                string songname;
                string diff;
                JObject b3 = info.B30;
                JObject ur = info.UserInfo;
                string filename;
                string real;
                string songname_en;
                string username = ur["content"]["name"].ToString();
                string score;
                string ptt;
                string[] pttvs;
                string pttI;
                string pttF;
                //Rectangle cover = new Rectangle(99, 168, 764, 764);

                string rating;
                Bitmap Pic = new Bitmap(2160, 2900);
                using (Graphics graphics = Graphics.FromImage(Pic))
                {
                    graphics.DrawImage(Image.FromStream(new FileStream($"{G.path.Apppath}Images\\ArcB30\\back.png", FileMode.Open, FileAccess.Read)), 0, 0, 2160, 2900);
                    int place = 0;
                    int x = 1;
                    int y = 1;
                    Rectangle O = new Rectangle(52, 523, 410, 381);
                    Rectangle Base = O;
                    Rectangle Lev = new Rectangle();
                    Rectangle cover = new Rectangle();
                    Rectangle Sn = new Rectangle();
                    Rectangle Sc = new Rectangle();
                    Rectangle Pt = new Rectangle();
                    Rectangle PtS = new Rectangle();
                    Rectangle PL = new Rectangle();
                    Font Level = new Font("Slant", 23);
                    Font Songid = new Font("Noto Sans", 18);
                    Font Bptt = new Font("Slant", 35);
                    Font Sptt = new Font("Slant", 18);
                    Font PLF = new Font("Slant", 40);
                    foreach (var item in b3["content"]["best30_list"])
                    {
                        if (x == 6)
                        {
                            x = 1;
                            y += 1;
                        }
                        Base.X = O.X + (x - 1) * O.Width;
                        Base.Y = O.Y + (y - 1) * O.Height;
                        cover.X = Base.X + 99 - 60 - 7;
                        cover.Y = Base.Y + 170 - 111 - 2;
                        cover.Height = 261;
                        cover.Width = 261;
                        Lev.X = cover.X + cover.Width + 19;
                        Lev.Y = cover.Y + cover.Height - 42 - 30;
                        Lev.Width = 100;
                        Lev.Height = 50;
                        Sn.Y = cover.Y + 266;
                        Sn.X = cover.X;
                        Sn.Width = 290;
                        Sn.Height = 48;
                        Sc = Sn;
                        Sc.Y = Sn.Y - 38;

                        PL = cover;
                        PL.X = cover.X + cover.Width + 5;
                        PL.Y = cover.Y + 15;
                        PL.Width = 114514;
                        PL.Height = 152;
                        Pt = PL;
                        Pt.Y = PL.Y + 128 - 65;
                        PtS = Pt;
                        PtS.Y = Pt.Y + 40;
                        place += 1;
                        diff = item["difficulty"].ToString();
                        songname = item["song_id"].ToString();
                        score = item["score"].ToString();
                        ptt = item["rating"].ToString();
                        table = DB.execute($"{G.path.Apppath}arcsong.db", $"SELECT * FROM songs Where sid='{songname}'");
                        songname_en = table.Rows[0]["name_en"].ToString();
                        real = getReal(diff, songname, table);
                        filename = getDiffFilename(diff);
                        graphics.DrawImage(Image.FromStream(new FileStream(filename, FileMode.Open, FileAccess.Read)), Base);
                        graphics.DrawImage(Image.FromStream(new FileStream(GetFilename_Songcover(songname, diff), FileMode.Open, FileAccess.Read)), cover);
                        graphics.DrawImage(Image.FromStream(new FileStream($"{G.path.Apppath}Images\\ArcB30\\shade.png", FileMode.Open, FileAccess.Read)), cover);
                        graphics.DrawString(real, Level, Brushes.White, Lev);
                        graphics.DrawString(songname_en, Songid, Brushes.Black, Sn);
                        graphics.DrawString(score, Level, Brushes.White, Sc);

                        graphics.DrawString(place.ToString(), PLF, Brushes.Black, PL);

                        ptt = (float.Parse(ptt)).ToString("0.0000");
                        pttvs = ptt.Split('.');
                        pttI = pttvs[0];
                        pttF = pttvs[1];
                        graphics.DrawString(pttI + ".", Bptt, Brushes.Black, Pt);
                        graphics.DrawString("   " + pttF, Sptt, Brushes.Black, PtS);
                        x += 1;
                    }
                    string UserPtt = (float.Parse(ur["content"]["rating"].ToString()) / 100).ToString();
                    string friendcode;
                    friendcode = Users.Info.GetUserConfig(g.fromAccount).ArcID;
                    if (G.EventCfg.SellVegetableMode)
                    {
                        if (Users.VegetableList.ContainsKey(friendcode))
                        {
                            UserPtt = (float.Parse(Users.VegetableList[friendcode].ToString()) / 100).ToString();
                        }
                    }
                    if (UserPtt == "-0.01")
                    {
                        UserPtt = "19.19";
                    }
                    string B30Avg = (float.Parse(b3["content"]["best30_avg"].ToString())).ToString("0.0000");
                    string R10Avg = (float.Parse(b3["content"]["recent10_avg"].ToString())).ToString("0.0000");
                    Rectangle un = new Rectangle(1317, 140, 114514, 155);
                    Rectangle up = new Rectangle(1317, 268, 467, 155);
                    Rectangle ufc = new Rectangle(1570, 268, 114514, 155);
                    Rectangle avg = new Rectangle(1317, 360, 114514, 155);

                    Font Favg = new Font("Slant", 35);
                    Font UnF = new Font("Slant", 80);
                    Font UnFCF = new Font("Slant", 50);
                    Font Unpt = new Font("Slant", 50);
                    graphics.DrawString(username, UnF, Brushes.White, un);
                    graphics.DrawString(friendcode, UnFCF, Brushes.White, ufc);
                    graphics.DrawString($"B30 Avg:{B30Avg}  R10 Avg:{R10Avg}", Favg, Brushes.White, avg);
                    graphics.DrawString(UserPtt, Unpt, Brushes.White, up);
                }

                ImageCodecInfo GetEncoderInfo(String mimeType)

                {
                    int j;
                    ImageCodecInfo[] encoders;
                    encoders = ImageCodecInfo.GetImageEncoders();
                    for (j = 0; j < encoders.Length; ++j)
                    {
                        if (encoders[j].MimeType == mimeType)
                            return encoders[j];
                    }
                    return null;
                }
                ImageCodecInfo myImageCodecInfo;
                myImageCodecInfo = GetEncoderInfo("image/jpeg");
                System.Drawing.Imaging.Encoder myEncoder;
                EncoderParameter myEncoderParameter;
                EncoderParameters myEncoderParameters;
                myEncoder = System.Drawing.Imaging.Encoder.Quality;
                myEncoderParameters = new EncoderParameters(1);
                myEncoderParameter = new EncoderParameter(myEncoder, 100L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                Pic.Save($"{G.path.Apppath}{G.path.ArcScore}{username}_b30.jpg", myImageCodecInfo, myEncoderParameters);

          //      KiraPlugin.SendFriendPic(g.s, g.fromAccount, $"{G.path.Apppath}{G.path.ArcScore}{username}_b30.jpg");
                KiraPlugin.sendMessage(g, $"[mirai:image:File:{G.path.Apppath}{G.path.ArcScore}{username}_b30.jpg]", true);


            }
            catch (Exception e)
            {

                return;
            }
        }
    }
}
