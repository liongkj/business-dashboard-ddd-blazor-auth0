using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JomMalaysia.Framework.WebServices;
using JomMalaysia.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace JomMalaysia.Presentation.Gateways.User
{
    public interface IUserGateway
    {
        Task<List<UserInfoViewModel>> GetAll();
        Task<IWebServiceResponse> Add(UserInfoViewModel vm);
        Task<IWebServiceResponse> Delete(string CategoryName);
    }
}
