using System.Threading.Tasks;
using JomMalaysia.Presentation.Gateways.Categories;
using Microsoft.AspNetCore.Mvc;

namespace JomMalaysia.Presentation.Views.Category.Components.CategoryDetail
{
    public class CategoryDetailViewComponent : ViewComponent
    {
       

        private readonly ICategoryGateway _categoryGateway;
        public CategoryDetailViewComponent(ICategoryGateway categoryGateway)
        {
            _categoryGateway = categoryGateway;
        }

        public async Task<IViewComponentResult> InvokeAsync(
            string id)
        {
           
            var category = await _categoryGateway.GetCategory(id).ConfigureAwait(false);
            
            return View(category);
        }
    }
}