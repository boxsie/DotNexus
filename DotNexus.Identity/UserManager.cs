using System;
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
using Microsoft.Extensions.Logging;

namespace DotNexus.Identity
{
    public class UserManager : IUserManager
    {
        public static string NodeAuthClaimType { get; private set; }
        public static string NodeAuthClaimResult { get; private set; }

        private readonly ILogger<UserManager> _log;
        private readonly INexusServiceFactory _serviceFactory;
        private readonly CookieConstants _cookieConstants;

        public UserManager(ILogger<UserManager> log, INexusServiceFactory serviceFactory, CookieConstants cookieConstants)
        {
            _log = log;
            _serviceFactory = serviceFactory;
            _cookieConstants = cookieConstants;

            NodeAuthClaimType = _cookieConstants.NodeAuthClaimType;
            NodeAuthClaimResult = cookieConstants.NodeAuthClaimResult;
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

            httpContext.Session.SetString(_cookieConstants.GenesisIdKey, nexusUser.GenesisId.Genesis);
            httpContext.Session.SetString(_cookieConstants.SessionIdKey, nexusUser.GenesisId.Session);
            httpContext.Session.SetString(_cookieConstants.UsernameKey, nexusUser.Username);

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

        public async Task LogoutAsync(HttpContext httpContext)
        {
            try
            {
                var session = httpContext.Session.GetString(_cookieConstants.SessionIdKey);

                if (!string.IsNullOrEmpty(session))
                {
                    var accountService = await _serviceFactory.GetAsync<AccountService>(httpContext);
                    await accountService.LogoutAsync(session);
                }

                if (httpContext.User.Identity.IsAuthenticated)
                {
                    RemoveUserClaims(((ClaimsIdentity)httpContext.User.Identity));

                    await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, httpContext.User);
                }
                else
                    await httpContext.SignOutAsync();
            }
            catch (Exception e)
            {
                _log.LogWarning($"There was an error logging out - {e.Message}");
            }
        }

        public NexusUser GetCurrentUser(HttpContext httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return null;

            var genesisClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            
            var genesisId = httpContext.Session.GetString(_cookieConstants.GenesisIdKey);
            var sessionId = httpContext.Session.GetString(_cookieConstants.SessionIdKey);
            var username = httpContext.Session.GetString(_cookieConstants.UsernameKey);

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

        private IEnumerable<Claim> AddUserClaims(NexusUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.GenesisId.Genesis),
                new Claim(_cookieConstants.NodeAuthClaimType, _cookieConstants.NodeAuthClaimResult)
            };

            return claims;
        }

        private void RemoveUserClaims(ClaimsIdentity user)
        {
            var nClaim = user.FindFirst(ClaimTypes.Name);

            if (nClaim != null)
                user.RemoveClaim(nClaim);

            var aClaim = user.FindFirst(_cookieConstants.NodeAuthClaimType);

            if (aClaim != null)
                user.RemoveClaim(aClaim);
        }
    }
}