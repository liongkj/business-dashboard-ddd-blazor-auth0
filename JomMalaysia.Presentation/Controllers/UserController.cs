using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JomMalaysia.Presentation.Gateways.Users;
using JomMalaysia.Presentation.Manager;
using JomMalaysia.Presentation.Models.AppUsers;
using Microsoft.AspNetCore.Mvc;

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
            return View(users);

        }
    }
}
