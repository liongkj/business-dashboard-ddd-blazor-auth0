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

        public AccountController(IAppSetting appSetting)
        {
            _appSetting = appSetting;
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
            if (ModelState.IsValid)
            {
                try
                {
                    AuthenticationApiClient client = new AuthenticationApiClient(new Uri($"https://{_appSetting.Auth0Domain}/"));

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
                    //var role = user.AdditionalClaims["https://jomn9:auth0:com//roles"].Values<String>().ToList();


                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(result.AccessToken);

                    var tokenS = handler.ReadToken(result.AccessToken) as JwtSecurityToken;
                    var role = tokenS.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).FirstOrDefault();



                    // Create claims principal
                    var claims = new List<Claim>
                    {
                        new Claim(ConstantHelper.Claims.accessToken, result.AccessToken),
                        new Claim(ConstantHelper.Claims.refreshToken, result.RefreshToken),
                        new Claim(ConstantHelper.Claims.expiry, result.ExpiresIn.ToString()),
                        new Claim(ConstantHelper.Claims.userId, user.UserId),
                        new Claim(ConstantHelper.Claims.name, user.FullName),
                        new Claim(ClaimTypes.Role,role)

                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    // Sign user into cookie middleware
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToLocal(vm.returnURL);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }
            // return RedirectToLocal(vm.returnURL); //auth: disable this
            return View(vm); //auth:enable this to bypass
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(AccountController.Login), "Account");
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