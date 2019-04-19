using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNexus.App.Models;
using DotNexus.Assets;
using DotNexus.Assets.Models;
using DotNexus.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNexus.App.Controllers
{
    public class AssetController : Controller
    {
        private readonly AssetService _assetService;
        private readonly IUserManager _userManager;

        public AssetController(AssetService assetService, IUserManager userManager)
        {
            _assetService = assetService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(string assetAddress)
        {
            return View();
        }

        [NexusAuthorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [NexusAuthorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAssetViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _userManager.GetCurrentUser(HttpContext);

            user.Pin = model.Pin;

            var asset = new Asset
            {
                Name = model.Name,
                Data = model.Data
            };

            asset = await _assetService.CreateAssetAsync(asset, user);
            
            return RedirectToAction("index");
        }
    }
}