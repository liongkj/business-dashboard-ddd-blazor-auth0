using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using JomMalaysia.Framework.Exceptions;
using JomMalaysia.Framework.Helper;
using JomMalaysia.Framework.Interfaces;
using JomMalaysia.Presentation.Gateways.Categories;
using JomMalaysia.Presentation.Gateways.Listings;
using JomMalaysia.Presentation.Models.Categories;
using JomMalaysia.Presentation.ViewModels.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RestSharp;


namespace JomMalaysia.Presentation.Controllers
{    
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryGateway _gateway;
        private readonly IListingGateway _listingGateway;


        public CategoryController(ICategoryGateway gateway, IListingGateway listingGateway)
        {
            _gateway = gateway;
            _listingGateway = listingGateway;

        }



        // GET: /<controller>/
        public async Task<IActionResult> Index()
        { 
            var vm = new List<Category>();
            try
            {
                var categories = await _gateway.GetCategories();
               

                var cats = categories.OrderBy(x => x.CategoryName).GroupBy(x => x.CategoryPath.Category);

                foreach (var category in cats)
                {
                    var c = category.FirstOrDefault(x => x.IsCategory());
                    if (c == null) continue;
                    c.LstSubCategory = new List<Category>();
                    foreach (var sub in category)
                    {
                        if (!sub.IsCategory())
                            c.LstSubCategory.Add(sub);
                    }

                    vm.Add(c);
                }

                return View(vm);
            }
            catch (GatewayException e)
            {
                if (e.StatusCode == HttpStatusCode.Unauthorized) RedirectToAction("Login", "Account");
                if(e.Type==WebServiceExceptionType.ConnectionError) ViewData["error"] = e.Message;
            }
 

            return View(vm);
        }

        [HttpGet]
        public ActionResult Create(string categoryId, string parentName)
        {
            if (string.IsNullOrEmpty(categoryId))
            {
                ViewData["title"] = "New Category";
                return View();
            }

            ViewData["title"] = $"Create new subcategory for {parentName}";
            TempData["categoryId"] = categoryId;
            return View();
        }

        
        [HttpPost]
        public async Task<Tuple<int, string>> Create( NewCategoryViewModel vm, string parentCategoryId = null)
        {
            IWebServiceResponse response;
            if (!ModelState.IsValid) return SweetDialogHelper.HandleResponse(null);
            try
            {
                response = await _gateway.CreateCategory(vm, parentCategoryId);
            }
            catch (GatewayException e)
            {
                return SweetDialogHelper.HandleStatusCode(e.StatusCode, e.Message);
            }
            
            return SweetDialogHelper.HandleResponse(response);
        }
        
        [HttpGet]
        public IActionResult Detail(string id)
        {
            var vm = new Category
            {
                CategoryId = id
            };
            return PartialView("_Detail", vm);
        }
            
    
        
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var categories = await _gateway.GetCategories();
            var vm = categories.FirstOrDefault(x => x.CategoryId == id);
            var listing = await _listingGateway.GetAll();
            if (vm != null)
            {
                vm.LstSubCategory = new List<Category>();
                foreach (var category in categories)
                {
                    if (!category.IsCategory() && category.CategoryPath.Category == vm.CategoryName)
                    {
                        category.LstListing = listing.Where(x => x.Category.CategoryId == category.CategoryId).ToList();
                        vm.LstSubCategory.Add(category);
                    }
                }

                return View(vm);
            }

            return NotFound();
        }
        
        [HttpPost]
        public async Task<Tuple<int, string>> Edit(NewCategoryViewModel category, string categoryId)
        {
            if (!ModelState.IsValid) return SweetDialogHelper.HandleResponse(null);
            {
                IWebServiceResponse response;
                try
                {
                    response = await _gateway.EditCategory(category, categoryId);
                }
                catch (GatewayException e)
                {
                    return SweetDialogHelper.HandleStatusCode(e.StatusCode, e.Message);
                }


                return SweetDialogHelper.HandleResponse(response);
            }
        }

        [HttpPost]
        //TODO [ValidateAntiForgeryToken]
        public async Task<Tuple<int, string>> ConfirmDelete(string categoryId)
        {
            IWebServiceResponse response;
            if (string.IsNullOrEmpty(categoryId)) return SweetDialogHelper.HandleNotFound();
            try
            {
                response = await _gateway.Delete(categoryId);
            }
            catch (GatewayException e)
            {
                return e.Type == WebServiceExceptionType.ConnectionError ? SweetDialogHelper.HandleStatusCode(HttpStatusCode.ServiceUnavailable) : SweetDialogHelper.HandleStatusCode(e.StatusCode, e.Message);
            }

            return SweetDialogHelper.HandleResponse(response);
        }

        [HttpGet]
        public async Task<ActionResult> GetCategoryByType(CategoryType type)
        {
            //categories
            var _categories = new List<SelectListItem>();
            var categories = await _gateway.GetCategories();
            var cats = categories.Where(x => x.CategoryPath.Subcategory != null && x.CategoryType == type)
                .OrderBy(x => x.CategoryName)
                .GroupBy(x => x.CategoryPath.Category);

            foreach (var category in cats)
            {
                var groups = new SelectListGroup {Name = category.Key};
                foreach (var sub in category)
                {
                    if (sub.CategoryPath.Subcategory != null)
                    {
                        _categories.Add(new SelectListItem
                        {
                            Text = sub.CategoryPath.Subcategory.CapitalizeOrConvertNullToEmptyString(),
                            Value = sub.CategoryId,
                            Group = groups
                        });
                    }
                }
            }

            return new JsonResult(_categories);
        }
    }
}