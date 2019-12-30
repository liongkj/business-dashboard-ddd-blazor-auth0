using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using JomMalaysia.Framework.Exceptions;
using JomMalaysia.Framework.Helper;
using JomMalaysia.Framework.WebServices;
using JomMalaysia.Presentation.Gateways.Categories;
using JomMalaysia.Presentation.Gateways.Listings;
using JomMalaysia.Presentation.Gateways.Merchants;
using JomMalaysia.Presentation.Models.Categories;
using JomMalaysia.Presentation.Models.Listings;
using JomMalaysia.Presentation.ViewModels.Common;
using JomMalaysia.Presentation.ViewModels.Listings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JomMalaysia.Presentation.Controllers
{
    [Authorize]
    public class ListingController : Controller
    {
        private readonly ICategoryGateway _categoryGateway;
        private readonly IListingGateway _gateway;
        private readonly IMapper _mapper;
        private readonly IMerchantGateway _merchantGateway;
        private bool refresh;

        private List<Listing> ListingList { get; set; }

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
                _operatingHours.Add(new OperatingHourViewModel
                {
                    Enabled = false,
                    Day = (DayOfWeek) day
                });

            var vm = new RegisterListingViewModel
            {
                OperatingHours = _operatingHours,
                ListingImages = new ListingImageViewModel(),
                MerchantList = init.MerchantList,
                CategoryTypeList = init.CategoryTypeList,
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
                var OperatingHours = (from days in vm.OperatingHours
                    where days.Enabled
                    select new OperatingHourViewModel
                        {Day = days.Day, OpenTime = days.OpenTime, CloseTime = days.CloseTime}).ToList();
                vm.OperatingHours = OperatingHours;
                var response = await _gateway.Add(vm).ConfigureAwait(false);
                if (response.StatusCode == HttpStatusCode.OK) refresh = true;

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
                if (open.FindIndex(x => x.DayOfWeek == day) >= 0)
                {
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

            //category
            var _categories = new List<SelectListItem>();
            var categories = await _categoryGateway.GetCategories().ConfigureAwait(false);
            var cats = categories.Where(x => x.CategoryPath.Subcategory != null && x.CategoryType == m.CategoryType)
                .OrderBy(x => x.CategoryName)
                .GroupBy(x => x.CategoryPath.Category);

            foreach (var category in cats)
            {
                var groups = new SelectListGroup {Name = category.Key};
                foreach (var sub in category)
                    if (sub.CategoryPath.Subcategory != null)
                        _categories.Add(new SelectListItem
                        {
                            Text = sub.CategoryPath.Subcategory.CapitalizeOrConvertNullToEmptyString(),
                            Value = sub.CategoryId,
                            Group = groups
                        });
            }

            //populate fields
            vm.MerchantList = init.MerchantList;
            vm.CategoryTypeList = init.CategoryTypeList;
            vm.Address.CountryList = init.Address.CountryList;
            vm.Address.StateList = init.Address.StateList;
            vm.OperatingHours = _operatingHours;
            vm.CategoryList = _categories;
            ViewBag.States = init.StateDictionary;
            return View(vm);
        }

        [HttpPost]
        public async Task<Tuple<int, string>> Edit(RegisterListingViewModel vm, string listingId)
        {
            if (!ModelState.IsValid) return SweetDialogHelper.HandleResponse(null);
            try
            {
                var OperatingHours = (from days in vm.OperatingHours
                    where days.Enabled
                    select new OperatingHourViewModel
                        {Day = days.Day, OpenTime = days.OpenTime, CloseTime = days.CloseTime}).ToList();
                vm.OperatingHours = OperatingHours;
                var response = await _gateway.Edit(vm, listingId).ConfigureAwait(false);
                if (response.StatusCode == HttpStatusCode.OK) refresh = true;

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
                if (response.StatusCode == HttpStatusCode.OK) refresh = true;

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


            //category type
            var _categoryType = new List<SelectListItem>();
            foreach (Enum category in Enum.GetValues(typeof(CategoryType)))
                _categoryType.Add(new SelectListItem
                {
                    Text = category.ToString(),
                    Value = category.ToString()
                });

            var add = new AddressViewModel();
            var vm = new RegisterListingViewModel
            {
                MerchantList = _merchants,
                CategoryTypeList = _categoryType,
                Address = add,
                StateDictionary = add.StateList.Item2
            };
            return vm;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Tuple<int, string>> ConfirmDelete(string id)
        {
            IWebServiceResponse response;
            if (string.IsNullOrEmpty(id)) return SweetDialogHelper.HandleNotFound();
            try
            {
                response = await _gateway.Delete(id).ConfigureAwait(false);
            }
            catch (GatewayException e)
            {
                return SweetDialogHelper.HandleStatusCode(e.StatusCode, e.Message);
            }

            if (response.StatusCode == HttpStatusCode.OK) refresh = true;
            return SweetDialogHelper.HandleResponse(response);
        }

        public IActionResult GetByMerchant()
        {
            throw new NotImplementedException();
        }

        #region gateway helper

        public ListingController(IListingGateway gateway, IMerchantGateway merchantGateway,
            ICategoryGateway categoryGateway, IMapper mapper)
        {
            _gateway = gateway;
            _merchantGateway = merchantGateway;
            _categoryGateway = categoryGateway;
            _mapper = mapper;
            Refresh();
        }

        private async void Refresh()
        {
            if (ListingList != null && !refresh)
                ListingList = await GetListings().ConfigureAwait(false);
            else
                ListingList = new List<Listing>();
        }


        // GET: Listing
        private async Task<List<Listing>> GetListings()
        {
            ListingList = await _gateway.GetAll().ConfigureAwait(false);
            return ListingList;
        }

        #endregion
    }
}