using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using JomMalaysia.Presentation.Models.Categories;
using JomMalaysia.Presentation.Models.Common;
using JomMalaysia.Presentation.Models.Listings;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JomMalaysia.Presentation.ViewModels.Listings
{
    public class RegisterListingViewModel
    {
        [Required]
        [Display(Name = "Business Name / Shop Name")]
        public string ListingName { get; set; }

        [MinLength(5)]
        [MaxLength(50)]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Category")]
        public string CategoryId { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public IEnumerable<OperatingHourViewModel> OperatingHours { get; set; }
        
        [Required]
        [Display(Name = "Type")]
        public CategoryType? CategoryType { get; set; }
        [Display(Name = "Address")]
        public string FullAddress { get; set; }
        public Address Address { get; set; }
        public ListingImageViewModel ImageUris{get;set;}
        public OfficialContact Contact { get; set; }

        
        [Required]
        [Display(Name ="Merchant")]
        public string MerchantId { get; set; }

        public IEnumerable<SelectListItem> MerchantList { get; set; }


        
    }
}
