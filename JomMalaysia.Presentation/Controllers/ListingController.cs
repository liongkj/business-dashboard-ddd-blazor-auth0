using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using JomMalaysia.Framework.Exceptions;
using JomMalaysia.Framework.Helper;
using JomMalaysia.Presentation.Gateways.Categories;
using JomMalaysia.Presentation.Gateways.Listings;
using JomMalaysia.Presentation.Gateways.Merchants;
using JomMalaysia.Presentation.Models.Common;
using JomMalaysia.Presentation.Models.Listings;
using JomMalaysia.Presentation.ViewModels.Listings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JomMalaysia.Presentation.Controllers
{
    public class ListingController : Controller
    {
        private readonly IListingGateway _gateway;
        private readonly IMerchantGateway _merchantGateway;
        private readonly ICategoryGateway _categoryGateway;

        private List<Listing> ListingList { get; set; }
        private Boolean refresh;

        #region gateway helper

        public ListingController(IListingGateway gateway, IMerchantGateway merchantGateway,
            ICategoryGateway categoryGateway)
        {
            _gateway = gateway;
            _merchantGateway = merchantGateway;
            _categoryGateway = categoryGateway;

            Refresh();
        }

        async void Refresh()
        {
            if (ListingList != null && !refresh)
                ListingList = await GetListings().ConfigureAwait(false);
            else
            {
                ListingList = new List<Listing>();
            }
        }


        // GET: Listing
        private async Task<List<Listing>> GetListings()
        {
            ListingList = await _gateway.GetAll().ConfigureAwait(false);
            return ListingList;
        }

        #endregion

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var listings = await GetListings().ConfigureAwait(false);

            return View(listings);
        }


        // GET: Listing/Create
        public async Task<IActionResult> Create()
        {
            //merchant
            var merchants = await _merchantGateway.GetMerchants().ConfigureAwait(false);
            var _merchants = merchants.Select(m => new SelectListItem
            {
                Text = $"{m.CompanyRegistration.RegistrationName} ({m.CompanyRegistration.SsmId})",
                Value = m.MerchantId
            }).ToList();

            var _country = new List<SelectListItem>();
            foreach (Enum country in Enum.GetValues(typeof(CountryEnum)))
            {
                _country.Add(new SelectListItem
                {
                    Text = country.GetDescription(),
                    Value = country.ToString(),
                });
            }
            
            var _states = new List<SelectListItem>();
            var dicState = new Dictionary<string,string>();
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
            
            var vm = new RegisterListingViewModel
            {
                ImageUris = new ListingImageViewModel(),
                MerchantList = _merchants,
                Address = new Address
                {
                    StateList =  _states,
                    CountryList = _country
                }
            };
            ViewBag.States = dicState;
            return PartialView(vm);
        }

        // POST: Listing/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Tuple<int, string>> Create(RegisterListingViewModel vm)
        {
            if (!ModelState.IsValid) return SweetDialogHelper.HandleResponse(null);
            try
            {
                var response = await _gateway.Add(vm).ConfigureAwait(false);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    refresh = true;
                }

                return SweetDialogHelper.HandleResponse(response);
            }
            catch (GatewayException e)
            {
                return SweetDialogHelper.HandleStatusCode(e.StatusCode, e.Message);
            }
        }
       
        [HttpPost]
        public async Task<Tuple<int, string>> Publish(string id, int months)
        {
            if (!ModelState.IsValid) return SweetDialogHelper.HandleResponse(null);
            try
            {
                var response = await _gateway.Publish(id, months).ConfigureAwait(false);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    refresh = true;
                }

                return SweetDialogHelper.HandleResponse(response);
            }
            catch (GatewayException e)
            {
                return SweetDialogHelper.HandleStatusCode(e.StatusCode, e.Message);
            }
        }
    }
}