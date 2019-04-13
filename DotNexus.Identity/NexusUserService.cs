using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using DotNexus.Accounts;
using DotNexus.Accounts.Models;
using DotNexus.Assets;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
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

    public interface IUserManager
    {
        Task SignIn(HttpContext httpContext, NexusUserCredential user, bool isPersistent = false, CancellationToken token = default);
        Task SignOut(HttpContext httpContext);
    }

    public class UserManager : IUserManager
    {
        private readonly AccountService _accountService;

        public UserManager(AccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task SignIn(HttpContext httpContext, NexusUserCredential user, bool isPersistent = false, CancellationToken token = default)
        {
            var nexusUser = await _accountService.LoginAsync(user, token);

            if (nexusUser == null)
                return;

            httpContext.Session.SetString("SessionId", nexusUser.GenesisId.Session);
            httpContext.Session.SetString("GenesisId", nexusUser.GenesisId.Genesis);

            if (user.Pin.HasValue)
                httpContext.Session.SetInt32("Pin", user.Pin.Value);

            var identity = new ClaimsIdentity(GetUserClaims(nexusUser), CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        public async Task SignOut(HttpContext httpContext)
        {
            var session = httpContext.Session.GetString("SessionId");

            await _accountService.LogoutAsync(session);
            await httpContext.SignOutAsync();
        }

        private IEnumerable<Claim> GetUserClaims(NexusUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.GenesisId.Genesis)
            };

            claims.AddRange(this.GetUserRoleClaims(user));
            return claims;
        }

        private IEnumerable<Claim> GetUserRoleClaims(NexusUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.GenesisId.Genesis)
            };

            return claims;
        }
    }
}
