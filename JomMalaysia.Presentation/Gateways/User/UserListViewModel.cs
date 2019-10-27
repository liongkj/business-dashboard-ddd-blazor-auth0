using System.Collections.Generic;
using JomMalaysia.Presentation.Models.AppUsers;

namespace JomMalaysia.Presentation.Gateways.User
{
    public class UserListViewModel
    {
        public ResultViewModel Data { get; set; }
    }

    public class ResultViewModel
    {
        public List<UserViewModel> Results { get; set; }
    }
}
