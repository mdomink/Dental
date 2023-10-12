using Dental.Data.Enum;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dental.Models
{
    public class UserModel : IdentityUser
    {
        public string Name1 { get; set; }

        public string Name2 { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public int PostalCode { get; set; }

        public UserCategory UserCategory { get; set; }

    }
}
