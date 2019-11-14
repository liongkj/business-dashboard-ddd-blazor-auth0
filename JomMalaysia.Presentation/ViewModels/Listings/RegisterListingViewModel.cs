﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using JomMalaysia.Presentation.Models.Common;
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

        public Address Address { get; set; }
        public ListingImageViewModel ImageUris{get;set;}

        [Required]
        [Display(Name = "Type Of Listing")]
        public string ListingType { get; set; }
        public IEnumerable<SelectListItem> ListingTypeList { get; set; }
        
        [Required]
        [Display(Name ="Merchant")]
        public string MerchantId { get; set; }

        public IEnumerable<SelectListItem> MerchantList { get; set; }

    }
}