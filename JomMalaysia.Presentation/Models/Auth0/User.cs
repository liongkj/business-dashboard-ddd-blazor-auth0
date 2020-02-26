using System.ComponentModel.DataAnnotations;

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