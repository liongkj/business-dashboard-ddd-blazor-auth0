using System;
using System.Collections.Generic;
using JomMalaysia.Presentation.Models.Common;

namespace JomMalaysia.Presentation.Models.AppUsers
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public Email Email { get; set; }
        public Name Name { get; set; }
        public Role Role { get; set; }
        public List<string> AdditionalPermissions { get; set; }
        public string PictureUri { get; set; }
        public DateTime LastLogin { get; set; }
    }


    public class Role
    {
        public string Name { get; set; }



    }
}