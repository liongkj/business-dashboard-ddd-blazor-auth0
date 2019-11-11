using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using JomMalaysia.Framework.Exceptions;
using JomMalaysia.Framework.Helper;
using JomMalaysia.Framework.WebServices;
using JomMalaysia.Presentation.Gateways.Categories;
using JomMalaysia.Presentation.Gateways.Listings;
using JomMalaysia.Presentation.Gateways.Merchants;
using JomMalaysia.Presentation.Models.Listings;
using JomMalaysia.Presentation.ViewModels.Listings;
using Microsoft.AspNetCore.Http;
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
        private Boolean refresh = false;
        #region gateway helper
        public ListingController(IListingGateway gateway, IMerchantGateway merchantGateway, ICategoryGateway categoryGateway)
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
        async Task<List<Listing>> GetListings()
        {

            try
            {
                ListingList = await _gateway.GetAll().ConfigureAwait(false);
                return ListingList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var listings = await GetListings().ConfigureAwait(false);

            return View(listings);
        }

        [HttpGet]
        // GET: Listing/Details/5
        public IActionResult Details()
        {
            return PartialView();
        }

        // GET: Listing/Create
        public async Task<IActionResult> Create()
        {
            var _merchants = new List<SelectListItem>();
            var merchants = await _merchantGateway.GetMerchants().ConfigureAwait(false);

            foreach (var m in merchants)
            {
                _merchants.Add(new SelectListItem
                {
                    Text = $"{m.CompanyRegistration.RegistrationName} ({m.CompanyRegistration.SsmId})",
                    Value = m.MerchantId
                });
            }
            var _listingTypes = new List<SelectListItem>();
            var listingTypes = ListingTypeHelper.GetTypeList();

            foreach (var m in listingTypes)
            {
                _listingTypes.Add(new SelectListItem
                {
                    Text = m,
                    Value = m
                }); ;
            }
            var _categories = new List<SelectListItem>();
            var categories = await _categoryGateway.GetCategories().ConfigureAwait(false);
            var cats = categories.Where(x => x.CategoryPath.Subcategory != null).OrderBy(x => x.CategoryName).GroupBy(x => x.CategoryPath.Category);

            foreach (var category in cats)
            {
                var groups = new SelectListGroup() { Name = category.Key };
                foreach (var sub in category)
                {
                    if (sub.CategoryPath.Subcategory != null)
                    {
                        _categories.Add(new SelectListItem
                        {
                            Text = $"{sub.CategoryPath.Subcategory}",
                            Value = sub.CategoryId,
                            Group = groups

                        });
                    }
                }
            }

            var vm = new RegisterListingViewModel()
            {
                CategoryList = _categories,
                MerchantList = _merchants,

                ListingTypeList = _listingTypes
            };
            return PartialView(vm);
        }

        // POST: Listing/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterListingViewModel vm)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult Publish()
        {
            return View();
        }

        [HttpPost]
        public async Task<Tuple<int, string>> Publish(string id, int months)
        {
            IWebServiceResponse response = null;
            if (ModelState.IsValid)
            {
                try
                {
                    response = await _gateway.Publish(id, months).ConfigureAwait(false);
                }
                catch (GatewayException e)
                {
                    return SweetDialogHelper.HandleStatusCode(e.StatusCode, e.Message);
                }
                if (response.StatusCode == HttpStatusCode.OK)

                    refresh = true;
            }

            return SweetDialogHelper.HandleResponse(response);
          
           
        }
    }
}