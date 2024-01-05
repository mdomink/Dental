using DentalDomain.Data.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentalDomain.Models
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

        public UserCategory UserCategory { get; set; } = UserCategory.Basic;

        public virtual ICollection<PatientModel> Patients { get; set; }

    }
}
