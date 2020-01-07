using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JomMalaysia.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using JomMalaysia.Presentation.Models.Merchants;
using JomMalaysia.Presentation.Gateways.Merchants;
using JomMalaysia.Presentation.ViewModels.Merchants;
using JomMalaysia.Framework.WebServices;
using JomMalaysia.Framework.Exceptions;
using JomMalaysia.Framework.Helper;
using System.Net;
using AutoMapper;
using JomMalaysia.Presentation.ViewModels.Common;
using JomMalaysia.Presentation.ViewModels.Listings;

namespace JomMalaysia.Presentation.Controllers
{
    [Authorize]
    public class MerchantController : Controller
    {
        private readonly IMerchantGateway _gateway;

        private List<Merchant> Merchants { get; set; }
        private Boolean refresh = false;

        private readonly IMapper _mapper;

        #region gateway helper

        public MerchantController(IMerchantGateway gateway, IMapper mapper)
        {
            _gateway = gateway;
            _mapper = mapper;
            Refresh();
        }

        async void Refresh()
        {
            if (Merchants != null && !refresh)
                Merchants = await GetMerchants().ConfigureAwait(false);
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
                Merchants = await _gateway.GetMerchants().ConfigureAwait(false);
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
            var merchants = await GetMerchants().ConfigureAwait(false);

            return View(merchants);
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
                response = await _gateway.Add(vm).ConfigureAwait(false);
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
        public IActionResult Detail(string id)
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
                m = await _gateway.Detail(merchantId).ConfigureAwait(false);
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
                var response = await _gateway.Edit(vm, merchantId).ConfigureAwait(false);
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