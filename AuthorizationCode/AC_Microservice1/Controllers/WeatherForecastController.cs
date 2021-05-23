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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;

namespace Microservice1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static string _accessToken;
        private readonly HttpClient client;

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHttpContextAccessor httpContextAccessor;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            this.httpContextAccessor = httpContextAccessor;
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
            var discoveryDocumentResponse =
                            await client.GetDiscoveryDocumentAsync("https://localhost:5001/");
            if (discoveryDocumentResponse.IsError)
            {
                throw new Exception(discoveryDocumentResponse.Error);
            }


            var customParams = new Dictionary<string, string>
            {
                { "subject_token_type", "urn:ietf:params:oauth:token-type:access_token"},
                { "subject_token", await httpContextAccessor.HttpContext.GetTokenAsync("access_token")},
                { "scope", "openid profile Microservice2.FullAccess" }
            };

            var tokenResponse = await client.RequestTokenAsync(new TokenRequest()
            {
                Address = discoveryDocumentResponse.TokenEndpoint,
                GrantType = "urn:ietf:params:oauth:grant-type:token-exchange",
                Parameters = customParams,
                ClientId = "Microservice1ToMicroservice2_downstreamtokenexchangeclient",
                ClientSecret = "Microservice1_Secret"
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
