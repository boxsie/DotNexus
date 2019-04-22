using System.Threading;
using System.Threading.Tasks;
using DotNexus.Core.Accounts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNexus.Identity
{
    public interface IUserManager
    {
        Task CreateAccount(HttpContext httpContext, NexusUserCredential user, bool login = false, CancellationToken token = default);
        Task LoginAsync(HttpContext httpContext, NexusUserCredential user, bool isPersistent = false, CancellationToken token = default);
        Task Logout(HttpContext httpContext);
        NexusUser GetCurrentUser(HttpContext httpContext);
    }
}