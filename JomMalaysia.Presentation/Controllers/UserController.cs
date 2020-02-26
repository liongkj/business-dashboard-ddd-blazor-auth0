﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using JomMalaysia.Framework.Exceptions;
using JomMalaysia.Framework.Helper;
using JomMalaysia.Framework.Interfaces;
using JomMalaysia.Presentation.Gateways.Users;
using JomMalaysia.Presentation.Manager;
using JomMalaysia.Presentation.Models.AppUsers;
using JomMalaysia.Presentation.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JomMalaysia.Presentation.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IAuthorizationManagers _auth;

        private readonly IUserGateway _gateway;

        private bool refresh;

        public UserController(IUserGateway userGateway, IAuthorizationManagers authorization)
        {
            _gateway = userGateway;
            _auth = authorization;
            Refresh();
        }

        private List<AppUser> UserList { get; set; }

        private async void Refresh()
        {
            if (UserList != null && !refresh)
                UserList = await GetUsers();
            else
                UserList = new List<AppUser>();
        }

        private async Task<List<AppUser>> GetUsers()
        {
            try
            {
                UserList = await _gateway.GetAll().ConfigureAwait(false);
                return UserList;
            }
            catch (GatewayException e)
            {
                if (e.StatusCode == HttpStatusCode.Unauthorized)
                {
                   
                }
                throw e;
            }
        }

        // GET: /<controller>/

        public async Task<IActionResult> Index()
        {
            ViewData["Role"] = _auth.LoginInfo.Role;
            var users = await GetUsers().ConfigureAwait(false);

            return View(users.OrderBy(u =>
            {
                var index = RoleHelper.AuthorityList.IndexOf(u.Role);
                return index == -1 ? int.MaxValue : index;
            }));
        }

        [HttpGet]
        public IActionResult Create()
        {
            var vm = new RegisterUserViewModel
            {
                RoleList = GetAssignableRole(),

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

            if (response.StatusCode == HttpStatusCode.OK) refresh = true;

            return SweetDialogHelper.HandleResponse(response);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var vm = new RegisterUserViewModel
            {
                UserId = id,
                RoleList = GetAssignableRole(),
                Role = "editor"
            };
            return PartialView("_Edit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Tuple<int, string>> Edit(string UserId, RegisterUserViewModel vm)
        {
            IWebServiceResponse response;


            try
            {
                response = await _gateway.UpdateRole(UserId, vm).ConfigureAwait(false);
            }
            catch (GatewayException e)
            {
                return SweetDialogHelper.HandleStatusCode(e.StatusCode, e.Message);
            }

            if (response.StatusCode == HttpStatusCode.OK) refresh = true;

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
        public async Task<Tuple<int, string>> ConfirmDelete(string UserId)
        {
            IWebServiceResponse response;

            if (UserId == null) return SweetDialogHelper.HandleNotFound();
            try
            {
                response = await _gateway.Delete(UserId).ConfigureAwait(false);
            }
            catch (GatewayException e)
            {
                return SweetDialogHelper.HandleStatusCode(e.StatusCode, e.Message);
            }

            if (response.StatusCode == HttpStatusCode.OK) refresh = true;

            return SweetDialogHelper.HandleResponse(response);
        }

        private List<SelectListItem> GetAssignableRole()
        {
            return RoleHelper.GetAssignableRoles(_auth.LoginInfo.Role)
                .Select(role => new SelectListItem { Text = role, Value = role }).ToList();
        }
    }
}