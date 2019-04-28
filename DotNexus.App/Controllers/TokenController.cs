using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Boxsie.Wrapplication.Repository;
using DotNexus.App.Models;
using DotNexus.Core.Assets;
using DotNexus.Core.Assets.Models;
using DotNexus.Core.Enums;
using DotNexus.Core.Tokens;
using DotNexus.Core.Tokens.Models;
using DotNexus.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNexus.App.Controllers
{
    [Authorize]
    public class TokenController : Controller
    {
        private readonly INexusServiceFactory _serviceFactory;
        private readonly IUserManager _userManager;
        private readonly IRepository<Token> _tokenRepository;

        public TokenController(INexusServiceFactory serviceFactory, IUserManager userManager, IRepository<Token> tokenRepository)
        {
            _serviceFactory = serviceFactory;
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        [Route("/token")]
        [NexusAuthorize]
        public async Task<IActionResult> Index()
        {
            return View(new TokenIndexViewModel());
        }

        [Route("/token/details/{address}")]
        public async Task<IActionResult> Details(string address)
        {
            return View();
        }

        [NexusAuthorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [NexusAuthorize]
        public async Task<IActionResult> Create(CreateTokenViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _userManager.GetCurrentUser(HttpContext);

            user.Pin = model.Pin;

            var token = CreateToken(model);
            
            var tokenService = await _serviceFactory.GetAsync<TokenService>(HttpContext);

            var newToken = await tokenService.CreateTokenAsync(token, user);

            await _tokenRepository.CreateAsync(newToken);

            return RedirectToAction("index");
        }

        private static Token CreateToken(CreateTokenViewModel model)
        {
            Token token;

            switch (model.TokenType)
            {
                case TokenType.Register:
                    if (!model.TotalSupply.HasValue)
                        throw new ArgumentException("Total supply is required for token register");

                    token = new TokenRegister { Supply = model.TotalSupply.Value };
                    break;
                case TokenType.Account:
                    token = new TokenAccount();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            token.Name = model.Name;
            token.Identifier = model.Identifier;

            return token;
        }
    }
}