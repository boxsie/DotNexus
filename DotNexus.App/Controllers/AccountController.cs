using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNexus.App.Models;
using DotNexus.Core.Accounts.Models;
using DotNexus.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNexus.App.Controllers
{
    [Authorize]
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel viewModel, string returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            try
            {
                await _userManager.LoginAsync(HttpContext, new NexusUserCredential
                {
                    Username = viewModel.Username,
                    Password = viewModel.Password,
                    Pin = viewModel.Pin
                });
                
                if (string.IsNullOrWhiteSpace(returnUrl) || !Url.IsLocalUrl(returnUrl))
                    return RedirectToAction("index", "node");
                else
                    return Redirect(returnUrl);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("summary", e.Message);

                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _userManager.LogoutAsync(HttpContext);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("summary", e.Message);
            }

            return RedirectToAction("index", "node");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAccountViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var cred = new NexusUserCredential
                {
                    Username = model.Username,
                    Password = model.Password,
                    Pin = model.Pin
                };

                await _userManager.CreateAccount(HttpContext, cred, model.AutoLogin);

                return RedirectToAction("index", "node");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("summary", e.Message);

                return View(model);
            }
        }
    }
}