using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobsityStocksChat.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [Route("values")]
        public async Task<IActionResult> GetValues()
        {
            return Ok(new { val = "Value 1" });
        }

        [Route("values2")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetValues2()
        {
            return Ok(new { val = "Value 2" });
        }
    }
}
