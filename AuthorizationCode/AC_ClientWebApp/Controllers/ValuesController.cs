using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AC_ClientWebApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public readonly IHttpContextAccessor httpContextAccessor;

        public ValuesController(IHttpContextAccessor contextAccessor)
        {
            this.httpContextAccessor = contextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetName()
        {
            //get token from cookie and send along with the request
            var token = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");

            using (var client = new HttpClient())
            {
                client.SetBearerToken(token);
                client.BaseAddress = new Uri("https://localhost:44373");
                //HTTP GET
                var result = await client.GetAsync("api/Name");

                if (result.IsSuccessStatusCode)
                {
                    return Ok(await result.Content.ReadAsStringAsync());
                }
            }
            return BadRequest("No Response");
        }
    }
}
