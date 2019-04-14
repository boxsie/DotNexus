using System;
using System.Data;
using DotNexus.Accounts;
using DotNexus.Assets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace DotNexus.Identity
{
    public class NexusUserService
    {
        private readonly AccountService _accountService;

        public NexusUserService(AccountService accountService)
        {
            _accountService = accountService;
        }
    }
}
