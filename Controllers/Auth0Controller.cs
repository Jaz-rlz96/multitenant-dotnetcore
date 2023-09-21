using Microsoft.AspNetCore.Mvc;
using SampleMvcApp.ViewModels;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace SampleMvcApp.Controllers
{
    public class Auth0Controller : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public Auth0Controller(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> GetAuth0Token()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                string apiUrl = "https://dev-ea5oepbc2k553fws.us.auth0.com/oauth/token";

                var requestData = new
                {
                    client_id = "q7qYDTTNkwLBwihPWEEI7QciZe74Pvhw",
                    client_secret = "7Sk3VQGQhz8S8_iMXZky-xs3F6PPylFXqNPnRnOI5yDcvQBj7PzQ-zj1uUF70HCu",
                    audience = "https://dev-ea5oepbc2k553fws.us.auth0.com/api/v2/",
                    grant_type = "client_credentials"
                };

                var jsonRequestData = JsonSerializer.Serialize(requestData);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent(jsonRequestData, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<Auth0TokenResponse>(jsonResponse);

                return View("~/Views/Account/AuthToken.cshtml", tokenResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
