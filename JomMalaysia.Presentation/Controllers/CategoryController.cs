using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using JomMalaysia.Framework.Constant;
using JomMalaysia.Framework.Exceptions;
using JomMalaysia.Framework.Helper;
using JomMalaysia.Framework.WebServices;
using JomMalaysia.Presentation.Gateways.Categories;
using JomMalaysia.Presentation.Models.Categories;
using JomMalaysia.Presentation.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JomMalaysia.Presentation.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryGateway _gateway;

        private static List<Category> CategoryList { get; set; }
        private static Boolean refresh = false;

        public CategoryController(ICategoryGateway gateway)
        {
            _gateway = gateway;

            Refresh();
        }

        private async void Refresh()
        {
            if (CategoryList != null)
                CategoryList = await GetCategories().ConfigureAwait(false);
            else
            {
                CategoryList = new List<Category>();
            }
        }


        private async Task<List<Category>> GetCategories()
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
            var categories = await GetCategories().ConfigureAwait(false);
            var vm = new List<Category>();
            if (categories == null ) return View(vm);
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

        [HttpGet]
        public ActionResult Create(string CategoryId, string parentName)
        {
            if (string.IsNullOrEmpty(CategoryId))
            {
                ViewData["title"] = "New Category";
                return View();
            }

            ViewData["title"] = $"Create new subcategory for {parentName}";
            TempData["categoryId"] = CategoryId;
            return View();
        }


        [HttpPost]
        public async Task<Tuple<int, string>> Create(NewCategoryViewModel vm, string parentCategoryId = null)
        {
            IWebServiceResponse response;
            if (!ModelState.IsValid) return SweetDialogHelper.HandleResponse(null);
            try
            {
                response = await _gateway.CreateCategory(vm, parentCategoryId).ConfigureAwait(false);
            }
            catch (GatewayException e)
            {
                return SweetDialogHelper.HandleStatusCode(e.StatusCode, e.Message);
            }

            if (response.StatusCode == HttpStatusCode.OK) refresh = true;

            return SweetDialogHelper.HandleResponse(response);
        }

        [HttpGet]
        public async Task<ViewResult> Edit(string categoryId)
        {
            var category = await _gateway.GetCategory(categoryId).ConfigureAwait(false);
           
            return View(category);
        }

        [HttpPost]
        public async Task<Tuple<int, string>> Edit(NewCategoryViewModel category, string categoryId)
        {
            if (!ModelState.IsValid)return SweetDialogHelper.HandleResponse(null);
            {
                IWebServiceResponse response;
                try
                {
                  
                    response = await _gateway.EditCategory(category, categoryId).ConfigureAwait(false);
                 
                }
                catch (GatewayException e)
                {
                    return SweetDialogHelper.HandleStatusCode(e.StatusCode, e.Message);
                }
                if (response.StatusCode == HttpStatusCode.OK) refresh = true;

                return SweetDialogHelper.HandleResponse(response);
            }
            
        }

        [HttpPost]
        //TODO [ValidateAntiForgeryToken]
        public async Task<Tuple<int, string>> ConfirmDelete(string CategoryId)
        {
            IWebServiceResponse response;
            if (string.IsNullOrEmpty(CategoryId)) return SweetDialogHelper.HandleNotFound();
            try
            {
                response = await _gateway.Delete(CategoryId).ConfigureAwait(false);
            }
            catch (GatewayException e)
            {
                return SweetDialogHelper.HandleStatusCode(e.StatusCode, e.Message);
            }

            if (response.StatusCode == HttpStatusCode.OK) refresh = true;
            return SweetDialogHelper.HandleResponse(response);
        }
        [HttpGet]
        public async Task<ActionResult> GetCategoryByType(CategoryType type)
        {
            //categories
            var _categories = new List<SelectListItem>();
            var categories = await GetCategories().ConfigureAwait(false);
            var cats = categories.Where(x => x.CategoryPath.Subcategory != null && x.CategoryType == type).OrderBy(x => x.CategoryName)
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