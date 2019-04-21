using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotNexus.Core;
using DotNexus.Core.Enums;
using DotNexus.Core.Ledger;
using DotNexus.Core.Ledger.Models;
using Microsoft.Extensions.Logging;

namespace DotNexus.Jobs
{
    public class BlockNotifyJob : HostedService
    {
        private readonly Func<Block, Task> _onNotify;
        private readonly LedgerService _ledgerService;

        private Block _lastBlock;
        
        public BlockNotifyJob(ILogger<BlockNotifyJob> logger, LedgerService ledgerService, Func<Block, Task> onNotify) : base(logger)
        {
            _ledgerService = ledgerService;
            _onNotify = onNotify;
        }
        
        protected override async Task ExecuteAsync()
        {
            try
            {
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
            }
            catch (Exception)
            {
                Logger.LogCritical("Unable to connect to the Nexus node at this time");
                await Task.Delay(TimeSpan.FromSeconds(10));
                return;
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
                    await _onNotify.Invoke(_lastBlock);

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
