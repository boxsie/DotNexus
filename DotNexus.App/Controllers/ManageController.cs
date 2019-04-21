using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DotNexus.Identity;
using DotNexus.Core.Enums;
using DotNexus.App.Models;
using DotNexus.Core.Accounts;

namespace DotNexus.App.Controllers
{
    [NexusAuthorize]
    public class ManageController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly AccountService _accountService;

        public ManageController(IUserManager userManager, AccountService accountService)
        {
            _userManager = userManager;
            _accountService = accountService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Transactions()
        {
            var user = _userManager.GetCurrentUser(HttpContext);

            var txs = await _accountService.GetTransactionsAsync(user.GenesisId, TxVerbosity.PubKeySign);

            var model = new ManageTransactionsViewModel
            {
                Transactions = txs.ToList()
            };

            return View(model);
        }
    }
}