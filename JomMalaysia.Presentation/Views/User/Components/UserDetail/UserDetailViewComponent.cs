using System.Threading.Tasks;
using JomMalaysia.Presentation.Gateways.Users;
using Microsoft.AspNetCore.Mvc;

namespace JomMalaysia.Presentation.Views.User.Components.UserDetail
{
    public class UserDetailViewComponent : ViewComponent
    {
        private readonly IUserGateway _gateway;

        public UserDetailViewComponent(IUserGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<IViewComponentResult> InvokeAsync(
            string id)
        {
            var item = await _gateway.Detail(id).ConfigureAwait(false);

            return View(item);
        }
    }
}