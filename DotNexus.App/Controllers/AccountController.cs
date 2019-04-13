using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DotNexus.Accounts.Models;
using DotNexus.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DotNexus.App.Controllers
{
    public class LoginViewModel
    {
        [Required]
        [DisplayName("Username")]
        public string Username { get; set; }

        [Required]
        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("PIN")]
        public int? Pin { get; set; }
    }

    public class AccountController : Controller
    {
        private readonly IUserManager _userManager;

        public AccountController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _userManager.SignIn(HttpContext, new NexusUserCredential
                {
                    Username = model.Username,
                    Password = model.Password,
                    Pin = model.Pin
                });

                return RedirectToAction("index", "home");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("summary", e.Message);

                return View(model);
            }
        }
    }
}