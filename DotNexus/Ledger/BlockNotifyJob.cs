using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly LedgerService _ledgerService;
        private readonly Dictionary<Guid, Func<Block, Task>> _subscriptions;

        private Block _lastBlock;

        public BlockNotifyJob(ILogger<BlockNotifyJob> logger, LedgerService ledgerService, Func<Block, Task> onNotify = null) : base(logger)
        {
            _ledgerService = ledgerService;
            _subscriptions = new Dictionary<Guid, Func<Block, Task>>();
        }

        public Guid Subscribe(Func<Block, Task> onNotify)
        {
            if (onNotify == null)
                throw new ArgumentException("On notify function is required");

            var id = Guid.NewGuid();

            _subscriptions.Add(id, onNotify);

            return id;
        }

        public void Unsubscribe(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Subscription ID is required");
            
            if (!_subscriptions.ContainsKey(id))
                throw new ArgumentException($"Subscription {id} not found");

            _subscriptions.Remove(id);
        }

        protected override async Task ExecuteAsync()
        {
            if (!_subscriptions.Any())
                return;

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
                    foreach (var subscription in _subscriptions.Values)
                        await subscription.Invoke(_lastBlock);

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
