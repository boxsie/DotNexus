using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNexus.App.Models;
using DotNexus.Core.Accounts;
using DotNexus.Core.Accounts.Models;
using DotNexus.Core.Enums;
using DotNexus.Core.Ledger;
using DotNexus.Core.Ledger.Models;
using DotNexus.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DotNexus.App.Controllers
{
    [Authorize]
    public class BlockchainController : Controller
    {
        private readonly INexusServiceFactory _serviceFactory;

        public BlockchainController(INexusServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        [Route("/blocks")]
        public async Task<IActionResult> Blocks()
        {
            var ledgerService = await _serviceFactory.GetAsync<LedgerService>(HttpContext);

            var lastHeight = await ledgerService.GetHeightAsync();
            var startBlockHash = await ledgerService.GetBlockHashAsync(lastHeight - 50);
            var latestBlocks = await ledgerService.GetBlocksAsync(startBlockHash, 50);

            return View(new BlockchainBlocksViewModel
            {
                LatestBlocks = latestBlocks.ToList()
            });
        }

        [Route("/transactions")]
        public IActionResult Transactions()
        {
            return View();
        }

        [Route("/block/{blockId}")]
        public async Task<IActionResult> Block(string blockId)
        {
            var isHeight = int.TryParse(blockId, out var height);

            var ledgerService = await _serviceFactory.GetAsync<LedgerService>(HttpContext);

            var block = isHeight 
                ? await ledgerService.GetBlockAsync(height, TxVerbosity.PubKeySign) 
                : await ledgerService.GetBlockAsync(blockId, TxVerbosity.PubKeySign);

            var newTxs = new List<Transaction>();

            foreach (var tx in block.Tx)
                newTxs.Add(await ledgerService.GetTransactionAsync(tx.Hash, TxVerbosity.PubKeySign));

            block.Tx = newTxs;

            return View(block);
        }

        [Route("/transaction/{hash}")]
        public async Task<IActionResult> Transaction(string hash)
        {
            var ledgerService = await _serviceFactory.GetAsync<LedgerService>(HttpContext);
            var tx = await ledgerService.GetTransactionAsync(hash, TxVerbosity.PubKeySign);

            return View(tx);
        }

        [Route("/genesis/{hash}")]
        public async Task<IActionResult> Genesis(string hash)
        {
            var genesisId = new GenesisId { Genesis = hash };

            var accountService = await _serviceFactory.GetAsync<AccountService>(HttpContext);
            var txs = await accountService.GetTransactionsAsync(genesisId, TxVerbosity.PubKeySign);

            var model = new GenesisViewModel
            {
                Genesis = hash,
                Transactions = txs.ToList()
            };

            return View(model);
        }
    }
}