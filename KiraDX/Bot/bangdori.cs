using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace KiraDX.Bot
{
    class bangdori
    {
        public static JObject GetCar()
        {
            try
            {
                var r = new HttpClient();
                var R = Encoding.UTF8.GetString(r.GetByteArrayAsync("https://api.bandoristation.com/?function=query_room_number").Result);
                return (JObject)JsonConvert.DeserializeObject(R);
                //return (JObject)JsonConvert.DeserializeObject("{\"status\":\"success\",\"response\":[{\"number\":\"996725\",\"raw_message\":\"996725 q 3\",\"source_info\":{\"name\":\"Tsugu\",\"type\":\"qq\"},\"type\":\"other\",\"time\":1614164079643,\"user_info\":{\"type\":\"qq\",\"user_id\":1930300830,\"username\":\"QQ\\u7528\\u6237184 * ***159\",\"avatar\":\"\"}}]}");
            }
            catch (Exception)
            {

                return null;
            }
        }
        /*{"status":"success","response":[{"number":"996725","raw_message":"996725 q 3","source_info":{"name":"Tsugu","type":"qq"},"type":"other","time":1614164079643,"user_info":{"type":"qq","user_id":1930300830,"username":"QQ\u7528\u6237184****159","avatar":""}}]}
*/
        public static void IsCar(GroupMsg g)
        {
            try
            {

                /// <summary>
                /// 将时间戳转换为日期类型，并格式化
                /// </summary>
                /// <param name="longDateTime"></param>
                /// <returns></returns>
                DateTime LongDateTimeToDateTimeString(string longDateTime)
                {
                    //用来格式化long类型时间的,声明的变量
                    long unixDate;
                    DateTime start;
                    DateTime date;
                    //ENd

                    unixDate = long.Parse(longDateTime);
                    start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    date = start.AddMilliseconds(unixDate).ToLocalTime();

                    return date;

                }
                JObject cars = GetCar();
                if (cars == null)
                {
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "myc");
                    return;
                }
                if (cars["status"].ToString() != "success")
                {
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "出了一些问题，至于是什么问题，我也不知道");
                    return;
                }

                string str = "[Bandori Station]\n";
                int i = 0;
                string s;

                foreach (var item in cars["response"])
                {
                    i += 1;
                    s = "[" + float.Parse(System.DateTime.Now.Subtract(LongDateTimeToDateTimeString(item["time"].ToString())).TotalSeconds.ToString()).ToString("0.00") + "s前]车牌" + item["number"].ToString() + "\n描述:" + item["raw_message"].ToString() + "\n";
                    str += s;
                }
                if (i == 0)
                {
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "myc");
                    return;
                }
                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, str);
                return;
            }
            catch (Exception e)
            {

                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, e.Message);
            }
        }
    }
}
