using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JomMalaysia.Presentation.Models.Common;

namespace JomMalaysia.Presentation.ViewModels.Merchants
{
    public class RegisterMerchantViewModel
    {
        [Required]
        [Display(Name="Company Registration Number (SSM)")]
        public string SsmId { get; set; }
        [Required]
        [Display(Name = "Company Registered Name")]
        public string CompanyRegistrationName { get; set; }
       
        [Display(Name = "Registered Address")]
        public Address Address { get; set; }

        [Required]
        [Display(Name = "Contact")]
        public List<ContactViewModel> Contacts { get; set; }
    }
}