using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using JomMalaysia.Presentation.Models.Merchants;
using JomMalaysia.Presentation.Gateways.Merchants;
using JomMalaysia.Presentation.ViewModels.Merchants;
using JomMalaysia.Framework.Exceptions;
using JomMalaysia.Framework.Helper;
using System.Net;
using AutoMapper;
using JomMalaysia.Framework.Interfaces;
using JomMalaysia.Presentation.Gateways.Listings;

namespace JomMalaysia.Presentation.Controllers
{
    [Authorize]
    public class MerchantController : Controller
    {
        private readonly IMerchantGateway _gateway;
        private readonly IListingGateway _listingGateway;
        private List<Merchant> Merchants { get; set; }
        private Boolean refresh = false;

        private readonly IMapper _mapper;

        #region gateway helper

        public MerchantController(IMerchantGateway gateway, IMapper mapper, IListingGateway listingGateway)
        {
            _gateway = gateway;
            _mapper = mapper;
            _listingGateway = listingGateway;
            Refresh();
        }

        async void Refresh()
        {
            if (Merchants != null && !refresh)
                Merchants = await GetMerchants();
            else
            {
                Merchants = new List<Merchant>();
            }
        }

        // GET: Merchant
        async Task<List<Merchant>> GetMerchants()
        {
            if (Merchants.Count > 0)
            {
                return Merchants;
            }

            try
            {
                Merchants = await _gateway.GetMerchants();
                return Merchants;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var vm = new List<Merchant>();
            try
            {
                
                var merchants = await GetMerchants();
                var listings = await _listingGateway.GetAll();
                foreach (var m in merchants)
                {
                    m.Listing = listings.Where(x => x.Merchant.MerchantId == m.MerchantId).ToList();
                    vm.Add(m);
                }
            }
            catch (GatewayException e)
            {
                if (e.StatusCode == HttpStatusCode.Unauthorized) RedirectToAction("Login", "Account");
                if(e.Type==WebServiceExceptionType.ConnectionError) ViewData["error"] = e.Message;
            }

            return View(vm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var vm = new RegisterMerchantViewModel();

            return View(vm);
        }
        
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<Tuple<int, string>> Create(RegisterMerchantViewModel vm)
        {
            IWebServiceResponse response = null;


            if (!ModelState.IsValid) return SweetDialogHelper.HandleResponse(null);
            try
            {
                response = await _gateway.Add(vm);
            }
            catch (GatewayException e)
            {
                return SweetDialogHelper.HandleStatusCode(e.StatusCode, e.Message);
            }

            if (response.StatusCode == HttpStatusCode.OK)

                refresh = true;

            return SweetDialogHelper.HandleResponse(response);
        }

        [HttpGet]
        public IActionResult _Detail(string id)
        {
            var vm = new Merchant
            {
                MerchantId = id
            };
            return PartialView("_Detail", vm);
        }

        [HttpGet]
        public async Task<ViewResult> Edit(string merchantId)
        {
            Merchant m;
            try
            {
                m = await _gateway.Detail(merchantId);
            }
            catch (Exception e)
            {
                throw;
            }

            var vm = _mapper.Map<RegisterMerchantViewModel>(m);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Tuple<int, string>> Edit(RegisterMerchantViewModel vm, string merchantId)
        {
            if (!ModelState.IsValid) return SweetDialogHelper.HandleResponse(null);

            try
            {
                var response = await _gateway.Edit(vm, merchantId);
                if (response.StatusCode == HttpStatusCode.OK) refresh = true;
                return SweetDialogHelper.HandleResponse(response);
            }
            catch (GatewayException e)
            {
                return SweetDialogHelper.HandleStatusCode(e.StatusCode, e.Message);
            }
        }
    }
}