using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using JomMalaysia.Framework.Exceptions;
using JomMalaysia.Framework.Helper;
using JomMalaysia.Presentation.Gateways.Categories;
using JomMalaysia.Presentation.Gateways.Listings;
using JomMalaysia.Presentation.Gateways.Merchants;
using JomMalaysia.Presentation.Models.Categories;
using JomMalaysia.Presentation.Models.Common;
using JomMalaysia.Presentation.Models.Listings;
using JomMalaysia.Presentation.ViewModels.Common;
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
        private readonly IMapper _mapper;

        private List<Listing> ListingList { get; set; }
        private Boolean refresh;

        #region gateway helper

        public ListingController(IListingGateway gateway, IMerchantGateway merchantGateway,
            ICategoryGateway categoryGateway,IMapper mapper)
        {
            _gateway = gateway;
            _merchantGateway = merchantGateway;
            _categoryGateway = categoryGateway;
            _mapper = mapper;
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
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var init = await PopulateFields().ConfigureAwait(false);
            var _operatingHours = new List<OperatingHourViewModel>();
            
            foreach (var day in Enum.GetValues(typeof(DayOfWeek)))
            {
                _operatingHours.Add(new OperatingHourViewModel
                {
                    Enabled = false,
                    Day = (DayOfWeek)day,
                });
            }
            
            var vm = new RegisterListingViewModel
            {
                OperatingHours = _operatingHours,
                ListingImages = new ListingImageViewModel(),
                MerchantList = init.MerchantList,
                CategoryTypeList =  init.CategoryTypeList,
                Address = init.Address
            };
            ViewBag.States = init.StateDictionary;
            return View(vm);
        }

        // POST: Listing/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Tuple<int, string>> Create(RegisterListingViewModel vm)
        {
            if (!ModelState.IsValid) return SweetDialogHelper.HandleResponse(null);
            try
            {
                var OperatingHours = (from days in vm.OperatingHours where days.Enabled select new OperatingHourViewModel {Day = days.Day, OpenTime = days.OpenTime, CloseTime = days.CloseTime}).ToList();
                vm.OperatingHours = OperatingHours;
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
       
        
        [HttpGet]
        public async Task<ViewResult> Edit(string listingId)
        {
            Listing m;
            var init = await PopulateFields().ConfigureAwait(false);
            try
            {
                m = await _gateway.Detail(listingId).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw e;
            }
            
            var vm = _mapper.Map<RegisterListingViewModel>(m);
            
            //operating hours
            var _operatingHours = new List<OperatingHourViewModel>();
            var open = m.OperatingHours;
            var count = 0;
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                
                if(open.FindIndex(x=>x.DayOfWeek==day)>=0){
                    _operatingHours.Add(new OperatingHourViewModel
                    {
                        Enabled = true,
                        Day = open[count].DayOfWeek,
                        OpenTime = open[count].OpenTime,
                        CloseTime = open[count].CloseTime
                    });
                    count++;
                }
                else
                {
                    _operatingHours.Add(new OperatingHourViewModel
                    {
                        Enabled = false,
                        Day = day
                    });
                }
            }

            //populate fields
            vm.MerchantList = init.MerchantList.Where(x => x.Value != m.Merchant.MerchantId);
            vm.CategoryTypeList = init.CategoryTypeList;
            vm.Address.CountryList = init.Address.CountryList;
            vm.Address.StateList = init.Address.StateList;
            vm.OperatingHours = _operatingHours;
            
            ViewBag.States = init.StateDictionary;
            return View(vm);
        }
        
        [HttpPost]
        public async Task<Tuple<int, string>> Edit(RegisterListingViewModel vm, string listingId)
        {
         throw new NotImplementedException();
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
        
        //helper functions


    
        private async Task<RegisterListingViewModel> PopulateFields()
        {
            //merchant
            var merchants = await _merchantGateway.GetMerchants().ConfigureAwait(false);
            var _merchants = merchants.Select(m => new SelectListItem
            {
                Text = $"{m.CompanyRegistration.RegistrationName} ({m.CompanyRegistration.SsmId})",
                Value = m.MerchantId
            }).ToList();
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
            //category type
            var _categoryType = new List<SelectListItem>();
            foreach (Enum category in Enum.GetValues(typeof(CategoryType)))
            {
                _categoryType.Add(new SelectListItem
                {
                    Text = category.ToString(),
                    Value = category.ToString(),
                });
            }
            //states
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
                MerchantList = _merchants,
                CategoryTypeList =  _categoryType,
                Address = new AddressViewModel
                {
                    StateList =  _states,
                    CountryList = _country
                },
                StateDictionary = dicState,
            };
            return vm;
        }
    }
}