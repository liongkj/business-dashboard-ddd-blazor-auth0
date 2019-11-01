using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JomMalaysia.Presentation.ViewModels.Common
{
    public class ContactViewModel
    {
        [Required]
        [Phone]
        public string Phone { get; set; }
        [Required]
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
