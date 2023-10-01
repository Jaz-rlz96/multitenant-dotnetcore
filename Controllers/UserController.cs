using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SampleMvcApp.ViewModels;

namespace SampleMvcApp.Controllers
{
    public class UserController : Controller
    {
        private readonly Auth0ManagementApiService _Auth0ManagementApiService;

        public UserController(Auth0ManagementApiService Auth0ManagementApiService)
        {
            _Auth0ManagementApiService = Auth0ManagementApiService;
        }

        public async Task<IActionResult> Index(string orgId)
        {
            var model = new UserPageModel();
            try
            {
                model.Users = await _Auth0ManagementApiService.ShowOrganizationUsers(orgId);
                return View("~/Views/Auth0/User.cshtml", model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



    }
}
