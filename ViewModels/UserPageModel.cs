using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SampleMvcApp.Controllers;
using SampleMvcApp.ViewModels.SampleMvcApp.ViewModels;

namespace SampleMvcApp.ViewModels
{
    public class UserPageModel : PageModel
    {
        public List<UserResponse> Users { get; set; }

        public string ErrorMessage { get; set; }

        private readonly Auth0ManagementApiService _Auth0ManagementApiService;

        public UserPageModel(Auth0ManagementApiService Auth0ManagementApiService)
        {
            _Auth0ManagementApiService = Auth0ManagementApiService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                // Retrieve the list of users from Auth0 using the injected service
                Users = await _Auth0ManagementApiService.GetUsersAsync();

                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred while fetching user list. Error: " + ex.Message;
            }

            return Page();
        }
    }
}
