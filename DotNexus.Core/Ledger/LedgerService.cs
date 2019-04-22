using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DotNexus.Core.Enums;
using DotNexus.Core.Ledger.Models;
using DotNexus.Core.Nexus;
using Microsoft.Extensions.Logging;

namespace DotNexus.Core.Ledger
{
    public class LedgerService : NexusService
    {
        private const int GetBlocksDefaultCount = 10;

        public LedgerService(NexusNode node, ILogger<NexusService> log) : base(node, log) { }

        public async Task<int> GetHeightAsync(CancellationToken token = default)
        {
            var info = await GetMiningInfoAsync(token);

            if (info == null)
                throw new InvalidOperationException("Get blockchain height failed");

            return info.Blocks;
        }

        public async Task<string> GetBlockHashAsync(int height, CancellationToken token = default, bool logOutput = true)
        {
            token.ThrowIfCancellationRequested();

            if (height <= 0)
                throw new ArgumentException("Height must be greater than 0");

            var request = new NexusRequest(new Dictionary<string, string> {{"height", height.ToString()}});

            var block = await GetAsync<Block>("ledger/blockhash", request, token, logOutput);

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

        public async Task<IEnumerable<Block>> GetBlocksAsync(string hash, int count = GetBlocksDefaultCount,
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

        public async Task<IEnumerable<Block>> GetBlocksAsync(int height, int count = GetBlocksDefaultCount, 
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

        public async Task<Transaction> GetTransactionAsync(string hash, TxVerbosity txVerbosity = TxVerbosity.PubKeySign, CancellationToken token = default, bool logOutput = true)
        {
            token.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(hash))
                throw new ArgumentException("Hash must have a value");
            
            var request = new NexusRequest(new Dictionary<string, string>
            {
                {"hash", hash},
                {"verbose", ((int) txVerbosity).ToString()}
            });

            var tx = await GetAsync<Transaction>("ledger/transaction", request, token, logOutput);

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

            return await GetAsync<Block>("ledger/block", request, token, logOutput);
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

            return await GetAsync<IEnumerable<Block>>("ledger/blocks", request, token, logOutput);
        }
    }
}