using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ClientWebApp.Models;
using System.Net.Http;
using System.Web.Http;

namespace ClientWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Weather()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373");
                //HTTP GET
                var result = await client.GetAsync("WeatherForecast");

                if (result.IsSuccessStatusCode)
                {
                    return Ok(await result.Content.ReadAsAsync<WeatherForecast[]>());
                }
            }
            return BadRequest("No Response");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
