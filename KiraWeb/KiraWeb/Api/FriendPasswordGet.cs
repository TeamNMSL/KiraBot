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
    [Route("api/friend/[controller]")]
    [Produces("application/json")]
    public class FriendPasswordGet : ControllerBase
    {
        [HttpGet]
        public JsonResult Get(string ID)
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            try
            {
                if (!KiraDX.Users.Info.GetUserConfig(long.Parse(ID)).IsPassed)
                {
                    var sb = new SQLiteDB(G.path.DB_KiraPass);
                    sb.addParameters("id", ID);
                    sb.setcmd("INSERT INTO PassList VALUES(@id)");
                    sb.execute();
                    KiraDX.Users.Info.GetUserConfig(long.Parse(ID)).IsPassed = true;
                }
                long q = long.Parse(ID);
                return new JsonResult(new { result = $"成功？" });
            }
            catch (Exception e)
            {
                return new JsonResult(new { result = e.Message });
            }

        }
    }
}
