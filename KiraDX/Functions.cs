using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace KiraDX
{
    public static class Functions
    {
        /// <summary>
        /// 得到字符串长度，一个汉字长度为2
        /// </summary>
        /// <param name="inputString">参数字符串</param>
        /// <returns></returns>
        public static int StrLength(string inputString)
        {
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            int tempLen = 0;
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                    tempLen += 2;
                else
                    tempLen += 1;
            }
            return tempLen;
        }

        ///<summary>
        ///取出文本中间内容
        ///<summary>
        ///<param name="left">左边文本</param>
        ///<param name="right">右边文本</param>
        ///<param name="text">全文本</param>
        ///<return>完事返回成功文本|没有找到返回空</return>
        public static string TextGainCenter(string left, string right, string text)
        {
            //判断是否为null或者是empty
            if (string.IsNullOrEmpty(left))
                return "";
            if (string.IsNullOrEmpty(right))
                return "";
            if (string.IsNullOrEmpty(text))
                return "";
            //判断是否为null或者是empty

            int Lindex = text.IndexOf(left); //搜索left的位置

            if (Lindex == -1)
            { //判断是否找到left
                return "";
            }

            Lindex = Lindex + left.Length; //取出left右边文本起始位置

            int Rindex = text.IndexOf(right, Lindex);//从left的右边开始寻找right

            if (Rindex == -1)
            {//判断是否找到right
                return "";
            }

            return text.Substring(Lindex, Rindex - Lindex);//返回查找到的文本
        }

        /// <summary>
        /// 取随机数
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GetRandomNumber(int min, int max)
        {

            try
            {
                byte[] buffer = Guid.NewGuid().ToByteArray();
                int iSeed = BitConverter.ToInt32(buffer, 0);
                Random r = new Random(iSeed);
                int rtn = r.Next(min, max + 1);
                return rtn;
            }
            catch (Exception e) { return -1; }
        }
        /// <summary>
        /// 随机文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string Random_File(string path)
        {

            try
            {
                DirectoryInfo folder = new DirectoryInfo(path);
                FileInfo[] file = folder.GetFiles();
                int i = 0;
                foreach (FileInfo info in file)
                {
                    i++;
                }
                int random = GetRandomNumber(0, i - 1);
                return file[random].FullName;
            }
            catch (Exception e) { return "RandomFile.Dll-CsError:" + e.Message; }

        }


        /// <summary>
        /// 随机文件夹
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public static string Random_Folders(string dirPath)
        {
            try
            {
                ArrayList list = new ArrayList();
                List<string> dirs = new List<string>(Directory.GetDirectories(dirPath, "*", System.IO.SearchOption.AllDirectories));
                foreach (var dir in dirs)
                {
                    //Console.WriteLine("{0}", dir);
                    list.Add(dir);

                }
                int random_s = GetRandomNumber(0, dirs.Count - 1);
                return list[random_s].ToString();
            }
            catch (Exception e)
            {
                return "err";
            }
        }
        /// <summary>
        /// 获取KML中的KiraLine数量
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetKiraLines(string str) {
            int nums = 1;
            while (true)
            {
                if (!str.Contains(@"<kiraLine"+nums.ToString()+">")&& !str.Contains(@"</kiraLine" + nums.ToString() + ">"))
                {
                    return nums;
                }
                else
                {
                    nums += 1;
                }
            }
        
        }

        /// <summary>
        /// 提取字符串中的数字，如果没有数字就直接返回0
        /// </summary>
        /// <returns></returns>
        public static int GetNumberInString(string str)
        {

            try
            {
                str = Regex.Replace(str, @"[^\d.\d]", "");
                // 如果是数字，则转换为decimal类型
                if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
                {
                    decimal result = decimal.Parse(str);
                    return (int)result;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {

                return 0;
            }
            
        }
    }

    
}
