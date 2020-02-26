using System.ComponentModel.DataAnnotations;

namespace JomMalaysia.Presentation.ViewModels.Common
{
    public class ContactViewModel
    {
        [Required] [Phone] public string Phone { get; set; }
        [Required] public string Name { get; set; }
        [Required] [EmailAddress] public string Email { get; set; }
    }
}