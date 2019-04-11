using System;
using System.Threading;
using System.Threading.Tasks;
using DotNexus.Account;
using DotNexus.Account.Models;
using DotNexus.Assets;

namespace DotNexus.Identity
{
    public class NexusUserService
    {
        private readonly AccountService _accountService;

        public NexusUserService(AccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<NexusUser> CreateAsync(NexusUser user, CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();

            return await _accountService.CreateAccountAsync(user, token);
        }

        public async Task<NexusUser> LoginAsync(NexusUser user, CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();

            if (user?.Username == null)
                throw new ArgumentNullException(nameof(user));

            return await _accountService.LoginAsync(user, token);
        }

        public async Task<NexusUser> LogoutAsync(NexusUser user, CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();

            if (user?.GenesisId == null)
                throw new ArgumentNullException(nameof(user));

            user.GenesisId = await _accountService.LogoutAsync(user.GenesisId.Session, token);

            return user;
        }
    }
}
