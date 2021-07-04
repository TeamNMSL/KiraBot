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
    public class GetUserIcon : ControllerBase
    {
        [HttpGet]
        public JsonResult Get()
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            return new JsonResult(new { miz= $"https://q1.qlogo.cn/g?b=qq&nk=1848200159&s=100&rdm={new Random().Next(0,1000000)}",
            so= $"https://q1.qlogo.cn/g?b=qq&nk=2920507471&s=100&rdm={new Random().Next(0,1000000)}",
            la= $"https://q1.qlogo.cn/g?b=qq&nk=644115868&s=100&rdm={new Random().Next(0,1000000)}"
            });

        }
    }
}
