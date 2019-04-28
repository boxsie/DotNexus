using System.Security.Claims;

namespace DotNexus.Identity
{
    public static class UserHelpers
    {
        public static bool IsNexusAuthorised(this ClaimsPrincipal user)
        {
            return user.FindFirst(UserManager.NodeAuthClaimType)?.Value == UserManager.NodeAuthClaimResult;
        }
    }
}