using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using DotNexus.Accounts;
using DotNexus.Accounts.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace DotNexus.Identity
{
    public class UserManager : IUserManager
    {
        private readonly AccountService _accountService;

        public UserManager(AccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task CreateAccount(HttpContext httpContext, NexusUserCredential user, bool login = false, CancellationToken token = default)
        {
            var genesisId = await _accountService.CreateAccountAsync(user, token);

            if (genesisId != null && login)
                await Login(httpContext, user, false, token);
        }

        public async Task Login(HttpContext httpContext, NexusUserCredential user, bool isPersistent = false, CancellationToken token = default)
        {
            var nexusUser = await _accountService.LoginAsync(user, token);

            if (nexusUser == null)
                return;

            nexusUser.Username = user.Username;

            httpContext.Session.SetString("SessionId", nexusUser.GenesisId.Session);
            httpContext.Session.SetString("GenesisId", nexusUser.GenesisId.Genesis);

            if (user.Pin.HasValue)
                httpContext.Session.SetInt32("Pin", user.Pin.Value);

            var identity = new ClaimsIdentity(GetUserClaims(nexusUser), CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        public async Task Logout(HttpContext httpContext)
        {
            var session = httpContext.Session.GetString("SessionId");

            await _accountService.LogoutAsync(session);
            await httpContext.SignOutAsync();
        }

        public NexusUser GetCurrentUser(HttpContext httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return null;

            var genesisClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var usernameClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

            var genesisId = httpContext.Session.GetString("GenesisId");
            var sessionId = httpContext.Session.GetString("SessionId");

            if (genesisClaim == null || usernameClaim == null || genesisClaim.Value != genesisId)
                return null;

            return new NexusUser
            {
                Username = usernameClaim.Value,
                GenesisId = new GenesisId
                {
                    Genesis = genesisId,
                    Session = sessionId
                },
            };
        }

        private IEnumerable<Claim> GetUserClaims(NexusUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.GenesisId.Genesis),
                new Claim(ClaimTypes.Name, user.Username)
            };

            claims.AddRange(GetUserRoleClaims(user));
            return claims;
        }

        private static IEnumerable<Claim> GetUserRoleClaims(NexusUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.GenesisId.Genesis)
            };

            return claims;
        }
    }
}