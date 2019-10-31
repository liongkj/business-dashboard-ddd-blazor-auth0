using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JomMalaysia.Presentation.ViewModels.Users
{
    public class RegisterUserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [Required]
        [MinLength(5)]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Assign Role")]
        public string Role { get; set; }
        public IEnumerable<SelectListItem> RoleList { get; set; }
    }
}
