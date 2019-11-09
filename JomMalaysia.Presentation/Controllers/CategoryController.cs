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
        private readonly IWorkflowGateway _gateway;

        private static List<Category> CategoryList { get; set; }
        private static Boolean refresh = false;
        public CategoryController(IWorkflowGateway gateway)
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

            List<Category> lstcategory = new List<Category>();

            for (int i = 1; i <= 5; i++)
            {
                Category category = new Category();
                category.CategoryId = String.Concat(i + i + i + i + i);
                category.CategoryCode = "Code " + i;
                category.CategoryName = " Name " + i;
                category.CategoryNameMs = "Name Ms" + i;
                category.CategoryNameZh = " Name Zh " + i;
                category.CategoryThumbnailUrl = "Thumbnail url " + i;
                category.CategoryImageUrl = "Image url " + i;

                List<Category> lstsubcategory = new List<Category>();
                if (i != 3 && i != 5)
                {
                    for (int j = 1; j <= 3; j++)
                    {
                        Category subcategory = new Category();

                        subcategory.CategoryId = String.Concat("subcategory" + j + j + j + j + j);
                        subcategory.CategoryCode = "subCode " + j;
                        subcategory.CategoryName = " subName " + j;
                        subcategory.CategoryNameMs = "subName Ms" + j;
                        subcategory.CategoryNameZh = "subName Zh " + j;
                        subcategory.CategoryThumbnailUrl = "subThumbnail url " + j;
                        subcategory.CategoryImageUrl = "subImage url " + j;


                        lstsubcategory.Add(subcategory);
                    }
                }
                else
                {
                    lstsubcategory = null;
                }

                category.LstSubCategory = lstsubcategory;

                lstcategory.Add(category);
            }

            return View(lstcategory);
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
        public async Task<Tuple<int,string>> ConfirmDelete(string CategoryId)
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
