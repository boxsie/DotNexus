﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNexus.Accounts.Models;
using DotNexus.App.Models;
using DotNexus.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DotNexus.App.Controllers
{
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
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _userManager.Login(HttpContext, new NexusUserCredential
                {
                    Username = model.Username,
                    Password = model.Password,
                    Pin = model.Pin
                });
                
                if (string.IsNullOrWhiteSpace(returnUrl) || !Url.IsLocalUrl(returnUrl))
                    return RedirectToAction("index", "home");
                else
                    return Redirect(returnUrl);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("summary", e.Message);

                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _userManager.Logout(HttpContext);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("summary", e.Message);
            }

            return RedirectToAction("index", "home");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
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