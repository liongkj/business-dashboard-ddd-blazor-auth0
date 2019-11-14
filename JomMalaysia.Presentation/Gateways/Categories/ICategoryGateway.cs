using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JomMalaysia.Framework.WebServices;
using JomMalaysia.Presentation.Models.Categories;
using JomMalaysia.Presentation.ViewModels.Categories;

namespace JomMalaysia.Presentation.Gateways.Categories
{
    public interface ICategoryGateway
    {
        Task<List<Category>> GetCategories();
        Task<IWebServiceResponse> CreateCategory(NewCategoryViewModel vm, string categoryId);
        Task<IWebServiceResponse> EditCategory(Category vm);
        Task<IWebServiceResponse> Delete(string CategoryId);
    }
}
