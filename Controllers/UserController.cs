using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SampleMvcApp.Controllers
{
    public class UserController : Controller
    {
        private readonly Auth0ManagementApiService _Auth0ManagementApiService;

        public UserController(Auth0ManagementApiService Auth0ManagementApiService)
        {
            _Auth0ManagementApiService = Auth0ManagementApiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var users = await _Auth0ManagementApiService.GetUsersAsync();
                return View(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
