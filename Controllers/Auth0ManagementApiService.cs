using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SampleMvcApp.ViewModels;
using SampleMvcApp.ViewModels.SampleMvcApp.ViewModels;

namespace SampleMvcApp.Controllers
{
    public class Auth0ManagementApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _auth0Domain;
        private readonly string _accessToken;

        public Auth0ManagementApiService(HttpClient httpClient, string auth0Domain, string accessToken)
        {
            _httpClient = httpClient;
            _auth0Domain = auth0Domain;
            _accessToken = accessToken;
            _httpClient.BaseAddress = new Uri($"https://{auth0Domain}/api/v2/");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        // Get User Information by User ID
        public async Task<List<UserResponse>> GetUsersAsync()
        {
            var response = await _httpClient.GetAsync("users");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<UserResponse>>(json);
            }
            else
            {
                throw new Exception($"Failed to retrieve user list. Status code: {response.StatusCode}");
            }
        }
        
        // Add Claims to a User by User ID
        public async Task AddClaimsToUserAsync(string userId, Dictionary<string, string> claims)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(new { claims }), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"users/{userId}/multifactor/{_auth0Domain}", jsonContent);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to add claims to the user. Status code: {response.StatusCode}");
            }
        }
    }
}
