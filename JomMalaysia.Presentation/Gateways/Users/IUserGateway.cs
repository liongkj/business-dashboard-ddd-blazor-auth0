using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JomMalaysia.Framework.WebServices;
using JomMalaysia.Presentation.Models.AppUsers;
using JomMalaysia.Presentation.Models.Auth0;

namespace JomMalaysia.Presentation.Gateways.Users
{
    public interface IUserGateway
    {
        Task<List<AppUser>> GetAll();
        Task<IWebServiceResponse> Add(RegisterUserViewModel vm);
        Task<IWebServiceResponse> Delete(string CategoryName);
    }
}
