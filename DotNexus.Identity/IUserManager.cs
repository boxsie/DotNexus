using System;
using System.Threading;
using System.Threading.Tasks;
using DotNexus.Accounts.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNexus.Identity
{
    public interface IUserManager
    {
        Task CreateAccount(HttpContext httpContext, NexusUserCredential user, bool login = false, CancellationToken token = default);
        Task Login(HttpContext httpContext, NexusUserCredential user, bool isPersistent = false, CancellationToken token = default);
        Task Logout(HttpContext httpContext);
        NexusUser GetCurrentUser(HttpContext httpContext);
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class NexusAuthorizeAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public NexusAuthorizeAttribute()
        {

        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                // it isn't needed to set unauthorized result 
                // as the base class already requires the user to be authenticated
                // this also makes redirect to a login page work properly
                // context.Result = new UnauthorizedResult();
                return;
            }

            // you can also use registered services
            var userManager = (IUserManager)context.HttpContext.RequestServices.GetService(typeof(IUserManager));
            var nexusUser = userManager.GetCurrentUser(context.HttpContext);
            
            if (nexusUser == null)
            {
                await context.HttpContext.SignOutAsync();
                context.HttpContext.Response.Redirect("/");
                return;
            }
        }
    }
}