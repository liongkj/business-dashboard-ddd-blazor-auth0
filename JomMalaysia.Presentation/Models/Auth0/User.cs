using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JomMalaysia.Presentation.Models.Auth0
{
    public class User
    {
        [Display(Name = "Email Address / Username")]
        public string email { get; set; }

        [Display(Name = "Password")] public string password { get; set; }

        [Display(Name = "ReturnURL")] public string returnURL { get; set; }
    }
}