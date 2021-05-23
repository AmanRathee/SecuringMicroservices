using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http.Formatting;
using IdentityModel.Client;

namespace Microservice1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static string _accessToken;
        private readonly HttpClient client;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            client = new HttpClient();
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            using (var client = new HttpClient())
            {

                client.SetBearerToken(await GetToken());


                client.BaseAddress = new Uri("https://localhost:44359");
                //HTTP GET
                var result = await client.GetAsync("WeatherForecast");

                if (result.IsSuccessStatusCode)
                {
                    return await result.Content.ReadAsAsync<WeatherForecast[]>();
                }
            }

            return null;
        }



        
        private async Task<string> GetToken()
        {
            if (!string.IsNullOrWhiteSpace(_accessToken))
            {
                return _accessToken;
            }


            //get the identity server configuration document
            var identityServerConfigurationDocument =
                            await client.GetDiscoveryDocumentAsync("https://localhost:5001/");
            if (identityServerConfigurationDocument.IsError)
            {
                throw new Exception(identityServerConfigurationDocument.Error);
            }



            //using that doc, find the token endpoint. And provide the client credentials

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest()
                {
                    Address = identityServerConfigurationDocument.TokenEndpoint,
                    ClientId = "Microservice1_ClientID",
                    ClientSecret = "Microservice1_Secret",
                    Scope = "Microservice2.Read"
                });


            if (tokenResponse.IsError)
            {
                throw new Exception(tokenResponse.Error);
            }

            _accessToken = tokenResponse.AccessToken;
            return _accessToken;
        }
    }
}
