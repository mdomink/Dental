using System.Security.Claims;

namespace Dental
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserID(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
