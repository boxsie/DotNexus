using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNexus.Ledger;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DotNexus.App.Controllers
{
    public class BlockchainController : Controller
    {
        private readonly LedgerService _ledgerService;

        public BlockchainController(LedgerService ledgerService)
        {
            _ledgerService = ledgerService;
        }

        public IActionResult Blocks()
        {
            return View();
        }

        public IActionResult Transactions()
        {
            return View();
        }

        public async Task<IActionResult> Block(string blockId)
        {
            var isHeight = int.TryParse(blockId, out var height);

            var block = isHeight 
                ? await _ledgerService.GetBlockAsync(height) 
                : await _ledgerService.GetBlockAsync(blockId);

            return Content(JsonConvert.SerializeObject(block));
        }
    }
}