using System.Collections.Generic;
using System.Threading.Tasks;
using JomMalaysia.Framework.WebServices;
using JomMalaysia.Presentation.Models.AppUsers;
using JomMalaysia.Presentation.ViewModels.Users;

namespace JomMalaysia.Presentation.Gateways.Users
{
    public interface IUserGateway
    {
        Task<List<AppUser>> GetAll();
        Task<AppUser> Detail(string id);
        Task<IWebServiceResponse> Add(RegisterUserViewModel vm);
        Task<IWebServiceResponse> Delete(string UserId);
        Task<IWebServiceResponse> UpdateRole(string UserId, RegisterUserViewModel Role);
    }
}