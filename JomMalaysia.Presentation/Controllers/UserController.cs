using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JomMalaysia.Presentation.Gateways.Users;
using JomMalaysia.Presentation.Models.AppUsers;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JomMalaysia.Presentation.Controllers
{
    public class UserController : Controller
    {

        private readonly IUserGateway _gateway;
        private static List<AppUser> UserList { get; set; }

        private static Boolean refresh = false;

        public UserController(IUserGateway userGateway)
        {
            _gateway = userGateway;
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
            if (UserList.Count > 0 && !refresh)
            {
                return UserList;
            }
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

            var users = await GetUsers();

            return View(users);
        }
    }
}
