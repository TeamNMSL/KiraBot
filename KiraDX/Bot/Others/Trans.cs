using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Tmt.V20180321;
using TencentCloud.Tmt.V20180321.Models;

namespace KiraDX.Bot.Others
{
    class Trans
    {
        public static void Translate(GroupMsg g) {
            // /c trans jp
            try
            {
                string msg = g.msg;
                string[] ms = msg.Split(new[] { ' ' }, 4);
                string tar = ms[2];
                string text = ms[3];
                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "[翻译结果]\n"+GetInfo(text,tar,"auto"));

            }
            catch (Exception e)
            {

                KiraPlugin.SendGroupMessage(g.s,g.fromGroup,e.ToString());
            }
            

        }

        static string GetInfo(string sourcetxt,string target,string SLang ) {
            try
            {
                Credential cred = new Credential
                {
                    SecretId = G.APIs.BaiduApi.Trans.ID,
                    SecretKey = G.APIs.BaiduApi.Trans.Key
                };

                ClientProfile clientProfile = new ClientProfile();
                HttpProfile httpProfile = new HttpProfile();
                httpProfile.Endpoint = ("tmt.tencentcloudapi.com");
                clientProfile.HttpProfile = httpProfile;

                TmtClient client = new TmtClient(cred, "ap-chongqing", clientProfile);
                TextTranslateRequest req = new TextTranslateRequest();
                req.SourceText = sourcetxt;
                req.Source = SLang;
                req.Target = target;
                req.ProjectId = 0;
                TextTranslateResponse resp = client.TextTranslateSync(req);
                return ((JObject)JsonConvert.DeserializeObject(AbstractModel.ToJsonString(resp)))["TargetText"].ToString();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }
    }
}
