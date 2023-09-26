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
                return View(model);
            }
            catch (Exception ex)
            {
                model.ErrorMessage = "An error occurred while fetching user list. Error: " + ex.Message;
                return View(model);
            }
        }



    }
}
