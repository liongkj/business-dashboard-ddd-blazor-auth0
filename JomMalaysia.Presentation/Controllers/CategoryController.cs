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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JomMalaysia.Presentation.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryGateway _gateway;

        private static List<CategoryViewModel> CategoryList { get; set; }
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
            if (CategoryList.Count > 0)
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
                    message = "swal('Good job!', 'You clicked the button!', 'success');";
                    TempData["Message"] = message;
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
        public ActionResult Edit(CategoryViewModel std)
        {

            return RedirectToAction("Index");
        }



        [HttpGet("{categoryName}")]
        public async Task<IActionResult> Delete(string categoryName)
        {
            if (categoryName == null) return NotFound();
            IWebServiceResponse response;

            try
            {
                response = await _gateway.Delete(categoryName);
            }
            catch (Exception e)
            {
                throw e;
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
