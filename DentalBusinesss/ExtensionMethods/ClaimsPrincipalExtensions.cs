using System.Security.Claims;

namespace DentalWeb
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserID(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public static bool IsInRole(this ClaimsPrincipal claimPrincipal, params string[] roles)
        {
            return roles.Any(r => claimPrincipal.IsInRole(r));
        }
    }
}
