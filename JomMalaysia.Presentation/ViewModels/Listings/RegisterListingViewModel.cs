using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JomMalaysia.Presentation.Models.Categories;
using JomMalaysia.Presentation.Models.Listings;
using JomMalaysia.Presentation.ViewModels.Common;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JomMalaysia.Presentation.ViewModels.Listings
{
    public class RegisterListingViewModel
    {
        public string ListingId { get; set; }

        [Required]
        [Display(Name = "Business Name / Shop Name")]
        public string ListingName { get; set; }

        [Display(Name = "Listing Description")]
        public ListingDescription Description { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string CategoryId { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public List<OperatingHourViewModel> OperatingHours { get; set; }

        [Required] [Display(Name = "Type")] public CategoryType? CategoryType { get; set; }


        [Required] [Display(Name = "Address")] public string FullAddress { get; set; }

        [Required] public AddressViewModel Address { get; set; }

        public ListingImageViewModel ListingImages { get; set; }

        [Display(Name = "Public Contact Info")]
        public OfficialContact OfficialContact { get; set; }

        [Display(Name = "Featured Listing")]
        public bool IsFeatured { get; set; } = false;

        [Required]
        [Display(Name = "Merchant")]
        public string MerchantId { get; set; }

        //for populate select options
        public IEnumerable<SelectListItem> MerchantList { get; set; }
        public IEnumerable<SelectListItem> CategoryTypeList { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public Dictionary<string, string> StateDictionary { get; set; }
        public string ContactString { get; set; }
    }
}