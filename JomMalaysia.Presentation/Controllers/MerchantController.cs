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
using JomMalaysia.Presentation.ViewModels.Common;

namespace JomMalaysia.Presentation.Controllers
{
    // [Authorize("read:merchant")] auth:enable this
    public class MerchantController : Controller
    {
        private readonly IMerchantGateway _gateway;

        private List<Merchant> Merchants { get; set; }
        private Boolean refresh = false;
        #region gateway helper
        public MerchantController(IMerchantGateway gateway)
        {
            _gateway = gateway;

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

        // GET: Listing
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
                throw e;
            }
        }

        #endregion
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var merchants = await GetMerchants();

            return View(merchants);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new RegisterMerchantViewModel());
        }

        [HttpPost]
        public async Task<Tuple<int, string>> Create(RegisterMerchantViewModel vm)
        {
            IWebServiceResponse response = null;
            vm.Contacts.Add(new ContactViewModel { 
            Name="Liong Khai Jiet",
            Email="khaijiet@hotmail.com",
            Phone="0187627267"
            });

            if (ModelState.IsValid)
            {
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
            }

            return SweetDialogHelper.HandleResponse(response);
        }
    }
}