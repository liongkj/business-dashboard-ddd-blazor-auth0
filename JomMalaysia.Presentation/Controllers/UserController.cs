using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using JomMalaysia.Framework.Exceptions;
using JomMalaysia.Framework.Helper;
using JomMalaysia.Framework.Interfaces;
using JomMalaysia.Presentation.Gateways.Users;
using JomMalaysia.Presentation.Models.AppUsers;
using JomMalaysia.Presentation.ViewModels.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JomMalaysia.Presentation.Controllers
{
    [Authorize]
    public class UserController : Controller
    {

        private readonly IUserGateway _gateway;


        public UserController(IUserGateway userGateway)
        {
            _gateway = userGateway;
            
        }
        // GET: /<controller>/

        public async Task<IActionResult> Index()
        {

            try
            {
                await GetUserRole();
                // ViewData["Role"] = ;
                List<AppUser> users = new List<AppUser>();
                try
                {
                    users = await _gateway.GetAll();
                }
                catch (GatewayException e)
                {
                    if (e.StatusCode == HttpStatusCode.Unauthorized) RedirectToAction("Login", "Account");
                    if (e.Type == WebServiceExceptionType.ConnectionError) ViewData["error"] = e.Message;
                }

                return View(users.OrderBy(u =>
                {
                    var index = RoleHelper.AuthorityList.IndexOf(u.Role);
                    return index == -1 ? int.MaxValue : index;
                }));
            }
            catch (Exception e)
            {
                
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new RegisterUserViewModel
            {
                RoleList = await GetAssignableRole(),

                Role = "editor"
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<Tuple<int, string>> Create(RegisterUserViewModel vm)
        {
            IWebServiceResponse response;

            if (!ModelState.IsValid) return SweetDialogHelper.HandleResponse(null);
            try
            {
                response = await _gateway.Add(vm).ConfigureAwait(false);
            }
            catch (GatewayException e)
            {
                return SweetDialogHelper.HandleStatusCode(e.StatusCode, e.Message);
            }


            return SweetDialogHelper.HandleResponse(response);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var vm = new RegisterUserViewModel
            {
                UserId = id,
                RoleList =await GetAssignableRole(),
                Role = "editor"
            };
            return PartialView("_Edit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Tuple<int, string>> Edit(string userId, RegisterUserViewModel vm)
        {
            IWebServiceResponse response;


            try
            {
                response = await _gateway.UpdateRole(userId, vm).ConfigureAwait(false);
            }
            catch (GatewayException e)
            {
                return SweetDialogHelper.HandleStatusCode(e.StatusCode, e.Message);
            }


            return SweetDialogHelper.HandleResponse(response);
        }

        [HttpGet]
        public IActionResult Detail(string id)
        {
            var vm = new RegisterUserViewModel
            {
                UserId = id
            };
            return PartialView("_Detail", vm);
        }

        [HttpPost]
        //TODO [ValidateAntiForgeryToken]
        public async Task<Tuple<int, string>> ConfirmDelete(string userId)
        {
            IWebServiceResponse response;

            if (userId == null) return SweetDialogHelper.HandleNotFound();
            try
            {
                response = await _gateway.Delete(userId).ConfigureAwait(false);
            }
            catch (GatewayException e)
            {
                return SweetDialogHelper.HandleStatusCode(e.StatusCode, e.Message);
            }
            
            return SweetDialogHelper.HandleResponse(response);
        }

        private async Task<string> GetUserRole()
        {
            if (User.Identity.IsAuthenticated)
            {
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                return accessToken;
            }

            throw new GatewayException(HttpStatusCode.Unauthorized,"User Not Authorized");
        } 
        
        private async Task< IEnumerable<SelectListItem>> GetAssignableRole()
        {
            var role =  HttpContext.User.Identities;
            return RoleHelper.GetAssignableRoles(await GetUserRole())
                .Select(role => new SelectListItem { Text = role, Value = role }).ToList();
        }
    }
}