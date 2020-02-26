using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using JomMalaysia.Framework.Configuration;
using JomMalaysia.Framework.Constant;
using JomMalaysia.Framework.Helper;
using JomMalaysia.Presentation.Manager;
using JomMalaysia.Presentation.Models;
using JomMalaysia.Presentation.Models.Auth0;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace JomMalaysia.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAppSetting _appSetting;
        private readonly IAuthorizationManagers _authorizationManagers;

        public AccountController(IAppSetting appSetting, IAuthorizationManagers authorizationManagers)
        {
            _appSetting = appSetting;
            _authorizationManagers = authorizationManagers;
        }


        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User vm)
        {
            if (!ModelState.IsValid)
                return RedirectToLocal(vm.returnURL); //auth: disable this to force logout
            try
            {
                AuthenticationApiClient client =
                    new AuthenticationApiClient(new Uri($"https://{_appSetting.Auth0Domain}/"));
                
                var result = await client.GetTokenAsync(new ResourceOwnerTokenRequest
                {
                    ClientId = _appSetting.Auth0ClientId,
                    ClientSecret = _appSetting.Auth0ClientSecret,
                    Scope = _appSetting.Scope,
                    Realm = _appSetting.DBConnection,
                    Username = vm.email,
                    Password = vm.password,
                    Audience = _appSetting.Audience
                }).ConfigureAwait(false);

                // Get user info from token
                var user = await client.GetUserInfoAsync(result.AccessToken).ConfigureAwait(false);
                

                var handler = new JwtSecurityTokenHandler();

                if (handler.ReadToken(result.AccessToken) is JwtSecurityToken tokenS)
                {
                    var role = tokenS.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value)
                        .FirstOrDefault();
                    var exp = tokenS.Claims.FirstOrDefault(x => x.Type == "exp")?.Value;
                    // Create claims principal
                    var claims = new List<Claim>
                    {
                        new Claim(ConstantHelper.Claims.accessToken, result.AccessToken),
                        new Claim(ConstantHelper.Claims.refreshToken, result.RefreshToken),
                        new Claim(ConstantHelper.Claims.expiry, exp),
                        //current 86400 
                        new Claim(ConstantHelper.Claims.userId, user.UserId),
                        new Claim(ConstantHelper.Claims.name, user.FullName),
                        new Claim(ClaimTypes.Role, role)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    // Sign user into cookie middleware
                    
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity))
                        .ConfigureAwait(false);
                    await HttpContext.ChallengeAsync("Auth0", new AuthenticationProperties() { RedirectUri = vm.returnURL });
                }

                return RedirectToLocal(vm.returnURL);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            return RedirectToLocal(vm.returnURL); //auth: enable this to force logout
            // return View(vm); //auth:enable this to bypass
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Auth0", new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(Login), "Account")
            });
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false);

            // return RedirectToAction(nameof(Login), "Account");
        }

        public async Task<string> RefreshToken()
        {
            try{
                AuthenticationApiClient client =
                    new AuthenticationApiClient(new Uri($"https://{_appSetting.Auth0Domain}/"));
                var request = new RefreshTokenRequest
                {
                    RefreshToken =  _authorizationManagers.refreshToken,
                    ClientId = _appSetting.Auth0ClientId,
                    ClientSecret = _appSetting.Auth0ClientSecret,
                };
              var response =  await client.GetTokenAsync(request);
              // if(response)
              // HttpContext.User.Claims

            }catch(Exception e)
            {
                throw e;
            }

            return "";
        }
        
        public IActionResult Claims()
        {
            return View();
        }

        #region Helpers

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}