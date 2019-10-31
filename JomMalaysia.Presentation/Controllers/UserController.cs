using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using JomMalaysia.Framework.Helper;
using JomMalaysia.Framework.WebServices;
using JomMalaysia.Presentation.Gateways.Users;
using JomMalaysia.Presentation.Manager;
using JomMalaysia.Presentation.Models.AppUsers;
using JomMalaysia.Presentation.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JomMalaysia.Presentation.Controllers
{
    public class UserController : Controller
    {
        private readonly IAuthorizationManagers _auth;

        private readonly IUserGateway _gateway;
        private static List<AppUser> UserList { get; set; }

        private static Boolean refresh = false;

        public UserController(IUserGateway userGateway, IAuthorizationManagers authorization)
        {
            _gateway = userGateway;
            _auth = authorization;
            Refresh();
        }
        async void Refresh()
        {
            if (UserList != null)
                UserList = await GetUsers();
            else
            {
                UserList = new List<AppUser>();
            }
        }

        async Task<List<AppUser>> GetUsers()
        {

            try
            {
                UserList = await _gateway.GetAll().ConfigureAwait(false);
                return UserList;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        // GET: /<controller>/

        public async Task<IActionResult> Index()
        {

            ViewData["Role"] = _auth.LoginInfo.Role;
            var users = await GetUsers();

            return View(users.OrderBy(u =>
                        {
                            var index = RoleHelper.AuthorityList.IndexOf(u.Role);
                            return index == -1 ? int.MaxValue : index;
                        }));

        }

        [HttpGet]
        public IActionResult Create()
        {
            var _roles = new List<SelectListItem>();
            foreach (var role in RoleHelper.GetAssignableRoles(_auth.LoginInfo.Role))
            {
                _roles.Add(new SelectListItem
                {
                    Text = role,
                    Value = role
                });
            }
            var vm = new RegisterUserViewModel
            {
                RoleList = _roles,
                
            Role = "editor",
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<int> Create(RegisterUserViewModel vm)
        {
            int message;
            if (ModelState.IsValid)
            {
                
                IWebServiceResponse response;
                try
                {
                    response = await _gateway.Add(vm).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    throw e;
                }
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    refresh = true;
                    message = GlobalConstant.RESPONSE_OK;
                }
                else
                {
                    message = GlobalConstant.RESPONSE_ERR_UNKNOWN;
                }


            }
            else
            {
                message = GlobalConstant.RESPONSE_ERR_VALIDATION_FAILED;
            }
            return message;
        }
    }
}
