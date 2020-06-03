using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebClient.Controllers
{
    [Authorize]
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.User?.GetDisplayName();
            return View();
        }

        public IActionResult LogOut()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}