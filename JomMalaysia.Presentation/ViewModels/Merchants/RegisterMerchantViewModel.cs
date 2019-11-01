using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JomMalaysia.Presentation.Models.Common;
using JomMalaysia.Presentation.ViewModels.Common;

namespace JomMalaysia.Presentation.ViewModels.Merchants
{
    public class RegisterMerchantViewModel
    {
        public RegisterMerchantViewModel()
        {
            Contacts = new List<ContactViewModel>();
        }

        [Required]
        [Display(Name="Company Registration Number (SSM)")]
        [RegularExpression(@"([\d]{12})", ErrorMessage = "Registration Number must be only digits")]
        [StringLength(12,MinimumLength =12, ErrorMessage = "Registration Number must be 12 digits")]
        public string SsmId { get; set; }

        [Display(Name = "Company Registration Number (old)")]
        public string OldSsmId { get; set; }


        [Required]
        [Display(Name = "Company Registered Name")]
        public string CompanyRegistrationName { get; set; }
       
        [Display(Name = "Registered Address")]
        public Address Address { get; set; }

        //[Required]
        [Display(Name = "Contact")]
        public List<ContactViewModel> Contacts { get; set; }
    }
}