using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JomMalaysia.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using JomMalaysia.Presentation.Models.Merchants;
using JomMalaysia.Presentation.Gateways.Merchants;

namespace JomMalaysia.Presentation.Controllers
{
    // [Authorize("read:merchant")] auth:enable this
    public class MerchantController : Controller
    {
        private readonly IMerchantGateway _gateway;

        private List<Merchant> ListingList { get; set; }

        #region gateway helper
        public MerchantController(IMerchantGateway gateway)
        {
            _gateway = gateway;

            Refresh();
        }

        async void Refresh()
        {
            if (ListingList != null)
                ListingList = await GetMerchants();
            else
            {
                ListingList = new List<Merchant>();
            }
        }



        // GET: Listing
        async Task<List<Merchant>> GetMerchants()
        {
            if (ListingList.Count > 0)
            {
                return ListingList;
            }
            try
            {
                ListingList = await _gateway.GetMerchants().ConfigureAwait(false);
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
            var merchants = await GetMerchants();

            return View(merchants);
        }
    }
}