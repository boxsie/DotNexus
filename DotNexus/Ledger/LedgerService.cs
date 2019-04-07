using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DotNexus.Core;
using DotNexus.Core.Enums;
using DotNexus.Ledger.Models;
using NLog;

namespace DotNexus.Ledger
{
    public class LedgerService : NexusService
    {
        private const int _getBlocksDefaultCount = 10;

        public LedgerService(ILogger log, HttpClient client, string connectionString)
            : base(log, client, connectionString) { }

        public async Task<string> GetBlockHashAsync(int height, CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();

            if (height <= 0)
                throw new Exception("Height must be greater than 0");

            var request = new NexusRequest(new Dictionary<string, string>
            {
                {"height", height.ToString()}
            });

            var block = await GetAsync<Block>("ledger/blockhash", request, token);

            return block.Hash;
        }

        public async Task<Block> GetBlockAsync(string hash, TxVerbosity txVerbosity = TxVerbosity.PubKeySign,
            CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(hash))
                throw new Exception("Hash must have a value");

            return await GetBlockAsync((object) hash, txVerbosity, token);
        }

        public async Task<Block> GetBlockAsync(int height, TxVerbosity txVerbosity = TxVerbosity.PubKeySign,
            CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();

            if (height <= 0)
                throw new Exception("Height must be greater than 0");

            return await GetBlockAsync((object) height, txVerbosity, token);
        }

        public async Task<IEnumerable<Block>> GetBlocksAsync(string hash, int count = _getBlocksDefaultCount,
            TxVerbosity txVerbosity = TxVerbosity.PubKeySign, CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(hash))
                throw new Exception("Hash must have a value");

            return await GetBlocks(hash, count, txVerbosity, token);
        }

        public async Task<IEnumerable<Block>> GetBlocksAsync(int height, int count = _getBlocksDefaultCount, 
            TxVerbosity txVerbosity = TxVerbosity.PubKeySign, CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();

            if (height <= 0)
                throw new Exception("Height must be greater than 0");

            return await GetBlocks(height, count, txVerbosity, token);
        }

        public async Task<Tx> GetTransactionAsync(string hash, TxVerbosity txVerbosity = TxVerbosity.PubKeySign, CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(hash))
                throw new Exception("Hash must have a value");
            
            var request = new NexusRequest(new Dictionary<string, string>
            {
                {"hash", hash},
                {"verbose", ((int) txVerbosity).ToString()}
            });

            return await GetAsync<Tx>("ledger/transaction", request, token);
        }

        private async Task<Block> GetBlockAsync(object retVal, TxVerbosity txVerbosity, CancellationToken token)
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

            return await GetAsync<Block>("ledger/block", request, token);
        }

        private async Task<IEnumerable<Block>> GetBlocks(object retVal, int count, TxVerbosity txVerbosity, CancellationToken token)
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

            return await GetAsync<IEnumerable<Block>>("ledger/blocks", request, token);
        }
    }
}