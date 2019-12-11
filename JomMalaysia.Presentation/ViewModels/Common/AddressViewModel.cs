using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JomMalaysia.Framework.Helper;
using JomMalaysia.Presentation.Models.Common;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JomMalaysia.Presentation.ViewModels.Common
{
    public class AddressViewModel
    {
        [Required(ErrorMessage = "Please complete the address")]
        [Display(Name = "Street 1")]

        public string Add1 { get; set; }

        [Display(Name = "Street 2 (Optional)")]
        public string Add2 { get; set; }

        [Required] public string City { get; set; }
        [Required] public StateEnum State { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        [Required] public CountryEnum Country { get; set; }
        public Coordinates Coordinates { get; set; }


        public IEnumerable<SelectListItem> CountryList { get; set; }
        public Tuple<IEnumerable<SelectListItem>,Dictionary<string,string>> StateList { get; set; }

        private IEnumerable<SelectListItem> populateCountryItem()
        {
            //country
            var _country = new List<SelectListItem>();
            foreach (Enum country in Enum.GetValues(typeof(CountryEnum)))
            {
                _country.Add(new SelectListItem
                {
                    Text = country.GetDescription(),
                    Value = country.ToString(),
                });
            }

            return _country;
        }

        public AddressViewModel()
        {
            CountryList=populateCountryItem();
            StateList = populateStatesItem();
        }
       private Tuple<IEnumerable<SelectListItem>,Dictionary<string,string>> populateStatesItem()
        {
            //states
            var _states = new List<SelectListItem>();
            var dicState = new Dictionary<string, string>();
            foreach (Enum state in Enum.GetValues(typeof(StateEnum)))
            {
                var fullName = state.GetDescription();
                _states.Add(new SelectListItem
                {
                    Text = fullName,
                    Value = state.ToString(),
                });

                if (fullName != null) dicState.Add(key: fullName.ToLower(), value: state.ToString());
            }

            return new Tuple<IEnumerable<SelectListItem>,Dictionary<string,string>>(_states,dicState);
        }
        
        public override string ToString()
        {
            var formatted = String.Format("{0} {1} \n{2} {3} {4} {5}",
                Add1, (!string.IsNullOrEmpty(Add2) ? Add2 : ""),
                PostalCode, City, State, Country
            );
            return formatted;
        }
    }
}