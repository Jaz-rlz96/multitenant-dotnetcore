using SampleMvcApp.Controllers;
using System.Collections.Generic;

namespace SampleMvcApp.ViewModels
{
    public class UserPageModel
    {
        public List<UserResponse> Users { get; set; }

        public string ErrorMessage { get; set; }

        // This constructor is for creating an empty model
        public UserPageModel() { }

        // This constructor is for when you want to initialize with the service, 
        // but it's not strictly necessary for the ViewModel.
        public UserPageModel(Auth0ManagementApiService Auth0ManagementApiService)
        {
            // If you need to use the service here, you can, 
            // but typically the ViewModel shouldn't contain business logic.
        }
    }
}
