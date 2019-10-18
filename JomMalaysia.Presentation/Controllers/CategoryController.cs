using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using JomMalaysia.Framework.WebServices;
using JomMalaysia.Presentation.Gateways.Category;
using JomMalaysia.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JomMalaysia.Presentation.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryGateway _gateway;

        private static List<CategoryViewModel> CategoryList { get; set; }
        private static Boolean refresh = false;
        public CategoryController(ICategoryGateway gateway)
        {
            _gateway = gateway;

            Refresh();
        }

        async void Refresh()
        {
            if (CategoryList != null)
                CategoryList = await GetCategories();
            else
            {
                CategoryList = new List<CategoryViewModel>();
            }
        }


        async Task<List<CategoryViewModel>> GetCategories()
        {
            if (CategoryList.Count > 0 && !refresh)
            {
                return CategoryList;
            }
            try
            {
                CategoryList = await _gateway.GetCategories().ConfigureAwait(false);

                return CategoryList;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var cat = await GetCategories();
            //var filtered = cat.AsQueryable().Where(c => c.CategoryPath.Subcategory == null);

            return View(cat);
        }

        [HttpGet]
        public ViewResult Create()
        {
            ViewBag.Messages = "";
            TempData["Message"] = "";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel vm)
        {
            //handle statuscode = 0; handle bad request
            var message = "";
            IWebServiceResponse response;
            if (ModelState.IsValid)
            {
                try
                {
                    // remove subcategory if category.lstCategory[i].isDeleted = true
                    response = await _gateway.CreateCategory(vm).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    throw e;
                }
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    refresh = true;
                    message = "success";
                    TempData["Message"] = message;
                    ViewBag.Messages = "success";
                }
                else
                {
                    TempData["Message"] = "Failed to create category. Category name may be duplicated.";
                }
            }

            return RedirectToAction("Index");

            // update student to the database
        }

        [HttpGet]
        public ViewResult Edit(String categoryName)
        {
            CategoryViewModel categoryViewModel = new CategoryViewModel();

            categoryViewModel = CategoryList.FirstOrDefault(m => m.CategoryName == categoryName);

            return View(categoryViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryViewModel categoryViewModel)
        {
            var message = "";
            IWebServiceResponse response;
            if (ModelState.IsValid)
            {
                try
                {
                    // remove subcategory if category.lstCategory[i].isDeleted = true
                    response = await _gateway.EditCategory(categoryViewModel).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    throw e;
                }
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    refresh = true;
                    message = "swal('Good job!', 'You clicked the button!', 'success');";
                    TempData["Message"] = message;
                    ViewBag.Message = "success";
                }
                else
                {
                    TempData["Message"] = "Failed to create category. Category name may be duplicated.";
                }
            }


            return RedirectToAction("Index");
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(string categoryId)
        {
            if (categoryId == null)
            {
                return NotFound();
            }

            var cat = await CategoryList.AsQueryable()
                .FirstOrDefaultAsync(m => m.CategoryId == categoryId);
            if (cat == null)
            {
                return NotFound();
            }

            return View(cat);
        }



        [HttpPost]
        //TODO [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string CategoryId)
        {
            if (CategoryId == null) return NotFound();
            IWebServiceResponse response;

            try
            {
                response = await _gateway.Delete(CategoryId);
            }
            catch (Exception e)
            {
                throw e;
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {
                refresh = true;
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                //return message with still has subcategory
            }
            return RedirectToAction(nameof(Index));

        }

        public ActionResult Publish(int id)
        {
            // delete student from the database whose id matches with specified id

            return RedirectToAction("Index");
        }
    }
}
