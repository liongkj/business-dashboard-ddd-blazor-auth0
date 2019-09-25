using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JomMalaysia.Framework.Configuration;
using JomMalaysia.Framework.Constant;
using JomMalaysia.Framework.WebServices;
using JomMalaysia.Presentation.Gateways.User;
using JomMalaysia.Presentation.Manager;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JomMalaysia.Presentation.Controllers
{
    public class UserController : Controller
    {

        private readonly IUserGateway userGateway;

        public UserController(IUserGateway userGateway)
        {
            this.userGateway = userGateway;

        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {

            await userGateway.GetAll();

            return View();
        }
    }
}
