using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JomMalaysia.Framework.WebServices;
using JomMalaysia.Presentation.Models.Categories;

namespace JomMalaysia.Presentation.Gateways.Categories
{
    public interface ICategoryGateway
    {
        Task<List<Category>> GetCategories();
        Task<IWebServiceResponse> CreateCategory(Category vm);
        Task<IWebServiceResponse> EditCategory(Category vm);
        Task<IWebServiceResponse> Delete(string CategoryId);
    }
}
