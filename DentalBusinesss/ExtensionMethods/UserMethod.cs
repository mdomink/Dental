using System.Security.Claims;
using DentalDomain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DentalBusiness.ExtensionMethods
{
    public static class UserMethod
    {
        public static int GetUserMaxDentalScan(this UserModel user)
        {
            return (int)user.UserCategory;
        }        
    }
}
