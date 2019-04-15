using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotNexus.Core;
using DotNexus.Core.Enums;
using DotNexus.Ledger.Models;
using Microsoft.Extensions.Logging;

namespace DotNexus.Ledger
{
    public class BlockNotifyJob : HostedService
    {
        public Func<Block, Task> OnNotify { get; set; }

        private readonly LedgerService _ledgerService;

        private Block _lastBlock;

        public BlockNotifyJob(ILogger<BlockNotifyJob> logger, LedgerService ledgerService, Func<Block, Task> onNotify = null) : base(logger)
        {
            _ledgerService = ledgerService;
            OnNotify = onNotify;
        }
        
        protected override async Task ExecuteAsync()
        {
            if (OnNotify == null)
                return;

            if (_lastBlock == null)
            {
                var info = await _ledgerService.GetMiningInfoAsync();

                if (info == null)
                    return;

                var lastBlock = await _ledgerService.GetBlockAsync(info.Blocks, TxVerbosity.Hash);
                
                if (lastBlock == null)
                    return;

                _lastBlock = lastBlock;
            }

            //_lastBlock = await _ledgerService.GetBlockAsync(_lastBlock.Hash, TxVerbosity.GenNextPrev, CancellationToken.None, false);

            //while (!string.IsNullOrWhiteSpace(_lastBlock.NextBlockHash))
            //{
            //    _lastBlock = await _ledgerService.GetBlockAsync(_lastBlock.NextBlockHash, TxVerbosity.GenNextPrev);

            //    await OnNotify.Invoke(_lastBlock);

            //    Logger.LogInformation($"Block {_lastBlock.Height} has arrived");
            //}

            try
            {
                var newBlock = await _ledgerService.GetBlockAsync(_lastBlock.Height + 1, TxVerbosity.Hash, CancellationToken.None, false);

                while (newBlock != null)
                {
                    await OnNotify.Invoke(_lastBlock);

                    Logger.LogInformation($"Block {_lastBlock.Height} has arrived");

                    _lastBlock = newBlock;

                    newBlock = await _ledgerService.GetBlockAsync(_lastBlock.Height + 1, TxVerbosity.GenNextPrev);
                }
            }
            catch (InvalidOperationException)
            {
                Logger.LogWarning("No new blocks found");
            }
        }
    }
}
