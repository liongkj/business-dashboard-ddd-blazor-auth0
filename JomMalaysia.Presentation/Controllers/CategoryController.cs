using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using JomMalaysia.Framework.Constant;
using JomMalaysia.Framework.Helper;
using JomMalaysia.Framework.WebServices;
using JomMalaysia.Presentation.Gateways.Categories;
using JomMalaysia.Presentation.Models.Categories;
using Microsoft.AspNetCore.Mvc;

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

        async void Refresh()
        {
            if (CategoryList != null)
                CategoryList = await GetCategories().ConfigureAwait(false);
            else
            {
                CategoryList = new List<Category>();
            }
        }


        async Task<List<Category>> GetCategories()
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
            var categories = await GetCategories();
            var cats = categories.OrderBy(x => x.CategoryName).GroupBy(x => x.CategoryPath.Category);

            List<Category> vm = new List<Category>();
            foreach (var category in cats)
            {
                Category c = category.Where(x => x.IsCategory()).FirstOrDefault();
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
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<int> Create(Category vm)
        {
            //handle statuscode = 0; handle bad request
            int subCount = vm.LstSubCategory == null ? 0 : vm.LstSubCategory.Count;
            int message = 0;
            IWebServiceResponse response;

            for (int i = subCount - 1; i >= 0; i--)
            {
                if (vm.LstSubCategory[i].IsDeleted)
                {
                    vm.LstSubCategory.RemoveAt(i);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    response = await _gateway.CreateCategory(vm).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    throw e;
                }
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    refresh = true;
                    message = GlobalConstant.StatusCode.RESPONSE_OK;
                }
                else
                {
                    message = GlobalConstant.StatusCode.RESPONSE_ERR_UNKNOWN;
                }
            }
            else
            {
                message = GlobalConstant.StatusCode.RESPONSE_ERR_VALIDATION_FAILED;
            }

            return message;

        }

        [HttpGet]
        public ViewResult Edit(String categoryId)
        {
            Category category = new Category();

            category = CategoryList.FirstOrDefault(m => m.CategoryId == categoryId);

            return View(category);
        }

        [HttpPost]
        public async Task<int> Edit(Category category)
        {
            int message = 0;
            IWebServiceResponse response;
            if (ModelState.IsValid)
            {
                try
                {
                    // remove subcategory if category.lstCategory[i].isDeleted = true
                    response = await _gateway.EditCategory(category).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    //should change message to string. pass exception details.
                    message = GlobalConstant.StatusCode.RESPONSE_ERR_UNKNOWN;
                    throw e;
                }
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    refresh = true;
                    message = GlobalConstant.StatusCode.RESPONSE_OK;
                }
                else
                {
                    message = GlobalConstant.StatusCode.RESPONSE_ERR_UNKNOWN;
                }
            }
            else
            {
                message = GlobalConstant.StatusCode.RESPONSE_ERR_VALIDATION_FAILED;
            }


            return message;
        }

        // GET: Movies/Delete/5
        /* public async Task<IActionResult> Delete(string categoryId)
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
         }*/



        [HttpPost]
        //TODO [ValidateAntiForgeryToken]
        public async Task<Tuple<int, string>> ConfirmDelete(string CategoryId)
        {
            IWebServiceResponse response;

            if (CategoryId == null) return SweetDialogHelper.HandleNotFound();
            try
            {
                response = await _gateway.Delete(CategoryId);
            }
            catch (Exception e)
            {
                throw e;
            }
            if (response.StatusCode == HttpStatusCode.OK) refresh = true;

            return SweetDialogHelper.HandleResponse(response);


        }

        /*  public ActionResult Constants()
          {
              var constants = typeof(GlobalConstant)StatusCode.
                  .GetFields()
                  .ToDictionary(x => x.Name, x => x.GetValue(null));
              var json = new JavaScriptSerializer().Serialize(constants);
              return Content("var constants = " + json + ";");
          }
  */
        public ActionResult Publish(int id)
        {
            // delete student from the database whose id matches with specified id

            return RedirectToAction("Index");
        }
    }
}
