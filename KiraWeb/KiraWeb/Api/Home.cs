using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KiraWeb.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class Home : ControllerBase
    {
        [HttpGet]
        public JsonResult Get() {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            string[] valuedbot = KiraDX.API.BotInfo.ValuedBot();
            var r = new {
                ValuedBot = valuedbot
            };
            return new JsonResult(r);

        }
    }
}
