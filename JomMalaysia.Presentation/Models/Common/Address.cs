using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JomMalaysia.Presentation.Models.Common
{
    public class Address
    {
        [Required(ErrorMessage = "Please complete the address")]
        [Display(Name = "Street 1")]
        
        public string Add1 { get; set; }
        [Display(Name = "Street 2 (Optional)")]
        public string Add2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public StateEnum State { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public CountryEnum Country { get; set; }
        [Required]
        public Coordinates Coordinates { get; set; }
        
               
        public IEnumerable<SelectListItem> CountryList { get; set; }
        public IEnumerable<SelectListItem> StateList { get; set; }


        public override string ToString()
        {
            var formatted = String.Format("{0} {1} \n{2} {3} {4} {5}",
                Add1, (!string.IsNullOrEmpty(Add2) ? Add2 : ""),
                PostalCode, City, State, Country
                );
            return formatted;
        }
    }

    public enum CountryEnum
    {
        [Description("Malaysia")]
        MY
    }

    public enum StateEnum
    {
        [Description("Negeri Sembilan")]
        NSN
    }
}