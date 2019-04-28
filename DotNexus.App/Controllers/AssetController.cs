using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Boxsie.Wrapplication.Repository;
using DotNexus.App.Models;
using DotNexus.Core.Assets;
using DotNexus.Core.Assets.Models;
using DotNexus.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DotNexus.App.Controllers
{
    [Authorize]
    public class AssetController : Controller
    {
        private readonly INexusServiceFactory _serviceFactory;
        private readonly IUserManager _userManager;
        private readonly IRepository<Asset> _assetRepository;

        public AssetController(INexusServiceFactory serviceFactory, IUserManager userManager, IRepository<Asset> assetRepository)
        {
            _serviceFactory = serviceFactory;
            _userManager = userManager;
            _assetRepository = assetRepository;
        }

        [Route("/asset")]
        [NexusAuthorize]
        public async Task<IActionResult> Index()
        {
            var assets = User.IsNexusAuthorised() 
                ? (await _assetRepository.GetWhereAsync(new List<WhereClause> { new WhereClause("Genesis", "=", User.Identity.Name) })).ToList()
                : null;

            return View(new AssetIndexViewModel
            {
                UserAssets = assets
            });
        }

        [Route("/asset/details/{address}")]
        public async Task<IActionResult> Details(string address)
        {
            var asset = new Asset { Address = address };
            var assetService = await _serviceFactory.GetAsync<AssetService>(HttpContext);
            var history = await assetService.GetAssetHistoryAsync(asset);

            return View(new AssetDetailsViewModel
            {
                AssetInfo = await assetService.GetAssetAsync(asset),
                AssetHistory = history.ToList()
            });
        }

        [NexusAuthorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [NexusAuthorize]
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

            var assetService = await _serviceFactory.GetAsync<AssetService>(HttpContext);

            var newAsset = await assetService.CreateAssetAsync(asset, user);

            await _assetRepository.CreateAsync(newAsset);
            
            return RedirectToAction("index");
        }
    }
}