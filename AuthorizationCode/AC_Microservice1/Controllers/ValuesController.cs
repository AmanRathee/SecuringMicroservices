using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AC_Microservice1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NameController : ControllerBase
    {
        [HttpGet]
        [Authorize(policy: "CanRead")]
        public string Name()
        {
            return "aman";
        }

        [HttpPost]
        [Authorize(policy: "CanWrite")]
        public string Post(string name)
        {
            return name;
        }
    }
}
