using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JomMalaysia.Presentation.Models;

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
