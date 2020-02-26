using System.Collections.Generic;
using System.Threading.Tasks;
using JomMalaysia.Framework.Interfaces;
using JomMalaysia.Presentation.Models.Categories;
using JomMalaysia.Presentation.ViewModels.Categories;

namespace JomMalaysia.Presentation.Gateways.Categories
{
    public interface ICategoryGateway
    {
        Task<List<Category>> GetCategories();
        Task<Category> GetCategory(string CategoryId);
        Task<IWebServiceResponse> CreateCategory(NewCategoryViewModel vm, string categoryId);
        Task<IWebServiceResponse> EditCategory(NewCategoryViewModel vm, string categoryId);
        Task<IWebServiceResponse> Delete(string CategoryId);
    }
}