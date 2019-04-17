using System.Threading;
using System.Threading.Tasks;
using DotNexus.Accounts.Models;
using Microsoft.AspNetCore.Http;

namespace DotNexus.Identity
{
    public interface IUserManager
    {
        Task CreateAccount(HttpContext httpContext, NexusUserCredential user, bool login = false, CancellationToken token = default);
        Task Login(HttpContext httpContext, NexusUserCredential user, bool isPersistent = false, CancellationToken token = default);
        Task Logout(HttpContext httpContext);
        NexusUser GetCurrentUser(HttpContext httpContext);
    }
}