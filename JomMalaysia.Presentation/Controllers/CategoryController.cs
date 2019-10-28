using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
                CategoryList = await GetCategories();
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
        public async Task<int> Create(Category vm)
        {
            //handle statuscode = 0; handle bad request
            int message = 0;
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
                    message = GlobalConstant.RESPONSE_OK;
                }
                else
                {
                    message = GlobalConstant.RESPONSE_ERR_UNKNOWN;
                }
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
                    message = GlobalConstant.RESPONSE_ERR_UNKNOWN;
                    throw e;
                }
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    refresh = true;
                    message = GlobalConstant.RESPONSE_OK;
                }
                else
                {
                    message = GlobalConstant.RESPONSE_ERR_UNKNOWN;
                }
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
        public async Task<int> ConfirmDelete(string CategoryId)
        {
            int message = 0;

            if (CategoryId == null) { message = GlobalConstant.RESPONSE_ERR_NOT_FOUND; }

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
                message = GlobalConstant.RESPONSE_OK;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                message = GlobalConstant.RESPONSE_ERR_DEPENDENCY;
            }
            else
            {
                message = GlobalConstant.RESPONSE_ERR_UNKNOWN;
            }
            return message;

        }

        /*  public ActionResult Constants()
          {
              var constants = typeof(GlobalConstant)
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
