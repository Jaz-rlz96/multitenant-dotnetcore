using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SampleMvcApp.ViewModels;

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
        
        public async Task AddClaimsToUserAsync(string userId, Dictionary<string, string> claims)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(new { claims }), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"users/{userId}/multifactor/{_auth0Domain}", jsonContent);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to add claims to the user. Status code: {response.StatusCode}");
            }
        }


        public async Task<List<UserResponse>> ShowOrganizationUsers(string orgId)
        {
            var endpoint = $"organizations/{orgId}/members";
            var response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error fetching users from organization {orgId}. Status: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<UserResponse>>(content);
            return users;
        }
    }
}
