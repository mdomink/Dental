using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalBusiness.Repository;
using DentalDomain.Data;
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
