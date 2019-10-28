using System.Collections.Generic;
using JomMalaysia.Presentation.Models.AppUsers;

namespace JomMalaysia.Presentation.Gateways.Users
{
    public class UserListResponse
    {
        public UserListViewModel Data { get; set; }

    }
    public class UserListViewModel
    {
        public List<UserViewModel> Results { get; set; }

    }
}
