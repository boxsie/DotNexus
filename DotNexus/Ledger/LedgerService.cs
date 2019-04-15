using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DotNexus.Core;
using DotNexus.Core.Enums;
using DotNexus.Core.Nexus;
using DotNexus.Ledger.Models;
using Microsoft.Extensions.Logging;

namespace DotNexus.Ledger
{
    public class LedgerService : NexusService
    {
        private const int _getBlocksDefaultCount = 10;

        public LedgerService(ILogger log, INexusClient client, NexusSettings settings)
            : base(log, client, settings) { }

        public async Task<string> GetBlockHashAsync(int height, CancellationToken token = default, bool logOutput = true)
        {
            token.ThrowIfCancellationRequested();

            if (height <= 0)
                throw new ArgumentException("Height must be greater than 0");

            var request = new NexusRequest(new Dictionary<string, string> {{"height", height.ToString()}});

            var block = await PostAsync<Block>("ledger/blockhash", request, token, logOutput);

            if (string.IsNullOrWhiteSpace(block?.Hash))
                throw new InvalidOperationException($"Get block hash {height} failed");

            return block.Hash;
        }

        public async Task<Block> GetBlockAsync(string hash, TxVerbosity txVerbosity = TxVerbosity.PubKeySign, CancellationToken token = default, bool logOutput = true)
        {
            token.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(hash))
                throw new ArgumentException("Hash must have a value");

            var block = await GetBlockAsync((object)hash, txVerbosity, token, logOutput);

            if (string.IsNullOrWhiteSpace(block?.Hash))
                throw new InvalidOperationException($"Get block {hash} failed");

            return block;
        }

        public async Task<Block> GetBlockAsync(int height, TxVerbosity txVerbosity = TxVerbosity.PubKeySign, CancellationToken token = default, bool logOutput = true)
        {
            token.ThrowIfCancellationRequested();

            if (height <= 0)
                throw new ArgumentException("Height must be greater than 0");

            var block = await GetBlockAsync((object)height, txVerbosity, token, logOutput);

            if (string.IsNullOrWhiteSpace(block?.Hash))
                throw new InvalidOperationException($"Get block {height} failed");

            return block;
        }

        public async Task<IEnumerable<Block>> GetBlocksAsync(string hash, int count = _getBlocksDefaultCount,
            TxVerbosity txVerbosity = TxVerbosity.PubKeySign, CancellationToken token = default, bool logOutput = true)
        {
            token.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(hash))
                throw new ArgumentException("Hash must have a value");

            var blocks = await GetBlocks(hash, count, txVerbosity, token, logOutput);

            if (blocks == null)
                throw new InvalidOperationException($"Get {count} blocks from {hash} failed");

            return blocks;
        }

        public async Task<IEnumerable<Block>> GetBlocksAsync(int height, int count = _getBlocksDefaultCount, 
            TxVerbosity txVerbosity = TxVerbosity.PubKeySign, CancellationToken token = default, bool logOutput = true)
        {
            token.ThrowIfCancellationRequested();

            if (height <= 0)
                throw new ArgumentException("Height must be greater than 0");
            
            var blocks = await GetBlocks(height, count, txVerbosity, token, logOutput);

            if (blocks == null)
                throw new InvalidOperationException($"Get {count} blocks from {height} failed");

            return blocks;
        }

        public async Task<Tx> GetTransactionAsync(string hash, TxVerbosity txVerbosity = TxVerbosity.PubKeySign, CancellationToken token = default, bool logOutput = true)
        {
            token.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(hash))
                throw new ArgumentException("Hash must have a value");
            
            var request = new NexusRequest(new Dictionary<string, string>
            {
                {"hash", hash},
                {"verbose", ((int) txVerbosity).ToString()}
            });

            var tx = await PostAsync<Tx>("ledger/transaction", request, token, logOutput);

            if (string.IsNullOrWhiteSpace(tx?.Hash))
                throw new InvalidOperationException($"Get tx {hash} failed");

            return tx;
        }

        public async Task<MiningInfo> GetMiningInfoAsync(CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            var info = await GetAsync<MiningInfo>("ledger/mininginfo", null, token);

            if (info == null)
                throw new InvalidOperationException($"Get mining info failed");

            return info;
        }

        private async Task<Block> GetBlockAsync(object retVal, TxVerbosity txVerbosity, CancellationToken token, bool logOutput)
        {
            token.ThrowIfCancellationRequested();

            var useHash = retVal is string;

            var key = useHash ? "hash" : "height";
            var val = retVal.ToString();

            var request = new NexusRequest(new Dictionary<string, string>
            {
                {key, val},
                {"verbose", ((int) txVerbosity).ToString()}
            });

            return await PostAsync<Block>("ledger/block", request, token, logOutput);
        }

        private async Task<IEnumerable<Block>> GetBlocks(object retVal, int count, TxVerbosity txVerbosity, CancellationToken token, bool logOutput)
        {
            token.ThrowIfCancellationRequested();

            var useHash = retVal is string;

            var key = useHash ? "hash" : "height";
            var val = retVal.ToString();

            var request = new NexusRequest(new Dictionary<string, string>
            {
                {key, val},
                {"verbose", ((int) txVerbosity).ToString()},
                {"count", count.ToString()}
            });

            return await PostAsync<IEnumerable<Block>>("ledger/blocks", request, token, logOutput);
        }
    }
}