using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNexus.App.Models;
using DotNexus.Core.Enums;
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

        public async Task<IActionResult> Blocks()
        {
            var lastHeight = await _ledgerService.GetHeightAsync();
            var startBlockHash = await _ledgerService.GetBlockHashAsync(lastHeight - 10);
            var latestBlocks = await _ledgerService.GetBlocksAsync(startBlockHash, 10);

            return View(new BlockchainBlocksViewModel
            {
                LatestBlocks = latestBlocks.ToList()
            });
        }

        public IActionResult Transactions()
        {
            return View();
        }

        public async Task<IActionResult> Block(string blockId)
        {
            var isHeight = int.TryParse(blockId, out var height);

            var block = isHeight 
                ? await _ledgerService.GetBlockAsync(height, TxVerbosity.PubKeySign) 
                : await _ledgerService.GetBlockAsync(blockId, TxVerbosity.PubKeySign);

            return Content(JsonConvert.SerializeObject(block));
        }

        public async Task<IActionResult> Transaction(string hash)
        {
            var tx = await _ledgerService.GetTransactionAsync(hash, TxVerbosity.PubKeySign);

            return Content(JsonConvert.SerializeObject(tx));
        }
    }
}