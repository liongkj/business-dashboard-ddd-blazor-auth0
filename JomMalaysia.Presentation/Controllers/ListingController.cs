using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using JomMalaysia.Presentation.Gateways.Listings;
using JomMalaysia.Presentation.Models.Listings;
using JomMalaysia.Presentation.ViewModels.Listings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JomMalaysia.Presentation.Controllers
{
    public class ListingController : Controller
    {
        private readonly IListingGateway _gateway;

        private List<Listing> ListingList { get; set; }
        private Boolean refresh = false;
        #region gateway helper
        public ListingController(IListingGateway gateway)
        {
            _gateway = gateway;

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

        // GET: Listing/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Listing/Create
        public IActionResult Create()
        {
            return PartialView(new CreateListingViewModel());
        }

        // POST: Listing/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateListingViewModel vm)
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

        // GET: Listing/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Listing/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Listing/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Listing/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}