using DentalBusiness.Data.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentalBusiness.Models
{
    [Index(nameof(OutUserId))]
    public class UserModel : IdentityUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OutUserId { get; set; }

        public string Name1 { get; set; }

        public string Name2 { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public int PostalCode { get; set; }

        public UserCategory UserCategory { get; set; }

    }
}
