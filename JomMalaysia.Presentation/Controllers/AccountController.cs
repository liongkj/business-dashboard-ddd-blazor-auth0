using System.Threading.Tasks;
using JomMalaysia.Framework.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JomMalaysia.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAppSetting _appSetting;

        public AccountController(IAppSetting appSetting)
        {
            _appSetting = appSetting;
        }

        //
        // [HttpGet]
        // public IActionResult Login(string returnUrl)
        // {
        //     ViewData["ReturnUrl"] = returnUrl;
        //
        //     return View();
        // }

        
        public async Task Login(string returnUrl = "/")
        {
            // if (!ModelState.IsValid)
            //     return RedirectToLocal(vm.returnURL); //auth: disable this to force logout
            await HttpContext.ChallengeAsync("Auth0", new AuthenticationProperties{ RedirectUri = returnUrl });
            // try
            // {
            //     AuthenticationApiClient client =
            //         new AuthenticationApiClient(new Uri($"https://{_appSetting.Auth0Domain}/"));
            //     
            //     var result = await client.GetTokenAsync(new ResourceOwnerTokenRequest
            //     {
            //         ClientId = _appSetting.Auth0ClientId,
            //         ClientSecret = _appSetting.Auth0ClientSecret,
            //         Scope = _appSetting.Scope,
            //         Realm = _appSetting.DBConnection,
            //         Username = vm.email,
            //         Password = vm.password,
            //         Audience = _appSetting.Audience
            //     }).ConfigureAwait(false);
            //
            //     // Get user info from token
            //     var user = await client.GetUserInfoAsync(result.AccessToken).ConfigureAwait(false);
            //     
            //
            //     var handler = new JwtSecurityTokenHandler();
            //
            //     if (handler.ReadToken(result.AccessToken) is JwtSecurityToken tokenS)
            //     {
            //         var role = tokenS.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value)
            //             .FirstOrDefault();
            //         var exp = tokenS.Claims.FirstOrDefault(x => x.Type == "exp")?.Value;
            //         // Create claims principal
            //         var claims = new List<Claim>
            //         {
            //             new Claim(ConstantHelper.Claims.accessToken, result.AccessToken),
            //             new Claim(ConstantHelper.Claims.refreshToken, result.RefreshToken),
            //             new Claim(ConstantHelper.Claims.expiry, exp),
            //             //current 86400 
            //             new Claim(ConstantHelper.Claims.userId, user.UserId),
            //             new Claim(ConstantHelper.Claims.name, user.FullName),
            //             new Claim(ClaimTypes.Role, role)
            //         };
            //
            //         var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //         // Sign user into cookie middleware
            //         
            //         await HttpContext.SignInAsync("Auth0",
            //             new ClaimsPrincipal(claimsIdentity))
            //             .ConfigureAwait(false);
            //         // await HttpContext.ChallengeAsync("Auth0", new AuthenticationProperties() { RedirectUri = vm.returnURL });
            //     }
            //
            //     return RedirectToLocal(vm.returnURL);
            // }
            // catch (Exception e)
            // {
            //     ModelState.AddModelError("", e.Message);
            // }
            //
            // return RedirectToLocal(vm.returnURL); //auth: enable this to force logout
            // return View(vm); //auth:enable this to bypass
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Auth0", new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(Login), "Account")
            });
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // return RedirectToAction(nameof(Login), "Account");
        }

        [Authorize]
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
            
                return RedirectToAction(nameof(HomeController.Index), "Home");
            
        }

        #endregion
    }
}