using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNexus.Identity
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class NexusAuthorizeAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
                return;

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