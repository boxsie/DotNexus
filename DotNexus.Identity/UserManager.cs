using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using DotNexus.Core.Accounts;
using DotNexus.Core.Accounts.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace DotNexus.Identity
{
    public class UserManager : IUserManager
    {
        public const string NodeAuthClaimType = "NodeAuth";
        public const string NodeAuthClaimResult = "Autherised";
        public const string SessionIdKey = "SessionId";
        public const string GenesisIdKey = "GenesisId";
        public const string UsernameKey = "Username";

        private readonly INexusServiceFactory _serviceFactory;

        public UserManager(INexusServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public async Task CreateAccount(HttpContext httpContext, NexusUserCredential user, bool login = false, CancellationToken token = default)
        {
            var accountService = await _serviceFactory.GetAsync<AccountService>(httpContext);

            var genesisId = await accountService.CreateAccountAsync(user, token);

            if (genesisId != null && login)
                await LoginAsync(httpContext, user, false, token);
        }

        public async Task LoginAsync(HttpContext httpContext, NexusUserCredential user, bool isPersistent = false, CancellationToken token = default)
        {
            var accountService = await _serviceFactory.GetAsync<AccountService>(httpContext);

            var nexusUser = await accountService.LoginAsync(user, token);

            if (nexusUser == null)
                return;

            nexusUser.Username = user.Username;

            httpContext.Session.SetString(GenesisIdKey, nexusUser.GenesisId.Genesis);
            httpContext.Session.SetString(SessionIdKey, nexusUser.GenesisId.Session);
            httpContext.Session.SetString(UsernameKey, nexusUser.Username);

            if (httpContext.User.Identity.IsAuthenticated)
            {
                ((ClaimsIdentity)httpContext.User.Identity).AddClaims(AddUserClaims(nexusUser));
                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, httpContext.User);
            }
            else
            {
                var identity = new ClaimsIdentity(AddUserClaims(nexusUser), CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            }
        }

        public async Task Logout(HttpContext httpContext)
        {
            var session = httpContext.Session.GetString(SessionIdKey);

            if (!string.IsNullOrEmpty(session))
            {
                var accountService = await _serviceFactory.GetAsync<AccountService>(httpContext);
                await accountService.LogoutAsync(session);
            }

            if (httpContext.User.Identity.IsAuthenticated)
            {
                RemoveUserClaims(((ClaimsIdentity) httpContext.User.Identity));

                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, httpContext.User);
            }
            else
                await httpContext.SignOutAsync();
        }

        public NexusUser GetCurrentUser(HttpContext httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return null;

            var genesisClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            
            var genesisId = httpContext.Session.GetString(GenesisIdKey);
            var sessionId = httpContext.Session.GetString(SessionIdKey);
            var username = httpContext.Session.GetString(UsernameKey);

            if (string.IsNullOrEmpty(genesisId) || string.IsNullOrEmpty(sessionId) || genesisClaim == null || genesisClaim.Value != genesisId)
                return null;

            return new NexusUser
            {
                Username = username,
                GenesisId = new GenesisId
                {
                    Genesis = genesisId,
                    Session = sessionId
                },
            };
        }

        private static IEnumerable<Claim> AddUserClaims(NexusUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.GenesisId.Genesis),
                new Claim(NodeAuthClaimType, NodeAuthClaimResult)
            };

            return claims;
        }

        private static void RemoveUserClaims(ClaimsIdentity user)
        {
            user.RemoveClaim(user.FindFirst(ClaimTypes.Name));
            user.RemoveClaim(user.FindFirst(NodeAuthClaimType));
        }
    }
}