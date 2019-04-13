using System;
using System.Threading.Tasks;
using DotNexus.Accounts.Models;
using DotNexus.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNexus.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserManager _userManager;

        public HomeController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("login", "account");

            return View();
        }

        [Authorize]
        public IActionResult Secure()
        {
            return View();
        }
    }
}