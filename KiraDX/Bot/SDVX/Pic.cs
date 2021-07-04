using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace KiraDX.Bot.SDVX
{
    class Pic
    {
        public static void SVPic(GroupMsg g, string username, string songname, string score, string diff, string friendcode, string userptt, string pure, string shinepure, string far, string lost, string songptt, string playtime, string type, string RealRating)
        {
            
            string vocalPath = $"{G.path.Apppath}{G.path.Vocal}{G.path.Vocal_arc}";
            bool IsVocal = false;
            if (Functions.GetRandomNumber(1, 100) == 9)
            {
                IsVocal = true;
            }
            int width = 1280;
            int height = 960;

            
            Bitmap Pic = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(Pic))
            {
                graphics.DrawImage(Image.FromStream(new FileStream($"{G.path.Apppath}Images\\SDVXINFO\\Back.png", FileMode.Open, FileAccess.Read)), 0, 0, 1280, 960);
                //DataBase dataBase = new DataBase($"{G.path.Apppath}arcsong.db");
                //dataBase.open();    
                //DataTable Table = dataBase.Run($"SELECT * FROM songs Where sid='{songname}'");
                //DataTable Table = DB.execute($"{G.path.Apppath}arcsong.db", $"SELECT * FROM songs Where sid='{songname}'");
                //dataBase.close();
                string songname_en = songname;
                string diffcode="3";
                string filename;

                filename = $"{G.path.Apppath}Images\\SDVXINFO\\";
                switch (diff)
                {
                    case "NOV":

                        diffcode = "1";
                        filename += "diff_0.png";
                        break;
                    case "ADV":

                        diffcode = "2";
                        filename += "diff_1.png";
                        break;
                    case "EXH":

                        diffcode = "3";
                        filename += "diff_2.png";
                        break;
                    case "MXM":

                        diffcode = "4";
                        filename += "diff_3.png";
                        break;
                    case "INF":

                        diffcode = "5";
                        filename += "diff_4.png";
                        break;
                    

                }
                //RealRating = (float.Parse(RealRating) / 10).ToString();
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

                filename = $"{G.path.Apppath}Images\\SDVXINFO\\";
                rectangle.Y = 280;
                rectangle.X = 780;
                rectangle.Width = 54;
                rectangle.Height = 90;
                foreach (var item in score)
                {
                    filename += $"{item}.png";
                    graphics.DrawImage(Image.FromStream(new FileStream(filename, FileMode.Open, FileAccess.Read)), rectangle);
                    rectangle.X += 54 + 0;
                    filename = $"{G.path.Apppath}Images\\SDVXINFO\\";
                }//分数

                rectangle.X = 121;
                rectangle.Y = 156;
                rectangle.Width = 466;
                rectangle.Height = 466;
                string songid = GetInfo.GetSongid(songname_en);
                filename = $"{G.path.Apppath}sdvxcover\\jk_{songid}_{diffcode}.png";
                if (!File.Exists(filename))
                {
                    int i = 1;
                    while (i<6)
                    {
                        if (File.Exists($"{G.path.Apppath}sdvxcover\\jk_{songid}_{i}.png"))
                        {
                            filename= $"{G.path.Apppath}sdvxcover\\jk_{songid}_{i}.png";
                            break;
                        }
                        i += 1;
                    }
                    if (i>=6)
                    {
                        filename=$"{G.path.Apppath}sdvxcover\\jk_0_1.png";
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
                //userptt = (float.Parse(userptt) / 100).ToString();
                Font PttUser = new Font("Slant", 16);
                //graphics.DrawString(userptt, PttUser, Brushes.Black, rectangle);
                Font Data = new Font("Noto Sans", 13);
                rectangle.X = 1012;
                rectangle.Y = 553;
                rectangle.Width = 214;
                rectangle.Height = 21;
                string puretxt = (int.Parse(pure)).ToString();
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
                //string time = LongDateTimeToDateTimeString(playtime);
                rectangle.Y = 744;
                //graphics.DrawString(time, Data, Brushes.White, rectangle);
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
                //graphics.DrawString(grade, Data, Brushes.White, rectangle);
                rectangle.X = 956;
                rectangle.Y = 631;
                rectangle.Width = 94;
                rectangle.Height = 25;
                //graphics.DrawString((int.Parse(far) + int.Parse(lost) * 2).ToString(), Data, Brushes.White, rectangle);
                rectangle.X = 1125;
                //graphics.DrawString((float.Parse(lost) + float.Parse(far) / 2).ToString(), Data, Brushes.White, rectangle);
                filename = $"{G.path.Apppath}Images\\SDVXINFO\\";
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
                //graphics.DrawImage(Image.FromStream(new FileStream(filename, FileMode.Open, FileAccess.Read)), rectangle);
                rectangle.X = 0;
                rectangle.Y = 0;
                rectangle.Width = 1280;
                rectangle.Height = 960;
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
            Pic.Save($"{G.path.Apppath}{G.path.SVScore}{username}.jpg", myImageCodecInfo, myEncoderParameters);
            KiraPlugin.SendGroupPic(g.s, g.fromGroup, $"{G.path.Apppath}{G.path.SVScore}{username}.jpg");
            
        }
        
    }
}
