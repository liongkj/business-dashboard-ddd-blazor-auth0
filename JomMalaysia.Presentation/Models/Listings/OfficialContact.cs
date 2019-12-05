using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace JomMalaysia.Presentation.Models.Listings
{
    public class OfficialContact
    {
        [Required(ErrorMessage = "Please enter a phone number")]
        [Phone]
        [Display(Name = "Mobile Phone")]
        public string MobileNumber { get; set; }
        [Phone]
        [Display(Name = "Office")]
        public string OfficeNumber { get; set; }
        [DataType(DataType.Url)]
        public string Website { get; set; }
        [Phone]
        public string Fax { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}