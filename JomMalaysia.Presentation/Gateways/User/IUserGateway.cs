using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JomMalaysia.Framework.WebServices;
using JomMalaysia.Presentation.Models.AppUsers;
using JomMalaysia.Presentation.Models.Auth0;

namespace JomMalaysia.Presentation.Gateways.User
{
    public interface IUserGateway
    {
        Task<List<UserViewModel>> GetAll();
        Task<IWebServiceResponse> Add(UserViewModel vm);
        Task<IWebServiceResponse> Delete(string CategoryName);
    }
}
