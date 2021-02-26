using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KiraDX.Bot.Others
{
    class Writer
    {
        public static void GetNovel(GroupMsg g) {
            // /k write xxxxxx
            string[] cmds = g.msg.Split(" ", count: 3);
            switch (cmds[2].ToLower())
            {
                case "浴霸":
                case "daisuke":
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "稍等");
                    System.Threading.Thread.Sleep(4000);
                    KiraPlugin.SendGroupPic(g.s, g.fromGroup, $"{G.path.Pictures}daisuke.gif");
                    return;
                default:
                    break;
            }

            string xid = GetInfoAsync(cmds[2]).Result;
            
            if (xid== "我不知道出了啥问题")
            {
                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "我不知道出啥问题了，反正不是我的问题，写不下去了");
                return;
            }
            KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "稍等");
            System.Threading.Thread.Sleep(4000);
            KiraPlugin.SendGroupMessage(g.s, g.fromGroup,"[续写内容]\n"+cmds[2]+ GetLoop(xid).Result);
        }
        static async Task<string> GetLoop(string xid) {

            try
            {
                //System.Threading.Thread.Sleep(30000);
                var client = new RestClient("http://if.caiyunai.com/v1/dream/602ce32169ca3041c6dd006b/novel_dream_loop");
                client.Timeout = 30000;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", $"{{\"nid\": \"602ce7ebf375530266803044\",\"xid\":\"{xid}\",\"ostype\": \"\"}}", ParameterType.RequestBody);
                IRestResponse response;
                int i = 0;
                while (true) {
                    i += 1;
                    response = client.Execute(request);
                    if (((JObject)JsonConvert.DeserializeObject(response.Content))["data"]["count"].ToString()=="0")
                    {
                        break;
                    }
                    if (i==5)
                    {
                        return "思考了一下，我觉得写不出来，走了";
                    }
                    System.Threading.Thread.Sleep(10000);

                }
                //Console.WriteLine(response.Content);
                return ((JObject)JsonConvert.DeserializeObject(response.Content))["data"]["rows"][0]["content"].ToString();
            }
            catch (Exception e)
            {

                return e.ToString();
            }
        
        
        }
        static async Task<string> GetInfoAsync(string start) {
            try
            {
                /* string url = "http://if.caiyunai.com/v1/dream/602ce32169ca3041c6dd006b/novel_ai";
                 var re = new HttpClient();

                 //re.DefaultRequestHeaders.Add("Content-Type", "application/json");
                 var content = new FormUrlEncodedContent(new[]{
                     new KeyValuePair<string, string>("Content-Type","application/json"),
         new KeyValuePair<string, string>("nid", "602ce7ebf375530266803044"),
         new KeyValuePair<string, string>("content", start),
         new KeyValuePair<string, string>("uid", "602ce32169ca3041c6dd006b"),
         new KeyValuePair<string, string>("mid", "60094a2a9661080dc490f75a"),
         new KeyValuePair<string, string>("title", ""),
         new KeyValuePair<string, string>("ostype", "")
             });

                 var r = Encoding.UTF8.GetString(await re.PostAsync(url, content).Result.Content.ReadAsByteArrayAsync());

                 return r;*/
                
                var client = new RestClient("http://if.caiyunai.com/v1/dream/602ce32169ca3041c6dd006b/novel_ai");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", $"{{\"nid\":\"602ce7ebf375530266803044\",\"content\":\"{start}\",\"uid\":\"602ce32169ca3041c6dd006b\",\"mid\":\"60094a2a9661080dc490f75a\",\"title\":\"\",\"ostype\":\"\"}}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                //QConsole.WriteLine(response.Content);
                return ((JObject)JsonConvert.DeserializeObject(response.Content))["data"]["xid"].ToString();
            }
            catch (Exception e)
            {

                return "我不知道出了啥问题";
            }
        }
    }
}
