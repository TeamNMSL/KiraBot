using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KiraDX;

namespace KiraWeb.Api
{
    [ApiController]
    [Route("api/group/[controller]")]
    [Produces("application/json")]
    public class AddGroup : ControllerBase
    {
        [HttpGet]
        public JsonResult Get(string GroupNumber,string MemberCount,string IsAgree,string GroupType)
        {
            var sb = new SQLiteDB(G.path.WhiteGroupList);
            int.TryParse(GroupNumber, out int gn);
            sb.addParameters("mc", gn.ToString());
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            if (GroupType!= "PureRhythmGamesGroup" && GroupType!= "WithRhythmGamePersonalGroup")
            {
                sb.setcmd("INSERT INTO ApplicationList VALUES (@mc)");
                sb.execute();
                return new JsonResult(new { result="申请失败，不符合要求的群"});
            }
            int.TryParse(MemberCount,out int mc);
            if (mc<100)
            {
                sb.setcmd("INSERT INTO ApplicationList VALUES (@mc)");
                sb.execute();
                return new JsonResult(new { result = "申请失败，不符合要求的群" });
            }
            if (IsAgree!="true")
            {
                sb.setcmd("INSERT INTO ApplicationList VALUES (@mc)");
                sb.execute();
                return new JsonResult(new { result = "申请失败，不符合要求的群" });
            }
            
            sb.setcmd($"SELECT * from GroupList where GroupID = @mc");
            if (sb.execute().Rows.Count!=0)
            {
                sb.setcmd("INSERT INTO ApplicationList VALUES (@mc)");
                sb.execute();
                return new JsonResult(new { result = "该群已经位于白名单列表" });
            }
            sb.setcmd($"SELECT * from ApplicationList where GroupID = @mc");
            if (sb.execute().Rows.Count != 0)
            {
                return new JsonResult(new { result = "该群已经申请过，但是好像没有成功" });
            }
            sb.setcmd("INSERT INTO ApplicationList VALUES (@mc)");
            sb.execute();
            sb.setcmd("INSERT INTO GroupList VALUES (@mc)");
            sb.execute();
            return new JsonResult(new { result="申请成功，请联系该群群主和管理员拉Bot入群"});

        }
    }
}
