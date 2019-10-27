using System;
using System.Collections.Generic;

namespace JomMalaysia.Presentation.Models.AppUsers
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        public string Isername { get; set; }
        public Email Email { get; set; }
        public Name Name { get; set; }
        public Role Role { get; set; }
        public List<string> additionalPermissions { get; set; }
        public string pictureUri { get; set; }
        public DateTime lastLogin { get; set; }
    }
    public class Email
    {
        public string user { get; set; }
        public string domain { get; set; }
    }

    public class Name
    {
        public string lastName { get; set; }
    }

    public class Role
    {
        public string name { get; set; }
        public int id { get; set; }
    }


}