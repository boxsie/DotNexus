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
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DotNexus.App.Controllers
{
    public class BlockchainController : Controller
    {
        private readonly LedgerService _ledgerService;
        private readonly AccountService _accountService;

        public BlockchainController(LedgerService ledgerService, AccountService accountService)
        {
            _ledgerService = ledgerService;
            _accountService = accountService;
        }

        [Route("/blocks")]
        public async Task<IActionResult> Blocks()
        {
            var lastHeight = await _ledgerService.GetHeightAsync();
            var startBlockHash = await _ledgerService.GetBlockHashAsync(lastHeight - 50);
            var latestBlocks = await _ledgerService.GetBlocksAsync(startBlockHash, 50);

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

        public async Task<IActionResult> Block(string blockId)
        {
            var isHeight = int.TryParse(blockId, out var height);

            var block = isHeight 
                ? await _ledgerService.GetBlockAsync(height, TxVerbosity.PubKeySign) 
                : await _ledgerService.GetBlockAsync(blockId, TxVerbosity.PubKeySign);

            var newTxs = new List<Transaction>();
            foreach (var tx in block.Tx)
                newTxs.Add(await _ledgerService.GetTransactionAsync(tx.Hash, TxVerbosity.PubKeySign));
            block.Tx = newTxs;

            return View(block);
        }

        public async Task<IActionResult> Transaction(string hash)
        {
            var tx = await _ledgerService.GetTransactionAsync(hash, TxVerbosity.PubKeySign);

            return View(tx);
        }

        public async Task<IActionResult> Genesis(string hash)
        {
            var genesisId = new GenesisId { Genesis = hash };

            var txs = await _accountService.GetTransactionsAsync(genesisId, TxVerbosity.PubKeySign);

            var model = new GenesisViewModel
            {
                Genesis = hash,
                Transactions = txs.ToList()
            };

            return View(model);
        }
    }
}