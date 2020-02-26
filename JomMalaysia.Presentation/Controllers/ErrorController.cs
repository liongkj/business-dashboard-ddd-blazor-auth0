using Microsoft.AspNetCore.Mvc;

namespace JomMalaysia.Presentation.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult AccessDeniedError()
        {
            return View();
        }
    }
}