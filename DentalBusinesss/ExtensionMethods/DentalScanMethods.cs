using DentalBusiness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DentalBusiness.ExtensionMethods
{
    public static class DentalScanMethods
    {
        public static bool IsActive(this DentalScanModel dentalScan)
        {
            return (DateTime.Now - dentalScan.CreationDate).Days < 365 * 2;
        }
    }
}
