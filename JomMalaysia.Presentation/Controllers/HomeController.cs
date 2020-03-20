using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using JomMalaysia.Framework.Configuration;
using JomMalaysia.Presentation.ViewModels;
using Microsoft.AspNetCore.Authentication;

namespace JomMalaysia.Presentation.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IAppSetting _appSetting;

        public HomeController(IAppSetting appSetting)
        {
            _appSetting = appSetting;
        }


        public async Task<IActionResult> Index()
        {
            ViewData["AT"] = await HttpContext.GetTokenAsync("access_token");
            ViewData["ID"] = await HttpContext.GetTokenAsync("id_token");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}